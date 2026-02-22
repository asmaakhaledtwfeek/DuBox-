import { Component, EventEmitter, Input, OnChanges, OnDestroy, OnInit, Output, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators, FormsModule } from '@angular/forms';
import { forkJoin, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { ProgressUpdateService } from '../../../core/services/progress-update.service';
import { WIRService } from '../../../core/services/wir.service';
import { BoxService } from '../../../core/services/box.service';
import { FactoryService, Factory, FactorySection, FactorySectionType, FactorySectionPart } from '../../../core/services/factory.service';
import { ActivityProgressStatus, BoxActivityDetail, ProgressUpdate } from '../../../core/models/progress-update.model';
import { WIRRecord, WIRStatus, WIRCheckpoint, CheckpointStatus } from '../../../core/models/wir.model';
import { Box, BoxStatus } from '../../../core/models/box.model';
import { 
  getCurrentActiveWIRStage, 
  getWIRStageInfo,
  WIRStageInfo
} from '../../../core/utils/wir-stage.util';

@Component({
  selector: 'app-update-progress-modal',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule],
  templateUrl: './update-progress-modal.component.html',
  styleUrls: ['./update-progress-modal.component.scss']
})
export class UpdateProgressModalComponent implements OnInit, OnChanges, OnDestroy {
  @Input() activity!: BoxActivityDetail;
  @Input() isOpen: boolean = false;
  @Input() allActivities: BoxActivityDetail[] = []; // All activities to find nearest WIR
  /** When provided (e.g. from activity-details), used to check if previous updates have images when completing at 100% */
  @Input() existingProgressUpdates: ProgressUpdate[] | undefined;
  @Input() isProjectOnHold: boolean = false; // Track if project is on hold
  @Input() isProjectArchived: boolean = false; // Track if project is archived
  @Output() closeModal = new EventEmitter<void>();
  @Output() progressUpdated = new EventEmitter<any>();

  progressForm!: FormGroup;
  isSubmitting = false;
  errorMessage = '';
  successMessage = '';
  selectedFiles: File[] = [];
  ActivityProgressStatus = ActivityProgressStatus;

  // Photo upload state - multiple images support
  selectedImages: Array<{ type: 'file' | 'url'; file?: File; url?: string; preview?: string; name?: string }> = [];
  currentPhotoUrl: string = '';
  isUploadingPhoto = false;
  photoUploadError = '';
  cameraStream: MediaStream | null = null;
  showCamera = false;
  photoInputMethod: 'url' | 'upload' | 'camera' = 'url';

  // WIR position fields
  nearestWIR: WIRRecord | null = null;
  nearestWIRCheckpoint: WIRCheckpoint | null = null; // Checkpoint for nearest WIR
  hasWIRBelow: boolean = false;
  hasWIRActivityBelow: boolean = false; // Track if WIR activity exists (regardless of record)
  positionLockedReason: string = ''; // Reason why position fields are locked

  // Factory Layout Grid
  currentBox: Box | null = null;
  factoryBoxes: Box[] = [];
  factory: Factory | null = null;
  factorySections: FactorySection[] = []; // Factory sections (Assembly 1, Assembly 2, Finishing, etc.)
  isLoadingFactoryLayout = false;
  showFactoryLayout = false; // Show only when WIR Position is editable
  
  // Map of boxId to WIR records for color coding
  boxWIRRecordsMap: Map<string, WIRRecord[]> = new Map();

  // Freezing cells: partId → Set<rowNumber> and sectionId → Set<rowNumber>
  freezingCellsMap: Map<string, Set<number>> = new Map();
  sectionFrozenRowsMap: Map<string, Set<number>> = new Map();
  
  // Dropdown options for available positions
  availableBays: string[] = [];
  availableRows: string[] = [];
  selectedBay: string = '';
  selectedRow: string = '';
  selectedSectionId: string = ''; // Track which section the selected cell is in
  FactorySectionType = FactorySectionType; // Expose enum to template

  constructor(
    private fb: FormBuilder,
    private progressUpdateService: ProgressUpdateService,
    private wirService: WIRService,
    private boxService: BoxService,
    private factoryService: FactoryService
  ) {}

  ngOnInit(): void {
    this.initializeForm();
  }

  ngOnChanges(changes: SimpleChanges): void {
    // Reinitialize form when activity changes, allActivities changes, or modal opens
    if (changes['activity'] || changes['allActivities'] || (changes['isOpen'] && this.isOpen && this.activity)) {
      this.initializeForm();
      // Clear previous errors and messages when opening
      if (this.isOpen) {
        this.errorMessage = '';
        this.successMessage = '';
        this.selectedFiles = [];
        this.selectedImages = [];
        this.currentPhotoUrl = '';
        this.showCamera = false;
        this.stopCamera();
        
        // Load factory layout when modal opens
        this.loadFactoryLayout();
      }
    }
  }

  ngOnDestroy(): void {
    // Cleanup: stop camera stream on component unmount
    const video = document.getElementById('progress-camera-preview') as HTMLVideoElement;
    if (video) {
      const stream = video.srcObject as MediaStream;
      if (stream) {
        stream.getTracks().forEach((track) => track.stop());
      }
      video.srcObject = null;
    }
    
    // Also stop any stored stream reference
    if (this.cameraStream) {
      this.cameraStream.getTracks().forEach((track) => track.stop());
      this.cameraStream = null;
    }
    
    this.stopCamera();
  }

  /**
   * Check if the Update Progress button should be disabled
   * For completed activities: Disabled until user fills WIR Position inputs (Bay and Row) when fields are editable
   * If position is already locked (has values), button is enabled (user can still update progress)
   */
  get isUpdateButtonDisabled(): boolean {
    if (!this.activity || !this.progressForm) return false;
    
    const isCompleted = this.activity.status === ActivityProgressStatus.Completed || 
                       this.activity.status === ActivityProgressStatus.Delayed;
    
    const wirBayDisabled = this.progressForm.get('wirBay')?.disabled ?? false;
    const wirRowDisabled = this.progressForm.get('wirRow')?.disabled ?? false;
    const isWirPositionLocked = this.positionLockedReason !== '' || wirBayDisabled || wirRowDisabled;
    
    if (!isCompleted) {
      // For non-completed activities, allow update (don't disable based on position)
      return false;
    }
    
    // For completed activities:
    // If position is locked (already has values set), allow update (button enabled)
    // User can't change locked position fields, but they can still submit the update
    if (isWirPositionLocked) {
      return false;
    }
    
    // If position fields are editable, check if Bay and Row are filled
    // For completed activities, require both Bay and Row to be filled before allowing update
    const wirBayValue = this.progressForm.get('wirBay')?.value?.toString().trim() || '';
    const wirRowValue = this.progressForm.get('wirRow')?.value?.toString().trim() || '';
    
    // Disable if either Bay or Row is empty (user must fill both when fields are editable)
    const isBayOrRowEmpty = !wirBayValue || !wirRowValue;
    
    return isBayOrRowEmpty;
  }

  initializeForm(): void {
    if (!this.activity) return;
    
    const isCompleted = this.activity.status === ActivityProgressStatus.Completed || this.activity.status === ActivityProgressStatus.Delayed;
    
    console.log('🔧 Initializing form for activity:', {
      name: this.activity.activityName,
      isWIRCheckpoint: this.isWIRCheckpoint,
      sequence: this.activity.sequence,
      isCompleted: isCompleted
    });
    
    // Clear checkpoint and WIR from previous activity
    this.nearestWIRCheckpoint = null;
    this.nearestWIR = null;
    this.positionLockedReason = '';
    
    // Find WIR for current activity (only if it's a WIR checkpoint)
    this.findNearestWIRBelow();
    
    this.progressForm = this.fb.group({
      progressPercentage: [
        {value: this.activity.progressPercentage || 0, disabled: isCompleted}, 
        isCompleted ? [] : [Validators.required, Validators.min(0), Validators.max(100)]
      ],
      workDescription: [{value: '', disabled: isCompleted}],
      issuesEncountered: [{value: '', disabled: isCompleted}],
      // WIR Position fields - always start empty, NOT required
      wirBay: [''],
      wirRow: [''],
      wirPosition: [{value: '', disabled: true}] // Position is always calculated, never manually editable
    });

    // Setup automatic calculation of Position = Bay × Row
    this.setupPositionCalculation();
    
    console.log('✅ Form initialized. Is valid?', this.progressForm.valid);
  }

