# PowerShell script to fill Excel template with DuBox project structure
# Uses COM objects to work with Excel directly

param(
    [string]$ExcelTemplate = "C:\Users\DELL\Desktop\DUBOX Tracking Application.xls",
    [string]$ProjectRoot = "C:\Users\DELL\source\repos\DuBox\DuBox-",
    [string]$OutputFile = "$ProjectRoot\Documentation\Project_Filled.xlsx"
)

$ErrorActionPreference = "Stop"

# Exclude directories
$excludeDirs = @('node_modules', 'bin', 'obj', 'dist', '.angular', '.vscode', 'coverage', 'logs', 'wwwroot', 'packages', 'out-tsc')

$separator = -join (1..60 | ForEach-Object { "=" })
Write-Host $separator -ForegroundColor Cyan
Write-Host "DuBox Project Structure Documentation Generator" -ForegroundColor Cyan
Write-Host $separator -ForegroundColor Cyan

# Step 1: Check if template exists
Write-Host "`nStep 1: Checking Excel template..." -ForegroundColor Yellow
if (-not (Test-Path $ExcelTemplate)) {
    Write-Host "ERROR: Template not found at: $ExcelTemplate" -ForegroundColor Red
    exit 1
}
Write-Host "[OK] Template found: $ExcelTemplate" -ForegroundColor Green

# Step 2: Open Excel and load template
Write-Host "`nStep 2: Opening Excel template..." -ForegroundColor Yellow
try {
    $excel = New-Object -ComObject Excel.Application
    $excel.Visible = $false
    $excel.DisplayAlerts = $false
    
    # Open the template
    $workbook = $excel.Workbooks.Open($ExcelTemplate)
    $worksheet = $workbook.ActiveSheet
    
    Write-Host "[OK] Template opened successfully" -ForegroundColor Green
} catch {
    Write-Host "ERROR: Could not open Excel template: $_" -ForegroundColor Red
    exit 1
}

# Step 3: Read column headers
Write-Host "`nStep 3: Reading column structure..." -ForegroundColor Yellow
$headers = @()
$headerRow = 1
$maxCol = $worksheet.UsedRange.Columns.Count

for ($col = 1; $col -le $maxCol; $col++) {
    $headerValue = $worksheet.Cells.Item($headerRow, $col).Value2
    if ($headerValue) {
        $headers += $headerValue
    }
}

# If no headers found, add default headers
if ($headers.Count -eq 0) {
    $headers = @('Component / Module', 'File / Path', 'Purpose', 'What it does', 'Dependencies', 'Status', 'Notes')
    for ($col = 1; $col -le $headers.Count; $col++) {
        $worksheet.Cells.Item($headerRow, $col).Value2 = $headers[$col - 1]
    }
}

Write-Host "[OK] Found columns: $($headers -join ', ')" -ForegroundColor Green

# Map column indices
$colMap = @{}
for ($i = 0; $i -lt $headers.Count; $i++) {
    $header = $headers[$i].ToString().ToLower()
    if ($header -match 'component|module') { $colMap['component'] = $i + 1 }
    if ($header -match 'file|path') { $colMap['file_path'] = $i + 1 }
    if ($header -match 'purpose') { $colMap['purpose'] = $i + 1 }
    if ($header -match 'what.*does') { $colMap['what_it_does'] = $i + 1 }
    if ($header -match 'dependenc') { $colMap['dependencies'] = $i + 1 }
    if ($header -match 'status') { $colMap['status'] = $i + 1 }
    if ($header -match 'note') { $colMap['notes'] = $i + 1 }
}

# Default mapping if not found
if ($colMap.Count -eq 0) {
    $colMap = @{
        'component' = 1
        'file_path' = 2
        'purpose' = 3
        'what_it_does' = 4
        'dependencies' = 5
        'status' = 6
        'notes' = 7
    }
}

