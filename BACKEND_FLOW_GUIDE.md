# 🔄 Backend Flow - DuBox Inspection Checklist System

## 📊 Complete Backend Architecture Flow

```
┌─────────────────────────────────────────────────────────────────┐
│                        DATABASE LAYER                            │
├─────────────────────────────────────────────────────────────────┤
│  1. References (36 records)                                      │
│  2. Categories (42 records)                                      │
│  3. WIRMasters (6 records - WIR-1 to WIR-6)                     │
│  4. PredefinedChecklistItems (284 records)                      │
│     ├── WIR-1: 30 items                                          │
│     ├── WIR-2: 113 items                                         │
│     ├── WIR-3: 44 items                                          │
│     ├── WIR-4: 28 items                                          │
│     ├── WIR-5: 34 items                                          │
│     └── WIR-6: 35 items                                          │
│                                                                   │
│  Runtime Data (Created per Box):                                 │
│  5. WIRCheckpoints (6 per box)                                   │
│  6. WIRChecklistItems (284 per box, cloned from predefined)     │
│  7. WIRCheckpointImages (optional attachments)                   │
└─────────────────────────────────────────────────────────────────┘
                              ⬇️
┌─────────────────────────────────────────────────────────────────┐
│                      DOMAIN ENTITIES                             │
├─────────────────────────────────────────────────────────────────┤
│  📦 Reference.cs                                                 │
│     ├── ReferenceId (Guid)                                       │
│     ├── ReferenceName (string)                                   │
│     └── CreatedDate (DateTime)                                   │
│                                                                   │
│  📦 Category.cs                                                  │
│     ├── CategoryId (Guid)                                        │
│     ├── CategoryName (string)                                    │
│     └── CreatedDate (DateTime)                                   │
│                                                                   │
│  📦 WIRMaster.cs ⭐ NEW                                          │
│     ├── WIRMasterId (Guid)                                       │
│     ├── WIRNumber (string) - "WIR-1", "WIR-2", etc.            │
│     ├── WIRName (string)                                         │
│     ├── Description (string)                                     │
│     ├── Sequence (int)                                           │
│     ├── Discipline (string) - "Civil", "MEP"                    │
│     ├── Phase (string) - "Pre-Production", "Assembly", etc.     │
│     └── IsActive (bool)                                          │
│                                                                   │
│  📦 PredefinedChecklistItem.cs (Template)                       │
│     ├── PredefinedItemId (Guid)                                 │
│     ├── WIRNumber (string)                                       │
│     ├── ItemNumber (string) - "A1", "A2", "B1"                  │
│     ├── CheckpointDescription (string)                           │
│     ├── CategoryId (Guid FK → Category)                         │
│     ├── ReferenceId (Guid FK → Reference)                       │
│     ├── Sequence (int)                                           │
│     └── IsActive (bool)                                          │
│                                                                   │
│  📦 WIRCheckpoint.cs (Instance per Box)                         │
│     ├── WIRId (Guid)                                             │
│     ├── BoxId (Guid FK → Box)                                    │
│     ├── WIRNumber (string)                                       │
│     ├── WIRName (string)                                         │
│     ├── Status (Enum) - Pending/UnderReview/Approved/Rejected   │
│     ├── RequestedDate, InspectionDate, ApprovedDate             │
│     ├── InspectorName, InspectorRole                             │
│     ├── Comments                                                  │
│     ├── ChecklistItems (Collection)                              │
│     └── Images (Collection)                                      │
│                                                                   │
│  📦 WIRChecklistItem.cs (Instance per Item per Box)             │
│     ├── ChecklistItemId (Guid)                                  │
│     ├── WIRId (Guid FK → WIRCheckpoint)                         │
│     ├── PredefinedItemId (Guid FK → PredefinedChecklistItem)    │
│     ├── CheckpointDescription (cloned)                           │
│     ├── ReferenceDocument (cloned)                               │
│     ├── Status (Enum) - Pending/Pass/Fail                       │
│     ├── Remarks (string)                                         │
│     └── Sequence (int)                                           │
└─────────────────────────────────────────────────────────────────┘
                              ⬇️
┌─────────────────────────────────────────────────────────────────┐
│               APPLICATION LAYER (CQRS Pattern)                   │
├─────────────────────────────────────────────────────────────────┤
│                                                                   │
│  🔵 COMMANDS (Write Operations)                                  │
│  ┌───────────────────────────────────────────────────────────┐  │
│  │ 1. GenerateWIRsForBoxCommand                              │  │
│  │    ├── Input: BoxId (Guid)                                │  │
│  │    ├── Handler: GenerateWIRsForBoxCommandHandler          │  │
│  │    └── Output: List<WIRCheckpointDto>                     │  │
│  │                                                            │  │
│  │    Flow:                                                   │  │
│  │    1️⃣ Validate Box exists                                 │  │
│  │    2️⃣ Check if WIRs already exist (prevent duplicates)    │  │
│  │    3️⃣ Load WIRMasters (6 records)                         │  │
│  │    4️⃣ For each WIRMaster:                                 │  │
│  │       a) Create WIRCheckpoint instance                     │  │
│  │       b) Load PredefinedChecklistItems for this WIR        │  │
│  │       c) Load Categories & References                      │  │
│  │       d) Clone each PredefinedItem → WIRChecklistItem     │  │
│  │       e) Add items to WIRCheckpoint.ChecklistItems         │  │
│  │    5️⃣ Save all WIRCheckpoints to database                 │  │
│  │    6️⃣ Return DTOs with created data                       │  │
│  └───────────────────────────────────────────────────────────┘  │
│                                                                   │
│  🔵 QUERIES (Read Operations)                                    │
│  ┌───────────────────────────────────────────────────────────┐  │
│  │ 2. GetWIRsByBoxWithChecklistQuery                         │  │
│  │    ├── Input: BoxId (Guid)                                │  │
│  │    ├── Handler: GetWIRsByBoxWithChecklistQueryHandler     │  │
│  │    └── Output: List<WIRWithChecklistDto>                  │  │
│  │                                                            │  │
│  │    Flow:                                                   │  │
│  │    1️⃣ Use Specification: GetWIRsWithChecklistByBoxId      │  │
│  │    2️⃣ Load WIRCheckpoints with ChecklistItems (EF Include)│  │
│  │    3️⃣ For each WIR:                                       │  │
│  │       a) Extract PredefinedItemIds from ChecklistItems     │  │
│  │       b) Use Spec: GetPredefinedItemsByCategory            │  │
│  │       c) Load PredefinedItems with Category & Reference    │  │
│  │       d) Group ChecklistItems by Category                  │  │
│  │       e) Create Sections (A, B, C...) from Categories      │  │
│  │       f) Calculate progress (completed/total items)        │  │
│  │    4️⃣ Map to DTOs (WIRWithChecklistDto)                   │  │
│  │    5️⃣ Return grouped, organized data                      │  │
│  └───────────────────────────────────────────────────────────┘  │
│                                                                   │
│  📋 DTOs (Data Transfer Objects)                                 │
│  ┌───────────────────────────────────────────────────────────┐  │
│  │ • WIRCheckpointDto - Basic WIR info                       │  │
│  │ • WIRWithChecklistDto - WIR + grouped checklist           │  │
│  │ • ChecklistSectionDto - Category group (A, B, C...)       │  │
│  │ • ChecklistItemDetailDto - Individual item with status    │  │
│  │ • ReviewWIRRequest - Submit inspection results            │  │
│  └───────────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────────┘
                              ⬇️
┌─────────────────────────────────────────────────────────────────┐
│                  SPECIFICATIONS (Query Logic)                    │
├─────────────────────────────────────────────────────────────────┤
│                                                                   │
│  🔍 GetWIRsWithChecklistByBoxIdSpecification                     │
│      ├── Criteria: WIR.BoxId == {boxId}                         │
│      ├── Include: ChecklistItems                                 │
│      ├── OrderBy: WIRNumber                                      │
│      └── SplitQuery: true (avoids cartesian explosion)           │
│                                                                   │
│  🔍 GetPredefinedItemsByCategorySpecification                    │
│      ├── Criteria: PredefinedItemId IN {list}                   │
│      ├── Include: Category, Reference                            │
│      └── OrderBy: Sequence                                       │
└─────────────────────────────────────────────────────────────────┘
                              ⬇️
┌─────────────────────────────────────────────────────────────────┐
│                    INFRASTRUCTURE LAYER                          │
├─────────────────────────────────────────────────────────────────┤
│                                                                   │
│  🔧 IUnitOfWork Pattern                                          │
│      ├── Repository<T>() - Generic repository access             │
│      ├── CompleteAsync() - Commit transaction                    │
│      └── Dispose() - Clean up                                    │
│                                                                   │
│  🔧 IGenericRepository<T>                                        │
│      ├── FindAsync(expression, cancellationToken)                │
│      ├── GetWithSpec(specification)                              │
│      ├── AddAsync(entity)                                        │
│      └── UpdateAsync(entity)                                     │
│                                                                   │
│  🔧 Specification<T> Base Class                                  │
│      ├── AddCriteria(expression)                                 │
│      ├── AddInclude(navigationProperty)                          │
│      ├── AddOrderBy(expression)                                  │
│      └── SplitQuery(enable)                                      │
└─────────────────────────────────────────────────────────────────┘
                              ⬇️
┌─────────────────────────────────────────────────────────────────┐
│                        API LAYER                                 │
├─────────────────────────────────────────────────────────────────┤
│                                                                   │
│  🌐 WIRCheckPointsController.cs                                  │
│                                                                   │
│  📍 POST /api/WIRCheckPoints/generate-for-box/{boxId}           │
│      ├── Purpose: Auto-generate all 6 WIRs for a box            │
│      ├── Input: boxId (Guid)                                     │
│      ├── Handler: Send(GenerateWIRsForBoxCommand)                │
│      ├── Returns: Result<List<WIRCheckpointDto>>                 │
│      └── Example Response:                                       │
│          {                                                        │
│            "isSuccess": true,                                    │
│            "data": [                                              │
│              {                                                    │
│                "wirId": "guid...",                               │
│                "wirNumber": "WIR-1",                             │
│                "wirName": "Material Receiving",                  │
│                "totalItems": 30                                  │
│              },                                                   │
│              ... (5 more WIRs)                                   │
│            ],                                                     │
│            "message": "Successfully generated 6 WIRs"            │
│          }                                                        │
│                                                                   │
│  📍 GET /api/WIRCheckPoints/box/{boxId}/with-checklist          │
│      ├── Purpose: Get all WIRs with detailed checklists          │
│      ├── Input: boxId (Guid)                                     │
│      ├── Handler: Send(GetWIRsByBoxWithChecklistQuery)           │
│      ├── Returns: Result<List<WIRWithChecklistDto>>              │
│      └── Example Response:                                       │
│          {                                                        │
│            "isSuccess": true,                                    │
│            "data": [                                              │
│              {                                                    │
│                "wirId": "guid...",                               │
│                "wirNumber": "WIR-2",                             │
│                "wirName": "MEP Installation",                    │
│                "status": "Pending",                              │
│                "sections": [                                     │
│                  {                                                │
│                    "sectionLetter": "A",                         │
│                    "sectionName": "HVAC Duct",                   │
│                    "items": [                                    │
│                      {                                            │
│                        "itemNumber": "A1",                       │
│                        "description": "Check materials...",      │
│                        "referenceDocument": "MA",                │
│                        "status": "Pending",                      │
│                        "remarks": ""                             │
│                      },                                           │
│                      ...                                          │
│                    ]                                              │
│                  },                                               │
│                  ...                                              │
│                ],                                                 │
│                "totalItems": 113,                                │
│                "completedItems": 0,                              │
│                "progressPercentage": 0                           │
│              },                                                   │
│              ... (5 more WIRs)                                   │
│            ]                                                      │
│          }                                                        │
│                                                                   │
│  📍 PUT /api/WIRCheckPoints/{wirId}/review                      │
│      ├── Purpose: Submit inspection review (Approve/Reject)      │
│      ├── Input: wirId (Guid), ReviewWIRRequest body             │
│      ├── Body:                                                    │
│      │   {                                                        │
│      │     "status": "Approved" | "Rejected" | "Conditional",   │
│      │     "comment": "All items verified",                      │
│      │     "inspectorRole": "QC Engineer",                       │
│      │     "items": [                                             │
│      │       {                                                    │
│      │         "checklistItemId": "guid...",                     │
│      │         "status": "Pass" | "Fail" | "Pending",           │
│      │         "remarks": "Verified"                             │
│      │       },                                                   │
│      │       ...                                                  │
│      │     ]                                                      │
│      │   }                                                        │
│      └── Returns: Result<WIRCheckpointDto>                       │
└─────────────────────────────────────────────────────────────────┘
```