  /**
   * Setup automatic calculation of Position = Bay × Row
   * Bay is a text field (e.g., "A", "B"), Row is numeric
   * Position is calculated as numeric multiplication if Bay is numeric, otherwise concatenated
   * Note: Fields are NOT disabled here - that's handled by populateWIRPositionFields based on WIR state
   */
  private setupPositionCalculation(): void {
    const bayControl = this.progressForm.get('wirBay');
    const rowControl = this.progressForm.get('wirRow');
    const positionControl = this.progressForm.get('wirPosition');

    if (bayControl && rowControl && positionControl) {
      const calculatePosition = () => {
        const bayValue = bayControl.value?.toString().trim();
        const rowValue = rowControl.value?.toString().trim();
        
        if (bayValue && rowValue) {
          // Try to parse bay as number for multiplication
          const bayNum = parseInt(bayValue);
          const rowNum = parseInt(rowValue) || 0;
          
          // If bay is numeric, multiply; otherwise concatenate with a hyphen
          let position: string;
          if (!isNaN(bayNum)) {
            position = (bayNum * rowNum).toString();
          } else {
            // Bay is text (e.g., "A"), so concatenate: "A-3"
            position = `${bayValue}-${rowValue}`;
          }
          
          positionControl.setValue(position, { emitEvent: false });
        } else {
          positionControl.setValue('', { emitEvent: false });
        }
        
        // Don't disable fields here - let populateWIRPositionFields handle it
        // This allows each WIR section to have editable fields
      };

      bayControl.valueChanges.subscribe(() => calculatePosition());
      rowControl.valueChanges.subscribe(() => calculatePosition());
      
      // Initial calculation in case values are loaded
      calculatePosition();
    }
  }

  /**
   * Load factory layout data (current box and all boxes in the same factory)
   */
  private loadFactoryLayout(): void {
    if (!this.activity?.boxId) {
      this.showFactoryLayout = false;
      return;
    }

    this.isLoadingFactoryLayout = true;

    // First, get the current box to retrieve its factory ID
    this.boxService.getBox(this.activity.boxId).subscribe({
      next: (box) => {
        this.currentBox = box;
        
        // Check if box has a factory assigned
        if (!box.factoryId) {
          this.showFactoryLayout = false;
          this.isLoadingFactoryLayout = false;
          return;
        }

        // Store factoryId in a variable to satisfy TypeScript
        const factoryId = box.factoryId;

        // Load factory data and sections
        this.factoryService.getFactoryById(factoryId).subscribe({
          next: (factory) => {
            this.factory = factory;
            
            // Factory sections are included in the factory object
            this.factorySections = factory.sections || [];
            this.buildFreezingCellsMap();
            
            // Load all boxes in the same factory
            this.boxService.getBoxesByFactory(factoryId).subscribe({
              next: (boxes) => {
                // Backend already filters boxes (InProgress/Completed from active projects)
                // Only filter by position information for factory layout display
                this.factoryBoxes = boxes.filter(b => 
                  (b.bay || b.row || b.position)
                );
                
                // Load WIR records for all boxes to enable color coding
                this.loadWIRRecordsForBoxes(this.factoryBoxes);
                
                this.updateAvailablePositions();
                this.updateFactoryLayoutVisibility();
                this.isLoadingFactoryLayout = false;
              },
              error: (err: any) => {
                console.error('Error loading factory boxes:', err);
                this.isLoadingFactoryLayout = false;
                this.showFactoryLayout = false;
              }
            });
          },
          error: (err) => {
            console.error('Error loading factory:', err);
            // Continue loading boxes even if factory load fails (factoryId is guaranteed to be defined here)
            if (factoryId) {
              this.boxService.getBoxesByFactory(factoryId).subscribe({
                next: (boxes) => {
                  this.factoryBoxes = boxes.filter(b => 
                    (b.bay || b.row || b.position)
                  );
                  
                  // Load WIR records for all boxes to enable color coding
                  this.loadWIRRecordsForBoxes(this.factoryBoxes);
                  
                  this.updateAvailablePositions();
                  this.updateFactoryLayoutVisibility();
                  this.isLoadingFactoryLayout = false;
                },
                error: (boxErr) => {
                  console.error('Error loading factory boxes:', boxErr);
                  this.isLoadingFactoryLayout = false;
                  this.showFactoryLayout = false;
                }
              });
            } else {
              this.isLoadingFactoryLayout = false;
              this.showFactoryLayout = false;
            }
          }
        });
      },
      error: (err) => {
        console.error('Error loading current box:', err);
        this.isLoadingFactoryLayout = false;
        this.showFactoryLayout = false;
      }
    });
  }

  /**
   * Load WIR records for all boxes in parallel
   * This allows us to determine the current WIR stage for color coding
   */
  private loadWIRRecordsForBoxes(boxes: Box[]): void {
    if (boxes.length === 0) {
      return;
    }

    // Create observables for loading WIR records for each box
    const wirObservables = boxes.map(box => 
      this.wirService.getWIRRecordsByBox(box.id).pipe(
        map((wirs: WIRRecord[]) => ({ boxId: box.id, wirs })),
        catchError(err => {
          console.warn(`Failed to load WIR records for box ${box.id}:`, err);
          // Return empty array on error
          return of({ boxId: box.id, wirs: [] as WIRRecord[] });
        })
      )
    );

    // Load all WIR records in parallel
    forkJoin(wirObservables).subscribe({
      next: (results) => {
        // Build the map of boxId to WIR records
        this.boxWIRRecordsMap.clear();
        results.forEach(result => {
          this.boxWIRRecordsMap.set(result.boxId, result.wirs);
        });
      },
      error: (err: any) => {
        console.error('Error loading WIR records:', err);
        // Continue even if WIR loading fails - boxes will use default colors
      }
    });
  }

  /**
   * Update visibility of factory layout based on WIR position field state
   */
  private updateFactoryLayoutVisibility(): void {
    if (!this.progressForm || !this.currentBox?.factoryId) {
      this.showFactoryLayout = false;
      return;
    }

    // Show factory layout when WIR Position fields are editable AND factory data is loaded.
    // Do NOT require factoryBoxes.length > 0 — a factory with zero positioned boxes
    // still needs the grid so the user can pick an available cell.
    const isBayEditable = !this.progressForm.get('wirBay')?.disabled;
    const isRowEditable = !this.progressForm.get('wirRow')?.disabled;
    this.showFactoryLayout = isBayEditable && isRowEditable && this.factory !== null;
  }

  /**
   * Get simplified factory grid layout for display in modal
   * If sections are available, returns sectioned layout; otherwise returns full factory grid
   */
  getFactoryGridLayout(): any {
    if (!this.factory) {
      return { rows: [], columns: [], matrix: [], totalBoxes: 0 };
    }

    // If sections are available, return first section's grid as fallback for non-sectioned display
    if (this.factorySections.length > 0) {
      return this.getGridLayoutForSection(this.factorySections[0]);
    }

    // Fallback: Use factory's min/max row and bay values for single grid
    const minRow = this.factory.minRow ?? 1;
    const maxRow = this.factory.maxRow ?? 20;
    const minBay = this.factory.minBay ?? 'A';
    const maxBay = this.factory.maxBay ?? 'Z';

    // Generate all bays from min to max (A-Z range) based on factory configuration
    const allBays: string[] = [];
    if (minBay && maxBay) {
      const startCharCode = minBay.toUpperCase().charCodeAt(0);
      const endCharCode = maxBay.toUpperCase().charCodeAt(0);
      for (let i = startCharCode; i <= endCharCode; i++) {
        allBays.push(String.fromCharCode(i));
      }
    }

    // Generate all rows from min to max based on factory configuration
    const allRows: string[] = [];
    for (let i = minRow; i <= maxRow; i++) {
      allRows.push(i.toString());
    }

    // Use complete range
    const columns = allBays;
    const rows = allRows;

    // Create matrix structure with ALL combinations
    const matrix: any[][] = [];
    
    rows.forEach(row => {
      const rowCells: any[] = [];
      columns.forEach(column => {
        // Find box at this position
        const box = this.factoryBoxes.find(b => 
          b.bay === column && b.row === row
        );
        
        rowCells.push({
          row,
          column,
          bay: column,
          box: box || null,
          position: box?.position || null,
          isCurrentBox: box?.id === this.currentBox?.id,
          sectionId: undefined // No sections available in fallback mode
        });
      });
      matrix.push(rowCells);
    });

    return {
      rows,
      columns,
      matrix,
      totalBoxes: this.factoryBoxes.length
    };
  }

