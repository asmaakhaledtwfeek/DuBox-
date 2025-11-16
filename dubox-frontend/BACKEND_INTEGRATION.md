# Backend Integration Guide

## 🚨 Current Issues Fixed (Temporarily)

### Issue 1: Missing Roles in Backend Response ✅ FIXED (Temporary)
**Status:** Frontend now has a temporary workaround that assigns roles based on email

### Issue 2: Response Format Mismatch ✅ FIXED
**Status:** Frontend now transforms backend response to match expected format

---

## 📡 Required Backend Changes

### Login Endpoint: `POST /api/auth/login`

#### Current Backend Response:
```json
{
  "data": {
    "token": "eyJhbGci...",
    "user": {
      "userId": "f1111111-1111-1111-...",
      "email": "admin@groupamana.com",
      "fullName": "System Administrator",
      "departmentId": "d1000000-0000-...",
      "isActive": true,
      "lastLoginDate": "2025-11-16...",
      "createdDate": "2024-11-01..."
    }
  }
}
```

#### ⚠️ Missing Fields:
- ❌ `allRoles` - User's combined roles (direct + inherited)
- ❌ `directRoles` - User's directly assigned roles
- ❌ `groups` - User's group memberships with roles

### Required Backend Response Format:

```json
{
  "data": {
    "token": "eyJhbGci...",
    "refreshToken": "refresh_token_here",
    "expiresIn": 3600,
    "user": {
      "userId": "f1111111-1111-1111-...",
      "email": "admin@groupamana.com",
      "fullName": "System Administrator",
      "departmentId": "d1000000-0000-...",
      "department": "IT",
      "isActive": true,
      "lastLoginDate": "2025-11-16...",
      "createdDate": "2024-11-01...",
      
      // ✅ ADD THESE FIELDS:
      "directRoles": ["SystemAdmin"],
      "groups": [
        {
          "id": "group-id",
          "name": "Management",
          "description": "Management team",
          "roles": ["SystemAdmin", "ProjectManager"]
        }
      ],
      "allRoles": ["SystemAdmin", "ProjectManager"]
    }
  },
  "isSuccess": true,
  "isFailure": false,
  "message": "Success"
}
```

---

## 🔧 Backend Implementation Steps

### Step 1: Update User DTO

Add to your `UserDto` or login response model:

```csharp
public class LoginResponseDto
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public int ExpiresIn { get; set; } = 3600;
    public UserDto User { get; set; }
}

public class UserDto
{
    public string UserId { get; set; }
    public string Email { get; set; }
    public string FullName { get; set; }
    public string DepartmentId { get; set; }
    public string Department { get; set; }
    public bool IsActive { get; set; }
    public DateTime? LastLoginDate { get; set; }
    public DateTime? CreatedDate { get; set; }
    
    // ✅ ADD THESE:
    public List<string> DirectRoles { get; set; }
    public List<UserGroupDto> Groups { get; set; }
    public List<string> AllRoles { get; set; }
}

public class UserGroupDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<string> Roles { get; set; }
}
```

### Step 2: Populate Roles in Login Handler

In your login command handler:

```csharp
public async Task<Result<LoginResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
{
    // ... existing authentication logic ...
    
    var user = await _userRepository.GetByEmailAsync(request.Email);
    
    // Get direct roles assigned to user
    var directRoles = await _userRoleRepository.GetUserDirectRolesAsync(user.Id);
    
    // Get groups and their roles
    var userGroups = await _userGroupRepository.GetUserGroupsWithRolesAsync(user.Id);
    
    // Combine all roles (direct + from groups)
    var allRoles = directRoles
        .Union(userGroups.SelectMany(g => g.Roles))
        .Distinct()
        .ToList();
    
    var response = new LoginResponseDto
    {
        Token = token,
        RefreshToken = refreshToken,
        ExpiresIn = 3600,
        User = new UserDto
        {
            UserId = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            DepartmentId = user.DepartmentId,
            Department = user.Department?.Name,
            IsActive = user.IsActive,
            LastLoginDate = user.LastLoginDate,
            CreatedDate = user.CreatedDate,
            DirectRoles = directRoles.Select(r => r.Name).ToList(),
            Groups = userGroups.Select(g => new UserGroupDto
            {
                Id = g.Id,
                Name = g.Name,
                Description = g.Description,
                Roles = g.Roles.Select(r => r.Name).ToList()
            }).ToList(),
            AllRoles = allRoles.Select(r => r.Name).ToList()
        }
    };
    
    return Result<LoginResponseDto>.Success(response);
}
```

---

## ⚡ Temporary Workaround (Currently Active)

Until you update the backend, the frontend **automatically assigns roles** based on email:

| Email | Assigned Roles |
|-------|---------------|
| admin@groupamana.com | SystemAdmin, ProjectManager |
| ahmed.almazrouei@groupamana.com | SystemAdmin, ProjectManager, CostEstimator |
| mohammed.hassan@groupamana.com | SiteEngineer, DesignEngineer, ProjectManager, Viewer |
| layla.ibrahim@groupamana.com | QCInspector, Viewer |
| rania.khalifa@groupamana.com | DesignEngineer, ProjectManager |
| noor.alhassan@groupamana.com | ProcurementOfficer, Viewer |
| maryam.said@groupamana.com | HSEOfficer, Viewer |
| Other users | Viewer (default) |

**⚠️ Remove this workaround once backend returns roles!**

---

## 🧪 Testing

### Test 1: Login and Check Console
1. Login with: `admin@groupamana.com`
2. Open browser console (F12)
3. Look for: `"Assigned roles for admin@groupamana.com : [...]"`
4. If you see this warning, backend is NOT returning roles

### Test 2: Check Network Tab
1. Open DevTools > Network tab
2. Login
3. Find `POST /api/auth/login` request
4. Check Response:
   - ✅ Should have: `allRoles`, `directRoles`, `groups`
   - ❌ If missing: Backend needs update

### Test 3: Verify Menu Items
1. Login as different users
2. Check sidebar menu items
3. SystemAdmin should see: All menus
4. QCInspector should see: Projects, QC, Reports, Notifications
5. Viewer should see: Projects, Reports, Notifications only

---

## 📋 Backend Checklist

- [ ] Add `DirectRoles` to user response
- [ ] Add `Groups` array with roles
- [ ] Add `AllRoles` (combined direct + group roles)
- [ ] Add `Department` name (not just ID)
- [ ] Add `RefreshToken` to response
- [ ] Test with all user types
- [ ] Verify role inheritance works
- [ ] Remove frontend temporary workaround

---

## 🚀 Once Backend is Fixed

1. Remove temporary role assignment in `auth.service.ts`:
   - Delete the entire `if (!allRoles || allRoles.length === 0)` block
   - Keep only: `const allRoles = backendUser.allRoles || backendUser.roles || [UserRole.Viewer];`

2. Test with real backend roles

3. Verify all permissions work correctly

---

## 💡 Quick Test Command

Test your backend response:

```bash
curl -X POST https://localhost:7098/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "admin@groupamana.com",
    "password": "AMANA@2024"
  }' | jq '.data.user | {allRoles, directRoles, groups}'
```

Expected output:
```json
{
  "allRoles": ["SystemAdmin", "ProjectManager"],
  "directRoles": ["SystemAdmin"],
  "groups": [{ "name": "Management", "roles": [...] }]
}
```

---

## 📞 Questions?

- Check `GROUP_AMANA_ROLES.md` for role definitions
- Check `QUICK_REFERENCE.md` for usage examples
- Review `TROUBLESHOOTING.md` for common issues

---

© 2024 Group AMANA - DuBox Platform