---

## 🔄 Complete User Flow (Box Lifecycle)

### **Phase 1: Box Creation** 📦

```
1. User creates a Box (via CreateBoxCommand)
   └── Box saved to database
       └── BoxId: 12345678-1234-1234-1234-123456789abc

2. (Optional) Auto-generate WIRs immediately:
   POST /api/WIRCheckPoints/generate-for-box/{boxId}
   
   OR integrate in CreateBoxCommandHandler:
   
   var box = await _unitOfWork.Repository<Box>().AddAsync(boxEntity);
   await _unitOfWork.CompleteAsync();
   
   // Auto-generate WIRs
   var generateCmd = new GenerateWIRsForBoxCommand(box.BoxId);
   await _mediator.Send(generateCmd, cancellationToken);
```

### **Phase 2: WIR Generation (Automatic)** 🏗️

```
GenerateWIRsForBoxCommandHandler.Handle():

1️⃣ Validate Box exists
   └── Query: Box.Find(BoxId)
       └── If not found → Return Error

2️⃣ Check for existing WIRs (prevent duplicates)
   └── Query: WIRCheckpoint.Where(w => w.BoxId == BoxId)
       └── If exists → Return existing WIRs

3️⃣ Load WIRMaster configurations
   └── Query: WIRMaster.Where(w => w.IsActive).OrderBy(Sequence)
       └── Returns: 6 WIRMasters (WIR-1 to WIR-6)

4️⃣ For each WIRMaster:
   
   a) Create WIRCheckpoint instance:
      └── WIRCheckpoint {
            BoxId = boxId,
            WIRNumber = wirMaster.WIRNumber,
            WIRName = wirMaster.WIRName,
            Status = Pending,
            RequestedDate = DateTime.UtcNow,
            ChecklistItems = new List<>()
          }
   
   b) Load PredefinedChecklistItems for this WIR:
      └── Query: PredefinedChecklistItem
                .Where(p => p.WIRNumber == wirMaster.WIRNumber 
                         && p.IsActive)
                .OrderBy(Sequence)
      
      └── For WIR-2: Returns 113 items
      
   c) Load related Categories & References:
      └── Extract CategoryIds from PredefinedItems
      └── Extract ReferenceIds from PredefinedItems
      └── Query Categories: Category.Where(CategoryId IN list)
      └── Query References: Reference.Where(ReferenceId IN list)
   
   d) Clone each PredefinedItem → WIRChecklistItem:
      └── For each predefinedItem:
            var checklistItem = new WIRChecklistItem {
              PredefinedItemId = predefinedItem.Id,
              CheckpointDescription = predefinedItem.Description,
              ReferenceDocument = reference?.ReferenceName,
              Status = Pending,
              Sequence = predefinedItem.Sequence,
              Remarks = ""
            };
            wirCheckpoint.ChecklistItems.Add(checklistItem);
   
   e) Add WIRCheckpoint to database:
      └── Repository.AddAsync(wirCheckpoint)

5️⃣ Save all changes:
   └── UnitOfWork.CompleteAsync()
       └── SQL: INSERT 6 WIRCheckpoints + 284 WIRChecklistItems

6️⃣ Map to DTOs and return:
   └── Mapster: List<WIRCheckpoint>.Adapt<List<WIRCheckpointDto>>()
   └── Return: Result.Success(wirDtos)
```