  /**
   * Get grid layout for a specific section (transposed: rows as columns, bays as rows)
   */
  getGridLayoutForSection(section: FactorySection): any {
    if (!this.factory) {
      return { rows: [], columns: [], matrix: [], totalBoxes: 0, sectionId: section.sectionId, sectionName: section.sectionName };
    }

    const minRow = section.minRow ?? 1;
    const maxRow = section.maxRow ?? 20;
    const minBay = section.minBay ?? 'A';
    const maxBay = section.maxBay ?? 'Z';

    // Generate all bays from min to max for this section
    const allBays: string[] = [];
    if (minBay && maxBay) {
      const startCharCode = minBay.toUpperCase().charCodeAt(0);
      const endCharCode = maxBay.toUpperCase().charCodeAt(0);
      for (let i = startCharCode; i <= endCharCode; i++) {
        allBays.push(String.fromCharCode(i));
      }
    }

    // Generate all rows from min to max for this section
    const allRows: number[] = [];
    for (let i = minRow; i <= maxRow; i++) {
      allRows.push(i);
    }

    // TRANSPOSED: Rows go across (columns), Bays go down (rows)
    let columns = allRows.map(r => r.toString()); // Row numbers across the top
    const rows = allBays; // Bay letters down the side

    // Reverse columns for Finishing sections (right-to-left)
    if (section.sectionType === FactorySectionType.Finishing) {
      columns = [...columns].reverse();
    }

    // Create matrix structure: Each matrix row represents a BAY, each cell represents a ROW
    const matrix: any[][] = [];
    
    rows.forEach(bay => {
      const rowCells: any[] = [];
      columns.forEach(column => {
        const rowNumber = column; // column IS the row number
        
        // Find box at this position (within this section's ranges)
        // Filter by bay, row, and sectionId - only show boxes that match the section
        const box = this.factoryBoxes.find(b => {
          const bayMatch = b.bay === bay;
          const rowMatch = b.row === rowNumber;
          // If section has an ID, box MUST have a matching factorySectionId
          // If section doesn't have an ID, show boxes from all sections (backward compatibility)
          const sectionMatch = section.sectionId 
            ? (b.factorySectionId && b.factorySectionId === section.sectionId)
            : true;
          return bayMatch && rowMatch && sectionMatch;
        });
        
        rowCells.push({
          row: rowNumber,
          column: rowNumber, // For consistency
          bay: bay,
          box: box || null,
          position: box?.position || null,
          isCurrentBox: box?.id === this.currentBox?.id,
          hidden: false,
          sectionId: section.sectionId // Add section ID to identify which section this cell belongs to
        });
      });
      matrix.push(rowCells);
    });

    // Count boxes in this section (filter by bay, row, and sectionId)
    const sectionBoxCount = this.factoryBoxes.filter(b => {
      const bayInRange = b.bay && allBays.includes(b.bay);
      const rowInRange = b.row && allRows.includes(parseInt(b.row));
      // If section has an ID, box MUST have a matching factorySectionId
      const sectionMatch = section.sectionId 
        ? (b.factorySectionId && b.factorySectionId === section.sectionId)
        : true;
      return bayInRange && rowInRange && sectionMatch;
    }).length;

    return {
      rows,
      columns,
      matrix,
      totalBoxes: sectionBoxCount,
      sectionId: section.sectionId,
      sectionName: section.sectionName
    };
  }

  /**
   * Get grid layout for a specific part within a section
   * Uses the part's specific bay range and row range
   */
  getGridLayoutForPart(part: FactorySectionPart, section: FactorySection): any {
    if (!this.factory) {
      return { rows: [], columns: [], matrix: [], totalBoxes: 0, partId: part.partId, partName: part.partName };
    }

    // Use part's bay range
    const minBay = part.minBay ?? 'A';
    const maxBay = part.maxBay ?? 'Z';
    
    // Use PART's row range
    const minRow = part.minRow ?? 1;
    const maxRow = part.maxRow ?? 12;

    // Generate all bays from min to max for this part
    const allBays: string[] = [];
    if (minBay && maxBay) {
      const startCharCode = minBay.toUpperCase().charCodeAt(0);
      const endCharCode = maxBay.toUpperCase().charCodeAt(0);
      for (let i = startCharCode; i <= endCharCode; i++) {
        allBays.push(String.fromCharCode(i));
      }
    }

    // Generate all rows from min to max for this part
    const allRows: number[] = [];
    for (let i = minRow; i <= maxRow; i++) {
      allRows.push(i);
    }

    // TRANSPOSED: Rows go across (columns), Bays go down (rows)
    // Always reverse for consistent right-to-left flow (matches factory layout page)
    let columns = allRows.map(r => r.toString()).reverse();
    const rows = allBays;

    // Create matrix structure
    const matrix: any[][] = [];
    
    rows.forEach(bay => {
      const rowCells: any[] = [];
      columns.forEach(column => {
        const rowNumber = column;
        
        // Find box at this position (within this part's ranges)
        // Filter by bay, row, and sectionId - only show boxes that match the section
        const box = this.factoryBoxes.find(b => {
          const bayMatch = b.bay === bay;
          const rowMatch = b.row === rowNumber;
          // If section has an ID, box MUST have a matching factorySectionId
          // If section doesn't have an ID, show boxes from all sections (backward compatibility)
          const sectionMatch = section.sectionId 
            ? (b.factorySectionId && b.factorySectionId === section.sectionId)
            : true;
          return bayMatch && rowMatch && sectionMatch;
        });
        
        rowCells.push({
          row: rowNumber,
          column: rowNumber,
          bay: bay,
          box: box || null,
          position: box?.position || null,
          isCurrentBox: box?.id === this.currentBox?.id,
          hidden: false,
          sectionId: section.sectionId,
          partId: part.partId
        });
      });
      matrix.push(rowCells);
    });

    return {
      rows,
      columns,
      matrix,
      totalBoxes: 0,
      partId: part.partId,
      partName: part.partName,
      partNumber: part.partNumber
    };
  }

  /**
   * Get layout configuration with sections ordered by DisplayOrder from backend
   * Sections are rendered in natural grid order (left-to-right, top-to-bottom) based on DisplayOrder
   * 
   * Expected DisplayOrder example:
   * 1 → Finishing-2 (Top-Left)
   * 2 → Assembly-2 (Top-Right)
   * 3 → Finishing-1 (Bottom-Left)
   * 4 → Assembly-1 (Bottom-Right)
   */
  getLayoutConfiguration(): any {
    if (!this.factory || this.factorySections.length === 0) {
      return {
        leftColumn: { sections: [] },
        rightColumn: { sections: [] },
        hasSections: false
      };
    }

    console.log('🔍 Update Progress Modal - Layout Configuration:', this.factorySections.map(s => 
      `${s.sectionName} (DisplayOrder: ${s.displayOrder}, Type: ${s.sectionType})`
    ));

    // Backend already ordered sections by DisplayOrder ASC
    // Split sections into rows based on DisplayOrder
    // Top row: first 2 sections (indices 0, 1)
    // Bottom row: next 2 sections (indices 2, 3)
    const topRowSections = this.factorySections.slice(0, 2);
    const bottomRowSections = this.factorySections.slice(2, 4);

    // Map sections to include their grid data and sort parts by minBay
    const mapSectionToData = (section: FactorySection) => {
      // Sort parts by minBay to ensure they appear in the correct bay order (B-D, E-G, etc.)
      const sortedParts = (section.parts || []).sort((a, b) => {
        if (!a.minBay || !b.minBay) return 0;
        return a.minBay.localeCompare(b.minBay);
      });

      return {
        section: section,
        grid: this.getGridLayoutForSection(section),
        parts: sortedParts.map(part => ({
          ...part,
          grid: this.getGridLayoutForPart(part, section)
        }))
      };
    };

    return {
      leftColumn: {
        // LEFT COLUMN: sections at indices 0 (top) and 2 (bottom)
        sections: [
          topRowSections[0] ? mapSectionToData(topRowSections[0]) : null,
          bottomRowSections[0] ? mapSectionToData(bottomRowSections[0]) : null
        ].filter(s => s !== null)
      },
      rightColumn: {
        // RIGHT COLUMN: sections at indices 1 (top) and 3 (bottom)
        sections: [
          topRowSections[1] ? mapSectionToData(topRowSections[1]) : null,
          bottomRowSections[1] ? mapSectionToData(bottomRowSections[1]) : null
        ].filter(s => s !== null)
      },
      hasSections: true
    };
  }

  /**
   * Get tooltip for grid cell
   */
  getGridCellTooltip(cell: any): string {
    if (cell.box) {
      const status = cell.box.status || 'Unknown';
      const isCurrent = cell.isCurrentBox ? ' (Current Box)' : '';
      const wirStage = this.getBoxWIRStage(cell.box);
      const stageLine = wirStage ? `\nStage: ${wirStage.displayName}` : '';
      return `${cell.box.code}${isCurrent}\nBay: ${cell.bay}, Row: ${cell.row}\nPosition: ${cell.position || '-'}\nStatus: ${status}${stageLine}\nProgress: ${cell.box.progress ?? 0}%`;
    }
    return `Available - Click to select\nBay: ${cell.bay}, Row: ${cell.row}`;
  }

  /**
   * Update available bays that have at least one empty position across all sections
   * Only shows positions that are not occupied in the same section
   * Excludes frozen rows (6 and 7) from consideration
   */
  private updateAvailablePositions(): void {
    const baysWithAvailable = new Set<string>();
    
    if (this.factorySections.length > 0) {
      // Check all sections for available positions
      // A position is available if it's not occupied in the same section AND not a frozen row
      this.factorySections.forEach(section => {
        const gridLayout = this.getGridLayoutForSection(section);
        gridLayout.matrix.forEach((rowCells: any[]) => {
          rowCells.forEach((cell: any) => {
            if (this.isSectionRowFrozen(section.sectionId, cell.row)) return;
            
            // Check if this position is occupied in the same section
            const isOccupiedInSameSection = cell.box && 
              cell.box.factorySectionId === section.sectionId;
            
            if (!isOccupiedInSameSection && !cell.hidden) {
              baysWithAvailable.add(cell.bay);
            }
          });
        });
      });
    } else {
      // Fallback: use single grid layout (no sections - check all positions)
      const gridLayout = this.getFactoryGridLayout();
      gridLayout.matrix.forEach((rowCells: any[]) => {
        rowCells.forEach((cell: any) => {
          if (!cell.box) {
            baysWithAvailable.add(cell.bay);
          }
        });
      });
    }
    
    this.availableBays = Array.from(baysWithAvailable).sort((a, b) => a.localeCompare(b));
  }