# Step 4: Scan project files
Write-Host "`nStep 4: Scanning project files..." -ForegroundColor Yellow
$projectFiles = @()

# Backend paths
$backendPaths = @(
    "$ProjectRoot\Dubox.Api",
    "$ProjectRoot\Dubox.Application",
    "$ProjectRoot\Dubox.Domain",
    "$ProjectRoot\Dubox.Infrastructure"
)

# Frontend path
$frontendPath = "$ProjectRoot\dubox-frontend\src\app"

$allPaths = $backendPaths + @($frontendPath)

foreach ($basePath in $allPaths) {
    if (Test-Path $basePath) {
        $files = Get-ChildItem -Path $basePath -Recurse -File -ErrorAction SilentlyContinue |
            Where-Object {
                $pathParts = $_.FullName.Split('\')
                ($pathParts | Where-Object { $excludeDirs -contains $_ }).Count -eq 0
            }
        
        foreach ($file in $files) {
            $relPath = $file.FullName.Replace($ProjectRoot, "").TrimStart('\')
            $projectFiles += @{
                Path = $relPath
                FullPath = $file.FullName
                Extension = $file.Extension.ToLower()
                Name = $file.Name
            }
        }
    }
}

Write-Host "[OK] Found $($projectFiles.Count) files to analyze" -ForegroundColor Green

# Step 5: Analyze components
Write-Host "`nStep 5: Analyzing components..." -ForegroundColor Yellow
$components = @()
$analyzedCount = 0

function Analyze-File {
    param($FileInfo)
    
    $content = Get-Content -Path $FileInfo.FullPath -Raw -ErrorAction SilentlyContinue
    if (-not $content) { return $null }
    
    $ext = $FileInfo.Extension
    $path = $FileInfo.Path
    $name = $FileInfo.Name
    
    $component = $null
    $purpose = ""
    $whatItDoes = ""
    $dependencies = ""
    $status = "Done"
    $notes = ""
    
    # Backend Analysis
    if ($path -like "Dubox.Api\Controllers\*") {
        if ($content -match 'class\s+(\w+Controller)\s*:') {
            $component = $matches[1]
            $purpose = "API Controller"
            $whatItDoes = "Handles HTTP requests for $($component.Replace('Controller', '')) operations"
            $dependencies = "MediatR, Application Layer"
            
            # Extract endpoints
            $endpoints = [regex]::Matches($content, '\[(HttpGet|HttpPost|HttpPut|HttpDelete|HttpPatch)\]\s*\w+\s+(\w+)')
            if ($endpoints.Count -gt 0) {
                $endpointList = $endpoints | Select-Object -First 3 | ForEach-Object { "$($_.Groups[1].Value)($($_.Groups[2].Value))" }
                $notes = "Endpoints: $($endpointList -join ', ')"
            }
        }
    }
    elseif ($path -like "Dubox.Application\Features\*\Commands\*") {
        $match = [regex]::Match($content, '(?:public\s+record\s+|class\s+)(\w+Command)\s*(?::|$)')
        if ($match.Success) {
            $component = $match.Groups[1].Value
            $purpose = "CQRS Command"
            $whatItDoes = "Defines command for $($component.Replace('Command', '')) operation"
            $dependencies = "MediatR, Domain Layer"
        }
    }
    elseif ($path -like "Dubox.Application\Features\*\Queries\*") {
        $match = [regex]::Match($content, '(?:public\s+record\s+|class\s+)(\w+Query)\s*(?::|$)')
        if ($match.Success) {
            $component = $match.Groups[1].Value
            $purpose = "CQRS Query"
            $whatItDoes = "Defines query for retrieving $($component.Replace('Query', '')) data"
            $dependencies = "MediatR, Domain Layer"
        }
    }
    elseif ($name -like "*Handler.cs") {
        if ($content -match 'class\s+(\w+Handler)\s*:') {
            $component = $matches[1]
            $purpose = "CQRS Handler"
            $whatItDoes = "Handles $($component.Replace('Handler', '')) business logic"
            $dependencies = "UnitOfWork, Repository, Domain Services"
        }
    }
        elseif ($path -like "Dubox.Application\DTOs\*") {
            $match = [regex]::Match($content, '(?:public\s+class|interface)\s+(\w+Dto)\s*(?:extends|implements|:|{)')
            if ($match.Success) {
                $component = $match.Groups[1].Value
                $purpose = "Data Transfer Object"
                $whatItDoes = "Transfers $($component.Replace('Dto', '')) data between layers"
                $dependencies = "Domain Entities"
            }
        }
    elseif ($path -like "Dubox.Domain\Entities\*") {
        if ($content -match 'class\s+(\w+)\s*(?::|$)') {
            $component = $matches[1]
            $purpose = "Domain Entity"
            $whatItDoes = "Represents $component in the domain model"
            $dependencies = "Domain Enums, Domain Interfaces"
        }
    }
    elseif ($path -like "Dubox.Infrastructure\*") {
        if ($name -like "*Repository.cs") {
            $component = $name.Replace(".cs", "")
            $purpose = "Repository Implementation"
            $whatItDoes = "Implements data access for $component"
            $dependencies = "Entity Framework, Domain Entities"
        }
        elseif ($name -like "*Service.cs") {
            $component = $name.Replace(".cs", "")
            $purpose = "Infrastructure Service"
            $whatItDoes = "Provides infrastructure service: $component"
            $dependencies = "External Libraries, Domain Interfaces"
        }
    }
    # Frontend Analysis
    elseif ($path -like "dubox-frontend\src\app\*") {
        if ($ext -eq ".ts" -and $name -like "*component.ts") {
            if ($content -match 'export\s+class\s+(\w+Component)') {
                $component = $matches[1]
                $purpose = "Angular Component"
                
                if ($path -match 'features\\(\w+)') {
                    $feature = $matches[1]
                    $whatItDoes = "UI component for $feature feature"
                } else {
                    $whatItDoes = "Reusable UI component"
                }
                $dependencies = "Angular Core, Services, Models"
            }
        }
        elseif ($ext -eq ".ts" -and $name -like "*service.ts") {
            $match = [regex]::Match($content, '(?:class|export\s+class)\s+(\w+Service)\s*(?:extends|implements|:)')
            if ($match.Success) {
                $component = $match.Groups[1].Value
                $purpose = "Angular Service"
                $whatItDoes = "Provides $($component.Replace('Service', '')) functionality to components"
                $dependencies = "HttpClient, API Service"
            }
        }
        elseif ($ext -eq ".ts" -and $name -like "*model.ts") {
            $match = [regex]::Match($content, '(?:export\s+)?(?:interface|class|type)\s+(\w+)')
            if ($match.Success) {
                $component = $match.Groups[1].Value
                $purpose = "TypeScript Model/Interface"
                $whatItDoes = "Defines $component data structure"
                $dependencies = "None"
            }
        }
        elseif ($ext -eq ".ts" -and $name -like "*guard.ts") {
            $match = [regex]::Match($content, 'export\s+(?:class|const)\s+(\w+Guard)')
            if ($match.Success) {
                $component = $match.Groups[1].Value
                $purpose = "Angular Route Guard"
                $whatItDoes = "Protects routes with $($component.Replace('Guard', '')) logic"
                $dependencies = "Router, Auth Service"
            }
        }
        elseif ($ext -eq ".ts" -and $name -like "*module.ts") {
            if ($content -match 'export\s+class\s+(\w+Module)') {
                $component = $matches[1]
                $purpose = "Angular Module"
                $whatItDoes = "Organizes $($component.Replace('Module', '')) feature components"
                $dependencies = "Angular Common, Feature Components"
            }
        }
    }
    
    # Generic fallback
    if (-not $component) {
        $match = [regex]::Match($content, '(?:export\s+)?(?:class|interface|type)\s+(\w+)')
        if ($match.Success) {
            $component = $match.Groups[1].Value
            $purpose = "Code File"
            $whatItDoes = "Contains $component implementation"
            $status = "Not Clear"
            $notes = "Purpose unclear - needs clarification."
        }
    }
    
    if ($component) {
        return @{
            Component = $component
            FilePath = $path
            Purpose = if ($purpose) { $purpose } else { "Code File" }
            WhatItDoes = if ($whatItDoes) { $whatItDoes } else { "Implements $component" }
            Dependencies = if ($dependencies) { $dependencies } else { "See code" }
            Status = $status
            Notes = $notes
        }
    }
    
    return $null
}

# Analyze files
foreach ($file in $projectFiles) {
    if ($file.Extension -in @('.cs', '.ts', '.html', '.scss')) {
        $result = Analyze-File -FileInfo $file
        if ($result) {
            $components += $result
            $analyzedCount++
            if ($analyzedCount % 50 -eq 0) {
                Write-Host "  Analyzed $analyzedCount components..." -ForegroundColor Gray
            }
        }
    }
}

Write-Host "[OK] Analyzed $($components.Count) components" -ForegroundColor Green

# Step 6: Fill Excel
Write-Host "`nStep 6: Filling Excel template..." -ForegroundColor Yellow
$nextRow = $worksheet.UsedRange.Rows.Count + 1
if ($nextRow -eq 1) { $nextRow = 2 }

foreach ($comp in $components) {
    if ($colMap['component']) {
        $worksheet.Cells.Item($nextRow, $colMap['component']).Value2 = $comp.Component
    }
    if ($colMap['file_path']) {
        $worksheet.Cells.Item($nextRow, $colMap['file_path']).Value2 = $comp.FilePath
    }
    if ($colMap['purpose']) {
        $worksheet.Cells.Item($nextRow, $colMap['purpose']).Value2 = $comp.Purpose
    }
    if ($colMap['what_it_does']) {
        $worksheet.Cells.Item($nextRow, $colMap['what_it_does']).Value2 = $comp.WhatItDoes
    }
    if ($colMap['dependencies']) {
        $worksheet.Cells.Item($nextRow, $colMap['dependencies']).Value2 = $comp.Dependencies
    }
    if ($colMap['status']) {
        $worksheet.Cells.Item($nextRow, $colMap['status']).Value2 = $comp.Status
    }
    if ($colMap['notes']) {
        $worksheet.Cells.Item($nextRow, $colMap['notes']).Value2 = $comp.Notes
    }
    $nextRow++
}

Write-Host "[OK] Filled $($components.Count) rows" -ForegroundColor Green

# Step 7: Save file
Write-Host "`nStep 7: Saving filled template..." -ForegroundColor Yellow
$outputDir = Split-Path -Path $OutputFile -Parent
if (-not (Test-Path $outputDir)) {
    New-Item -ItemType Directory -Path $outputDir -Force | Out-Null
}

# Save as .xlsx
$workbook.SaveAs($OutputFile, 51) # 51 = xlOpenXMLWorkbook (.xlsx format)
$workbook.Close($false)
$excel.Quit()

# Release COM objects
[System.Runtime.Interopservices.Marshal]::ReleaseComObject($worksheet) | Out-Null
[System.Runtime.Interopservices.Marshal]::ReleaseComObject($workbook) | Out-Null
[System.Runtime.Interopservices.Marshal]::ReleaseComObject($excel) | Out-Null
[System.GC]::Collect()
[System.GC]::WaitForPendingFinalizers()

Write-Host "[OK] Successfully saved to: $OutputFile" -ForegroundColor Green
Write-Host "[OK] Total components documented: $($components.Count)" -ForegroundColor Green

Write-Host "`n$separator" -ForegroundColor Cyan
Write-Host "[OK] Documentation complete!" -ForegroundColor Green
Write-Host $separator -ForegroundColor Cyan

