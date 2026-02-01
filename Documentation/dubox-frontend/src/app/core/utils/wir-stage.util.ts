import { WIRRecord, WIRStatus } from '../models/wir.model';
import { Box } from '../models/box.model';

/**
 * WIR Stage Information
 * Represents a WIR stage with its code, name, and color mapping
 */
export interface WIRStageInfo {
  wirCode: string; // e.g., "WIR-1", "WIR-2"
  stageName: string; // e.g., "Assembly", "Mechanical Clearance"
  colorClass: string; // CSS class name for styling
  displayName: string; // Display name for UI
}

/**
 * WIR Stage Color Mapping
 * Maps WIR codes to their visual representation
 */
export const WIR_STAGE_COLORS: Record<string, WIRStageInfo> = {
  'WIR-1': {
    wirCode: 'WIR-1',
    stageName: 'Assembly',
    colorClass: 'wir-stage-1',
    displayName: 'Assembly Clearance - WIR-1'
  },
  'WIR-2': {
    wirCode: 'WIR-2',
    stageName: 'Mechanical',
    colorClass: 'wir-stage-2',
    displayName: 'Mechanical Clearance - WIR-2'
  },
  'WIR-3': {
    wirCode: 'WIR-3',
    stageName: 'Electrical',
    colorClass: 'wir-stage-3',
    displayName: 'Electrical Clearance - WIR-3'
  },
  'WIR-4': {
    wirCode: 'WIR-4',
    stageName: '3rd Fix',
    colorClass: 'wir-stage-4',
    displayName: '3rd Fix Installation - WIR-4'
  },
  'WIR-5': {
    wirCode: 'WIR-5',
    stageName: '3rd Fix',
    colorClass: 'wir-stage-5',
    displayName: '3rd Fix Installation - WIR-5'
  },
  'WIR-6': {
    wirCode: 'WIR-6',
    stageName: 'Dispatch',
    colorClass: 'wir-stage-6',
    displayName: 'Readiness for Dispatch - WIR-6'
  }
};

/**
 * Completed WIR Stage
 * Used when all WIRs are completed
 */
export const COMPLETED_WIR_STAGE: WIRStageInfo = {
  wirCode: 'COMPLETED',
  stageName: 'Completed',
  colorClass: 'wir-completed',
  displayName: 'Completed'
};

/**
 * Determines the current active WIR stage for a box
 * 
 * Logic:
 * 1. Get all WIR records for the box, sorted by sequence/date
 * 2. Find the latest WIR that is NOT completed (status !== Approved)
 * 3. If all existing WIRs are approved, check if there are more WIR stages to start
 * 4. Only return COMPLETED stage if progress is 100% AND all WIRs are approved
 * 5. If no WIRs exist, return null (box hasn't started WIR process)
 * 
 * @param wirRecords - Array of WIR records for the box
 * @param boxProgress - Optional box progress percentage (0-100)
 * @param boxStatus - Optional box status to validate completion
 * @returns The current active WIR stage info, or null if no active WIR
 */
export function getCurrentActiveWIRStage(
  wirRecords: WIRRecord[], 
  boxProgress?: number,
  boxStatus?: string
): WIRStageInfo | null {
  if (!wirRecords || wirRecords.length === 0) {
    return null;
  }

  // Sort WIR records by WIR code number (WIR-1, WIR-2, etc.) to get proper sequence
  const sortedWIRs = [...wirRecords].sort((a, b) => {
    const numA = extractWIRNumber(a.wirCode);
    const numB = extractWIRNumber(b.wirCode);
    return numA - numB;
  });

  // Find the latest WIR that is not completed (not Approved)
  // A WIR is considered "active" if it's Pending, Rejected, or ConditionalApproval
  const activeWIR = sortedWIRs.find(wir => 
    wir.status !== WIRStatus.Approved
  );

  if (activeWIR) {
    // Return the stage info for this WIR code
    return WIR_STAGE_COLORS[activeWIR.wirCode] || createDefaultStageInfo(activeWIR.wirCode);
  }

  // All existing WIRs are approved - check if box is truly completed
  // Only mark as completed if:
  // 1. Progress is 100% (or very close to 100%)
  // 2. AND box status indicates completion (if provided)
  const isProgressComplete = boxProgress !== undefined && boxProgress >= 99.5;
  
  // Check if status indicates completion
  // If status is provided and it's InProgress/NotStarted/etc, NEVER show as completed
  // If status is not provided, we'll check progress only
  let isStatusComplete = true; // Default to true if status not provided
  if (boxStatus !== undefined) {
    // Status is provided - only consider it complete if it's explicitly a completed status
    isStatusComplete = boxStatus === 'Completed' || 
                       boxStatus === 'ReadyForDelivery' || 
                       boxStatus === 'Delivered';
    
    // If status is InProgress or any other non-completed status, never show as completed
    if (boxStatus === 'InProgress' || boxStatus === 'NotStarted' || boxStatus === 'OnHold' || boxStatus === 'QAReview') {
      isStatusComplete = false;
    }
  }
  
  // Only mark as completed if BOTH progress is 100% AND status indicates completion
  // This prevents showing completed color when box is still in progress
  if (isProgressComplete && isStatusComplete) {
    // Box is truly completed
    return COMPLETED_WIR_STAGE;
  }
  
  // If progress < 100% OR status is InProgress/NotStarted/etc, don't show as completed
  // even if all WIRs are approved - show the last WIR stage instead

  // All existing WIRs are approved, but box is not fully completed
  // Find the next WIR stage that should be started
  const nextStage = getNextWIRStage(wirRecords);
  if (nextStage) {
    // There's a next WIR stage that hasn't been started yet
    // Box should be in this stage, but since no WIR record exists for it,
    // we'll show the last completed WIR stage as current
    // This indicates the box is transitioning to the next stage
    const lastWIR = sortedWIRs[sortedWIRs.length - 1];
    if (lastWIR) {
      return WIR_STAGE_COLORS[lastWIR.wirCode] || createDefaultStageInfo(lastWIR.wirCode);
    }
    // If no last WIR exists (shouldn't happen), return the next stage
    return nextStage;
  }

  // All WIR stages exist and are approved, but progress < 100%
  // This means box is still working on the last WIR stage's activities
  // Return the last WIR stage as the current stage
  const lastWIR = sortedWIRs[sortedWIRs.length - 1];
  if (lastWIR) {
    return WIR_STAGE_COLORS[lastWIR.wirCode] || createDefaultStageInfo(lastWIR.wirCode);
  }

  // Fallback: if progress is 100% but status doesn't match, still show as completed
  // Otherwise, return null to fall back to box status colors
  return isProgressComplete ? COMPLETED_WIR_STAGE : null;
}