  /**
   * Update available rows when a bay is selected across all sections
   * Only shows rows that are not occupied in the same section
   * Also determines which section(s) contain this bay
   * Filters out frozen rows (6 and 7) from the dropdown
   */
  onBayChange(bay: string): void {
    this.selectedBay = bay;
    this.progressForm.patchValue({ wirBay: bay });
    
    if (!bay) {
      this.availableRows = [];
      this.selectedRow = '';
      this.selectedSectionId = ''; // Clear section selection
      return;
    }
    
    const rowsWithAvailable = new Set<string>();
    const sectionsBayMap = new Map<string, Set<string>>(); // Map section to available rows
    
    if (this.factorySections.length > 0) {
      // Check all sections for available rows in this bay
      // A row is available if it's not occupied in the same section AND not a frozen row
      this.factorySections.forEach(section => {
        const gridLayout = this.getGridLayoutForSection(section);
        const sectionAvailableRows = new Set<string>();
        
        gridLayout.matrix.forEach((rowCells: any[]) => {
          rowCells.forEach((cell: any) => {
            if (cell.bay === bay) {
              if (this.isSectionRowFrozen(section.sectionId, cell.row)) return;
              
              const isOccupiedInSameSection = cell.box && 
                cell.box.factorySectionId === section.sectionId;
              
              if (!isOccupiedInSameSection && !cell.hidden) {
                sectionAvailableRows.add(cell.row);
                rowsWithAvailable.add(cell.row);
              }
            }
          });
        });
        
        // Store which rows are available in this section
        if (sectionAvailableRows.size > 0 && section.sectionId) {
          sectionsBayMap.set(section.sectionId, sectionAvailableRows);
        }
      });
      
      // Determine section selection strategy:
      // If bay exists in only ONE section, auto-select that section
      // If bay exists in multiple sections, DON'T auto-select - let user see and click
      if (sectionsBayMap.size === 1) {
        // Bay only exists in one section - auto-select it
        this.selectedSectionId = Array.from(sectionsBayMap.keys())[0];
        console.log(`📍 Bay ${bay} belongs to section:`, this.selectedSectionId);
      } else if (sectionsBayMap.size > 1) {
        // Bay exists in multiple sections - show available positions in all sections
        // User must click on grid to specify which section
        this.selectedSectionId = '';
        console.log(`⚠️ Bay ${bay} exists in ${sectionsBayMap.size} sections - available positions will be highlighted, click to select`);
      } else {
        // No sections have this bay (shouldn't happen)
        this.selectedSectionId = '';
      }
    } else {
      // Fallback: use single grid layout (no sections)
      const gridLayout = this.getFactoryGridLayout();
      gridLayout.matrix.forEach((rowCells: any[]) => {
        rowCells.forEach((cell: any) => {
          if (cell.bay === bay && !cell.box) {
            rowsWithAvailable.add(cell.row);
          }
        });
      });
      this.selectedSectionId = ''; // No sections
    }
    
    // Sort rows numerically
    this.availableRows = Array.from(rowsWithAvailable)
      .sort((a, b) => {
        const numA = parseInt(a) || 0;
        const numB = parseInt(b) || 0;
        return numA - numB;
      });
  }

  /**
   * Update selected row and highlight position
   * Section ID is already determined by onBayChange, so we don't clear it
   */
  onRowChange(row: string): void {
    this.selectedRow = row;
    // Don't clear selectedSectionId here - it was set by onBayChange
    this.progressForm.patchValue({ wirRow: row });
  }

  /**
   * Handle cell click - auto-fill WIR position inputs
   * Only allows clicking on cells that are not occupied in the same section
   */
  onCellClick(cell: any): void {
    // Check if this position is occupied in the same section
    // If a box exists and it's in the same section, prevent selection
    if (cell.box && cell.sectionId && cell.box.factorySectionId === cell.sectionId) {
      return;
    }
    
    // Check if fields are editable
    const isBayEditable = !this.progressForm.get('wirBay')?.disabled;
    const isRowEditable = !this.progressForm.get('wirRow')?.disabled;
    
    if (!isBayEditable || !isRowEditable) {
      return;
    }
    
    // Fill the form with selected position
    this.selectedBay = cell.bay;
    this.selectedRow = cell.row;
    
    this.progressForm.patchValue({
      wirBay: cell.bay,
      wirRow: cell.row
    });
    
    // Update available rows for the selected bay
    // onBayChange will auto-detect the section if bay belongs to only one section
    this.onBayChange(cell.bay);
    
    // If the cell has a specific section ID and onBayChange didn't set it
    // (e.g., bay exists in multiple sections), use the clicked cell's section
    if (cell.sectionId && !this.selectedSectionId) {
      this.selectedSectionId = cell.sectionId;
      console.log(`📍 Using clicked cell's section: ${cell.sectionId}`);
    }
  }

  /**
   * Check if a cell is selected
   * Highlights cells that match bay/row AND are available (not occupied in that section)
   * This allows user to see all possible positions and click on the one they want
   */
  isCellSelected(cell: any): boolean {
    const bayMatch = cell.bay === this.selectedBay;
    const rowMatch = cell.row === this.selectedRow;
    
    // If no bay or row is selected, nothing is highlighted
    if (!this.selectedBay || !this.selectedRow) {
      return false;
    }
    
    // If section is already confirmed (clicked from grid), only highlight that specific cell
    if (this.selectedSectionId) {
      return bayMatch && rowMatch && cell.sectionId === this.selectedSectionId;
    }
    
    // If section is not yet confirmed (dropdown selection):
    // Highlight all cells that match bay/row AND are available (no box in that section)
    if (!cell.box) {
      return bayMatch && rowMatch;
    }
    
    // Don't highlight if position is occupied in this section
    return false;
  }

  /**
   * Check if user needs to click on grid to complete position selection
   * Returns true when bay and row are selected but section is not determined
   */
  get needsGridClickForSection(): boolean {
    return !!(this.selectedBay && this.selectedRow && !this.selectedSectionId);
  }

  /**
   * Get the current active WIR stage for a box
   * Used for determining the color class
   */
  getBoxWIRStage(box: Box): WIRStageInfo | null {
    const wirRecords = this.boxWIRRecordsMap.get(box.id) || [];
    return getCurrentActiveWIRStage(wirRecords, box.progress, box.status);
  }

  /**
   * Get the CSS class for a box based on its WIR stage
   * Falls back to box status if no WIR stage is available
   */
  getBoxColorClass(box: Box): string {
    const wirStage = this.getBoxWIRStage(box);
    
    if (wirStage) {
      return wirStage.colorClass;
    }
    
    // Fallback to box status if no WIR records exist
    if (box.status === BoxStatus.Completed) {
      return 'completed';
    } else if (box.status === BoxStatus.InProgress) {
      return 'in-progress';
    }
    
    return 'empty';
  }

  /**
   * Build freezing-cell lookup maps from the loaded factory data.
   * Called once after factory sections are available.
   */
  private buildFreezingCellsMap(): void {
    this.freezingCellsMap.clear();
    this.sectionFrozenRowsMap.clear();
    if (!this.factory?.sections) return;

    for (const section of this.factory.sections) {
      const sectionFrozen = new Set<number>();
      for (const part of (section.parts || [])) {
        if (part.partId) {
          const partFrozen = new Set<number>(
            (part.freezingCells || []).map((c: any) => c.rowNumber as number)
          );
          this.freezingCellsMap.set(part.partId, partFrozen);
          partFrozen.forEach(r => sectionFrozen.add(r));
        }
      }
      if (section.sectionId) {
        this.sectionFrozenRowsMap.set(section.sectionId, sectionFrozen);
      }
    }
  }

  /** True when the given row string is frozen for the given part. */
  isRowFrozen(partId: string | undefined, rowStr: string): boolean {
    if (!partId) return false;
    return this.freezingCellsMap.get(partId)?.has(parseInt(rowStr, 10)) ?? false;
  }

  /** True when the given row string is frozen for any part in the given section. */
  isSectionRowFrozen(sectionId: string | undefined, rowStr: string): boolean {
    if (!sectionId) return false;
    return this.sectionFrozenRowsMap.get(sectionId)?.has(parseInt(rowStr, 10)) ?? false;
  }

