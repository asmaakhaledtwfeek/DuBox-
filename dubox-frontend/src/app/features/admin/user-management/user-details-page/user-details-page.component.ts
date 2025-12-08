import { Component, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule, Router } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { HeaderComponent } from '../../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../../shared/components/sidebar/sidebar.component';
import { PermissionService } from '../../../../core/services/permission.service';
import { UserService } from '../../../../core/services/user.service';
import type { UserDto, UpdateUserRequest, RoleSummary } from '../../../../core/services/user.service';
import { RoleService, RoleDto } from '../../../../core/services/role.service';
import { GroupService, GroupDto } from '../../../../core/services/group.service';
import { DepartmentService } from '../../../../core/services/department.service';
import { Department } from '../../../../core/models/team.model';
import { forkJoin, of, Subscription } from 'rxjs';
import { catchError, finalize, map, switchMap } from 'rxjs/operators';

@Component({
  selector: 'app-user-details-page',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    ReactiveFormsModule,
    HeaderComponent,
    SidebarComponent
  ],
  templateUrl: './user-details-page.component.html',
  styleUrls: ['./user-details-page.component.scss']
})
export class UserDetailsPageComponent implements OnInit, OnDestroy {
  userForm: FormGroup;
  user?: UserDto;
  roles: RoleDto[] = [];
  groups: GroupDto[] = [];
  departments: Department[] = [];

  loadingUser = true;
  saving = false;
  rolesModalOpen = false;
  groupsModalOpen = false;
  roleSelection: string[] = [];
  groupSelection: string[] = [];
  private pendingRoleNames: string[] = [];