### **Phase 3: Inspector Loads Form** 👨‍🔧

```
GET /api/WIRCheckPoints/box/{boxId}/with-checklist

GetWIRsByBoxWithChecklistQueryHandler.Handle():

1️⃣ Load WIRCheckpoints with ChecklistItems
   └── Specification: GetWIRsWithChecklistByBoxIdSpecification
       ├── Criteria: BoxId == {boxId}
       ├── Include: ChecklistItems
       ├── OrderBy: WIRNumber
       └── SplitQuery: true
   
   └── Returns: 6 WIRCheckpoints, each with ~47 ChecklistItems

2️⃣ For each WIRCheckpoint:
   
   a) Extract PredefinedItemIds:
      └── var ids = wir.ChecklistItems
                      .Select(ci => ci.PredefinedItemId)
                      .ToList()
   
   b) Load PredefinedItems with Categories:
      └── Specification: GetPredefinedItemsByCategorySpecification
          ├── Criteria: PredefinedItemId IN ids
          ├── Include: Category, Reference
          └── OrderBy: Sequence
      
      └── Returns: PredefinedItems with navigation properties loaded
   
   c) Group ChecklistItems by Category:
      └── Group by: predefinedItem.Category.CategoryName
      
      └── Example for WIR-2:
          Section A: "INSTALLATION OF HVAC DUCT" (14 items)
          Section B: "INSTALLATION OF CHILLED WATER PIPING" (16 items)
          Section C: "INSTALLATION OF HOT WATER PIPING" (11 items)
          ... (9 sections total)
   
   d) Create ChecklistSectionDto for each group:
      └── ChecklistSectionDto {
            SectionLetter = "A", "B", "C"...,
            SectionName = category.CategoryName,
            Items = [ChecklistItemDetailDto...]
          }
   
   e) Calculate progress:
      └── totalItems = wir.ChecklistItems.Count
      └── completedItems = items.Count(status == Pass || Fail)
      └── progressPercentage = (completed / total) * 100

3️⃣ Map to WIRWithChecklistDto:
   └── WIRWithChecklistDto {
         WIRId, WIRNumber, WIRName,
         Status, Dates, Inspector info,
         Sections = [ChecklistSectionDto...],
         TotalItems, CompletedItems, ProgressPercentage
       }

4️⃣ Return organized data:
   └── Return: Result.Success(List<WIRWithChecklistDto>)
```