  /**
   * Find the nearest NEXT WIR record (at or after current activity) to inherit position values
   * 
   * CRITICAL REQUIREMENT:
   * - All activities up to a WIR should show the SAME position values from that WIR
   * - If WIR has position values already set, they are LOCKED (read-only) for all activities
   * - If WIR doesn't have position values yet, fields are editable
   * - Activities 1-4 before WIR-1 all show WIR-1's position (e.g., Bay: Z, Row: 88, Position: Z-88)
   * - Activities 5-8 before WIR-2 all show WIR-2's position
   * - And so on...
   */
  private findNearestWIRBelow(): void {
    if (!this.activity || !this.allActivities || this.allActivities.length === 0) {
      this.hasWIRBelow = false;
      this.hasWIRActivityBelow = false;
      this.nearestWIR = null;
      return;
    }

    const currentSequence = this.activity.sequence || 0;
    
    // Find the nearest WIR activity AT OR AFTER the current activity
    const nextWIRActivities = this.allActivities
      .filter(a => {
        const isWIR = a.isWIRCheckpoint || a.activityMaster?.isWIRCheckpoint;
        const isAtOrAfter = (a.sequence || 0) >= currentSequence;
        return isWIR && isAtOrAfter;
      })
      .sort((a, b) => (a.sequence || 0) - (b.sequence || 0)); // Sort ascending to get nearest

    const nextWIRActivity = nextWIRActivities.length > 0 ? nextWIRActivities[0] : null;

    if (!nextWIRActivity) {
      // No WIR found at or after this activity - fields are editable but empty
      this.hasWIRBelow = false;
      this.hasWIRActivityBelow = false;
      this.nearestWIR = null;
      this.positionLockedReason = '';
      console.log('📍 No next WIR found - fields are editable');
      return;
    }
    
    this.hasWIRActivityBelow = true;
    console.log('📍 Found next WIR activity:', nextWIRActivity.activityName, 'at sequence', nextWIRActivity.sequence);

    // Load WIR records to get position values
    if (this.activity.boxId) {
      this.wirService.getWIRRecordsByBox(this.activity.boxId).subscribe({
        next: (wirRecords) => {
          const nextWIR = wirRecords.find(wir => wir.boxActivityId === nextWIRActivity.boxActivityId);
          
          if (nextWIR) {
            this.nearestWIR = nextWIR;
            this.hasWIRBelow = true;
            console.log('📍 Found WIR record:', nextWIR.wirCode, 'with position:', {
              bay: nextWIR.bay,
              row: nextWIR.row,
              position: nextWIR.position
            });
            
            // Populate position values from this WIR (for all activities up to it)
            this.populateWIRPositionFields(nextWIR);
          } else {
            // WIR activity exists but no WIR record yet
            // We still need to check if PREVIOUS WIR is approved before allowing edits
            this.nearestWIR = null;
            this.hasWIRBelow = false;
            console.log('📍 WIR activity exists but no WIR record yet - checking previous WIR');
            
            // Check if previous WIR is approved before allowing position edits
            this.checkPreviousWIRForLocking();
          }
        },
        error: (error) => {
          console.error('Error loading WIR records:', error);
          this.hasWIRBelow = false;
          this.nearestWIR = null;
        }
      });
    }
  }

  /**
   * Get the latest (highest version) checkpoint for a given WIR code
   * Returns null if no checkpoint exists
   * CRITICAL: When multiple checkpoint versions exist (after rejection), 
   * we must check the LATEST version, not just any version
   */
  private getLatestCheckpoint(checkpoints: WIRCheckpoint[], wirCode: string): WIRCheckpoint | null {
    if (!checkpoints || !wirCode) return null;
    
    const matchingCheckpoints = checkpoints.filter(cp => 
      cp.wirNumber && cp.wirNumber.toUpperCase() === wirCode.toUpperCase()
    );
    
    if (matchingCheckpoints.length === 0) return null;
    
    // Sort by version descending and return the first one (latest version)
    matchingCheckpoints.sort((a, b) => (b.version || 1) - (a.version || 1));
    
    console.log(`📋 Found ${matchingCheckpoints.length} checkpoint(s) for ${wirCode}, latest version: ${matchingCheckpoints[0].version || 1}`);
    
    return matchingCheckpoints[0];
  }

  /**
   * Check if position fields should be locked based on current WIR status and previous WIR status
   * Called only when WIR has NO position values set yet
   * 
   * Locking Rules:
   * 1. FIRST: Check THIS WIR status - if Pending, Rejected, or Under Review, lock fields
   * 2. THEN: Check PREVIOUS WIR - if not fully approved, lock fields
   * 3. Unlock only if BOTH THIS WIR and PREVIOUS WIR are fully approved
   * 
   * This ensures activities cannot set position until:
   * - THIS WIR is ready (not Pending/Rejected/Under Review)
   * - PREVIOUS WIR is fully approved
   */
  private checkWIRStatusAndPreviousWIRForLocking(wir: WIRRecord): void {
    if (!this.progressForm) return;
    
    // Reset locked reason
    this.positionLockedReason = '';
    
    // STEP 1: Load checkpoint and get effective status
    // This matches what activity-table.component.ts does (line 570-582)
    // The checkpoint status is the source of truth, not wir.checkpointStatus
    if (!wir.wirCode || !this.activity.boxId) {
      // No WIR code or box ID - check previous WIR
      this.checkPreviousWIRForLocking();
      return;
    }
    
    this.wirService.getWIRCheckpointsByBox(this.activity.boxId).subscribe({
      next: (checkpoints) => {
        // Get the LATEST checkpoint version for this WIR (critical fix for multiple versions)
        const currentCheckpoint = this.getLatestCheckpoint(checkpoints, wir.wirCode);
        
        // Use checkpoint.status if available, otherwise fall back to wir.status
        const effectiveStatus = currentCheckpoint?.status || wir.status;
        
        console.log('🔍 THIS WIR status check:', {
          wirCode: wir.wirCode,
          recordStatus: wir.status,
          checkpointStatus: currentCheckpoint?.status,
          effectiveStatus: effectiveStatus
        });
        
        // Compare as strings since checkpoint uses WIRCheckpointStatus, wir uses WIRStatus
        const effectiveStatusStr = String(effectiveStatus);
        const isConditionalApproval = effectiveStatusStr === 'ConditionalApproval';
        
        // Handle ConditionalApproval FIRST - it's always acceptable
        if (isConditionalApproval) {
          this.progressForm.get('wirBay')?.enable();
          this.progressForm.get('wirRow')?.enable();
          this.positionLockedReason = ''; // Clear any previous lock reason
          console.log('✅ THIS WIR is Conditionally Approved - fields editable');
          return; // Exit early, no further checks needed for this WIR
        }
        
        const isPending = effectiveStatusStr === 'Pending';
        const isRejected = effectiveStatusStr === 'Rejected';
        
        if (isPending) {
          // THIS WIR is Pending - LOCK
          this.progressForm.get('wirBay')?.disable();
          this.progressForm.get('wirRow')?.disable();
          this.positionLockedReason = `Position is locked. ${wir.wirCode} is Pending. Complete the QA/QC inspection first.`;
          console.log('🔒 Locking: THIS WIR is Pending');
          return;
        }
        
        if (isRejected) {
          // THIS WIR is Rejected - LOCK
          this.progressForm.get('wirBay')?.disable();
          this.progressForm.get('wirRow')?.disable();
          this.positionLockedReason = `Position is locked. ${wir.wirCode} was Rejected. Resolve issues first.`;
          console.log('🔒 Locking: THIS WIR is Rejected');
          return;
        }
        
        // Check if WIR is "Under Review" (Approved but not all checklist items Pass)
        if (effectiveStatusStr === 'Approved') {
          if (currentCheckpoint) {
            // Checkpoint exists - check if all items are Pass
            const allItemsPass = this.areAllChecklistItemsPass(currentCheckpoint);
            
            if (!allItemsPass) {
              // THIS WIR is Under Review - LOCK
              this.progressForm.get('wirBay')?.disable();
              this.progressForm.get('wirRow')?.disable();
              this.positionLockedReason = `Position is locked. ${wir.wirCode} is Under Review. Complete all checklist items first.`;
              console.log('🔒 Locking: THIS WIR is Under Review (not all items Pass)');
              return;
            }
            
            // WIR is fully approved - check previous WIR
            console.log('✅ THIS WIR is fully approved (all items Pass) - checking previous WIR');
            this.checkPreviousWIRForLocking();
          } else {
            // No checkpoint found but WIR is Approved - inconsistent state
            this.progressForm.get('wirBay')?.disable();
            this.progressForm.get('wirRow')?.disable();
            this.positionLockedReason = `Position is locked. ${wir.wirCode} is Approved but has no checkpoint. Create QA/QC checkpoint first.`;
            console.log('🔒 Locking: THIS WIR has no checkpoint');
            return;
          }
        } else {
          // WIR is not Approved status - check previous WIR
          this.checkPreviousWIRForLocking();
        }
      },
      error: () => {
        // Error loading checkpoint - LOCK to be safe
        this.progressForm.get('wirBay')?.disable();
        this.progressForm.get('wirRow')?.disable();
        this.positionLockedReason = `Position is locked. Unable to verify WIR status.`;
        console.log('🔒 Locking: Error loading checkpoint (safe fallback)');
      }
    });
  }

