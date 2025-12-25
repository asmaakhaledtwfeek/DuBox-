import { Component, EventEmitter, Input, OnChanges, OnDestroy, OnInit, Output, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators, FormsModule } from '@angular/forms';
import { ProgressUpdateService } from '../../../core/services/progress-update.service';
import { WIRService } from '../../../core/services/wir.service';
import { ActivityProgressStatus, BoxActivityDetail } from '../../../core/models/progress-update.model';
import { WIRRecord, WIRStatus, WIRCheckpoint, CheckpointStatus } from '../../../core/models/wir.model';

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

  constructor(
    private fb: FormBuilder,
    private progressUpdateService: ProgressUpdateService,
    private wirService: WIRService
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

  initializeForm(): void {
    if (!this.activity) return;
    
    console.log('🔧 Initializing form for activity:', {
      name: this.activity.activityName,
      isWIRCheckpoint: this.isWIRCheckpoint,
      sequence: this.activity.sequence
    });
    
    // Clear checkpoint and WIR from previous activity
    this.nearestWIRCheckpoint = null;
    this.nearestWIR = null;
    this.positionLockedReason = '';
    
    // Find WIR for current activity (only if it's a WIR checkpoint)
    this.findNearestWIRBelow();
    
    this.progressForm = this.fb.group({
      progressPercentage: [
        this.activity.progressPercentage || 0, 
        [Validators.required, Validators.min(0), Validators.max(100)]
      ],
      workDescription: [''],
      issuesEncountered: [''],
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
        const currentCheckpoint = checkpoints.find(cp => 
          cp.wirNumber && cp.wirNumber.toUpperCase() === wir.wirCode.toUpperCase()
        );
        
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
            this.positionLockedReason = `Position is locked. Previous WIR (${previousWIRActivity.activityMaster?.wirCode || previousWIRActivity.wirCode || 'WIR'}) must be created first.`;
            console.log('🔒 Locking: Previous WIR record not created yet');
            return;
          }

          // Load checkpoint to get effective status (matches activity-table.component.ts)
          // The checkpoint.status is the source of truth, not wir.checkpointStatus
          this.wirService.getWIRCheckpointsByBox(this.activity.boxId).subscribe({
            next: (checkpoints) => {
              const prevCheckpoint = checkpoints.find(cp => 
                cp.wirNumber && cp.wirNumber.toUpperCase() === previousWIR.wirCode.toUpperCase()
              );
              
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
                return;
              }
              
              if (isPrevRejected) {
                // Previous WIR is Rejected - LOCK
                this.progressForm.get('wirBay')?.disable();
                this.progressForm.get('wirRow')?.disable();
                this.positionLockedReason = `Position is locked. Previous WIR (${previousWIR.wirCode}) was Rejected. Issues must be resolved.`;
                console.log('🔒 Locking: Previous WIR is Rejected');
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
                } else {
                  // No checkpoint found but WIR is Approved - inconsistent state
                  this.progressForm.get('wirBay')?.disable();
                  this.progressForm.get('wirRow')?.disable();
                  this.positionLockedReason = `Position is locked. Previous WIR (${previousWIR.wirCode}) is Approved but has no checkpoint. Create QA/QC checkpoint first.`;
                  console.log('🔒 Locking: Previous WIR has no checkpoint');
                }
              } else {
                // Other statuses - UNLOCK (fallback)
                this.progressForm.get('wirBay')?.enable();
                this.progressForm.get('wirRow')?.enable();
                this.positionLockedReason = ''; // Clear lock reason
                console.log('✅ Previous WIR status allows editing - fields editable');
              }
            },
            error: () => {
              // Error loading checkpoints - LOCK to be safe
              this.progressForm.get('wirBay')?.disable();
              this.progressForm.get('wirRow')?.disable();
              this.positionLockedReason = `Position is locked. Unable to verify previous WIR status.`;
              console.log('🔒 Locking: Error loading checkpoints (safe fallback)');
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
        // Find checkpoint matching the WIR code
        const checkpoint = checkpoints.find(cp => 
          cp.wirNumber && cp.wirNumber.toUpperCase() === wirCode.toUpperCase()
        );
        
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

  async onSubmit(): Promise<void> {
    if (this.progressForm.invalid) {
      this.errorMessage = 'Please fill in all required fields correctly';
      console.error('❌ Form is invalid:', this.progressForm.errors);
      console.error('❌ Invalid fields:', Object.keys(this.progressForm.controls).filter(key => this.progressForm.get(key)?.invalid));
      return;
    }

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

      const request = {
        boxId: this.activity.boxId,
        boxActivityId: this.activity.boxActivityId,
        progressPercentage: formValues.progressPercentage || 0,
        workDescription: formValues.workDescription || undefined,
        issuesEncountered: formValues.issuesEncountered || undefined,
        updateMethod: 'Web',
        deviceInfo: deviceInfo,
        // Include WIR position fields for ALL activities (if they have values)
        wirBay: formValues.wirBay?.toString().trim() || undefined,
        wirRow: formValues.wirRow?.toString().trim() || undefined,
        wirPosition: formValues.wirPosition?.toString().trim() || undefined
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
          }, 2000);
        },
        error: (error) => {
          console.error('❌ Error updating progress:', error);
          console.error('📋 Error structure:', {
            errorObj: error.error,
            status: error.status
          });
          
          // Extract error message from various possible structures
          this.errorMessage = error.error?.error?.description || 
                             error.error?.message || 
                             error.error?.error?.message ||
                             error.error?.title ||
                             error.message || 
                             'Failed to update progress. Please try again.';
          
          this.isSubmitting = false;
          
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
    this.closeModal.emit();
  }

  get isWIRCheckpoint(): boolean {
    return this.activity?.isWIRCheckpoint || false;
  }

  get wirCode(): string {
    return this.activity?.wirCode || '';
  }
}