### **Phase 4: Inspector Reviews Items** ✅

```
Frontend updates item statuses:

For each checklist item:
  - User selects: Y (Pass) / N (Fail) / N/A (Pending)
  - User enters remarks (optional)
  
Item status updated in local state:
  item.status = 'Pass'
  item.remarks = 'Verified - meets specifications'
```

### **Phase 5: Submit Inspection** 📋

```
PUT /api/WIRCheckPoints/{wirId}/review

Body: {
  status: "Approved",
  comment: "All items verified and approved",
  inspectorRole: "QC Engineer",
  items: [
    { checklistItemId: "guid-1", status: "Pass", remarks: "OK" },
    { checklistItemId: "guid-2", status: "Pass", remarks: "Verified" },
    { checklistItemId: "guid-3", status: "Fail", remarks: "Needs correction" },
    ...
  ]
}

Backend processes:

1️⃣ Load WIRCheckpoint by wirId
2️⃣ Update WIR status: Pending → Approved/Rejected/Conditional
3️⃣ Update inspection dates, inspector info, comments
4️⃣ For each item in request:
   └── Find ChecklistItem by checklistItemId
   └── Update status (Pass/Fail/Pending)
   └── Update remarks
5️⃣ Save changes: UnitOfWork.CompleteAsync()
6️⃣ Return updated WIRCheckpointDto
```

