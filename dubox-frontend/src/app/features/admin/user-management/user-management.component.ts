import { Component, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { FormsModule } from '@angular/forms';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import { UserService } from '../../../core/services/user.service';
import type { UserDto, UpdateUserRequest, CreateUserRequest } from '../../../core/services/user.service';
import { GroupDto, GroupService } from '../../../core/services/group.service';
import { RoleDto, RoleService } from '../../../core/services/role.service';
import { DepartmentService } from '../../../core/services/department.service';
import { Department } from '../../../core/models/team.model';
import { AuditLogService } from '../../../core/services/audit-log.service';
import { AuditLog, AuditLogQueryParams } from '../../../core/models/audit-log.model';
import { PermissionService, PermissionDto, PermissionGroupDto, RolePermissionMatrixDto } from '../../../core/services/permission.service';
import { debounceTime, distinctUntilChanged, switchMap, finalize, catchError, map } from 'rxjs/operators';
import { forkJoin, of, Subscription, Observable } from 'rxjs';

type UserManagementTab = 'users' | 'groups' | 'roles' | 'permissions';
type ModalMode = 'create' | 'edit';

interface ConfirmationState {
  open: boolean;
  title: string;
  message: string;
  confirmLabel: string;
  action?: () => void;
}

@Component({
  selector: 'app-user-management',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    FormsModule,
    HeaderComponent,
    SidebarComponent
  ],
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.scss']
})
export class UserManagementComponent implements OnInit, OnDestroy {
  activeTab: UserManagementTab = 'users';
  
  // Expose Math to template
  readonly Math = Math;

  readonly tabs: { id: UserManagementTab; label: string }[] = [
    { id: 'users', label: 'Users' },
    { id: 'groups', label: 'Groups' },
    { id: 'roles', label: 'Roles' },
    { id: 'permissions', label: 'Permissions' }
  ];

  filterForm: FormGroup;
  groupFilterForm: FormGroup;
  roleFilterForm: FormGroup;
  userForm: FormGroup;
  groupForm: FormGroup;
  roleForm: FormGroup;

  users: UserDto[] = [];
  filteredUsers: UserDto[] = [];
  
  // Pagination
  currentPage = 1;
  pageSize = 25;
  totalCount = 0;
  totalPages = 0;
  roles: RoleDto[] = [];
  filteredRoles: RoleDto[] = [];
  groups: GroupDto[] = [];
  filteredGroups: GroupDto[] = [];
  departments: Department[] = [];

  isLoadingUsers = false;
  usersError = '';
  isLoadingGroups = false;
  groupsError = '';
  isLoadingRoles = false;
  rolesError = '';
  isLoadingDepartments = false;

  groupMemberCounts = new Map<string, number>();

  // Permissions
  permissionGroups: PermissionGroupDto[] = [];
  permissionMatrix: RolePermissionMatrixDto[] = [];
  allPermissions: PermissionDto[] = [];
  isLoadingPermissions = false;
  permissionsError = '';
  selectedRoleForPermissions: RoleDto | null = null;
  selectedPermissionIds: Set<string> = new Set();
  savingPermissions = false;
  permissionMatrixModalOpen = false;

  userModalOpen = false;
  groupModalOpen = false;
  roleModalOpen = false;
  modalMode: ModalMode = 'create';
  savingUser = false;
  savingGroup = false;
  savingRole = false;
  showTempPassword = false;
  alertMessage = '';
  alertType: 'success' | 'error' = 'success';
  private alertTimeoutId?: ReturnType<typeof setTimeout>;

  confirmation: ConfirmationState = {
    open: false,
    title: '',
    message: '',
    confirmLabel: 'Confirm'
  };

