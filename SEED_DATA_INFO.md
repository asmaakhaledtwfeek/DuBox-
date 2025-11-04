# Group AMANA - DuBox Seed Data Information

## Default Login Credentials

**Default Password for All Users:** `AMANA@2024`

---

## Users Created (19 Users)

### 1. System Administrator
- **Email:** admin@groupamana.com
- **Name:** System Administrator
- **Department:** IT
- **Group:** Management
- **Roles:** SystemAdmin (via group) + ProjectManager (via group)

### 2. Management Team (2 Users)
- **Ahmed Al Mazrouei** - ahmed.almazrouei@groupamana.com
  - Department: Management
  - Group: Management
  - Roles: SystemAdmin, ProjectManager (via group) + CostEstimator (direct)

- **Sara Al Khan** - sara.alkhan@groupamana.com
  - Department: Management
  - Group: Management
  - Roles: SystemAdmin, ProjectManager (via group)

### 3. Engineering Team (3 Users)
- **Mohammed Hassan** - mohammed.hassan@groupamana.com
  - Department: Engineering
  - Group: Engineering
  - Roles: SiteEngineer, DesignEngineer, Viewer (via group) + ProjectManager (direct)

- **Fatima Al Ali** - fatima.alali@groupamana.com
  - Department: Engineering
  - Group: Engineering
  - Roles: SiteEngineer, DesignEngineer, Viewer (via group)

- **Khalid Omar** - khalid.omar@groupamana.com
  - Department: Engineering
  - Group: Engineering
  - Roles: SiteEngineer, DesignEngineer, Viewer (via group)

### 4. Construction Team (3 Users)
- **Ali Mohammed** - ali.mohammed@groupamana.com
  - Department: Construction
  - Group: Construction
  - Roles: Foreman, Viewer (via group)

- **Omar Saleh** - omar.saleh@groupamana.com
  - Department: Construction
  - Group: Construction
  - Roles: Foreman, Viewer (via group)

- **Youssef Ahmed** - youssef.ahmed@groupamana.com
  - Department: Construction
  - Group: Construction
  - Roles: Foreman, Viewer (via group)

### 5. Quality Control Team (2 Users)
- **Layla Ibrahim** - layla.ibrahim@groupamana.com
  - Department: Quality
  - Group: QualityControl
  - Roles: QCInspector, Viewer (via group)

- **Hamza Khalil** - hamza.khalil@groupamana.com
  - Department: Quality
  - Group: QualityControl
  - Roles: QCInspector, Viewer (via group)

### 6. Procurement Team (2 Users)
- **Noor Al Hassan** - noor.alhassan@groupamana.com
  - Department: Procurement
  - Group: Procurement
  - Roles: ProcurementOfficer, Viewer (via group)

- **Zaid Mansour** - zaid.mansour@groupamana.com
  - Department: Procurement
  - Group: Procurement
  - Roles: ProcurementOfficer, Viewer (via group)

### 7. HSE Team (2 Users)
- **Maryam Said** - maryam.Said@groupamana.com
  - Department: HSE
  - Group: HSE
  - Roles: HSEOfficer, Viewer (via group)

- **Tariq Abdullah** - tariq.abdullah@groupamana.com
  - Department: HSE
  - Group: HSE
  - Roles: HSEOfficer, Viewer (via group)

### 8. DuBox Team (2 Users)
- **Rania Khalifa** - rania.khalifa@groupamana.com
  - Department: DuBox
  - Group: DuBoxTeam
  - Roles: DesignEngineer, ProjectManager (via group)

- **Salim Rashid** - salim.rashid@groupamana.com
  - Department: DuBox
  - Group: DuBoxTeam
  - Roles: DesignEngineer, ProjectManager (via group)

### 9. DuPod Team (2 Users)
- **Huda Al Marri** - huda.almarri@groupamana.com
  - Department: DuPod
  - Group: DuPodTeam
  - Roles: DesignEngineer, ProjectManager (via group)

- **Faisal Sultan** - faisal.sultan@groupamana.com
  - Department: DuPod
  - Group: DuPodTeam
  - Roles: DesignEngineer, ProjectManager (via group)

---

## Roles Created (10 Roles)

1. **SystemAdmin** - Full system administration access
2. **ProjectManager** - Manage projects and teams
3. **SiteEngineer** - Oversee construction site activities
4. **Foreman** - Supervise construction workers
5. **QCInspector** - Quality control and inspection
6. **ProcurementOfficer** - Handle material procurement
7. **HSEOfficer** - Health, Safety, and Environment oversight
8. **DesignEngineer** - Design and BIM modeling
9. **CostEstimator** - Cost estimation and budgeting
10. **Viewer** - Read-only access to projects

---

## Groups Created (8 Groups)

1. **Management** 
   - Roles: SystemAdmin, ProjectManager
   - Members: System Admin, Ahmed Al Mazrouei, Sara Al Khan

2. **Engineering**
   - Roles: SiteEngineer, DesignEngineer, Viewer
   - Members: Mohammed Hassan, Fatima Al Ali, Khalid Omar

3. **Construction**
   - Roles: Foreman, Viewer
   - Members: Ali Mohammed, Omar Saleh, Youssef Ahmed

4. **QualityControl**
   - Roles: QCInspector, Viewer
   - Members: Layla Ibrahim, Hamza Khalil

5. **Procurement**
   - Roles: ProcurementOfficer, Viewer
   - Members: Noor Al Hassan, Zaid Mansour

6. **HSE**
   - Roles: HSEOfficer, Viewer
   - Members: Maryam Said, Tariq Abdullah

7. **DuBoxTeam** (Modular Construction)
   - Roles: DesignEngineer, ProjectManager
   - Members: Rania Khalifa, Salim Rashid

8. **DuPodTeam** (Plug-and-Play Modular)
   - Roles: DesignEngineer, ProjectManager
   - Members: Huda Al Marri, Faisal Sultan

---

## How to Login

1. Navigate to: `/api/auth/login`
2. Use any email from the list above
3. Password: `AMANA@2024`
4. You will receive a JWT token
5. Use the token to access protected endpoints

### Example Login Request:
```json
POST /api/auth/login
{
  "email": "admin@groupamana.com",
  "password": "AMANA@2024"
}
```

---

## Group AMANA Business Context

Based on [Group AMANA website](https://www.groupamana.com/):

- **Construct**: Design-Build solutions, Aviation Fueling
- **Manufacture**: DuBox (modular construction), DuPod (plug-and-play modular)
- **Enhance**: Solar energy, Energy-saving solutions, Mustadam (environmental)
- **Develop**: AMANA.X (innovation investments)

The seed data reflects the real organizational structure of Group AMANA's construction and modular building operations, with teams for traditional construction, modular solutions (DuBox & DuPod), quality control, procurement, and HSE compliance.

---

## Testing Role Inheritance

To test that users inherit roles from groups:

```http
GET /api/users/{userId}/roles
```

Example response will show:
- **directRoles**: Roles assigned directly to the user
- **groups**: Groups the user belongs to with their roles
- **allRoles**: Combined list of all roles (direct + inherited)

This demonstrates the RBAC system where users can have both direct role assignments and inherit roles from their group memberships.