  /**
   * Check if previous WIR is fully approved before allowing position edits
   * 
   * CRITICAL: This is the MAIN locking check
   * Position fields can ONLY be editable if previous WIR is fully approved
   * 
   * Locking Conditions:
   * 1. Previous WIR doesn't exist yet → LOCK
   * 2. Previous WIR status is Pending → LOCK
   * 3. Previous WIR status is Rejected → LOCK
   * 4. Previous WIR status is "Under Review" (Approved but not all checklist items Pass) → LOCK
   * 5. Previous WIR is fully approved (Approved + all checklist items Pass) → UNLOCK
   */
  private checkPreviousWIRForLocking(): void {
    console.log('🔍 checkPreviousWIRForLocking called for activity:', this.activity?.activityName, 'sequence:', this.activity?.sequence);
    
    if (!this.activity || !this.allActivities) {
      // No previous WIR - fields are editable
      this.progressForm.get('wirBay')?.enable();
      this.progressForm.get('wirRow')?.enable();
      console.log('✅ No activity/allActivities - fields editable');
      this.updateFactoryLayoutVisibility();
      return;
    }

    const currentSequence = this.activity.sequence || 0;
    console.log('🔍 Looking for previous WIR with sequence <', currentSequence);
    
    // Find previous WIR activities (sequence < current sequence)
    const previousWIRActivities = this.allActivities
      .filter(a => {
        const isWIR = a.isWIRCheckpoint || a.activityMaster?.isWIRCheckpoint;
        const isBefore = (a.sequence || 0) < currentSequence;
        return isWIR && isBefore;
      })
      .sort((a, b) => (b.sequence || 0) - (a.sequence || 0)); // Sort descending to get nearest

    console.log('🔍 Found', previousWIRActivities.length, 'previous WIR activities:', 
      previousWIRActivities.map(a => `${a.activityName} (seq ${a.sequence})`));

    const previousWIRActivity = previousWIRActivities.length > 0 ? previousWIRActivities[0] : null;

    if (!previousWIRActivity) {
      // No previous WIR - fields are editable (this is the first WIR or activity before first WIR)
      this.progressForm.get('wirBay')?.enable();
      this.progressForm.get('wirRow')?.enable();
      console.log('✅ No previous WIR activity - fields editable');
      this.updateFactoryLayoutVisibility();
      return;
    }

    console.log('🔍 Selected previous WIR activity:', previousWIRActivity.activityName, 'at sequence', previousWIRActivity.sequence);

    // Load previous WIR record to check its status
    if (this.activity.boxId) {
      this.wirService.getWIRRecordsByBox(this.activity.boxId).subscribe({
        next: (wirRecords) => {
          const previousWIR = wirRecords.find(wir => wir.boxActivityId === previousWIRActivity.boxActivityId);
          
          if (!previousWIR) {
            // Previous WIR record doesn't exist yet - LOCK fields
            this.progressForm.get('wirBay')?.disable();
            this.progressForm.get('wirRow')?.disable();
            this.positionLockedReason = `Position is locked. Previous Stage (${previousWIRActivity.activityMaster?.wirCode || previousWIRActivity.wirCode || 'Stage'}) must be created first.`;
            console.log('🔒 Locking: Previous WIR record not created yet');
            this.updateFactoryLayoutVisibility();
            return;
          }

          // Load checkpoint to get effective status (matches activity-table.component.ts)
          // The checkpoint.status is the source of truth, not wir.checkpointStatus
          this.wirService.getWIRCheckpointsByBox(this.activity.boxId).subscribe({
            next: (checkpoints) => {
              // Get the LATEST checkpoint version for previous WIR (critical fix for multiple versions)
              const prevCheckpoint = this.getLatestCheckpoint(checkpoints, previousWIR.wirCode);
              
              // Use checkpoint.status if available, otherwise fall back to wir.status
              const effectiveStatus = prevCheckpoint?.status || previousWIR.status;
              
              console.log('🔍 Previous WIR status check:', {
                wirCode: previousWIR.wirCode,
                recordStatus: previousWIR.status,
                checkpointStatus: prevCheckpoint?.status,
                effectiveStatus: effectiveStatus
              });
              
              // Compare as strings since checkpoint uses WIRCheckpointStatus, wir uses WIRStatus
              const effectiveStatusStr = String(effectiveStatus);
              const isPrevConditionalApproval = effectiveStatusStr === 'ConditionalApproval';
              
              // Handle ConditionalApproval FIRST - it's always acceptable
              if (isPrevConditionalApproval) {
                this.progressForm.get('wirBay')?.enable();
                this.progressForm.get('wirRow')?.enable();
                this.positionLockedReason = ''; // Clear lock reason
                console.log('✅ Previous WIR is Conditionally Approved - fields editable');
                this.updateFactoryLayoutVisibility();
                return;
              }
              
              const isPrevPending = effectiveStatusStr === 'Pending';
              const isPrevRejected = effectiveStatusStr === 'Rejected';
              
              if (isPrevPending) {
                // Previous WIR is Pending - LOCK
                this.progressForm.get('wirBay')?.disable();
                this.progressForm.get('wirRow')?.disable();
                this.positionLockedReason = `Position is locked. Previous WIR (${previousWIR.wirCode}) is Pending. It must be approved first.`;
                console.log('🔒 Locking: Previous WIR is Pending');
                this.updateFactoryLayoutVisibility();
                return;
              }
              
              if (isPrevRejected) {
                // Previous WIR is Rejected - LOCK
                this.progressForm.get('wirBay')?.disable();
                this.progressForm.get('wirRow')?.disable();
                this.positionLockedReason = `Position is locked. Previous WIR (${previousWIR.wirCode}) was Rejected. Issues must be resolved.`;
                console.log('🔒 Locking: Previous WIR is Rejected');
                this.updateFactoryLayoutVisibility();
                return;
              }
              
              // Check if previous WIR is "Under Review" (Approved but not all checklist items Pass)
              if (effectiveStatusStr === 'Approved') {
                if (prevCheckpoint) {
                  // Checkpoint exists - check if all items are Pass
                  const allItemsPass = this.areAllChecklistItemsPass(prevCheckpoint);
                  
                  if (!allItemsPass) {
                    // Previous WIR is Under Review - LOCK
                    this.progressForm.get('wirBay')?.disable();
                    this.progressForm.get('wirRow')?.disable();
                    this.positionLockedReason = `Position is locked. Previous WIR (${previousWIR.wirCode}) is Under Review. All checklist items must be Pass.`;
                    console.log('🔒 Locking: Previous WIR is Under Review (not all items Pass)');
                  } else {
                    // Previous WIR is fully approved - UNLOCK
                    this.progressForm.get('wirBay')?.enable();
                    this.progressForm.get('wirRow')?.enable();
                    this.positionLockedReason = ''; // Clear lock reason
                    console.log('✅ Previous WIR fully approved (all items Pass) - fields editable');
                  }
                  this.updateFactoryLayoutVisibility();
                } else {
                  // No checkpoint found but WIR is Approved - inconsistent state
                  this.progressForm.get('wirBay')?.disable();
                  this.progressForm.get('wirRow')?.disable();
                  this.positionLockedReason = `Position is locked. Previous WIR (${previousWIR.wirCode}) is Approved but has no checkpoint. Create QA/QC checkpoint first.`;
                  console.log('🔒 Locking: Previous WIR has no checkpoint');
                  this.updateFactoryLayoutVisibility();
                }
              } else {
                // Other statuses - UNLOCK (fallback)
                this.progressForm.get('wirBay')?.enable();
                this.progressForm.get('wirRow')?.enable();
                this.positionLockedReason = ''; // Clear lock reason
                console.log('✅ Previous WIR status allows editing - fields editable');
                this.updateFactoryLayoutVisibility();
              }
            },
            error: () => {
              // Error loading checkpoints - LOCK to be safe
              this.progressForm.get('wirBay')?.disable();
              this.progressForm.get('wirRow')?.disable();
              this.positionLockedReason = `Position is locked. Unable to verify previous WIR status.`;
              console.log('🔒 Locking: Error loading checkpoints (safe fallback)');
              this.updateFactoryLayoutVisibility();
            }
          });
        },
        error: () => {
          // Error loading WIR records - assume editable (fallback)
          this.progressForm.get('wirBay')?.enable();
          this.progressForm.get('wirRow')?.enable();
          console.log('✅ Error loading WIR records - fields editable (fallback)');
        }
      });
    }
  }

  /**
   * Populate WIR position fields from the next WIR record
   * 
   * CRITICAL REQUIREMENT:
   * - All activities up to a WIR inherit and show the SAME position values from that WIR
   * - If WIR has position values set, lock the fields (read-only, cannot edit)
   * - If WIR has no position values yet, fields are editable
   * 
   * Examples:
   * - Activities 1-4 updating → Show WIR-1 position (Bay: Z, Row: 88, Position: Z-88), locked if set
   * - WIR-1 itself updating → Show its own position, locked if already set
   * - Activities 5-8 updating → Show WIR-2 position, locked if set
   */
  private populateWIRPositionFields(wir: WIRRecord): void {
    if (!this.progressForm) return;
    
    const bayValue = (wir.bay && wir.bay.trim()) || '';
    const rowValue = (wir.row && wir.row.trim()) || '';
    const positionValue = (wir.position && wir.position.trim()) || '';
    
    // Check if WIR has position values already set (not empty strings)
    const hasPositionValues = !!(bayValue || rowValue || positionValue);
    
    console.log('📝 Populating position fields from WIR:', {
      wirCode: wir.wirCode,
      bay: bayValue,
      row: rowValue,
      position: positionValue,
      hasValues: hasPositionValues,
      recordStatus: wir.status,
      checkpointStatus: wir.checkpointStatus
    });
    
    // Always populate the values from the WIR (inherited by all activities up to this WIR)
    this.progressForm.patchValue({
      wirBay: bayValue,
      wirRow: rowValue,
      wirPosition: positionValue
    }, { emitEvent: false });
    
    // Sync with dropdown selections
    this.selectedBay = bayValue;
    this.selectedRow = rowValue;
    if (bayValue) {
      this.onBayChange(bayValue);
    }
    
    // Load checkpoint for this WIR if not already loaded
    if (wir.wirCode && !this.nearestWIRCheckpoint) {
      this.loadWIRCheckpoint(wir.wirCode);
    }
    
    // LOCKING LOGIC: Only lock if WIR already has position values set
    // If position is not set, allow editing regardless of WIR status
    if (hasPositionValues) {
        this.progressForm.get('wirBay')?.disable();
        this.progressForm.get('wirRow')?.disable();
      this.positionLockedReason = `Position is locked. ${wir.wirCode} already has position values set (Bay: ${bayValue || '-'}, Row: ${rowValue || '-'}, Position: ${positionValue || '-'}).`;
      console.log('🔒 Locking position fields: WIR has values set');
    } else {
      // WIR has no position values yet - allow editing, but check previous WIR for dependencies
      console.log('✅ WIR has no position values yet - fields are editable, checking previous WIR dependencies');
      this.progressForm.get('wirBay')?.enable();
      this.progressForm.get('wirRow')?.enable();
      this.positionLockedReason = ''; // Clear any lock reason
      // Only check previous WIR - don't lock based on current WIR status when position is not set
      this.checkPreviousWIRForLocking();
    }
    
    // Update factory layout visibility based on field editability
    this.updateFactoryLayoutVisibility();
  }


