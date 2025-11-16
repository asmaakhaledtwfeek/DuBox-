# 🐛 Debugging Steps for "View Boxes" Issue

## 📋 Step-by-Step Debugging Guide

### **STEP 1: Restart Backend (CRITICAL!)**

Open PowerShell in the backend directory and run:

```powershell
cd D:\Company\GroupAmana\DuBox-\Dubox.Api
dotnet clean
dotnet build
dotnet run
```

✅ Wait for: `Now listening on: http://localhost:5000`

---

### **STEP 2: Open Browser Console**

1. Press `F12` in your browser
2. Go to **Console** tab
3. Click the **🗑️ Clear Console** button

---

### **STEP 3: Load Projects Page**

Navigate to: `http://localhost:4200/projects`

**Look for these messages in console:**

```
🚀 Projects List Component Initialized
✅ Can create project: true/false
📡 Loading projects from API...
🌐 GET API Response for projects : {...}
🔑 Response keys: [...]
📦 Loaded projects: [...]
```

**⚠️ IMPORTANT: Copy and share the output of `🔑 Response keys:`**

---

### **STEP 4: Create a Test Project**

1. Click "New Project" button
2. Fill in form:
   - Name: `Test Project`
   - Code: `TEST-001`
   - Location: `Test Location`
   - Start Date: Any date
3. Click "Create Project"

**Look for these messages:**

```
🚀 Submitting project data: {...}
🌐 POST API Response for projects : {...}
🔑 Response keys: [...]
✅ Project created: {...}
🆔 Project ID: xxx
```

**⚠️ CRITICAL: What value do you see for `🆔 Project ID:`?**

---

### **STEP 5: Try "View Boxes"**

1. Click the "View Boxes" button on the project card

**Look for:**
- ✅ If it works: You'll navigate to the boxes page
- ❌ If it fails: Check console for error

---

## 🔍 What to Share:

Please copy and paste from the console:

1. **Response keys when loading projects:**
   ```
   🔑 Response keys: [?]
   ```

2. **Response keys when creating project:**
   ```
   🔑 Response keys: [?]
   ```

3. **Project ID values:**
   ```
   🆔 Project ID: ?
   🆔 Project projectId: ?
   🆔 Project ProjectId: ?
   ```

4. **Any error messages** (red text in console)

---

## 🎯 Quick Check:

Run this in browser console (F12 → Console tab):

```javascript
localStorage.getItem('jwt_token')
```

If it returns `null`, you need to **login again**.

---

## 🔧 Alternative: Direct API Test

Open a new browser tab and go to:

```
http://localhost:5000/api/projects
```

**What do you see?**
- ✅ JSON with projects → Backend is working
- ❌ 401 Unauthorized → Token expired, login again
- ❌ Cannot connect → Backend not running

---

## ⚡ Quick Fix Options:

### **Option A: If backend returns PascalCase (ProjectId, ProjectName)**

The JSON serialization didn't apply. Make sure you:
1. Saved `Dubox.Api/Program.cs`
2. Stopped the backend (Ctrl+C)
3. Ran `dotnet clean && dotnet build && dotnet run`

### **Option B: If projects have no ID at all**

The backend isn't returning the ID field. Check:
1. Does the database have projects? (check with SQL Server)
2. Is Mapster configured correctly?

### **Option C: If you see "401 Unauthorized"**

Your login token expired:
1. Logout
2. Login again with `admin@groupamana.com` / `AMANA@2024`
3. Try again

---

## 📞 What to Report:

Share a screenshot or copy-paste of:
1. The browser console output (all the 🌐 and 🔑 messages)
2. The exact error message
3. What you see when you visit `http://localhost:5000/api/projects`