  private subs: Subscription[] = [];
  private readonly passwordValidators = [
    Validators.required,
    Validators.minLength(8),
    Validators.maxLength(50),
    Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).*$/)
  ];


  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private groupService: GroupService,
    private roleService: RoleService,
    private departmentService: DepartmentService,
    private permissionService: PermissionService,
    private router: Router
  ) {
    this.filterForm = this.fb.group({
      search: [''],
      status: ['all'],
      role: ['all'],
      group: ['all']
    });

    this.groupFilterForm = this.fb.group({
      search: [''],
      status: ['all']
    });

    this.roleFilterForm = this.fb.group({
      search: [''],
      status: ['all']
    });

    this.userForm = this.fb.group({
      userId: [''],
      fullName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      departmentId: ['', Validators.required],
      password: [''],
      roles: [[] as string[]],
      groups: [[] as string[]],
      isActive: [true]
    });

    this.groupForm = this.fb.group({
      groupId: [''],
      name: ['', Validators.required],
      description: [''],
      status: ['Active', Validators.required]
    });

    this.roleForm = this.fb.group({
      roleId: [''],
      name: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(100)]],
      description: ['', [Validators.maxLength(500)]],
      isActive: [true]
    });
  }

  ngOnInit(): void {
    this.registerFilterListeners();
    this.bootstrap();
  }

  ngOnDestroy(): void {
    this.subs.forEach(sub => sub.unsubscribe());
    if (this.alertTimeoutId) {
      clearTimeout(this.alertTimeoutId);
    }
  }

  get totalUsers(): number {
    return this.totalCount;
  }
  
  goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.loadUsers();
      window.scrollTo({ top: 0, behavior: 'smooth' });
    }
  }

  changePageSize(size: number): void {
    this.pageSize = size;
    this.currentPage = 1;
    this.loadUsers();
  }

  getPageNumbers(): number[] {
    const pages: number[] = [];
    const maxPagesToShow = 7;
    let startPage = Math.max(1, this.currentPage - Math.floor(maxPagesToShow / 2));
    let endPage = Math.min(this.totalPages, startPage + maxPagesToShow - 1);
    
    if (endPage - startPage < maxPagesToShow - 1) {
      startPage = Math.max(1, endPage - maxPagesToShow + 1);
    }
    
    for (let i = startPage; i <= endPage; i++) {
      pages.push(i);
    }
    return pages;
  }

  get activeUsers(): number {
    return this.users.filter(u => u.isActive).length;
  }

  get totalGroups(): number {
    return this.groups.length;
  }

  get totalRoles(): number {
    return this.roles.length;
  }

  openUserModal(mode: ModalMode, user?: UserDto): void {
    this.modalMode = mode;
    this.userModalOpen = true;
    this.showTempPassword = false;

    const passwordControl = this.userForm.get('password');
    if (mode === 'create') {
      passwordControl?.setValidators(this.passwordValidators);
      this.userForm.reset({
        userId: '',
        fullName: '',
        email: '',
        departmentId: '',
        password: '',
        roles: [],
        groups: [],
        isActive: true
      });
    } else if (user) {
      passwordControl?.clearValidators();
      this.userForm.patchValue({
        userId: user.userId,
        fullName: user.fullName || '',
        email: user.email,
        departmentId: user.departmentId || '',
        password: '',
        roles: this.mapRoleNamesToIds(user.directRoles || []),
        groups: (user.groups || []).map(g => g.groupId),
        isActive: user.isActive
      });
    }
    passwordControl?.updateValueAndValidity({ emitEvent: false });
  }

  openGroupModal(mode: ModalMode, group?: GroupDto): void {
    this.modalMode = mode;
    this.groupModalOpen = true;

    if (mode === 'create') {
      this.groupForm.reset({
        groupId: '',
        name: '',
        description: '',
        status: 'Active'
      });
    } else if (group) {
      this.groupForm.patchValue({
        groupId: group.groupId,
        name: group.groupName,
        description: group.description,
        status: group.isActive ? 'Active' : 'Archived'
      });
    }
  }

  openRoleModal(mode: ModalMode, role?: RoleDto): void {
    this.modalMode = mode;
    this.roleModalOpen = true;

    if (mode === 'create') {
      this.roleForm.reset({
        roleId: '',
        name: '',
        description: '',
        isActive: true
      });
    } else if (role) {
      this.roleForm.patchValue({
        roleId: role.roleId,
        name: role.roleName,
        description: role.description,
        isActive: role.isActive
      });
    }
  }

  saveUser(): void {
    if (this.userForm.invalid) {
      this.userForm.markAllAsTouched();
      return;
    }

    if (this.modalMode === 'create') {
      this.createUser();
    } else {
      this.updateUser();
    }
  }

  saveGroup(): void {
    if (this.groupForm.invalid) {
      this.groupForm.markAllAsTouched();
      return;
    }

    if (this.modalMode === 'create') {
      this.createGroup();
    } else {
      this.updateGroup();
    }
  }

  saveRole(): void {
    if (this.roleForm.invalid) {
      this.roleForm.markAllAsTouched();
      return;
    }

    if (this.modalMode === 'create') {
      this.createRole();
    } else {
      this.updateRole();
    }
  }

  requestUserDeletion(user: UserDto): void {
    this.openConfirmation({
      title: 'Delete user',
      message: `Are you sure you want to delete ${user.fullName || user.email}?`,
      confirmLabel: 'Delete user',
      action: () => {
        this.userService.deleteUser(user.userId).subscribe({
          next: () => {
            this.notify('User deleted.');
            this.loadUsers();
            this.showAlert('User deleted successfully.', 'success');
          },
          error: (err: any) => this.handleError(err)
        });
      }
    });
  }

  requestGroupDeletion(group: GroupDto): void {
    this.openConfirmation({
      title: 'Delete group',
      message: `Delete ${group.groupName}? Members will keep their direct roles.`,
      confirmLabel: 'Delete group',
      action: () => {
        this.groupService.deleteGroup(group.groupId).subscribe({
          next: () => {
            this.notify('Group deleted.');
            this.loadGroups();
            this.loadUsers();
            this.showAlert('Group deleted successfully.', 'success');
          },
          error: (err: any) => this.handleError(err)
        });
      }
    });
  }

  requestRoleDeletion(role: RoleDto): void {
    this.openConfirmation({
      title: 'Delete role',
      message: `Are you sure you want to delete ${role.roleName}?`,
      confirmLabel: 'Delete role',
      action: () => {
        this.roleService.deleteRole(role.roleId).subscribe({
          next: () => {
            this.notify('Role deleted.');
            this.loadRoles();
            this.loadUsers();
            this.showAlert('Role deleted successfully.', 'success');
          },
          error: (err: any) => this.handleError(err)
        });
      }
    });
  }

  requestUserStatusToggle(user: UserDto): void {
    const nextState = !user.isActive;
    this.openConfirmation({
      title: nextState ? 'Activate user' : 'Deactivate user',
      message: nextState
        ? `${user.fullName || user.email} will regain platform access immediately.`
        : `${user.fullName || user.email} will be signed out and lose access to all projects.`,
      confirmLabel: nextState ? 'Activate' : 'Deactivate',
      action: () => {
        const payload: UpdateUserRequest = {
          userId: user.userId,
          email: user.email,
          fullName: user.fullName,
          departmentId: user.departmentId,
          isActive: nextState
        };

        this.userService.updateUser(payload).subscribe({
          next: () => {
            this.notify(`User ${nextState ? 'activated' : 'deactivated'}.`);
            this.loadUsers();
          },
          error: (err: any) => this.handleError(err)
        });
      }
    });
  }

  get passwordControl(): AbstractControl | null {
    return this.userForm.get('password');
  }

  toggleTempPasswordVisibility(): void {
    this.showTempPassword = !this.showTempPassword;
  }

  getDepartmentName(departmentId?: string): string {
    if (!departmentId) return '—';
    return this.departments.find(d => d.departmentId === departmentId)?.departmentName || '—';
  }

  getGroupMembershipCount(groupId: string): number {
    return this.groupMemberCounts.get(groupId) ?? 0;
  }

  trackById(_: number, item: { roleId?: string; groupId?: string; userId?: string }): string | undefined {
    return item.roleId || item.groupId || item.userId;
  }

  closeConfirmation(): void {
    this.confirmation = { open: false, title: '', message: '', confirmLabel: 'Confirm' };
  }

  confirmAction(): void {
    if (this.confirmation.action) {
      this.confirmation.action();
    }
    this.closeConfirmation();
  }

  private showAlert(message: string, type: 'success' | 'error' = 'success'): void {
    this.alertMessage = message;
    this.alertType = type;
    if (this.alertTimeoutId) {
      clearTimeout(this.alertTimeoutId);
    }
    this.alertTimeoutId = setTimeout(() => {
      this.alertMessage = '';
    }, 4000);
  }

  private bootstrap(): void {
    this.loadDepartments();
    this.loadRoles();
    this.loadGroups();
    this.loadUsers();
  }

  private registerFilterListeners(): void {
    this.subs.push(
      this.filterForm.valueChanges
        .pipe(debounceTime(200), distinctUntilChanged((a, b) => JSON.stringify(a) === JSON.stringify(b)))
        .subscribe(() => this.applyUserFilters()),
      this.groupFilterForm.valueChanges
        .pipe(debounceTime(200), distinctUntilChanged((a, b) => JSON.stringify(a) === JSON.stringify(b)))
        .subscribe(() => this.applyGroupFilters()),
      this.roleFilterForm.valueChanges
        .pipe(debounceTime(200), distinctUntilChanged((a, b) => JSON.stringify(a) === JSON.stringify(b)))
        .subscribe(() => this.applyRoleFilters())
    );
  }

  private loadUsers(): void {
    this.isLoadingUsers = true;
    this.usersError = '';
    console.log('🔄 UserManagementComponent.loadUsers - Loading users, page:', this.currentPage, 'size:', this.pageSize);
    
    this.userService.getUsers(this.currentPage, this.pageSize).pipe(
      finalize(() => {
        this.isLoadingUsers = false;
        console.log('✅ UserManagementComponent.loadUsers - Loading complete');
      }),
      catchError(err => {
        console.error('❌ UserManagementComponent.loadUsers - Error:', err);
        this.usersError = this.extractError(err);
        return of({ items: [], totalCount: 0, pageNumber: 1, pageSize: 25, totalPages: 0 });
      })
    ).subscribe(response => {
      console.log('📦 UserManagementComponent.loadUsers - Response received:', response);
      console.log('📦 UserManagementComponent.loadUsers - Items count:', response.items?.length || 0);
      
      this.users = response.items || [];
      this.totalCount = response.totalCount || 0;
      this.totalPages = response.totalPages || 0;
      this.currentPage = response.pageNumber || this.currentPage;
      this.pageSize = response.pageSize || this.pageSize;
      
      console.log('📦 UserManagementComponent.loadUsers - State updated:', {
        usersCount: this.users.length,
        totalCount: this.totalCount,
        totalPages: this.totalPages,
        currentPage: this.currentPage
      });
      
      this.updateGroupMembershipCounts();
      this.applyUserFilters();
    });
  }

  private loadGroups(): void {
    this.isLoadingGroups = true;
    this.groupsError = '';
    this.groupService.getGroups().pipe(
      finalize(() => (this.isLoadingGroups = false)),
      catchError((err: any) => {
        this.groupsError = this.extractError(err);
        return of([]);
      })
    ).subscribe((groups: GroupDto[]) => {
      this.groups = groups;
      this.applyGroupFilters();
    });
  }

  private loadRoles(): void {
    this.isLoadingRoles = true;
    this.rolesError = '';
    this.roleService.getRoles().pipe(
      finalize(() => (this.isLoadingRoles = false)),
      catchError((err: any) => {
        this.rolesError = this.extractError(err);
        return of([]);
      })
    ).subscribe((roles: RoleDto[]) => {
      this.roles = roles;
      this.applyRoleFilters();
    });
  }

  private loadDepartments(): void {
    this.isLoadingDepartments = true;
    this.departmentService.getDepartments().pipe(
      finalize(() => (this.isLoadingDepartments = false)),
      catchError(() => of([]))
    ).subscribe(departments => (this.departments = departments));
  }

  private applyUserFilters(): void {
    // Note: Since we're using server-side pagination, filtering should ideally be done on the server
    // For now, we'll filter the current page's results client-side
    const { search, status, role, group } = this.filterForm.value;
    const term = (search || '').toLowerCase().trim();

    this.filteredUsers = this.users.filter(user => {
      const matchesSearch =
        !term ||
        (user.fullName || '').toLowerCase().includes(term) ||
        user.email.toLowerCase().includes(term) ||
        (user.department || '').toLowerCase().includes(term);

      const matchesStatus =
        status === 'all' ||
        (status === 'active' && user.isActive) ||
        (status === 'inactive' && !user.isActive);

      const matchesRole =
        role === 'all' || this.userHasRoleId(user, role);

      const matchesGroup =
        group === 'all' || (user.groups || []).some(g => g.groupId === group);

      return matchesSearch && matchesStatus && matchesRole && matchesGroup;
    });
    
    // If filters are applied, reset to page 1
    if (term || status !== 'all' || role !== 'all' || group !== 'all') {
      if (this.currentPage !== 1) {
        this.currentPage = 1;
        this.loadUsers();
      }
    }
  }

  private applyGroupFilters(): void {
    const { search, status } = this.groupFilterForm.value;
    const term = (search || '').toLowerCase().trim();

    this.filteredGroups = this.groups.filter(group => {
      const matchesSearch =
        !term ||
        group.groupName.toLowerCase().includes(term) ||
        (group.description || '').toLowerCase().includes(term);

      const matchesStatus =
        status === 'all' ||
        (status === 'Active' && group.isActive) ||
        (status === 'Archived' && !group.isActive);

      return matchesSearch && matchesStatus;
    });
  }

  private applyRoleFilters(): void {
    const { search, status } = this.roleFilterForm.value;
    const term = (search || '').toLowerCase().trim();

    this.filteredRoles = this.roles.filter(role => {
      const matchesSearch =
        !term ||
        role.roleName.toLowerCase().includes(term) ||
        (role.description || '').toLowerCase().includes(term);

      const matchesStatus =
        status === 'all' ||
        (status === 'active' && role.isActive) ||
        (status === 'inactive' && !role.isActive);

      return matchesSearch && matchesStatus;
    });
  }

  private createUser(): void {
    if (this.userForm.invalid) {
      this.userForm.markAllAsTouched();
      this.notify('Please fill in all required fields correctly.', 'warning');
      return;
    }

    const { email, password, fullName, departmentId, roles, groups, isActive } = this.userForm.value;
    if (!password && this.modalMode === 'create') {
      this.notify('Password is required to create a new user.', 'warning');
      this.userForm.get('password')?.markAsTouched();
      return;
    }

    const payload: CreateUserRequest = {
      email,
      password,
      fullName,
      departmentId,
      isActive
    };

    this.savingUser = true;
    this.userService.createUser(payload).pipe(
      switchMap(created => this.syncUserAssignments(created.userId, roles || [], groups || [])),
      finalize(() => (this.savingUser = false))
    ).subscribe({
      next: () => {
        this.notify('User created successfully.');
        this.showAlert('User created successfully.', 'success');
        this.userModalOpen = false;
        this.loadUsers();
      },
      error: err => {
        this.handleBackendValidationErrors(err);
        const message = this.extractError(err);
        this.showAlert(message, 'error');
        this.handleError(err);
      }
    });
  }

  private updateUser(): void {
    const { userId, fullName, email, departmentId, roles, groups, isActive } = this.userForm.value;
    const payload: UpdateUserRequest = {
      userId,
      email,
      fullName,
      departmentId,
      isActive
    };

    this.savingUser = true;
    this.userService.updateUser(payload).pipe(
      switchMap(() => this.syncUserAssignments(userId, roles || [], groups || [])),
      finalize(() => (this.savingUser = false))
    ).subscribe({
      next: () => {
        this.notify('User updated successfully.');
        this.userModalOpen = false;
        this.loadUsers();
      },
      error: (err: any) => this.handleError(err)
    });
  }

  private createGroup(): void {
    if (this.groupForm.invalid) {
      this.groupForm.markAllAsTouched();
      this.notify('Please fill in all required fields correctly.', 'warning');
      return;
    }

    const { name, description } = this.groupForm.value;
    this.savingGroup = true;
    this.groupService.createGroup({ groupName: name, description }).pipe(
      finalize(() => (this.savingGroup = false))
    ).subscribe({
      next: () => {
        this.notify('Group created successfully.');
        this.showAlert('Group created successfully.', 'success');
        this.groupModalOpen = false;
        this.loadGroups();
      },
      error: (err: any) => {
        this.handleBackendValidationErrors(err, this.groupForm);
        const message = this.extractError(err);
        this.showAlert(message, 'error');
        this.handleError(err);
      }
    });
  }

  private updateGroup(): void {
    if (this.groupForm.invalid) {
      this.groupForm.markAllAsTouched();
      this.notify('Please fill in all required fields correctly.', 'warning');
      return;
    }

    const { groupId, name, description, status } = this.groupForm.value;
    this.savingGroup = true;
    this.groupService.updateGroup({
      groupId,
      groupName: name,
      description,
      isActive: status === 'Active'
    }).pipe(
      finalize(() => (this.savingGroup = false))
    ).subscribe({
      next: () => {
        this.notify('Group updated successfully.');
        this.showAlert('Group updated successfully.', 'success');
        this.groupModalOpen = false;
        this.loadGroups();
      },
      error: (err: any) => {
        this.handleBackendValidationErrors(err, this.groupForm);
        const message = this.extractError(err);
        this.showAlert(message, 'error');
        this.handleError(err);
      }
    });
  }

  private createRole(): void {
    const { name, description, isActive } = this.roleForm.value;
    this.savingRole = true;
    this.roleService.createRole({ roleName: name, description }).pipe(
      switchMap((role: RoleDto) => this.roleService.updateRole({
        roleId: role.roleId,
        roleName: name,
        description,
        isActive
      })),
      finalize(() => (this.savingRole = false))
    ).subscribe({
      next: () => {
        this.notify('Role created.');
        this.roleModalOpen = false;
        this.loadRoles();
        this.loadUsers();
      },
      error: (err: any) => this.handleError(err)
    });
  }

  private updateRole(): void {
    const { roleId, name, description, isActive } = this.roleForm.value;
    this.savingRole = true;
    this.roleService.updateRole({
      roleId,
      roleName: name,
      description,
      isActive
    }).pipe(
      finalize(() => (this.savingRole = false))
    ).subscribe({
      next: () => {
        this.notify('Role updated.');
        this.roleModalOpen = false;
        this.loadRoles();
        this.loadUsers();
      },
      error: (err: any) => this.handleError(err)
    });
  }

  private afterGroupPersisted(payload: {
    groupId: string;
    name: string;
    description?: string;
    isActive: boolean;
    roleIds: string[];
    memberIds: string[];
  }): Observable<void> {
    const operations = [
      this.groupService.updateGroup({
        groupId: payload.groupId,
        groupName: payload.name,
        description: payload.description,
        isActive: payload.isActive
      }),
      this.groupService.assignRolesToGroup(payload.groupId, payload.roleIds || []),
      this.updateGroupMembers(payload.groupId, payload.memberIds || [])
    ];

    return forkJoin(operations).pipe(map(() => void 0));
  }

  private updateGroupMembers(groupId: string, targetUserIds: string[]): Observable<void> {
    const tasks: ReturnType<typeof this.userService.assignUserToGroups>[] = [];

    this.users.forEach(user => {
      const currentGroupIds = (user.groups || []).map(g => g.groupId);
      const shouldHave = targetUserIds.includes(user.userId);
      const currentlyHas = currentGroupIds.includes(groupId);

      if (shouldHave && !currentlyHas) {
        tasks.push(this.userService.assignUserToGroups(user.userId, [...currentGroupIds, groupId]));
      } else if (!shouldHave && currentlyHas) {
        tasks.push(this.userService.assignUserToGroups(user.userId, currentGroupIds.filter(id => id !== groupId)));
      }
    });

    return tasks.length ? forkJoin(tasks).pipe(map(() => void 0)) : of(void 0);
  }

  private syncUserAssignments(userId: string, roleIds: string[], groupIds: string[]): Observable<void> {
    const operations = [
      this.userService.assignRolesToUser(userId, roleIds || []),
      this.userService.assignUserToGroups(userId, groupIds || [])
    ];

    return forkJoin(operations).pipe(map(() => void 0));
  }

  private updateGroupMembershipCounts(): void {
    const counts = new Map<string, number>();
    this.users.forEach(user => {
      (user.groups || []).forEach(group => {
        counts.set(group.groupId, (counts.get(group.groupId) || 0) + 1);
      });
    });
    this.groupMemberCounts = counts;
  }

  private getMemberIdsForGroup(groupId: string): string[] {
    return this.users
      .filter(user => (user.groups || []).some(group => group.groupId === groupId))
      .map(user => user.userId);
  }

  private userHasRoleId(user: UserDto, roleId: string): boolean {
    const role = this.roles.find(r => r.roleId === roleId);
    if (!role) return false;
    return (user.directRoles || []).some(name => name?.toLowerCase() === role.roleName.toLowerCase());
  }

  countUsersWithRole(roleId: string): number {
    return this.users.filter(user => this.userHasRoleId(user, roleId)).length;
  }

  countGroupsWithRole(roleId: string): number {
    return this.groups.filter(group => group.roles?.some((role: RoleDto) => role.roleId === roleId)).length;
  }

  private mapRoleNamesToIds(roleNames: string[]): string[] {
    if (!roleNames?.length) {
      return [];
    }

    return roleNames
      .map(name => this.roles.find(role => role.roleName.toLowerCase() === name.toLowerCase())?.roleId)
      .filter((id): id is string => !!id);
  }

  private openConfirmation(config: Partial<ConfirmationState>): void {
    this.confirmation = {
      open: true,
      title: config.title || 'Confirm action',
      message: config.message || 'Are you sure?',
      confirmLabel: config.confirmLabel || 'Confirm',
      action: config.action
    };
  }

  private notify(message: string, type: 'success' | 'warning' | 'error' = 'success'): void {
    document.dispatchEvent(
      new CustomEvent('app-toast', {
        detail: { message, type }
      })
    );
  }

  private handleError(error: any): void {
    this.notify(this.extractError(error), 'error');
    this.savingUser = false;
    this.savingGroup = false;
    this.savingRole = false;
  }

  private handleBackendValidationErrors(error: any, form?: FormGroup): void {
    if (!error?.error) return;

    const errorData = error.error;
    const targetForm = form || this.userForm;
    
    // Handle FluentValidation errors (typically in errors property)
    if (errorData.errors) {
      const validationErrors = errorData.errors;
      Object.keys(validationErrors).forEach(key => {
        const control = targetForm.get(key.toLowerCase());
        if (control) {
          const errorMessage = Array.isArray(validationErrors[key]) 
            ? validationErrors[key][0] 
            : validationErrors[key];
          control.setErrors({ backend: errorMessage });
          control.markAsTouched();
        }
      });
    }

    // Handle single error message for specific fields
    const errorMessage = errorData.message || errorData.error?.message;
    if (errorMessage) {
      // Try to match common field names in error messages
      const lowerMessage = errorMessage.toLowerCase();
      if (lowerMessage.includes('email')) {
        const emailControl = this.userForm.get('email');
        if (emailControl) {
          emailControl.setErrors({ backend: errorMessage });
          emailControl.markAsTouched();
        }
      } else if (lowerMessage.includes('password')) {
        const passwordControl = this.userForm.get('password');
        if (passwordControl) {
          passwordControl.setErrors({ backend: errorMessage });
          passwordControl.markAsTouched();
        }
      } else if (lowerMessage.includes('full name') || lowerMessage.includes('fullname')) {
        const fullNameControl = this.userForm.get('fullName');
        if (fullNameControl) {
          fullNameControl.setErrors({ backend: errorMessage });
          fullNameControl.markAsTouched();
        }
      } else if (lowerMessage.includes('department')) {
        const departmentControl = this.userForm.get('departmentId');
        if (departmentControl) {
          departmentControl.setErrors({ backend: errorMessage });
          departmentControl.markAsTouched();
        }
      } else if (lowerMessage.includes('group name') || lowerMessage.includes('groupname')) {
        const nameControl = this.groupForm.get('name');
        if (nameControl) {
          nameControl.setErrors({ backend: errorMessage });
          nameControl.markAsTouched();
        }
      }
    }
  }

  private extractError(error: any): string {
    if (!error) return 'An unexpected error occurred.';
    return error?.error?.message || error?.message || 'An unexpected error occurred.';
  }

  // Audit Logs Methods
  openAuditLogsModal(user: UserDto): void {
    this.router.navigate(['/admin/users', user.userId, 'audit-logs']);
  }

  // ================ Permissions Methods ================

  loadPermissions(): void {
    this.isLoadingPermissions = true;
    this.permissionsError = '';

    forkJoin({
      groups: this.permissionService.getPermissionsGrouped(),
      matrix: this.permissionService.getPermissionMatrix(),
      all: this.permissionService.getAllPermissions()
    }).pipe(
      finalize(() => this.isLoadingPermissions = false),
      catchError(err => {
        this.permissionsError = this.extractError(err);
        return of({ groups: [], matrix: [], all: [] });
      })
    ).subscribe(result => {
      this.permissionGroups = result.groups;
      this.permissionMatrix = result.matrix;
      this.allPermissions = result.all;
    });
  }

  openPermissionMatrixModal(role: RoleDto): void {
    this.selectedRoleForPermissions = role;
    this.permissionMatrixModalOpen = true;
    
    // Get current permissions for this role
    this.permissionService.getRolePermissions(role.roleId).subscribe(permissions => {
      this.selectedPermissionIds = new Set(permissions.map(p => p.permissionId));
    });
  }

  closePermissionMatrixModal(): void {
    this.permissionMatrixModalOpen = false;
    this.selectedRoleForPermissions = null;
    this.selectedPermissionIds = new Set();
  }

  togglePermission(permissionId: string): void {
    if (this.selectedPermissionIds.has(permissionId)) {
      this.selectedPermissionIds.delete(permissionId);
    } else {
      this.selectedPermissionIds.add(permissionId);
    }
  }

  isPermissionSelected(permissionId: string): boolean {
    return this.selectedPermissionIds.has(permissionId);
  }

  toggleAllPermissionsInGroup(group: PermissionGroupDto, select: boolean): void {
    group.permissions.forEach(p => {
      if (select) {
        this.selectedPermissionIds.add(p.permissionId);
      } else {
        this.selectedPermissionIds.delete(p.permissionId);
      }
    });
  }

  areAllPermissionsInGroupSelected(group: PermissionGroupDto): boolean {
    return group.permissions.every(p => this.selectedPermissionIds.has(p.permissionId));
  }

  areSomePermissionsInGroupSelected(group: PermissionGroupDto): boolean {
    const selected = group.permissions.filter(p => this.selectedPermissionIds.has(p.permissionId));
    return selected.length > 0 && selected.length < group.permissions.length;
  }

  saveRolePermissions(): void {
    if (!this.selectedRoleForPermissions) return;

    this.savingPermissions = true;
    const permissionIds = Array.from(this.selectedPermissionIds);

    this.permissionService.assignPermissionsToRole(
      this.selectedRoleForPermissions.roleId,
      permissionIds
    ).pipe(
      finalize(() => this.savingPermissions = false)
    ).subscribe({
      next: () => {
        this.notify('Permissions updated successfully.');
        this.showAlert('Permissions updated successfully.', 'success');
        this.closePermissionMatrixModal();
        this.loadPermissions();
      },
      error: (err: any) => {
        this.handleError(err);
        this.showAlert('Failed to update permissions.', 'error');
      }
    });
  }

  getRolePermissionCount(roleId: string): number {
    const roleMatrix = this.permissionMatrix.find(m => m.roleId === roleId);
    return roleMatrix?.permissionKeys?.length || 0;
  }

  hasRolePermission(roleId: string, permissionKey: string): boolean {
    const roleMatrix = this.permissionMatrix.find(m => m.roleId === roleId);
    return roleMatrix?.permissionKeys?.includes(permissionKey) || false;
  }

  setTab(tab: UserManagementTab): void {
    this.activeTab = tab;
    if (tab === 'permissions' && this.permissionGroups.length === 0) {
      this.loadPermissions();
    }
  }
}