  /**
   * Check if all checklist items in a checkpoint have PASS status
   * Matches the logic from activity-table.component.ts
   */
  private areAllChecklistItemsPass(checkpoint: WIRCheckpoint): boolean {
    if (!checkpoint.checklistItems || checkpoint.checklistItems.length === 0) {
      return false;
    }

    return checkpoint.checklistItems.every(item => {
      const status = item.status;
      if (!status) return false;
      const statusStr = typeof status === 'string' ? status : String(status);
      return statusStr === 'Pass' || statusStr === 'pass';
    });
  }

  /**
   * Load WIR checkpoint for a specific WIR code
   */
  private loadWIRCheckpoint(wirCode: string): void {
    if (!this.activity.boxId) return;
    
    this.wirService.getWIRCheckpointsByBox(this.activity.boxId).subscribe({
      next: (checkpoints) => {
        // Get the LATEST checkpoint version for this WIR (critical fix for multiple versions)
        const checkpoint = this.getLatestCheckpoint(checkpoints, wirCode);
        
        if (checkpoint) {
          this.nearestWIRCheckpoint = checkpoint;
          // Re-evaluate position field locking after checkpoint is loaded
          if (this.nearestWIR) {
            this.populateWIRPositionFields(this.nearestWIR);
          }
        }
      },
      error: (error) => {
        console.error('Error loading WIR checkpoint:', error);
      }
    });
  }

  onFileSelected(event: any): void {
    const files = event.target.files;
    if (files && files.length > 0) {
      Array.from(files as FileList).forEach((file: File) => {
        if (file.type.startsWith('image/')) {
          this.addImageFile(file);
        } else {
          this.photoUploadError = 'Please select image files only';
        }
      });
      // Reset file input to allow selecting the same file again
      const fileInput = event.target as HTMLInputElement;
      if (fileInput) {
        fileInput.value = '';
      }
    }
  }

  addImageFile(file: File): void {
    const reader = new FileReader();
    reader.onload = (e) => {
      this.selectedImages.push({
        type: 'file',
        file: file,
        preview: e.target?.result as string,
        name: file.name
      });
      this.photoInputMethod = 'upload';
      this.photoUploadError = '';
    };
    reader.readAsDataURL(file);
  }

  addImageFromDataUrl(imageData: string): void {
    // Convert data URL to File for consistency with existing structure
    fetch(imageData)
      .then(res => res.blob())
      .then(blob => {
        const file = new File([blob], `progress-photo-${Date.now()}.jpg`, { type: 'image/jpeg' });
        this.selectedImages.push({
          type: 'file',
          file: file,
          preview: imageData,
          name: file.name
        });
        this.photoInputMethod = 'camera';
        this.photoUploadError = '';
      })
      .catch(err => {
        console.error('Error converting data URL to file:', err);
        this.photoUploadError = 'Failed to process captured image.';
      });
  }

  addImageUrl(url: string): void {
    if (url && url.trim()) {
      // Validate URL format
      try {
        new URL(url);
        this.selectedImages.push({
          type: 'url',
          url: url.trim(),
          preview: url.trim() // Use URL as preview
        });
        this.currentPhotoUrl = '';
        this.photoInputMethod = 'url';
        this.photoUploadError = '';
      } catch {
        this.photoUploadError = 'Please enter a valid URL';
      }
    }
  }

  openFileInput(): void {
    this.showCamera = false;
    const fileInput = document.getElementById('progress-photo-file-input') as HTMLInputElement;
    if (fileInput) {
      fileInput.click();
    }
  }

  removeImage(index: number): void {
    this.selectedImages.splice(index, 1);
    this.photoUploadError = '';
  }

  clearAllImages(): void {
    this.selectedImages = [];
    this.currentPhotoUrl = '';
    this.photoUploadError = '';
  }


  // Camera methods
  async openCamera(): Promise<void> {
    try {
      const stream = await navigator.mediaDevices.getUserMedia({ 
        video: { 
          facingMode: 'environment' // Use back camera on mobile
        } 
      });
      this.cameraStream = stream;
      this.showCamera = true;
      this.photoInputMethod = 'camera';
      
      // Wait for video element to be rendered
      setTimeout(() => {
        const video = document.getElementById('progress-camera-preview') as HTMLVideoElement;
        const cameraContainer = document.getElementById('camera-preview-container') as HTMLElement;
        
        if (!video) {
          console.error('Video element not found');
          return;
        }
        
        // Set video source and play
        video.srcObject = stream;
        video.muted = true;
        video.playsInline = true;
        video.autoplay = true;
        video.play();
        
        // Wait for video to start playing before going fullscreen
        const handlePlaying = () => {
          console.log('Video is playing, requesting fullscreen');
          
          // Request fullscreen for camera container
          if (cameraContainer) {
            const requestFullscreen = () => {
              if (cameraContainer.requestFullscreen) {
                return cameraContainer.requestFullscreen();
              } else if ((cameraContainer as any).webkitRequestFullscreen) {
                return (cameraContainer as any).webkitRequestFullscreen();
              } else if ((cameraContainer as any).mozRequestFullScreen) {
                return (cameraContainer as any).mozRequestFullScreen();
              } else if ((cameraContainer as any).msRequestFullscreen) {
                return (cameraContainer as any).msRequestFullscreen();
              }
              return Promise.reject('Fullscreen not supported');
            };
            
            requestFullscreen().catch((err: unknown) => {
              console.warn('Error attempting to enable fullscreen:', err);
            });
          }
        };
        
        // Ensure video plays
        video.play().then(() => {
          console.log('Video play() resolved');
          // Wait a bit more to ensure video is actually rendering
          setTimeout(() => {
            if (video.readyState >= 2 && video.videoWidth > 0) {
              handlePlaying();
            } else {
              // Fallback: wait for playing event
              video.addEventListener('playing', handlePlaying, { once: true });
            }
          }, 300);
        }).catch(err => {
          console.error('Error playing video:', err);
          this.photoUploadError = 'Error starting camera preview.';
        });
        
        // Also listen for playing event as backup
        video.addEventListener('playing', () => {
          console.log('Video playing event fired');
        }, { once: true });
        
      }, 100);
    } catch (err) {
      console.error('Error accessing camera:', err);
      this.photoUploadError = 'Unable to access camera. Please check permissions.';
      this.showCamera = false;
    }
  }

  stopCamera(): void {
    // Exit fullscreen if active
    this.exitFullscreen();
    
    // Stop video stream
    if (this.cameraStream) {
      this.cameraStream.getTracks().forEach(track => track.stop());
      this.cameraStream = null;
    }
    
    // Clear video element
    const video = document.getElementById('progress-camera-preview') as HTMLVideoElement;
    if (video) {
      const stream = video.srcObject as MediaStream;
      if (stream) {
        stream.getTracks().forEach(track => track.stop());
      }
      video.srcObject = null;
      video.pause();
    }
    
    this.showCamera = false;
  }

