# PowerShell script to apply responsive design to all component SCSS files
# Run this from: dubox-frontend directory

$responsiveStyles = @"

// ============================================
// RESPONSIVE DESIGN - AUTO-GENERATED
// ============================================

// Tablet (768px - 1024px)
@media (max-width: 1024px) and (min-width: 769px) {
  .page-content {
    padding: 32px;
  }

  .kpi-grid, .stats-grid, .summary-cards {
    grid-template-columns: repeat(2, 1fr);
  }
}

// Mobile (max-width: 768px)
@media (max-width: 768px) {
  .page-container {
    margin-left: 0;
    margin-top: var(--header-height, 64px);
  }

  .page-content {
    padding: 16px;
  }

  // Dashboard Header
  .dashboard-header, .page-header, .project-header {
    flex-direction: column;
    gap: 16px;
    margin-bottom: 32px;

    .header-left, .header-info, .project-info {
      text-align: center;
      align-items: center;
    }

    .dashboard-title, .page-title, h1 {
      font-size: 24px !important;
      text-align: center;
    }

    .header-right, .header-actions {
      width: 100%;
      justify-content: center;
      flex-wrap: wrap;

      .btn {
        flex: 1;
        min-width: 140px;
        padding: 14px 20px;
        font-size: 16px;
      }
    }
  }

  // Stats/KPI Grid - Stack Vertically
  .kpi-grid, .stats-grid, .summary-cards {
    grid-template-columns: 1fr;
    gap: 16px;
    margin-bottom: 32px;
  }

  .kpi-card, .stat-card, .summary-card {
    padding: 16px;
    min-height: 80px;

    .kpi-icon, .stat-icon, .card-icon {
      width: 48px;
      height: 48px;
      padding: 12px;

      svg {
        width: 20px;
        height: 20px;
      }
    }

    .kpi-value, .stat-value, .card-value {
      font-size: 24px;
    }

    .kpi-label, .stat-label, .card-label {
      font-size: 14px;
    }
  }

  // Filters
  .filters-card, .filter-section, .filters-section {
    .filters-content, .filter-controls {
      flex-direction: column;
      gap: 16px;

      .search-box {
        width: 100%;
        min-width: 100%;

        input {
          font-size: 16px;
          padding: 12px 12px 12px 42px;
        }
      }

      select, input[type="text"], input[type="date"], input[type="number"] {
        width: 100%;
        font-size: 16px;
        padding: 12px 14px;
      }

      .checkbox-label {
        width: 100%;
        padding: 12px;
        background: var(--gray-50, #f9fafb);
        border-radius: var(--radius-md, 8px);
        justify-content: center;
      }

      .btn {
        width: 100%;
      }
    }
  }

  // Tables - Hide on mobile
  .data-table, .table-container table, .teams-table {
    display: none;
  }

  // Mobile Card Layout
  .mobile-list, .mobile-cards {
    display: flex !important;
    flex-direction: column;
    gap: 16px;
    padding: 16px;
  }

  .mobile-card, .item-card-mobile {
    background: white;
    border: 1px solid #e5e7eb;
    border-radius: 12px;
    padding: 16px;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.05);
    transition: all 0.2s ease;

    &:active {
      transform: scale(0.98);
      box-shadow: 0 1px 2px rgba(0, 0, 0, 0.1);
    }

    .card-header {
      display: flex;
      justify-content: space-between;
      align-items: flex-start;
      margin-bottom: 12px;
      padding-bottom: 12px;
      border-bottom: 1px solid #f3f4f6;
    }

    .card-details {
      display: flex;
      flex-direction: column;
      gap: 8px;
      margin-bottom: 16px;

      .detail-item {
        display: flex;
        align-items: center;
        font-size: 14px;
        color: #4b5563;

        .detail-label {
          font-weight: 600;
          min-width: 100px;
          color: #374151;
        }

        .detail-value {
          flex: 1;
          overflow: hidden;
          text-overflow: ellipsis;
          white-space: nowrap;
        }
      }
    }

    .card-actions {
      display: flex;
      gap: 8px;
      justify-content: flex-end;
      padding-top: 12px;
      border-top: 1px solid #f3f4f6;

      .btn-icon {
        padding: 10px;
        min-width: 44px;
        min-height: 44px;
        display: flex;
        align-items: center;
        justify-content: center;

        svg {
          width: 20px;
          height: 20px;
        }
      }
    }
  }

  // Charts
  .chart-container {
    height: 300px !important;
  }

  // Tabs
  .tabs-nav, .tab-navigation {
    overflow-x: auto;
    -webkit-overflow-scrolling: touch;
    padding-bottom: 8px;

    .tab-button {
      flex-shrink: 0;
      font-size: 14px;
      padding: 10px 16px;
    }
  }

  // Pagination
  .pagination-footer, .pagination-section {
    flex-direction: column;
    gap: 16px;
    padding: 16px;

    .pagination-info {
      order: 1;
      text-align: center;
      font-size: 14px;
    }

    .pagination-controls-wrapper {
      order: 2;
      width: 100%;
    }

    .pagination-controls {
      flex-wrap: nowrap;
      overflow-x: auto;
      -webkit-overflow-scrolling: touch;

      .btn-pagination {
        flex-shrink: 0;
        min-width: 40px;
        height: 40px;
      }

      .page-numbers .btn-page {
        min-width: 40px;
        height: 40px;
        font-size: 14px;
        flex-shrink: 0;
      }
    }

    .page-size-selector {
      order: 3;
      width: 100%;
      justify-content: center;

      select {
        flex: 1;
        max-width: 120px;
        font-size: 16px;
        padding: 10px 32px 10px 12px;
      }
    }
  }

  // Modal
  .modal-overlay {
    padding: 0;
  }

  .modal-content {
    max-width: 100%;
    max-height: 100vh;
    border-radius: 0;
    margin: 0;

    .modal-header {
      padding: 16px;

      h2, h3 {
        font-size: 18px;
      }
    }

    .modal-body {
      padding: 16px;
      max-height: calc(100vh - 80px);
      overflow-y: auto;
    }

    .modal-footer {
      padding: 16px;
      flex-direction: column;

      .btn {
        width: 100%;
      }
    }
  }

  // Empty State
  .empty-state {
    padding: 48px 16px;

    svg {
      width: 48px;
      height: 48px;
    }

    h3 {
      font-size: 18px;
      margin-bottom: 8px;
    }

    p {
      font-size: 14px;
      margin-bottom: 16px;
    }

    .btn {
      width: 100%;
      max-width: 280px;
    }
  }

  // Loading State
  .loading-container, .loading-state {
    padding: 48px 16px;
    min-height: 200px;

    .spinner {
      width: 40px;
      height: 40px;
    }

    p {
      font-size: 14px;
    }
  }
}