---

## 🗃️ Database Relationships

```sql
-- Master/Template Data (Seeded)
References (36 rows)
    ↑
    │ FK
    │
Categories (42 rows)
    ↑
    │ FK
    │
PredefinedChecklistItems (284 rows)
    ├── CategoryId → Categories
    ├── ReferenceId → References
    └── WIRNumber (string, not FK)

WIRMasters (6 rows)
    └── WIRNumber (PK alternate key)

-- Runtime Data (Created per Box)
Boxes
    ↓
    │ BoxId FK
    │
WIRCheckpoints (6 per box)
    ├── BoxId → Boxes
    └── WIRNumber (matches WIRMaster)
        ↓
        │ WIRId FK
        │
    WIRChecklistItems (284 per box)
        ├── WIRId → WIRCheckpoints
        └── PredefinedItemId → PredefinedChecklistItems (reference)
    
    WIRCheckpointImages (optional)
        └── WIRId → WIRCheckpoints
```

---

## 🎯 Key Design Patterns Used

### **1. CQRS (Command Query Responsibility Segregation)**
- **Commands**: Modify state (GenerateWIRsForBoxCommand)
- **Queries**: Read data (GetWIRsByBoxWithChecklistQuery)
- **Mediator**: MediatR handles routing

### **2. Repository Pattern**
- **IUnitOfWork**: Manages transactions
- **IGenericRepository<T>**: Generic CRUD operations
- **Abstraction**: Decouples data access from business logic