/**
 * Gets the latest WIR record that has position information
 * Used for determining box position in factory layout
 * 
 * @param wirRecords - Array of WIR records for the box
 * @returns The latest WIR with position, or null if none found
 */
export function getLatestPositionedWIR(wirRecords: WIRRecord[]): WIRRecord | null {
  if (!wirRecords || wirRecords.length === 0) {
    return null;
  }

  // Sort by requested date (most recent first)
  const sortedWIRs = [...wirRecords].sort((a, b) => {
    const dateA = new Date(a.requestedDate).getTime();
    const dateB = new Date(b.requestedDate).getTime();
    return dateB - dateA;
  });

  // Find the first WIR that has position information
  return sortedWIRs.find(wir => 
    wir.bay || wir.row || wir.position
  ) || null;
}

/**
 * Gets the WIR stage info for a specific WIR code
 * 
 * @param wirCode - The WIR code (e.g., "WIR-1")
 * @returns The WIR stage info, or a default if not found
 */
export function getWIRStageInfo(wirCode: string): WIRStageInfo {
  return WIR_STAGE_COLORS[wirCode] || createDefaultStageInfo(wirCode);
}

/**
 * Extracts the numeric part from a WIR code
 * e.g., "WIR-1" -> 1, "WIR-2" -> 2
 * 
 * @param wirCode - The WIR code
 * @returns The numeric part, or 0 if not found
 */
function extractWIRNumber(wirCode: string): number {
  const match = wirCode.match(/\d+/);
  return match ? parseInt(match[0], 10) : 0;
}

/**
 * Creates a default stage info for unknown WIR codes
 * 
 * @param wirCode - The WIR code
 * @returns A default WIR stage info
 */
function createDefaultStageInfo(wirCode: string): WIRStageInfo {
  return {
    wirCode,
    stageName: wirCode,
    colorClass: `wir-stage-default`,
    displayName: wirCode
  };
}

/**
 * Determines if a box has completed all WIR stages
 * 
 * @param wirRecords - Array of WIR records for the box
 * @returns True if all WIRs are approved, false otherwise
 */
export function hasCompletedAllWIRs(wirRecords: WIRRecord[]): boolean {
  if (!wirRecords || wirRecords.length === 0) {
    return false;
  }

  return wirRecords.every(wir => wir.status === WIRStatus.Approved);
}

/**
 * Gets the next WIR stage that should be started
 * 
 * @param wirRecords - Array of WIR records for the box
 * @returns The next WIR stage info, or null if all are completed
 */
export function getNextWIRStage(wirRecords: WIRRecord[]): WIRStageInfo | null {
  if (!wirRecords || wirRecords.length === 0) {
    // If no WIRs exist, the first stage is WIR-1
    return WIR_STAGE_COLORS['WIR-1'];
  }

  // Get all WIR codes that have been started
  const startedWIRCodes = new Set(wirRecords.map(wir => wir.wirCode));

  // Find the first WIR stage that hasn't been started
  const allWIRCodes = Object.keys(WIR_STAGE_COLORS).sort((a, b) => {
    return extractWIRNumber(a) - extractWIRNumber(b);
  });

  for (const wirCode of allWIRCodes) {
    if (!startedWIRCodes.has(wirCode)) {
      return WIR_STAGE_COLORS[wirCode];
    }
  }

  // All stages have been started
  return null;
}