  private subs: Subscription[] = [];

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private userService: UserService,
    private roleService: RoleService,
    private groupService: GroupService,
    private departmentService: DepartmentService,
    private permissionService: PermissionService
  ) {
    this.userForm = this.fb.group({
      userId: [''],
      fullName: ['', Validators.required],
      email: [{ value: '', disabled: true }],
      departmentId: ['', Validators.required],
      roles: [[] as string[]],
      groups: [[] as string[]],
      isActive: [true]
    });
  }

  get selectedRoleNames(): string {
    const ids = this.userForm.get('roles')?.value as string[] || [];
    const names = this.roles
      .filter(role => ids.includes(role.roleId))
      .map(role => role.roleName);
    if (names.length) {
      return names.join(', ');
    }
    if (this.user?.directRoleSummaries?.length) {
      const summaryNames = this.user.directRoleSummaries.map(role => role.roleName).filter(Boolean);
      return summaryNames.length ? summaryNames.join(', ') : 'No roles assigned';
    }
    return 'No roles assigned';
  }

  get selectedGroupNames(): string {
    const ids = this.userForm.get('groups')?.value as string[] || [];
    const names = this.groups
      .filter(group => ids.includes(group.groupId))
      .map(group => group.groupName);
    if (names.length) {
      return names.join(', ');
    }
    if (this.user?.groups?.length) {
      const groupNames = this.user.groups.map(group => group.groupName).filter(Boolean);
      return groupNames.length ? groupNames.join(', ') : 'No groups assigned';
    }
    return 'No groups assigned';
  }

  // Permission getters for template
  get canAssignRoles(): boolean {
    return this.permissionService.hasPermission('users', 'assign-roles');
  }

  get canAssignGroups(): boolean {
    return this.permissionService.hasPermission('users', 'assign-groups');
  }

  openRolesModal(): void {
    this.ensureRolesLoaded(() => {
      const ids = this.user ? this.extractDirectRoleIds(this.user) : (this.userForm.get('roles')?.value || []);
      this.roleSelection = [...ids];
      this.rolesModalOpen = true;
    });
  }

  closeRolesModal(): void {
    this.rolesModalOpen = false;
  }

  openGroupsModal(): void {
    this.ensureGroupsLoaded(() => {
      const ids = this.user ? this.extractGroupIds(this.user) : (this.userForm.get('groups')?.value || []);
      this.groupSelection = [...ids];
      this.groupsModalOpen = true;
    });
  }

  closeGroupsModal(): void {
    this.groupsModalOpen = false;
  }

  onRoleToggle(roleId: string, checked: boolean): void {
    if (checked) {
      if (!this.roleSelection.includes(roleId)) {
        this.roleSelection = [...this.roleSelection, roleId];
      }
    } else {
      this.roleSelection = this.roleSelection.filter(id => id !== roleId);
    }
  }

  onGroupToggle(groupId: string, checked: boolean): void {
    if (checked) {
      if (!this.groupSelection.includes(groupId)) {
        this.groupSelection = [...this.groupSelection, groupId];
      }
    } else {
      this.groupSelection = this.groupSelection.filter(id => id !== groupId);
    }
  }

  applyRoleSelection(): void {
    if (!this.user) {
      this.closeRolesModal();
      return;
    }
    this.saving = true;
    this.userService.assignRolesToUser(this.user.userId, this.roleSelection).pipe(
      finalize(() => (this.saving = false))
    ).subscribe({
      next: () => {
        this.userForm.get('roles')?.setValue([...this.roleSelection], { emitEvent: false });
        this.notify('Roles updated successfully.');
        this.reloadUser();
        this.closeRolesModal();
      },
      error: err => this.handleError(err)
    });
  }

  applyGroupSelection(): void {
    if (!this.user) {
      this.closeGroupsModal();
      return;
    }
    this.saving = true;
    this.userService.assignUserToGroups(this.user.userId, this.groupSelection).pipe(
      finalize(() => (this.saving = false))
    ).subscribe({
      next: () => {
        this.userForm.get('groups')?.setValue([...this.groupSelection], { emitEvent: false });
        this.notify('Groups updated successfully.');
        this.reloadUser();
        this.closeGroupsModal();
      },
      error: err => this.handleError(err)
    });
  }

  ngOnInit(): void {
    this.loadLookups();
    const sub = this.route.paramMap.pipe(
      switchMap(params => {
        const id = params.get('userId');
        if (!id) {
          return of(null);
        }
        this.loadingUser = true;
        return this.userService.getUserById(id).pipe(
          catchError(err => {
            this.handleError(err);
            return of(null);
          }),
          finalize(() => (this.loadingUser = false))
        );
      })
    ).subscribe(user => {
      if (!user) {
        return;
      }
      this.user = user;
      this.patchForm(user);
    });

    this.subs.push(sub);
  }

  ngOnDestroy(): void {
    this.subs.forEach(sub => sub.unsubscribe());
  }

  get userAllRoleSummaries(): RoleSummary[] {
    if (this.user?.allRoleSummaries?.length) {
      return this.user.allRoleSummaries;
    }
    if (!this.user) {
      return [];
    }

    const aggregate: RoleSummary[] = [
      ...(this.user.directRoleSummaries || []),
      ...((this.user.groups || []).flatMap(group => group.roles || []))
    ];

    const unique = new Map<string, RoleSummary>();
    aggregate.forEach(role => {
      const key = role.roleId || role.roleName;
      if (!key) {
        return;
      }
      if (!unique.has(key)) {
        unique.set(key, role);
      }
    });

    return Array.from(unique.values());
  }

  getGroupRoles(groupId: string): string[] {
    const membership = this.user?.groups?.find(g => g.groupId === groupId);
    return membership?.roles?.map(role => role.roleName).filter(Boolean) || [];
  }

  onSave(): void {
    if (this.userForm.invalid || !this.user) {
      this.userForm.markAllAsTouched();
      return;
    }
    const { userId, fullName, departmentId, groups, isActive } = this.userForm.getRawValue();
    const payload: UpdateUserRequest = {
      userId,
      email: this.user.email,
      fullName,
      departmentId,
      isActive
    };

    this.saving = true;
    this.userService.updateUser(payload).pipe(
      finalize(() => (this.saving = false))
    ).subscribe({
      next: () => {
        this.notify('User updated successfully.');
        this.reloadUser();
      },
      error: err => this.handleError(err)
    });
  }

  goBack(): void {
    this.router.navigate(['/admin/users']);
  }

  private loadLookups(): void {
    this.roleService.getRoles().pipe(
      catchError(() => of([]))
    ).subscribe((roles: RoleDto[]) => {
      this.roles = roles;
      this.alignRolesWithPendingNames();
    });

    this.groupService.getGroups().pipe(
      catchError(() => of([]))
    ).subscribe((groups: GroupDto[]) => (this.groups = groups));

    this.departmentService.getDepartments().pipe(
      catchError(() => of([]))
    ).subscribe(depts => (this.departments = depts));
  }

  private patchForm(user: UserDto): void {
    const roleIds = this.extractDirectRoleIds(user);
    const groupIds = this.extractGroupIds(user);
    this.userForm.patchValue({
      userId: user.userId,
      fullName: user.fullName || '',
      email: user.email,
      departmentId: user.departmentId || '',
      roles: roleIds,
      groups: groupIds,
      isActive: user.isActive
    }, { emitEvent: false });
    if (roleIds.length) {
      this.roleSelection = [...roleIds];
      this.pendingRoleNames = [];
    } else {
      this.pendingRoleNames = (user.directRoles || []).map(name => name.toLowerCase());
    }
    this.groupSelection = [...groupIds];
  }

  private reloadUser(): void {
    if (!this.user?.userId) {
      return;
    }
    this.userService.getUserById(this.user.userId).subscribe({
      next: user => {
        this.user = user;
        this.patchForm(user);
      },
      error: err => this.handleError(err)
    });
  }

  private extractDirectRoleIds(user: UserDto): string[] {
    if (user.directRoleIds?.length) {
      return user.directRoleIds.filter(Boolean) as string[];
    }
    if (user.directRoleSummaries?.length) {
      return user.directRoleSummaries.map(role => role.roleId).filter(Boolean) as string[];
    }
    const lookup = user.directRoles?.map(name => name.toLowerCase()) || [];
    if (!lookup.length || !this.roles.length) {
      return [];
    }
    return this.roles
      .filter(role => lookup.includes(role.roleName.toLowerCase()))
      .map(role => role.roleId);
  }

  private extractGroupIds(user: UserDto): string[] {
    return (user.groups || [])
      .map(group => group.groupId)
      .filter((id): id is string => !!id);
  }

  private alignRolesWithPendingNames(): void {
    if (!this.pendingRoleNames.length || !this.roles.length) {
      return;
    }
    const matchedIds = this.roles
      .filter(role => this.pendingRoleNames.includes(role.roleName.toLowerCase()))
      .map(role => role.roleId);
    if (matchedIds.length) {
      this.userForm.get('roles')?.setValue(matchedIds, { emitEvent: false });
      this.roleSelection = [...matchedIds];
      this.pendingRoleNames = [];
    }
  }

  private ensureRolesLoaded(callback?: () => void): void {
    if (this.roles.length) {
      this.alignRolesWithPendingNames();
      callback?.();
      return;
    }
    this.roleService.getRoles().pipe(
      catchError(() => of([]))
    ).subscribe((roles: RoleDto[]) => {
      this.roles = roles;
      this.alignRolesWithPendingNames();
      callback?.();
    });
  }

  private ensureGroupsLoaded(callback?: () => void): void {
    if (this.groups.length) {
      callback?.();
      return;
    }
    this.groupService.getGroups().pipe(
      catchError(() => of([]))
    ).subscribe((groups: GroupDto[]) => {
      this.groups = groups;
      callback?.();
    });
  }

  private notify(message: string, type: 'success' | 'error' | 'warning' = 'success'): void {
    document.dispatchEvent(new CustomEvent('app-toast', {
      detail: { message, type }
    }));
  }

  private handleError(error: any): void {
    const message =
      error?.error?.message ||
      error?.message ||
      'Something went wrong while processing your request.';
    this.notify(message, 'error');
  }
}