### **3. Specification Pattern**
- **Specifications**: Encapsulate complex query logic
- **Reusable**: Same spec used across different queries
- **Type-safe**: Compile-time checking

### **4. DTO Pattern**
- **Separation**: Domain entities vs. API contracts
- **Mapster**: Fast object mapping
- **Flexibility**: API changes don't affect domain

---

## 📂 File Structure

```
Dubox.Domain/
├── Entities/
│   ├── WIRMaster.cs ⭐
│   ├── PredefinedChecklistItem.cs
│   ├── WIRCheckpoint.cs
│   ├── WIRChecklistItem.cs
│   ├── Category.cs
│   └── Reference.cs
└── Enums/
    ├── WIRStatusEnum.cs
    └── CheckListItemStatusEnum.cs

Dubox.Application/
├── Features/WIRCheckpoints/
│   ├── Commands/
│   │   ├── GenerateWIRsForBoxCommand.cs ⭐
│   │   └── GenerateWIRsForBoxCommandHandler.cs ⭐
│   └── Queries/
│       ├── GetWIRsByBoxWithChecklistQuery.cs ⭐
│       └── GetWIRsByBoxWithChecklistQueryHandler.cs ⭐
├── DTOs/
│   ├── WIRCheckpointDto.cs
│   ├── WIRWithChecklistDto.cs ⭐
│   ├── ChecklistSectionDto.cs ⭐
│   └── ChecklistItemDetailDto.cs ⭐
└── Specifications/
    ├── GetWIRsWithChecklistByBoxIdSpecification.cs ⭐
    └── GetPredefinedItemsByCategorySpecification.cs ⭐

Dubox.Infrastructure/
├── ApplicationContext/
│   └── ApplicationDbContext.cs (Updated with WIRMasters DbSet)
└── Seeding/
    ├── ReferenceSeedData.cs (Updated +12 refs)
    ├── CategorySeedData.cs (Updated +28 categories)
    ├── WIRMasterSeedData.cs ⭐
    ├── WIR1_MaterialVerificationSeedData.cs ⭐
    ├── WIR4_StructuralAssemblySeedData.cs ⭐
    ├── WIR5_FinishingWorksSeedData.cs ⭐
    └── WIR6_FinalInspectionSeedData.cs ⭐

Dubox.API/
└── Controllers/
    └── WIRCheckPointsController.cs (Updated with 2 new endpoints)
```

---

## 🚀 Performance Considerations

### **1. Batch Operations**
- Generate all 6 WIRs in single transaction
- Bulk insert 284 checklist items

### **2. Eager Loading**
- Specifications use `.Include()` for related data
- Avoids N+1 query problem

### **3. Split Queries**
- `SplitQuery(true)` prevents cartesian explosion
- Multiple optimized queries instead of one huge join

### **4. Projection**
- DTOs only include needed fields
- Reduces payload size

---

## ✅ Summary

**Backend Flow in 5 Steps:**

1️⃣ **Seed Database** → 6 WIRMasters, 42 Categories, 36 References, 284 PredefinedItems

2️⃣ **Generate WIRs** → POST /generate-for-box/{boxId} → Creates 6 WIRCheckpoints + 284 WIRChecklistItems

3️⃣ **Load Checklist** → GET /box/{boxId}/with-checklist → Returns organized, grouped data

4️⃣ **Inspector Reviews** → Frontend updates item statuses locally

5️⃣ **Submit Review** → PUT /{wirId}/review → Updates database with results

**All working seamlessly with CQRS, Repository, Specification, and DTO patterns!** 🎉