// Small Mobile (max-width: 480px)
@media (max-width: 480px) {
  .page-content {
    padding: 12px;
  }

  .dashboard-header h1, .page-header h1 {
    font-size: 20px !important;
  }

  .kpi-card, .stat-card {
    .kpi-icon, .stat-icon {
      width: 40px;
      height: 40px;

      svg {
        width: 18px;
        height: 18px;
      }
    }

    .kpi-value, .stat-value {
      font-size: 20px;
    }

    .kpi-label, .stat-label {
      font-size: 13px;
    }
  }

  .mobile-card, .item-card-mobile {
    padding: 12px;
  }

  .pagination-controls {
    .page-numbers .btn-page {
      min-width: 36px;
      height: 36px;
      font-size: 13px;
    }

    .btn-pagination {
      min-width: 36px;
      height: 36px;
    }
  }
}
"@

$scssFiles = Get-ChildItem -Path "src\app\features" -Filter "*.component.scss" -Recurse

Write-Host "Found $($scssFiles.Count) SCSS files" -ForegroundColor Green

foreach ($file in $scssFiles) {
    $content = Get-Content $file.FullName -Raw
    
    # Check if responsive styles are already added
    if ($content -notmatch "// RESPONSIVE DESIGN - AUTO-GENERATED") {
        Write-Host "Adding responsive styles to: $($file.Name)" -ForegroundColor Cyan
        
        # Append responsive styles
        $newContent = $content.TrimEnd() + "`n" + $responsiveStyles
        
        # Write back to file
        Set-Content -Path $file.FullName -Value $newContent -NoNewline
        
        Write-Host "  ✓ Updated: $($file.FullName)" -ForegroundColor Green
    } else {
        Write-Host "  ⊘ Skipped (already has responsive styles): $($file.Name)" -ForegroundColor Yellow
    }
}

Write-Host "`nDone! Applied responsive design to all components." -ForegroundColor Green
Write-Host "Please review the changes and test on different screen sizes." -ForegroundColor Yellow

