import { Directive, Input, TemplateRef, ViewContainerRef, OnInit, OnDestroy } from '@angular/core';
import { PermissionService } from '../services/permission.service';
import { Subscription } from 'rxjs';

/**
 * Structural directive to conditionally show/hide elements based on user permissions
 * 
 * Usage:
 * <button *hasPermission="'projects.create'">Create Project</button>
 * <button *hasPermission="{ module: 'projects', action: 'edit' }">Edit Project</button>
 * <div *hasPermission="['projects.view', 'boxes.view']" [requireAll]="false">...</div>
 */
@Directive({
  selector: '[hasPermission]',
  standalone: true
})
export class HasPermissionDirective implements OnInit, OnDestroy {
  private permissions: string | { module: string; action: string } | string[] = '';
  private requireAll = true;
  private hasView = false;
  private permissionSubscription?: Subscription;

  constructor(
    private templateRef: TemplateRef<any>,
    private viewContainer: ViewContainerRef,
    private permissionService: PermissionService
  ) {}

  @Input()
  set hasPermission(permissions: string | { module: string; action: string } | string[]) {
    this.permissions = permissions;
    this.updateView();
  }

  @Input()
  set hasPermissionRequireAll(requireAll: boolean) {
    this.requireAll = requireAll;
    this.updateView();
  }

  ngOnInit(): void {
    // Subscribe to permission changes and update view
    this.permissionSubscription = this.permissionService.permissions$.subscribe(() => {
      this.updateView();
    });
  }

  ngOnDestroy(): void {
    this.permissionSubscription?.unsubscribe();
  }

  private updateView(): void {
    const hasPermission = this.checkPermissions();

    if (hasPermission && !this.hasView) {
      this.viewContainer.createEmbeddedView(this.templateRef);
      this.hasView = true;
    } else if (!hasPermission && this.hasView) {
      this.viewContainer.clear();
      this.hasView = false;
    }
  }

  private checkPermissions(): boolean {
    if (!this.permissions) {
      return false;
    }

    // Handle single permission string (e.g., "projects.view")
    if (typeof this.permissions === 'string') {
      const [module, action] = this.permissions.split('.');
      if (!module || !action) {
        console.warn(`Invalid permission format: "${this.permissions}". Expected "module.action"`);
        return false;
      }
      return this.permissionService.hasPermission(module, action);
    }

    // Handle permission object (e.g., { module: 'projects', action: 'view' })
    if (typeof this.permissions === 'object' && !Array.isArray(this.permissions)) {
      const { module, action } = this.permissions;
      if (!module || !action) {
        console.warn('Invalid permission object. Expected { module: string, action: string }');
        return false;
      }
      return this.permissionService.hasPermission(module, action);
    }

    // Handle array of permissions (e.g., ["projects.view", "boxes.view"])
    if (Array.isArray(this.permissions)) {
      if (this.permissions.length === 0) {
        return false;
      }

      const checks = this.permissions.map(perm => {
        const [module, action] = perm.split('.');
        if (!module || !action) {
          console.warn(`Invalid permission format in array: "${perm}". Expected "module.action"`);
          return false;
        }
        return this.permissionService.hasPermission(module, action);
      });

      // If requireAll is true, user must have ALL permissions
      // If requireAll is false, user must have ANY permission
      return this.requireAll ? checks.every(Boolean) : checks.some(Boolean);
    }

    return false;
  }
}