  capturePhoto(): void {
    const video = document.getElementById('progress-camera-preview') as HTMLVideoElement;
    
    if (!video) {
      this.photoUploadError = 'Camera element not found.';
      return;
    }
    
    // Check if video has valid dimensions
    if (!video.videoWidth || !video.videoHeight || video.videoWidth === 0 || video.videoHeight === 0) {
      console.warn('Video dimensions not ready:', { 
        width: video.videoWidth, 
        height: video.videoHeight,
        readyState: video.readyState 
      });
      this.photoUploadError = 'Camera not ready. Please wait a moment and try again.';
      return;
    }

    // Check if video is actually playing
    if (video.readyState < 2) {
      console.warn('Video not ready:', { readyState: video.readyState });
      this.photoUploadError = 'Camera stream not ready. Please wait a moment.';
      return;
    }
    
    // Check if video is paused
    if (video.paused) {
      console.warn('Video is paused, attempting to play');
      video.play().catch(err => {
        console.error('Error playing video for capture:', err);
        this.photoUploadError = 'Camera is paused. Please try again.';
        return;
      });
    }

    try {
      // Create canvas and draw video frame
      const canvas = document.createElement('canvas');
      canvas.width = video.videoWidth;
      canvas.height = video.videoHeight;
      
      const ctx = canvas.getContext('2d');
      
      if (!ctx) {
        this.photoUploadError = 'Unable to create canvas context.';
        return;
      }
      
      // Draw the current video frame to canvas
      ctx.drawImage(video, 0, 0, canvas.width, canvas.height);
      
      // Convert to Base64 data URL
      const imageData = canvas.toDataURL('image/jpeg', 0.9);
      
      // Stop camera stream immediately before any async operations
      const stream = video.srcObject as MediaStream;
      if (stream) {
        stream.getTracks().forEach((track) => track.stop());
      }
      
      // Clear video element srcObject
      video.srcObject = null;
      
      // Clear camera stream reference
      if (this.cameraStream) {
        this.cameraStream.getTracks().forEach((track) => track.stop());
        this.cameraStream = null;
      }
      
      // Close camera UI immediately
      this.showCamera = false;
      
      // Exit fullscreen
      this.exitFullscreen();
      
      // Add the captured image to the list
      this.addImageFromDataUrl(imageData);
      
    } catch (err) {
      console.error('Error capturing photo:', err);
      this.photoUploadError = 'Error capturing image. Please try again.';
      // Ensure camera is stopped even on error
      this.stopCamera();
    }
  }

  private exitFullscreen(): void {
    if (document.fullscreenElement || (document as any).webkitFullscreenElement || 
        (document as any).mozFullScreenElement || (document as any).msFullscreenElement) {
      if (document.exitFullscreen) {
        document.exitFullscreen().catch(err => console.warn('Error exiting fullscreen:', err));
      } else if ((document as any).webkitExitFullscreen) {
        (document as any).webkitExitFullscreen();
      } else if ((document as any).mozCancelFullScreen) {
        (document as any).mozCancelFullScreen();
      } else if ((document as any).msExitFullscreen) {
        (document as any).msExitFullscreen();
      }
    }
  }

  incrementProgress(amount: number): void {
    const currentProgress = this.progressForm.get('progressPercentage')?.value || 0;
    const newProgress = Math.min(100, Math.max(0, currentProgress + amount));
    this.progressForm.patchValue({ progressPercentage: newProgress });
  }

  /**
   * Check if a progress update has at least one image (images array or legacy photo field).
   */
  private progressUpdateHasAtLeastOneImage(update: ProgressUpdate): boolean {
    if (update.images && Array.isArray(update.images) && update.images.length > 0) {
      return true;
    }
    const photo = (update as any).photo || (update as any).Photo;
    if (!photo) return false;
    try {
      const parsed = JSON.parse(photo);
      if (Array.isArray(parsed) && parsed.some((url: string) => url && typeof url === 'string' && url.trim().length > 0)) {
        return true;
      }
    } catch {
      if (typeof photo === 'string' && photo.trim().length > 0) {
        return true;
      }
    }
    return false;
  }

  async onSubmit(): Promise<void> {
    if (this.isProjectArchived || this.isProjectOnHold) {
      this.errorMessage = this.isProjectArchived 
        ? 'Cannot update progress. This project is archived and read-only.' 
        : 'Cannot update progress. This project is on hold. Only project status changes are allowed.';
      return;
    }

    const isCompleted = this.activity.status === ActivityProgressStatus.Completed || this.activity.status === ActivityProgressStatus.Delayed;
    
    // For completed activities, only validate position fields (if WIR checkpoint)
    // Progress percentage is disabled and will use current value
    if (!isCompleted && this.progressForm.invalid) {
      this.errorMessage = 'Please fill in all required fields correctly';
      console.error('❌ Form is invalid:', this.progressForm.errors);
      console.error('❌ Invalid fields:', Object.keys(this.progressForm.controls).filter(key => this.progressForm.get(key)?.invalid));
      return;
    }
    
    // For completed activities, check if there's any position data to update
    if (isCompleted) {
      const formValues = this.progressForm.getRawValue();
      const hasPositionData = !!(formValues.wirBay?.toString().trim() || formValues.wirRow?.toString().trim());
      
      if (!hasPositionData && !this.isWIRCheckpoint) {
        this.errorMessage = 'For completed activities, you can only update the position. Please provide Bay and/or Row values.';
        return;
      }
    }

    // 100% image validation is enforced by the backend; any error is shown in a toast

    console.log('✅ Submitting progress update:', {
      isWIRCheckpoint: this.isWIRCheckpoint,
      activityName: this.activity.activityName,
      hasNearestWIR: !!this.nearestWIR
    });

    this.isSubmitting = true;
    this.errorMessage = '';
    this.successMessage = '';
    this.photoUploadError = '';

    try {
      // Truncate device info to 100 characters (database limit)
      const deviceInfo = navigator.userAgent.substring(0, 100);

      // Separate files and URLs
      const files: File[] = this.selectedImages
        .filter(img => img.type === 'file' && img.file)
        .map(img => img.file!);
      
      const imageUrls: string[] = this.selectedImages
        .filter(img => img.type === 'url' && img.url)
        .map(img => img.url!);

      // Extract file names (preserve original filenames for versioning)
      const fileNames: string[] = this.selectedImages
        .filter(img => img.type === 'file' && img.file)
        .map(img => img.name || img.file!.name);

      console.log('📎 VERSION DEBUG - Uploading files with names:', fileNames);

      // Use getRawValue() to include disabled field values (wirBay, wirRow, wirPosition)
      const formValues = this.progressForm.getRawValue();
      const isCompleted = this.activity.status === ActivityProgressStatus.Completed || this.activity.status === ActivityProgressStatus.Delayed;
      
      // For completed activities, use the current progress percentage (don't allow changes)
      const progressPercentage = isCompleted 
        ? (this.activity.progressPercentage || 100)
        : (formValues.progressPercentage || 0);

      const request = {
        boxId: this.activity.boxId,
        boxActivityId: this.activity.boxActivityId,
        progressPercentage: progressPercentage,
        workDescription: isCompleted ? undefined : (formValues.workDescription || undefined),
        issuesEncountered: isCompleted ? undefined : (formValues.issuesEncountered || undefined),
        updateMethod: 'Web',
        deviceInfo: deviceInfo,
        // Include WIR position fields for ALL activities (if they have values)
        wirBay: formValues.wirBay?.toString().trim() || undefined,
        wirRow: formValues.wirRow?.toString().trim() || undefined,
        wirPosition: formValues.wirPosition?.toString().trim() || undefined,
        // Include section ID selected by user (stored when clicking on factory grid)
        wirFactorySectionId: this.selectedSectionId || undefined
      };

      this.progressUpdateService.createProgressUpdate(request, files, imageUrls, fileNames).subscribe({
        next: (response) => {
          this.successMessage = 'Progress updated successfully!';
          
          // Clear cache for this box so progress history shows updated data
          this.progressUpdateService.clearCache(this.activity.boxId);
          
          // Show WIR creation message if applicable
          if (response.wirCreated) {
            this.successMessage += ` WIR ${response.wirCode} has been automatically created for QC inspection.`;
          }

          setTimeout(() => {
            this.progressUpdated.emit(response);
            this.close();
            // Parents (activity-details / activity-table) refresh only activities on progressUpdated
          }, 2000);
        },
        error: (error) => {
          console.error('❌ Error updating progress:', error);
          
          // Extract error message from backend (Result.Error.Description or Message)
          const backendMessage = error.error?.error?.description ||
                                 error.error?.message ||
                                 error.error?.error?.message ||
                                 error.error?.title ||
                                 error.message ||
                                 'Failed to update progress. Please try again.';
          this.errorMessage = backendMessage;
          this.isSubmitting = false;

          // Show backend error in toast alert
          document.dispatchEvent(new CustomEvent('app-toast', {
            detail: { message: backendMessage, type: 'error' }
          }));
          
          // Scroll error into view after a brief delay
          setTimeout(() => {
            const errorAlert = document.querySelector('.alert-danger');
            if (errorAlert) {
              errorAlert.scrollIntoView({ behavior: 'smooth', block: 'nearest' });
            }
          }, 100);
        }
      });
    } catch (error) {
      console.error('Error:', error);
      this.errorMessage = 'An unexpected error occurred';
      this.isSubmitting = false;
    }
  }

  close(): void {
    // Stop camera if it's open
    this.stopCamera();
    
    this.progressForm.reset();
    this.selectedFiles = [];
    this.selectedImages = [];
    this.currentPhotoUrl = '';
    this.showCamera = false;
    this.errorMessage = '';
    this.successMessage = '';
    this.nearestWIRCheckpoint = null; // Clear checkpoint
    this.positionLockedReason = ''; // Clear locked reason
    this.selectedBay = '';
    this.selectedRow = '';
    this.selectedSectionId = ''; // Clear selected section
    this.availableRows = [];
    this.closeModal.emit();
  }

  get isWIRCheckpoint(): boolean {
    return this.activity?.isWIRCheckpoint || false;
  }

  get wirCode(): string {
    return this.activity?.wirCode || '';
  }
}

