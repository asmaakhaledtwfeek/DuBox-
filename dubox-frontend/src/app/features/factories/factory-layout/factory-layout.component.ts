import { Component, OnInit, OnDestroy, ViewChild, ElementRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import html2canvas from 'html2canvas';
import { jsPDF } from 'jspdf';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Subscription, forkJoin, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { FactoryService, Factory, FactorySection, FactorySectionType, FactorySectionPart } from '../../../core/services/factory.service';
import { BoxService } from '../../../core/services/box.service';
import { Box, BoxStatus } from '../../../core/models/box.model';
import { WIRService } from '../../../core/services/wir.service';
import { WIRRecord } from '../../../core/models/wir.model';
import { ProjectService } from '../../../core/services/project.service';
import { Project } from '../../../core/models/project.model';
import { HeaderComponent } from '../../../shared/components/header/header.component';
import { SidebarComponent } from '../../../shared/components/sidebar/sidebar.component';
import { BoxQuickViewModalComponent } from '../box-quick-view-modal/box-quick-view-modal.component';
import {
    getCurrentActiveWIRStage,
    getWIRStageInfo,
    WIRStageInfo,
    COMPLETED_WIR_STAGE,
    WIR_STAGE_COLORS
} from '../../../core/utils/wir-stage.util';
import * as XLSX from 'xlsx';
import * as ExcelJS from 'exceljs';

@Component({
    selector: 'app-factory-layout',
    standalone: true,
    imports: [CommonModule, RouterModule, FormsModule, HeaderComponent, SidebarComponent, BoxQuickViewModalComponent],
    templateUrl: './factory-layout.component.html',
    styleUrls: ['./factory-layout.component.scss']
})
export class FactoryLayoutComponent implements OnInit, OnDestroy {
    @ViewChild('layoutPrintArea') layoutPrintArea!: ElementRef<HTMLElement>;

    factoryId: string = '';
    factory: Factory | null = null;
    boxes: Box[] = [];
    filteredBoxes: Box[] = [];
    // Map of boxId to WIR records for that box
    boxWIRRecordsMap: Map<string, WIRRecord[]> = new Map();
    // Map of partId to a set of frozen row numbers
    freezingCellsMap: Map<string, Set<number>> = new Map();
    loading = true;
    error = '';

    // Section support
    hasSections = false;
    selectedSectionId: string | null = null; // null means show all sections
    FactorySectionType = FactorySectionType;

    // Layout mode - toggle between matrix and 2-column layout
    layoutMode: 'matrix' | 'two-column' = 'two-column';

    // Assembly split configuration
    assemblySplitMode: 'horizontal' | 'vertical' = 'horizontal';

    // Filter properties
    projects: Project[] = [];
    allProjects: Project[] = []; // Store all projects before filtering
    selectedProjectId: string = '';
    selectedStage: string = '';
    selectedBuilding: string = '';
    selectedFloor: string = '';
    selectedLevel: string = '';
    selectedZone: string = '';
    availableStages: Array<{ wirCode: string; displayName: string }> = [];

    // Available filter options
    availableBuildings: string[] = [];
    availableFloors: string[] = [];
    availableLevels: string[] = [];
    availableZones: string[] = [];

    BoxStatus = BoxStatus;

    // Modal state
    isBoxQuickViewModalOpen = false;
    selectedBoxId: string = '';
    selectedBoxProjectId: string = '';

    // Export/Print state
    isExportModalOpen = false;
    selectedSectionsForExport: string[] = []; // Array of section IDs to export/print
    exportAction: 'print' | 'excel' | null = null;

    private subscriptions: Subscription[] = [];

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private factoryService: FactoryService,
        private boxService: BoxService,
        private wirService: WIRService,
        private projectService: ProjectService
    ) { }

    ngOnInit(): void {
        this.factoryId = this.route.snapshot.params['id'];
        this.initializeAvailableStages();
        this.loadProjects();
        this.loadFactoryData();
    }

    /**
     * Initialize available stages with predefined WIR stages from BUSINESS FLOW INDICATORS
     */
    initializeAvailableStages(): void {
        // Use predefined WIR stages from WIR_STAGE_COLORS
        // Remove the code part (e.g., "- WIR-1") from display name
        this.availableStages = Object.values(WIR_STAGE_COLORS).map(stage => ({
            wirCode: stage.wirCode,
            displayName: stage.displayName.replace(/\s*-\s*WIR-\d+$/, '')
        }));

        // Add Completed stage
        this.availableStages.push({
            wirCode: COMPLETED_WIR_STAGE.wirCode,
            displayName: COMPLETED_WIR_STAGE.displayName
        });
    }

    /**
     * Extract WIR number from stage code
     * Supports formats: WIR-1, WIR-01, WIR_1, WIR_01, etc.
     */
    private extractWIRNumber(wirCode: string): number {
        // Match patterns: WIR-1, WIR-01, WIR_1, WIR_01
        const match = wirCode.match(/WIR[-_](\d+)/i);
        return match ? parseInt(match[1], 10) : 0;
    }

    /**
     * Get stage info including formatted display name
     * Uses utility function for consistency
     */
    private getStageDisplayInfo(wirCode: string): { wirCode: string; displayName: string } {
        const stageInfo = getWIRStageInfo(wirCode);
        return {
            wirCode: stageInfo.wirCode,
            displayName: stageInfo.displayName
        };
    }

    /**
     * Update available stages dynamically based on actual WIR records found in boxes
     * This ensures new stages that aren't in the predefined list are still displayed
     * Supports various WIR code formats: WIR-1, WIR-01, WIR_1, WIR_01
     */
    updateAvailableStagesFromData(): void {
        // Get all unique WIR stage codes from the loaded WIR records
        const foundStages = new Set<string>();
        
        this.boxWIRRecordsMap.forEach((wirRecords, boxId) => {
            wirRecords.forEach(wir => {
                if (wir.wirCode) {
                    foundStages.add(wir.wirCode);
                }
            });
        });

        // Also check for active WIR stages in boxes
        this.boxes.forEach(box => {
            const wirStage = this.getBoxWIRStage(box);
            if (wirStage && wirStage.wirCode) {
                foundStages.add(wirStage.wirCode);
            }
        });

        // Check which stages are new (not in current availableStages)
        const currentStageCodes = new Set(this.availableStages.map(s => s.wirCode));
        const newStages: Array<{ wirCode: string; displayName: string }> = [];

        foundStages.forEach(wirCode => {
            if (!currentStageCodes.has(wirCode) && wirCode !== 'COMPLETED') {
                // New stage found - get formatted stage info
                const stageInfo = this.getStageDisplayInfo(wirCode);
                newStages.push({
                    wirCode: stageInfo.wirCode,
                    displayName: stageInfo.displayName
                });
                console.log(`✅ New WIR stage detected: ${wirCode} (${stageInfo.displayName})`);
            }
        });

        // Add new stages to availableStages (before COMPLETED)
        if (newStages.length > 0) {
            // Remove COMPLETED temporarily
            const completedIndex = this.availableStages.findIndex(s => s.wirCode === 'COMPLETED');
            const completedStage = completedIndex !== -1 ? this.availableStages[completedIndex] : null;
            
            if (completedStage) {
                this.availableStages.splice(completedIndex, 1);
            }

            // Sort new stages by WIR number
            newStages.sort((a, b) => {
                const numA = this.extractWIRNumber(a.wirCode);
                const numB = this.extractWIRNumber(b.wirCode);
                return numA - numB;
            });

            // Add new stages
            this.availableStages.push(...newStages);

            // Re-sort all stages by WIR number
            this.availableStages.sort((a, b) => {
                const numA = this.extractWIRNumber(a.wirCode);
                const numB = this.extractWIRNumber(b.wirCode);
                return numA - numB;
            });

            // Add COMPLETED back at the end
            if (completedStage) {
                this.availableStages.push(completedStage);
            }

            console.log(`✅ Updated available stages:`, this.availableStages.map(s => `${s.displayName} (${s.wirCode})`));
        }
    }

    ngOnDestroy(): void {
        this.subscriptions.forEach(sub => sub.unsubscribe());
    }

    loadFactoryData(): void {
        this.loading = true;
        this.error = '';

        // Load factory details
        const factorySub = this.factoryService.getFactoryById(this.factoryId).subscribe({
            next: (factory: Factory) => {
                this.factory = factory;

                // Check if factory has sections
                this.hasSections = !!(factory.sections && factory.sections.length > 0);

                this.loadFreezingCells();
                this.loadBoxes();
            },
            error: (err: any) => {
                this.error = 'Failed to load factory details';
                console.error('Error loading factory:', err);
                this.loading = false;
            }
        });

        this.subscriptions.push(factorySub);
    }

    loadProjects(): void {
        const projectsSub = this.projectService.getProjects().subscribe({
            next: (projects: Project[]) => {
                // Store all non-archived projects
                this.allProjects = projects.filter(p => p.status !== 'Archived');
                // Update projects list based on boxes (will be called after boxes are loaded)
                this.updateProjectsList();
            },
            error: (err: any) => {
                console.error('Error loading projects:', err);
            }
        });

        this.subscriptions.push(projectsSub);
    }

    /**
     * Update projects list to only show projects that have boxes in the current layout
     * (boxes that have bay, row, or position assigned)
     */
    updateProjectsList(): void {
        if (this.boxes.length === 0 || this.allProjects.length === 0) {
            // If boxes aren't loaded yet, show all projects temporarily
            this.projects = [...this.allProjects];
            return;
        }

        // Get unique project IDs from boxes that have positions in the layout
        const projectIdsWithBoxesInLayout = new Set<string>();
        this.boxes.forEach(box => {
            // Only include boxes that have position information (bay, row, or position)
            if (box.bay || box.row || box.position) {
                projectIdsWithBoxesInLayout.add(box.projectId);
            }
        });

        // Filter projects to only show those with boxes in layout
        this.projects = this.allProjects.filter(p => projectIdsWithBoxesInLayout.has(p.id));
    }

    loadBoxes(): void {
        const boxesSub = this.boxService.getBoxesByFactory(this.factoryId).subscribe({
            next: (boxes: Box[]) => {
                // Backend already filters boxes (InProgress/Completed from active projects)
                // Just use the boxes directly and load WIR records
                this.boxes = boxes;
                this.filteredBoxes = boxes;
                this.updateProjectsList();
                this.updateFilterOptions();
                this.loadWIRRecordsForBoxes(boxes);
            },
            error: (err: any) => {
                this.error = 'Failed to load boxes';
                console.error('Error loading boxes:', err);
                this.loading = false;
            }
        });

        this.subscriptions.push(boxesSub);
    }

    /**
     * Update available filter options based on boxes in the layout
     * If a project is selected, only show options for that project
     */
    updateFilterOptions(): void {
        // Get unique values from boxes that have positions in the layout
        const buildingsSet = new Set<string>();
        const floorsSet = new Set<string>();
        const levelsSet = new Set<string>();
        const zonesSet = new Set<string>();

        // Filter boxes based on selected project if one is selected
        const boxesToConsider = this.selectedProjectId
            ? this.boxes.filter(box => box.projectId === this.selectedProjectId)
            : this.boxes;

        boxesToConsider.forEach(box => {
            // Only consider boxes that have position information
            if (box.bay || box.row || box.position) {
                if (box.buildingNumber) {
                    buildingsSet.add(box.buildingNumber);
                }
                if (box.floor) {
                    floorsSet.add(box.floor);
                }
                // For level, we'll use floor as level (since there's no separate level property)
                // If level needs to be different, it can be added later
                if (box.floor) {
                    levelsSet.add(box.floor);
                }
                if (box.zone) {
                    zonesSet.add(box.zone);
                }
            }
        });

        this.availableBuildings = Array.from(buildingsSet).sort();
        this.availableFloors = Array.from(floorsSet).sort();
        this.availableLevels = Array.from(levelsSet).sort();
        this.availableZones = Array.from(zonesSet).sort();

        // Clear selections if they're no longer valid after project change
        if (this.selectedProjectId) {
            if (this.selectedBuilding && !this.availableBuildings.includes(this.selectedBuilding)) {
                this.selectedBuilding = '';
            }
            if (this.selectedFloor && !this.availableFloors.includes(this.selectedFloor)) {
                this.selectedFloor = '';
            }
            if (this.selectedLevel && !this.availableLevels.includes(this.selectedLevel)) {
                this.selectedLevel = '';
            }
            if (this.selectedZone && !this.availableZones.includes(this.selectedZone)) {
                this.selectedZone = '';
            }
        }
    }

    /**
     * Handle project filter change - update dependent filter options
     */
    onProjectChange(): void {
        // Update filter options based on selected project
        this.updateFilterOptions();
        // Apply filters to update the grid
        this.applyFilters();
    }

    /**
     * Handle click on stage card in BUSINESS FLOW INDICATORS
     * Toggles the stage filter - clicking again clears it
     */
    onStageCardClick(wirCode: string): void {
        // If clicking the same stage, clear the filter
        if (this.selectedStage === wirCode) {
            this.selectedStage = '';
        } else {
            // Set the new stage filter
            this.selectedStage = wirCode;
        }
        // Apply filters to update the grid
        this.applyFilters();
    }

    applyFilters(): void {
        let filtered = [...this.boxes];

        // When filtering by project or stage, show only InProgress or Completed boxes
        // Exception: EMPTY filter should show all boxes (to identify empty cells)
        if (this.selectedProjectId || (this.selectedStage && this.selectedStage !== 'EMPTY')) {
            filtered = filtered.filter(box =>
                box.status === BoxStatus.InProgress || box.status === BoxStatus.Completed
            );
        }

        // Filter by project
        if (this.selectedProjectId) {
            filtered = filtered.filter(box => box.projectId === this.selectedProjectId);
        }

        // Filter by stage
        if (this.selectedStage) {
            if (this.selectedStage === 'EMPTY') {
                // For EMPTY filter, we'll handle it in getGridLayout() by showing empty cells
                // Here we just don't filter boxes - empty cells will be shown in the grid
                // But we still need to filter out boxes that have positions to show empty cells
                // Actually, we want to show ALL positions, but highlight empty ones
                // So we don't filter boxes here - the grid will show empty cells
            } else {
                filtered = filtered.filter(box => {
                    const wirStage = this.getBoxWIRStage(box);
                    return wirStage && wirStage.wirCode === this.selectedStage;
                });
            }
        }

        // Filter by building
        if (this.selectedBuilding) {
            filtered = filtered.filter(box => box.buildingNumber === this.selectedBuilding);
        }

        // Filter by floor
        if (this.selectedFloor) {
            filtered = filtered.filter(box => box.floor === this.selectedFloor);
        }

        // Filter by level (using floor as level since there's no separate level property)
        if (this.selectedLevel) {
            filtered = filtered.filter(box => box.floor === this.selectedLevel);
        }

        // Filter by zone
        if (this.selectedZone) {
            filtered = filtered.filter(box => box.zone === this.selectedZone);
        }

        this.filteredBoxes = filtered;
    }

    clearFilters(): void {
        this.selectedProjectId = '';
        this.selectedStage = '';
        this.selectedBuilding = '';
        this.selectedFloor = '';
        this.selectedLevel = '';
        this.selectedZone = '';
        // Update filter options to show all available options (not filtered by project)
        this.updateFilterOptions();
        this.filteredBoxes = [...this.boxes];
    }

    /**
     * Check if a stage card is currently active/selected
     */
    isStageCardActive(wirCode: string): boolean {
        return this.selectedStage === wirCode;
    }

    /**
     * Load WIR records for all boxes in parallel
     * This allows us to determine the current WIR stage for color coding
     */
    loadWIRRecordsForBoxes(boxes: Box[]): void {
        if (boxes.length === 0) {
            this.loading = false;
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
        const wirSub = forkJoin(wirObservables).subscribe({
            next: (results) => {
                // Build the map of boxId to WIR records
                this.boxWIRRecordsMap.clear();
                results.forEach(result => {
                    this.boxWIRRecordsMap.set(result.boxId, result.wirs);
                });

                // Note: Box positions (bay, row, position) come directly from the Box entity
                // WIR records are only used for color coding based on WIR stage

                // Update available stages dynamically based on actual data
                this.updateAvailableStagesFromData();

                this.loading = false;
            },
            error: (err: any) => {
                console.error('Error loading WIR records:', err);
                // Continue even if WIR loading fails - boxes will use default colors
                this.loading = false;
            }
        });

        this.subscriptions.push(wirSub);
    }

    /**
     * Build the freezingCellsMap from the factory data already loaded in the response.
     * FreezingCells are included inside each FactorySectionPart via the backend ThenInclude.
     * Maps: partId -> Set<rowNumber>
     */
    loadFreezingCells(): void {
        if (!this.factory?.sections?.length) return;

        this.freezingCellsMap.clear();
        this.factory.sections.forEach(section => {
            (section.parts || []).forEach(part => {
                if (part.partId) {
                    const frozenRows = new Set<number>(
                        (part.freezingCells || []).map(c => c.rowNumber)
                    );
                    this.freezingCellsMap.set(part.partId, frozenRows);
                    if (frozenRows.size > 0) {
                        console.log(`[FreezingCells] Part "${part.partName}" (${part.partId}): frozen rows →`, [...frozenRows]);
                    }
                }
            });
        });
        console.log(`[FreezingCells] Map built: ${this.freezingCellsMap.size} part(s) tracked`);
    }

    /**
     * Returns true when a given row number is frozen for the specified part.
     * rowStr is the string row number from the grid cell (e.g. "6").
     */
    isRowFrozen(partId: string | undefined, rowStr: string): boolean {
        if (!partId) return false;
        const frozenRows = this.freezingCellsMap.get(partId);
        return frozenRows?.has(parseInt(rowStr, 10)) ?? false;
    }

    /**
     * Returns true when the given column (row number) is frozen in ANY part of the provided section parts.
     * Used for column-axis headers which are shared across all parts in a section.
     */
    isSectionColumnFrozen(sectionParts: any[], column: string): boolean {
        const rowNum = parseInt(column, 10);
        return sectionParts.some(part => {
            const frozenRows = this.freezingCellsMap.get(part.partId);
            return frozenRows?.has(rowNum) ?? false;
        });
    }

    /**
     * Get grid layout for a specific section or all sections
     */
    getSectionGridLayouts(): Array<{ section: FactorySection | null; grid: any }> {
        if (!this.factory) {
            return [];
        }

        // If factory has sections and a specific section is selected, return only that section
        if (this.hasSections && this.factory.sections && this.factory.sections.length > 0) {
            if (this.selectedSectionId) {
                const section = this.factory.sections.find(s => s.sectionId === this.selectedSectionId);
                if (section) {
                    return [{
                        section: section,
                        grid: this.getGridLayoutForSection(section)
                    }];
                }
            }

            // Return all sections
            return this.factory.sections.map(section => ({
                section: section,
                grid: this.getGridLayoutForSection(section)
            }));
        }

        // Legacy: return single grid with null section
        return [{
            section: null,
            grid: this.getGridLayout()
        }];
    }

    /**
     * Get grid layout for a specific section
     */
    getGridLayoutForSection(section: FactorySection): any {
        if (!this.factory) {
            return { rows: [], columns: [], matrix: [], totalBoxes: 0, sectionId: section.sectionId, sectionName: section.sectionName };
        }

        const minRow = section.minRow ?? 1;
        const maxRow = section.maxRow ?? 20;
        const minBay = section.minBay ?? 'A';
        const maxBay = section.maxBay ?? 'Z';

        // Columns displayed right to left (highest row number on the left)
        const reverseColumns = true;

        return this.generateGridLayoutInternal(minRow, maxRow, minBay, maxBay, section.sectionId, reverseColumns);
    }

    /**
     * Get grid layout for a specific part within a section
     * Uses the part's specific bay range (e.g., B-D or E-G) AND part's specific row range (e.g., 1-12 or 13-24)
     */
    getGridLayoutForPart(part: FactorySectionPart, section: FactorySection): any {
        if (!this.factory) {
            return { rows: [], columns: [], matrix: [], totalBoxes: 0, partId: part.partId, partName: part.partName };
        }

        // Use part's bay range (3 bays each typically: B-D or E-G)
        const minBay = part.minBay ?? 'A';
        const maxBay = part.maxBay ?? 'Z';

        // Use PART's row range (12 rows each: 1-12 or 13-24)
        const minRow = part.minRow ?? 1;
        const maxRow = part.maxRow ?? 12;

        // Columns displayed right to left (highest row number on the left)
        const reverseColumns = true;

        return this.generateGridLayoutInternal(minRow, maxRow, minBay, maxBay, section.sectionId, reverseColumns);
    }

    /**
     * Select a specific section to view
     */
    selectSection(sectionId: string | null): void {
        this.selectedSectionId = sectionId;
    }

    /**
     * Get section type name for display
     */
    getSectionTypeName(type: FactorySectionType): string {
        return type === FactorySectionType.Assembly ? 'Assembly' : 'Finishing';
    }

    /**
     * Box positions (bay, row, position) are now taken directly from the Box entity
     * which is stored in the database. WIR records are only used for color coding.
     * 
     * The getGridLayout() method uses box.bay, box.row, and box.position directly
     * to determine where each box should be displayed in the factory layout grid.
     */

    getGridLayout(): any {
        // Use factory's min/max row and bay values from database to determine the full layout range
        // This ensures we always show the complete factory layout as defined in the database
        if (!this.factory) {
            return { rows: [], columns: [], matrix: [], totalBoxes: 0 };
        }

        // Get min/max values from factory (with fallback defaults if not set)
        const minRow = this.factory.minRow ?? 1;
        const maxRow = this.factory.maxRow ?? 20;
        const minBay = this.factory.minBay ?? 'A';
        const maxBay = this.factory.maxBay ?? 'Z';

        return this.generateGridLayoutInternal(minRow, maxRow, minBay, maxBay, null, false);
    }

    /**
     * Core grid generation logic used by both legacy and section-based layouts
     */
    private generateGridLayoutInternal(minRow: number, maxRow: number, minBay: string, maxBay: string, sectionId: string | null | undefined, reverseColumns: boolean = false): any {
        // Get filtered boxes to determine which boxes to display in the grid
        // For EMPTY filter, we need all boxes to identify empty cells
        const allBoxesWithPosition = this.boxes.filter(box => (box.bay || box.row || box.position));
        const boxesForDisplay = this.selectedStage === 'EMPTY'
            ? allBoxesWithPosition
            : this.filteredBoxes.filter(box => (box.bay || box.row || box.position));

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

        // TRANSPOSED: Swap rows and columns for different orientation
        // columns = rows (numbers 1, 2, 3... go horizontally across top)
        // rows = bays (letters A, B, C... go vertically down left side)
        let columns = allRows;  // Row numbers now go across the top
        const rows = allBays;     // Bay letters now go down the left side

        // Reverse columns for Finishing sections (display right to left)
        if (reverseColumns) {
            columns = [...columns].reverse();
        }

        // Create matrix structure with ALL combinations
        const matrix: any[][] = [];
        let displayedBoxCount = 0;

        rows.forEach(bay => {  // Outer loop: Bays (vertical)
            const rowCells: any[] = [];
            columns.forEach(row => {  // Inner loop: Rows (horizontal)
                // Find boxes at this position from filtered boxes (for display)
                // Filter by bay, row, and sectionId (if sectionId is provided)
                const boxesAtPosition = boxesForDisplay.filter(b => {
                    const bayMatch = (b.bay || '') === bay;
                    const rowMatch = (b.row || '') === row;
                    // CRITICAL: If box has factorySectionId set, it MUST match the requested section
                    // This ensures boxes display in the correct section they're assigned to, not just by coordinates
                    // If sectionId is provided:
                    //   - Show box ONLY if its factorySectionId matches (strict matching)
                    // If sectionId is null/undefined:
                    //   - Show boxes without factorySectionId (orphan boxes for backward compatibility)
                    const sectionMatch = sectionId
                        ? (b.factorySectionId === sectionId) // Strict: box must be assigned to this section
                        : (!b.factorySectionId); // Only show orphan boxes when no section specified
                    return bayMatch && rowMatch && sectionMatch;
                });

                // For EMPTY filter, only show empty cells (no boxes)
                if (this.selectedStage === 'EMPTY' && boxesAtPosition.length > 0) {
                    // Skip this cell - it has a box, so it's not empty
                    rowCells.push({
                        row: row,        // Row number (from columns - now horizontal)
                        column: bay,     // Bay letter (from rows - now vertical)
                        bay: bay,        // Bay letter (from rows - now vertical)
                        box: null,
                        position: null,
                        boxCount: 0,
                        allBoxes: [],
                        hidden: true // Mark as hidden for EMPTY filter
                    });
                    return;
                }

                // Use the first box for display, but track if there are multiple
                const box = boxesAtPosition.length > 0 ? boxesAtPosition[0] : null;
                const hasMultipleBoxes = boxesAtPosition.length > 1;

                if (box) {
                    displayedBoxCount++;
                    if (hasMultipleBoxes) {
                        console.warn(`Multiple boxes (${boxesAtPosition.length}) at position Bay: ${bay}, Row: ${row}:`,
                            boxesAtPosition.map(b => b.code).join(', '));
                    }
                }

                rowCells.push({
                    row: row,        // Row number (from columns - now horizontal)
                    column: bay,     // Bay letter (from rows - now vertical)
                    bay: bay,        // Bay letter (from rows - now vertical)
                    box: box || null,
                    position: box?.position || null,
                    boxCount: boxesAtPosition.length, // Track how many boxes are at this position
                    allBoxes: boxesAtPosition // Keep reference to all boxes at this position
                });
            });

            // For EMPTY filter, only add rows that have at least one visible (empty) cell
            // For other filters, always add all rows to show full layout
            if (this.selectedStage === 'EMPTY') {
                const hasVisibleCells = rowCells.some(cell => !cell.hidden);
                if (hasVisibleCells) {
                    matrix.push(rowCells);
                }
            } else {
                // Always add all rows to show full layout with filtered boxes
                matrix.push(rowCells);
            }
        });

        return {
            rows,
            columns,
            matrix,
            totalBoxes: boxesForDisplay.length, // Total filtered boxes with positions
            displayedBoxes: displayedBoxCount, // Unique positions with filtered boxes
            totalBoxesInFactory: allBoxesWithPosition.length // Total boxes in factory (for reference)
        };
    }

    getCellTooltip(cell: any, frozen = false): string {
        if (frozen) {
            return `Frozen Row ${cell.row} — Not Available\nBay: ${cell.bay}`;
        }
        if (cell.box) {
            const status = this.getStatusLabel(cell.box.status);
            const wirStage = this.getBoxWIRStage(cell.box);
            const wirStageName = wirStage ? `\nWIR Stage: ${wirStage.displayName}` : '';
            let tooltip = `${cell.box.code}\nBay: ${cell.bay}, Row: ${cell.row}\nPosition: ${cell.position || '-'}\nStatus: ${status}${wirStageName}\nProgress: ${cell.box.progress}%`;

            // Add warning if multiple boxes share this position
            if (cell.boxCount > 1) {
                const otherBoxes = cell.allBoxes.slice(1).map((b: Box) => b.code).join(', ');
                tooltip += `\n\n⚠️ Position conflict: ${cell.boxCount - 1} more box(es) share this position:\n${otherBoxes}`;
            }

            return tooltip;
        }
        return `Available\nBay: ${cell.bay}, Row: ${cell.row}`;
    }

    /**
     * Get the current active WIR stage for a box
     * Used for determining the color class
     * Considers box progress and status to avoid false completion
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

    getStatusLabel(status: BoxStatus): string {
        switch (status) {
            case BoxStatus.NotStarted: return 'Not Started';
            case BoxStatus.ReadyToStart: return 'Ready to Start';
            case BoxStatus.InProgress: return 'In Progress';
            case BoxStatus.Completed: return 'Completed';
            case BoxStatus.OnHold: return 'On Hold';
            case BoxStatus.Dispatched: return 'Dispatched';
            case BoxStatus.QAReview: return 'QA Review';
            case BoxStatus.ReadyForDelivery: return 'Ready for Delivery';
            case BoxStatus.Delivered: return 'Delivered';
            default: return status;
        }
    }


    getWIRStageStats(): Array<{ stage: WIRStageInfo | null; count: number; percentage: number }> {
        const gridLayout = this.getGridLayout();
        const totalSlots = gridLayout.rows.length * gridLayout.columns.length;

        if (totalSlots === 0) return [];

        const stageCounts = new Map<string, number>();
        let completedCount = 0;
        let noWIRCount = 0;

        // Count boxes by WIR stage (use ALL boxes with positions, not filtered, for accurate percentages)
        // But only count boxes that are not dispatched
        const boxesToCount = this.boxes.filter(box =>
            (box.bay || box.row || box.position) && box.status !== BoxStatus.Dispatched
        );

        boxesToCount.forEach(box => {
            const wirStage = this.getBoxWIRStage(box);
            if (wirStage) {
                if (wirStage.wirCode === 'COMPLETED') {
                    completedCount++;
                } else {
                    const count = stageCounts.get(wirStage.wirCode) || 0;
                    stageCounts.set(wirStage.wirCode, count + 1);
                }
            } else {
                noWIRCount++;
            }
        });

        const stats: Array<{ stage: WIRStageInfo | null; count: number; percentage: number }> = [];

        // If a stage filter is active (and not EMPTY), only show that stage
        if (this.selectedStage && this.selectedStage !== 'EMPTY') {
            // Find the selected stage and add it to stats
            if (this.selectedStage === 'COMPLETED') {
                if (completedCount > 0) {
                    const percentage = totalSlots > 0 ? (completedCount / totalSlots) * 100 : 0;
                    const rounded = Math.round(percentage * 10) / 10;
                    const finalPercentage = (completedCount > 0 && rounded === 0) ? 0.1 : rounded;

                    stats.push({
                        stage: COMPLETED_WIR_STAGE,
                        count: completedCount,
                        percentage: finalPercentage
                    });
                }
            } else {
                const count = stageCounts.get(this.selectedStage) || 0;
                if (count > 0) {
                    const stage = getWIRStageInfo(this.selectedStage);
                    const percentage = totalSlots > 0 ? (count / totalSlots) * 100 : 0;
                    const rounded = Math.round(percentage * 10) / 10;
                    const finalPercentage = (count > 0 && rounded === 0) ? 0.1 : rounded;

                    stats.push({
                        stage,
                        count,
                        percentage: finalPercentage
                    });
                }
            }
        } else {
            // No stage filter - show all stages
            // Add WIR stages that have boxes
            // Percentage is calculated against total slots (cells) in the grid
            stageCounts.forEach((count, wirCode) => {
                const stage = getWIRStageInfo(wirCode);
                const percentage = totalSlots > 0 ? (count / totalSlots) * 100 : 0;
                const rounded = Math.round(percentage * 10) / 10; // Round to 1 decimal place

                // If percentage is very small but there are boxes, show at least 0.1%
                const finalPercentage = (count > 0 && rounded === 0) ? 0.1 : rounded;

                stats.push({
                    stage,
                    count,
                    percentage: finalPercentage
                });
            });

            // Add completed if there are any
            if (completedCount > 0) {
                const percentage = totalSlots > 0 ? (completedCount / totalSlots) * 100 : 0;
                const rounded = Math.round(percentage * 10) / 10;

                // If percentage is very small but there are boxes, show at least 0.1%
                const finalPercentage = (completedCount > 0 && rounded === 0) ? 0.1 : rounded;

                stats.push({
                    stage: COMPLETED_WIR_STAGE,
                    count: completedCount,
                    percentage: finalPercentage
                });
            }

            // Sort by WIR number
            stats.sort((a, b) => {
                if (!a.stage || !b.stage) return 0;
                const numA = a.stage.wirCode === 'COMPLETED' ? 999 : this.extractWIRNumber(a.stage.wirCode);
                const numB = b.stage.wirCode === 'COMPLETED' ? 999 : this.extractWIRNumber(b.stage.wirCode);
                return numA - numB;
            });
        }

        return stats;
    }

    /**
     * Get the sum of all stage percentages
     * Used to calculate empty percentage as remainder
     */
    private getTotalStagePercentage(): number {
        const stats = this.getWIRStageStats();
        return stats.reduce((sum, stat) => sum + stat.percentage, 0);
    }

    getInProgressPercentage(): number {
        // Calculate total slots based on factory's min/max row and bay configuration
        const gridLayout = this.getGridLayout();
        const totalSlots = gridLayout.rows.length * gridLayout.columns.length;

        if (totalSlots === 0) return 0;

        // Count boxes that are in any WIR stage (not completed) - use ALL boxes with positions
        const inProgressCount = this.boxes.filter(b => {
            if (!b.bay && !b.row && !b.position) return false;
            if (b.status === BoxStatus.Dispatched) return false;
            const wirStage = this.getBoxWIRStage(b);
            return wirStage && wirStage.wirCode !== 'COMPLETED';
        }).length;

        // Percentage is calculated against total slots from factory configuration
        return Math.round((inProgressCount / totalSlots) * 100);
    }

    getCompletedPercentage(): number {
        // Calculate total slots based on factory's min/max row and bay configuration
        const gridLayout = this.getGridLayout();
        const totalSlots = gridLayout.rows.length * gridLayout.columns.length;

        if (totalSlots === 0) return 0;

        // Count boxes that have completed all WIRs - use ALL boxes with positions
        const completedCount = this.boxes.filter(b => {
            if (!b.bay && !b.row && !b.position) return false;
            if (b.status === BoxStatus.Dispatched) return false;
            const wirStage = this.getBoxWIRStage(b);
            return wirStage && wirStage.wirCode === 'COMPLETED';
        }).length;

        // Percentage is calculated against total slots from factory configuration
        return Math.round((completedCount / totalSlots) * 100);
    }

    getEmptyPercentage(): number {
        const gridLayout = this.getGridLayout();
        // Calculate total cells based on factory's min/max row and bay configuration
        const totalCells = gridLayout.rows.length * gridLayout.columns.length;

        if (totalCells === 0) return 0;

        // Calculate empty percentage as remainder to ensure sum equals 100%
        // This accounts for rounding errors in individual stage percentages
        // Empty percentage represents unoccupied slots in the factory layout
        const totalStagePercentage = this.getTotalStagePercentage();
        const emptyPercentage = Math.max(0, 100 - totalStagePercentage);

        // Round to 1 decimal place to match stage percentages
        return Math.round(emptyPercentage * 10) / 10;
    }

    /**
     * Get the color class for a WIR stage
     * Used for coloring the stage dropdown options and indicators
     * 
     * Color Assignment Strategy:
     * - Predefined stages (WIR-1 to WIR-6): Use colors 1-6 (unique, fixed colors)
     * - Dynamic stages (WIR-7+, WIR-01, WIR_1, etc.): Use colors 7-12 (no overlap with predefined)
     * - WIR-1 and WIR-01 are treated as DIFFERENT stages with DIFFERENT colors
     * - This ensures NO color repetition between visible stages
     */
    getStageColorClass(wirCode: string): string {
        if (wirCode === 'COMPLETED') {
            return 'wir-completed';
        }
        
        const stageInfo = getWIRStageInfo(wirCode);
        
        // The stageInfo will always return a valid color class
        // - Known stages (WIR-1 to WIR-6): Use predefined colors (wir-stage-1 to wir-stage-6)
        // - Unknown/dynamic stages: Use colors 7-12 only, preventing conflicts
        return stageInfo.colorClass;
    }

    viewBox(boxId: string): void {
        // Find the box to get projectId
        const box = this.boxes.find(b => b.id === boxId);
        if (box && box.projectId) {
            this.selectedBoxId = boxId;
            this.selectedBoxProjectId = box.projectId;
            this.isBoxQuickViewModalOpen = true;
        } else {
            console.error('Box not found or missing projectId:', boxId);
        }
    }

    closeBoxQuickViewModal(): void {
        this.isBoxQuickViewModalOpen = false;
        this.selectedBoxId = '';
        this.selectedBoxProjectId = '';
    }

    goBack(): void {
        this.router.navigate(['/factories', this.factoryId]);
    }

    /**
     * Navigate to walls status page
     */
    viewWallsStatus(): void {
        this.router.navigate(['/factories', this.factoryId, 'walls-status']);
    }

    /**
     * Toggle between matrix and two-column layout modes
     */
    toggleLayoutMode(): void {
        this.layoutMode = this.layoutMode === 'matrix' ? 'two-column' : 'matrix';
    }

    /**
     * Get Assembly sections in the order provided by backend (DisplayOrder ASC)
     * Backend already orders sections by DisplayOrder field
     */
    getAssemblySections(): FactorySection[] {
        if (!this.factory || !this.factory.sections) {
            return [];
        }
        const assemblySections = this.factory.sections
            .filter(s => {
                const isAssembly = s.sectionType === FactorySectionType.Assembly;
                console.log(`Section "${s.sectionName}" - Type: ${s.sectionType} (${typeof s.sectionType}), IsAssembly: ${isAssembly}, Expected: ${FactorySectionType.Assembly}, DisplayOrder: ${s.displayOrder}`);
                return isAssembly;
            });
        console.log(`✅ Found ${assemblySections.length} Assembly sections:`, assemblySections.map(s => `${s.sectionName} (DisplayOrder: ${s.displayOrder})`));
        return assemblySections;
    }

    /**
     * Get Finishing sections in the order provided by backend (DisplayOrder ASC)
     * Backend already orders sections by DisplayOrder field
     */
    getFinishingSections(): FactorySection[] {
        if (!this.factory || !this.factory.sections) {
            return [];
        }
        const finishingSections = this.factory.sections
            .filter(s => {
                const isFinishing = s.sectionType === FactorySectionType.Finishing;
                console.log(`Section "${s.sectionName}" - Type: ${s.sectionType} (${typeof s.sectionType}), IsFinishing: ${isFinishing}, Expected: ${FactorySectionType.Finishing}, DisplayOrder: ${s.displayOrder}`);
                return isFinishing;
            });
        console.log(`✅ Found ${finishingSections.length} Finishing sections:`, finishingSections.map(s => `${s.sectionName} (DisplayOrder: ${s.displayOrder})`));
        return finishingSections;
    }

    /**
     * Get layout configuration for two-column view
     * Sections are rendered in natural grid order (left-to-right, top-to-bottom) based on DisplayOrder from backend
     * 
     * Expected DisplayOrder example:
     * 1 → Finishing-2 (Top-Left)
     * 2 → Assembly-2 (Top-Right)
     * 3 → Finishing-1 (Bottom-Left)
     * 4 → Assembly-1 (Bottom-Right)
     * 
     * The backend already orders sections by DisplayOrder ASC, so we just need to:
     * - First 2 sections (indices 0, 1) go to top row (left, right)
     * - Next 2 sections (indices 2, 3) go to bottom row (left, right)
     */
    getLayoutConfiguration(): any {
        if (!this.factory || !this.factory.sections) {
            return { leftColumn: { sections: [] }, rightColumn: { sections: [] } };
        }

        // Backend already ordered sections by DisplayOrder ASC
        const orderedSections = this.factory.sections;

        console.log('🔍 Layout Configuration - Ordered Sections:', orderedSections.map(s =>
            `${s.sectionName} (DisplayOrder: ${s.displayOrder}, Type: ${s.sectionType})`
        ));

        // Split sections into rows based on DisplayOrder
        // Top row: first 2 sections (indices 0, 1)
        // Bottom row: next 2 sections (indices 2, 3)
        const topRowSections = orderedSections.slice(0, 2);
        const bottomRowSections = orderedSections.slice(2, 4);

        // Map sections to include their grid data
        const mapSectionToData = (section: FactorySection) => {
            // Sort parts by minBay to ensure they appear in the correct bay order (B-D, E-G, etc.)
            const sortedParts = (section.parts || []).sort((a, b) => {
                // Compare minBay letters (e.g., 'B' vs 'E')
                if (!a.minBay || !b.minBay) return 0;
                return a.minBay.localeCompare(b.minBay);
            });

            return {
                section,
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
            }
        };
    }

    /**
     * Check if current layout mode is two-column
     */
    isTwoColumnLayout(): boolean {
        return this.layoutMode === 'two-column';
    }

    /**
     * Check if a cell should be rendered as a conveyor slot
     * Conveyor slots are positioned near the center line of the Finishing section
     */
    isConveyorSlot(rowIndex: number, colIndex: number, grid: any): boolean {
        // Twin narrow slots near the center line
        // Position them at approximately 1/3 and 2/3 of the grid width
        const totalCols = grid.columns.length;
        const slot1Position = Math.floor(totalCols / 3);
        const slot2Position = Math.floor((totalCols * 2) / 3);

        return colIndex === slot1Position || colIndex === slot2Position;
    }

    /**
     * Get summary statistics for boxes
     * Uses factorySectionId for accurate section assignment
     */
    getSummaryStats(): { assemblyCount: number; finishingCount: number; totalCount: number } {
        const boxesWithPosition = this.filteredBoxes.filter(box =>
            (box.bay || box.row || box.position) && box.status !== BoxStatus.Dispatched
        );

        let assemblyCount = 0;
        let finishingCount = 0;

        if (this.factory && this.factory.sections && this.factory.sections.length > 0) {
            const assemblySections = this.getAssemblySections();
            const finishingSections = this.getFinishingSections();

            // CRITICAL: Use factorySectionId for accurate counting
            // Match the same logic used in the grid display
            boxesWithPosition.forEach(box => {
                // First try to use factorySectionId (most accurate)
                if (box.factorySectionId) {
                    const inAssembly = assemblySections.some(section => 
                        section.sectionId === box.factorySectionId
                    );
                    const inFinishing = finishingSections.some(section => 
                        section.sectionId === box.factorySectionId
                    );
                    
                    if (inAssembly) {
                        assemblyCount++;
                    } else if (inFinishing) {
                        finishingCount++;
                    }
                } else {
                    // Fallback for orphan boxes: use bay coordinates
                    if (!box.bay) return;
                    
                    const inAssembly = assemblySections.some(section =>
                        section.minBay && section.maxBay &&
                        box.bay! >= section.minBay && box.bay! <= section.maxBay
                    );

                    const inFinishing = finishingSections.some(section =>
                        section.minBay && section.maxBay &&
                        box.bay! >= section.minBay && box.bay! <= section.maxBay
                    );

                    if (inAssembly) {
                        assemblyCount++;
                    } else if (inFinishing) {
                        finishingCount++;
                    }
                }
            });
        } else {
            // Fallback: If no sections defined, use WIR stage to determine assembly vs finishing
            // Typically WIR-1 to WIR-5 are assembly, WIR-6 onwards are finishing
            boxesWithPosition.forEach(box => {
                const wirStage = this.getBoxWIRStage(box);
                if (wirStage) {
                    const wirNum = this.extractWIRNumber(wirStage.wirCode);
                    if (wirNum >= 1 && wirNum <= 5) {
                        assemblyCount++;
                    } else if (wirNum >= 6 || wirStage.wirCode === 'COMPLETED') {
                        finishingCount++;
                    }
                } else {
                    // If no WIR stage, consider all boxes as assembly
                    assemblyCount++;
                }
            });
        }

        return {
            assemblyCount,
            finishingCount,
            totalCount: boxesWithPosition.length
        };
    }

    /**
     * Get dispatched boxes count by floor/level
     */
    getDispatchedByFloor(): Array<{ floor: string; count: number }> {
        const dispatchedBoxes = this.boxes.filter(box => box.status === BoxStatus.Dispatched);

        const floorMap = new Map<string, number>();

        dispatchedBoxes.forEach(box => {
            const floor = box.floor || 'Unknown';
            floorMap.set(floor, (floorMap.get(floor) || 0) + 1);
        });

        const result = Array.from(floorMap.entries()).map(([floor, count]) => ({
            floor,
            count
        }));

        // Sort by floor name
        result.sort((a, b) => a.floor.localeCompare(b.floor));

        return result;
    }

    /**
     * Get total count of dispatched boxes
     */
    getTotalDispatchedBoxes(): number {
        return this.getDispatchedByFloor().reduce((sum, item) => sum + item.count, 0);
    }

    /**
     * Get assembly and finishing boxes count by building
     */
    getBoxesByBuilding(): Array<{
        building: string;
        assemblyCount: number;
        readyForFinishing: number;
        finishingCount: number;
        totalCount: number
    }> {
        const boxesWithPosition = this.filteredBoxes.filter(box =>
            (box.bay || box.row || box.position) && box.status !== BoxStatus.Dispatched
        );

        const buildingMap = new Map<string, {
            assembly: number;
            readyForFinishing: number;
            finishing: number
        }>();

        boxesWithPosition.forEach(box => {
            const building = box.buildingNumber || 'Unknown';

            if (!buildingMap.has(building)) {
                buildingMap.set(building, { assembly: 0, readyForFinishing: 0, finishing: 0 });
            }

            const buildingData = buildingMap.get(building)!;

            // Check if box is ready for finishing (based on WIR stage or progress)
            const wirStage = this.getBoxWIRStage(box);
            const isReadyForFinishing = wirStage && (
                wirStage.wirCode === 'WIR-6' ||
                wirStage.wirCode === 'WIR-7' ||
                wirStage.wirCode === 'WIR-8' ||
                box.progress >= 80
            );

            if (this.factory && this.factory.sections && this.factory.sections.length > 0) {
                const assemblySections = this.getAssemblySections();
                const finishingSections = this.getFinishingSections();

                let inAssembly = false;
                let inFinishing = false;

                // CRITICAL: Use factorySectionId for accurate section assignment (same as getSummaryStats)
                if (box.factorySectionId) {
                    inAssembly = assemblySections.some(section => 
                        section.sectionId === box.factorySectionId
                    );
                    inFinishing = finishingSections.some(section => 
                        section.sectionId === box.factorySectionId
                    );
                } else {
                    // Fallback: Use bay coordinates if factorySectionId is not available
                    inAssembly = box.bay && assemblySections.some(section =>
                        section.minBay && section.maxBay &&
                        box.bay! >= section.minBay && box.bay! <= section.maxBay
                    ) || false;

                    inFinishing = box.bay && finishingSections.some(section =>
                        section.minBay && section.maxBay &&
                        box.bay! >= section.minBay && box.bay! <= section.maxBay
                    ) || false;
                }

                if (inAssembly) {
                    if (isReadyForFinishing) {
                        buildingData.readyForFinishing++;
                    } else {
                        buildingData.assembly++;
                    }
                } else if (inFinishing) {
                    buildingData.finishing++;
                } else {
                    // Default to assembly if not in any section
                    if (isReadyForFinishing) {
                        buildingData.readyForFinishing++;
                    } else {
                        buildingData.assembly++;
                    }
                }
            } else {
                // Fallback: Use WIR stage to determine assembly vs finishing
                if (wirStage) {
                    const wirNum = this.extractWIRNumber(wirStage.wirCode);
                    if (wirNum >= 1 && wirNum <= 5) {
                        if (isReadyForFinishing) {
                            buildingData.readyForFinishing++;
                        } else {
                            buildingData.assembly++;
                        }
                    } else if (wirNum >= 6 || wirStage.wirCode === 'COMPLETED') {
                        buildingData.finishing++;
                    }
                } else {
                    // No WIR stage - default to assembly
                    buildingData.assembly++;
                }
            }
        });

        const result = Array.from(buildingMap.entries()).map(([building, data]) => ({
            building,
            assemblyCount: data.assembly,
            readyForFinishing: data.readyForFinishing,
            finishingCount: data.finishing,
            totalCount: data.assembly + data.readyForFinishing + data.finishing
        }));

        // Sort by building name
        result.sort((a, b) => a.building.localeCompare(b.building));

        return result;
    }

    /**
     * Get total assembly boxes across all buildings
     */
    getTotalAssemblyBoxes(): number {
        return this.getBoxesByBuilding().reduce((sum, item) => sum + item.assemblyCount, 0);
    }

    /**
     * Get total ready for finishing boxes across all buildings
     */
    getTotalReadyForFinishing(): number {
        return this.getBoxesByBuilding().reduce((sum, item) => sum + item.readyForFinishing, 0);
    }

    /**
     * Get total finishing boxes across all buildings
     */
    getTotalFinishingBoxes(): number {
        return this.getBoxesByBuilding().reduce((sum, item) => sum + item.finishingCount, 0);
    }

    /**
     * Get total boxes across all buildings
     */
    getTotalBoxesByBuilding(): number {
        return this.getBoxesByBuilding().reduce((sum, item) => sum + item.totalCount, 0);
    }

    /**
     * Get the number of empty slots in the factory
     */
    getEmptySlotCount(): number {
        if (!this.factory) return 0;

        const gridLayout = this.getGridLayout();
        const totalSlots = gridLayout.rows.length * gridLayout.columns.length;
        const occupiedSlots = this.boxes.filter(box =>
            (box.bay || box.row || box.position) && box.status !== BoxStatus.Dispatched
        ).length;

        return Math.max(0, totalSlots - occupiedSlots);
    }

    /**
     * Calculate bar width percentage for visualization
     * @param count The count to calculate width for
     */
    getBarWidth(count: number): number {
        if (!this.factory) return 0;

        const gridLayout = this.getGridLayout();
        const totalSlots = gridLayout.rows.length * gridLayout.columns.length;

        if (totalSlots === 0) return 0;

        return Math.round((count / totalSlots) * 100);
    }

    /**
     * Open export/print modal
     */
    openExportModal(action: 'print' | 'excel'): void {
        this.exportAction = action;
        this.selectedSectionsForExport = [];
        this.isExportModalOpen = true;
    }

    /**
     * Close export/print modal
     */
    closeExportModal(): void {
        this.isExportModalOpen = false;
        this.selectedSectionsForExport = [];
        this.exportAction = null;
    }

    /**
     * Toggle section selection for export/print
     */
    toggleSectionSelection(sectionId: string): void {
        const index = this.selectedSectionsForExport.indexOf(sectionId);
        if (index > -1) {
            this.selectedSectionsForExport.splice(index, 1);
        } else {
            this.selectedSectionsForExport.push(sectionId);
        }
    }

    /**
     * Check if a section is selected for export/print
     */
    isSectionSelected(sectionId: string): boolean {
        return this.selectedSectionsForExport.includes(sectionId);
    }

    /**
     * Select all sections for export/print
     */
    selectAllSections(): void {
        if (this.factory && this.factory.sections) {
            this.selectedSectionsForExport = this.factory.sections
                .map(s => s.sectionId)
                .filter(id => id !== undefined && id !== null) as string[];
        }
    }

    /**
     * Clear all section selections
     */
    clearAllSections(): void {
        this.selectedSectionsForExport = [];
    }

    /**
     * Execute export or print based on selected action
     */
    executeExport(): void {
        if (this.exportAction === 'print') {
            this.printFactoryLayout();
        } else if (this.exportAction === 'excel') {
            this.exportToExcel();
        }
        this.closeExportModal();
    }

    /** High-resolution scale for canvas capture (2 = 2x resolution). */
    private readonly LAYOUT_CAPTURE_SCALE = 2;

    /**
     * Capture the layout DOM element as a high-resolution canvas using html2canvas.
     * Temporarily applies export-mode class to fix vertical text rendering issues.
     */
    private async captureLayoutCanvas(): Promise<HTMLCanvasElement> {
        const el = this.layoutPrintArea?.nativeElement;
        if (!el) {
            throw new Error('Layout not ready.');
        }
        
        // Add export-mode class to fix vertical text rendering with html2canvas
        // This removes transform: rotate() and relies on writing-mode for vertical text
        el.classList.add('export-mode');
        
        // Small delay to ensure DOM updates with new styles before capturing
        await new Promise(resolve => setTimeout(resolve, 100));
        
        try {
            // Capture the canvas with export-mode active
            const canvas = await html2canvas(el, {
                scale: this.LAYOUT_CAPTURE_SCALE,
                useCORS: true,
                allowTaint: true,
                backgroundColor: '#ffffff',
                logging: false,
            });
            
            return canvas;
        } finally {
            // Always remove export-mode class after capture (even if error occurs)
            el.classList.remove('export-mode');
        }
    }

    /**
     * Export the factory layout as a high-resolution PNG (captured via html2canvas).
     */
    async exportLayoutAsPng(): Promise<void> {
        try {
            const canvas = await this.captureLayoutCanvas();
            const dataUrl = canvas.toDataURL('image/png');
            const fileName = `Factory_Layout_${(this.factory?.factoryCode || 'Layout').replace(/\s+/g, '_')}.png`;
            const link = document.createElement('a');
            link.download = fileName;
            link.href = dataUrl;
            link.click();
        } catch (err) {
            console.error('Error exporting layout PNG:', err);
            alert('Failed to export PNG. Please wait for the page to load and try again.');
        }
    }

    /**
     * Print factory layout as A3 landscape PDF (high-resolution image via html2canvas).
     */
    async printFactoryLayout(): Promise<void> {
        try {
            const canvas = await this.captureLayoutCanvas();
            const imgData = canvas.toDataURL('image/jpeg', 0.95);
            const fileName = `Factory_Layout_${(this.factory?.factoryCode || 'Layout').replace(/\s+/g, '_')}.pdf`;
            // A3 landscape: 420mm x 297mm
            const pdf = new jsPDF({
                orientation: 'landscape',
                unit: 'mm',
                format: 'a3',
                compress: true,
            });
            const pageW = pdf.internal.pageSize.getWidth();
            const pageH = pdf.internal.pageSize.getHeight();
            pdf.addImage(imgData, 'JPEG', 0, 0, pageW, pageH, undefined, 'FAST');
            pdf.save(fileName);
        } catch (err) {
            console.error('Error generating layout PDF:', err);
            alert('Failed to generate PDF. Please wait for the page to load and try again.');
        }
    }

    /**
     * Export factory layout to Excel with Visual Styling using ExcelJS
     * Creates a visual factory layout with colors, borders, and proper spacing
     */
    async exportToExcel(): Promise<void> {
        // For Two-Column layout, create a comprehensive visual layout
        if (this.isTwoColumnLayout() && this.hasSections) {
            await this.exportVisualLayoutToExcel();
        } else {
            // For matrix view or non-sectioned layouts
            this.exportMatrixLayoutToExcel();
        }
    }

    /**
     * Export modern, clean factory layout - Matching reference image UI/UX
     */
    private async exportVisualLayoutToExcel(): Promise<void> {
        const workbook = new ExcelJS.Workbook();
        const worksheet = workbook.addWorksheet('Factory Layout', {
            views: [{ showGridLines: false }]
        });

        // Cell dimensions matching the UI
        const CELL_SIZE = 8;
        const CELL_HEIGHT = 35;
        const AISLE_WIDTH = 1;

        let currentRow = 1;
        const LAYOUT_START_COL = 1; // Grid starts at column 1 (no separate legend column)

        // 1) Summary section (three cards side-by-side)
        currentRow = this.renderSummarySectionExcel(worksheet, currentRow);
        currentRow++; // blank spacer row

        // 2) Business Flow Indicators (single horizontal row — the only legend)
        currentRow = this.renderBusinessFlowHorizontalExcel(worksheet, currentRow, 50);
        currentRow += 2; // spacing before grid

        // 3) Factory grid
        const layoutConfig = this.getLayoutConfiguration();
        const gridStartRow = currentRow;

        // Start at column 2 to leave space for left part labels
        const GRID_START_COL = 2;

        await this.renderUltraCleanLayout(
            worksheet,
            layoutConfig,
            gridStartRow,
            GRID_START_COL,
            CELL_SIZE,
            CELL_HEIGHT,
            AISLE_WIDTH
        );

        // Column widths
        // Left part label column (column 1)
        worksheet.getColumn(1).width = 3;
        
        // Bay letter column (B, C, D…)
        worksheet.getColumn(GRID_START_COL).width = 5;
        
        // Data columns
        for (let i = GRID_START_COL + 1; i <= 100; i++) {
            worksheet.getColumn(i).width = CELL_SIZE;
        }

        // Set right bay column and right part label column widths
        // Frozen columns keep the same width as regular cells (matching the factory grid UI)
        const topLeftSection = layoutConfig.leftColumn.sections[0];
        const topRightSection = layoutConfig.rightColumn.sections[0];
        
        const leftParts: any[] = topLeftSection?.parts || [];
        const rightParts: any[] = topRightSection?.parts || [];

        const leftGrid = leftParts.length > 0 
            ? leftParts[0].grid 
            : topLeftSection?.grid;
            
        const rightGrid = rightParts.length > 0 
            ? rightParts[0].grid 
            : topRightSection?.grid;

        if (leftGrid && rightGrid) {
            // Right bay column: immediately after right data columns
            const rightBayCol = GRID_START_COL + 1 + leftGrid.columns.length + AISLE_WIDTH + rightGrid.columns.length;
            worksheet.getColumn(rightBayCol).width = 5;
            // Right part label column: after right bay column
            worksheet.getColumn(rightBayCol + 1).width = 3;
        } else if (leftGrid) {
            // Only left grid available
            const rightBayCol = GRID_START_COL + 1 + leftGrid.columns.length + AISLE_WIDTH;
            worksheet.getColumn(rightBayCol).width = 5;
            worksheet.getColumn(rightBayCol + 1).width = 3;
        }

        // Freeze: summary + business-flow rows stay visible when scrolling
        worksheet.views = [
            {
                state: 'frozen',
                xSplit: 0,
                ySplit: gridStartRow - 1, // Freeze all rows above the grid
                topLeftCell: 'A' + gridStartRow,
                activeCell: 'A' + gridStartRow,
                showGridLines: false
            }
        ];

        // Generate and download file
        const timestamp = new Date().toISOString().split('T')[0];
        const filename = `Factory_Layout_${this.factory?.factoryCode || 'Export'}_${timestamp}.xlsx`;

        const buffer = await workbook.xlsx.writeBuffer();
        const blob = new Blob([buffer], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.download = filename;
        link.click();
        window.URL.revokeObjectURL(url);

        // Show success message
        document.dispatchEvent(new CustomEvent('app-toast', {
            detail: {
                message: 'Factory layout exported successfully!',
                type: 'success'
            }
        }));
    }

    /**
     * Render summary section in Excel — three cards side-by-side using grid column widths.
     * Card 1 (Total Boxes):     cols 1-3
     * Card 2 (Dispatched Boxes): cols 5-7   (col 4 = gap)
     * Card 3 (Boxes by Building): cols 9-14 (col 8 = gap)
     * Returns the next row after the section.
     */
    private renderSummarySectionExcel(worksheet: ExcelJS.Worksheet, startRow: number): number {
        const stats = this.getSummaryStats();
        const emptySlots = this.getEmptySlotCount();
        const totalBoxes = this.getTotalBoxesByBuilding();
        const dispatched = this.getDispatchedByFloor();
        const byBuilding = this.getBoxesByBuilding();

        const card1Rows = 5;
        const card2Rows = dispatched.length > 0 ? 2 + dispatched.length + 1 : 3;
        const card3Rows = byBuilding.length > 0 ? 2 + byBuilding.length + 1 : 3;
        const maxRows = Math.max(card1Rows, card2Rows, card3Rows);

        const headerFill: ExcelJS.Fill = { type: 'pattern', pattern: 'solid', fgColor: { argb: 'FFF8FAFC' } };
        const whiteFill: ExcelJS.Fill = { type: 'pattern', pattern: 'solid', fgColor: { argb: 'FFFFFFFF' } };
        const headerFont: Partial<ExcelJS.Font> = { name: 'Calibri', size: 9, bold: true, color: { argb: 'FF1E293B' } };
        const cellFont: Partial<ExcelJS.Font> = { name: 'Calibri', size: 9, color: { argb: 'FF334155' } };
        const boldFont: Partial<ExcelJS.Font> = { name: 'Calibri', size: 9, bold: true, color: { argb: 'FF334155' } };
        const border: Partial<ExcelJS.Borders> = {
            top: { style: 'thin', color: { argb: 'FFE2E8F0' } },
            left: { style: 'thin', color: { argb: 'FFE2E8F0' } },
            bottom: { style: 'thin', color: { argb: 'FFE2E8F0' } },
            right: { style: 'thin', color: { argb: 'FFE2E8F0' } }
        };

        // Card column ranges
        const C1_START = 1; const C1_END = 3;
        const C2_START = 5; const C2_END = 7;
        const C3_START = 9; const C3_END = 14;

        // --- Card titles ---
        worksheet.mergeCells(startRow, C1_START, startRow, C1_END);
        const t1 = worksheet.getCell(startRow, C1_START);
        t1.value = 'TOTAL BOXES OVERVIEW';
        t1.font = headerFont; t1.alignment = { horizontal: 'left', vertical: 'middle' }; t1.fill = headerFill; t1.border = border;

        worksheet.mergeCells(startRow, C2_START, startRow, C2_END);
        const t2 = worksheet.getCell(startRow, C2_START);
        t2.value = 'DISPATCHED BOXES';
        t2.font = headerFont; t2.alignment = { horizontal: 'left', vertical: 'middle' }; t2.fill = headerFill; t2.border = border;

        worksheet.mergeCells(startRow, C3_START, startRow, C3_END);
        const t3 = worksheet.getCell(startRow, C3_START);
        t3.value = 'BOXES BY BUILDING';
        t3.font = headerFont; t3.alignment = { horizontal: 'left', vertical: 'middle' }; t3.fill = headerFill; t3.border = border;

        worksheet.getRow(startRow).height = 20;
        let row = startRow + 1;

        // Helper to style a range of cells
        const styleCell = (r: number, c: number, value: any, font: Partial<ExcelJS.Font>) => {
            const cell = worksheet.getCell(r, c);
            cell.value = value;
            cell.font = font;
            cell.border = border;
            cell.fill = whiteFill;
            cell.alignment = { vertical: 'middle' };
        };

        for (let i = 0; i < maxRows; i++) {
            // --- Card 1: Total Boxes Overview (cols 1-2 label, col 3 value) ---
            if (i === 0) { styleCell(row, 1, 'Assembly', cellFont); worksheet.mergeCells(row, 1, row, 2); styleCell(row, 3, stats.assemblyCount, cellFont); }
            else if (i === 1) { styleCell(row, 1, 'Finishing', cellFont); worksheet.mergeCells(row, 1, row, 2); styleCell(row, 3, stats.finishingCount, cellFont); }
            else if (i === 2) { styleCell(row, 1, 'Empty Slots', cellFont); worksheet.mergeCells(row, 1, row, 2); styleCell(row, 3, emptySlots, cellFont); }
            else if (i === 3) { styleCell(row, 1, 'Total Boxes', boldFont); worksheet.mergeCells(row, 1, row, 2); styleCell(row, 3, totalBoxes, boldFont); }
            else { for (let c = C1_START; c <= C1_END; c++) styleCell(row, c, '', cellFont); }

            // --- Card 2: Dispatched Boxes (cols 5-6 label, col 7 value) ---
            if (i === 0) {
                styleCell(row, C2_START, 'Level / Floor Count', headerFont); worksheet.mergeCells(row, C2_START, row, C2_START + 1);
                styleCell(row, C2_END, 'Count', headerFont);
            } else if (dispatched.length === 0 && i === 1) {
                styleCell(row, C2_START, 'No dispatched boxes', cellFont);
                worksheet.mergeCells(row, C2_START, row, C2_END);
            } else if (dispatched.length > 0 && i >= 1 && i <= dispatched.length) {
                const item = dispatched[i - 1];
                styleCell(row, C2_START, item.floor, cellFont); worksheet.mergeCells(row, C2_START, row, C2_START + 1);
                styleCell(row, C2_END, item.count, cellFont);
            } else if (dispatched.length > 0 && i === dispatched.length + 1) {
                styleCell(row, C2_START, 'Total', boldFont); worksheet.mergeCells(row, C2_START, row, C2_START + 1);
                styleCell(row, C2_END, this.getTotalDispatchedBoxes(), boldFont);
            } else { for (let c = C2_START; c <= C2_END; c++) styleCell(row, c, '', cellFont); }

            // --- Card 3: Boxes by Building (cols 9-14) ---
            if (i === 0) {
                ['Building', 'Assembly', 'Ready', 'Finishing', 'Total', ''].forEach((val, idx) => {
                    const c = C3_START + idx;
                    if (c <= C3_END) {
                        const cell = worksheet.getCell(row, c);
                        cell.value = val; cell.font = headerFont; cell.border = border; cell.fill = headerFill;
                        cell.alignment = { horizontal: 'center', vertical: 'middle' };
                    }
                });
            } else if (byBuilding.length > 0 && i >= 1 && i <= byBuilding.length) {
                const b = byBuilding[i - 1];
                styleCell(row, 9, b.building, cellFont);
                styleCell(row, 10, b.assemblyCount, cellFont);
                styleCell(row, 11, b.readyForFinishing, cellFont);
                styleCell(row, 12, b.finishingCount, cellFont);
                styleCell(row, 13, b.totalCount, cellFont);
                styleCell(row, 14, '', cellFont);
            } else if (byBuilding.length > 0 && i === byBuilding.length + 1) {
                styleCell(row, 9, 'Total', boldFont);
                styleCell(row, 10, this.getTotalAssemblyBoxes(), boldFont);
                styleCell(row, 11, this.getTotalReadyForFinishing(), boldFont);
                styleCell(row, 12, this.getTotalFinishingBoxes(), boldFont);
                styleCell(row, 13, this.getTotalBoxesByBuilding(), boldFont);
                styleCell(row, 14, '', boldFont);
            } else if (byBuilding.length === 0 && i === 1) {
                styleCell(row, C3_START, 'No data available', cellFont);
                worksheet.mergeCells(row, C3_START, row, C3_END);
            } else {
                for (let c = C3_START; c <= C3_END; c++) styleCell(row, c, '', cellFont);
            }

            worksheet.getRow(row).height = 18;
            row++;
        }

        return row;
    }

    /**
     * Render Business Flow Indicators with wrapping support (multiple rows if needed).
     * Returns the next row after the section.
     */
    private renderBusinessFlowHorizontalExcel(worksheet: ExcelJS.Worksheet, startRow: number, maxCol: number): number {
        // Title cell
        worksheet.mergeCells(startRow, 1, startRow, 2);
        const titleCell = worksheet.getCell(startRow, 1);
        titleCell.value = 'BUSINESS FLOW INDICATORS';
        titleCell.font = { name: 'Calibri', size: 9, bold: true, color: { argb: 'FF1E293B' } };
        titleCell.alignment = { horizontal: 'left', vertical: 'middle' };
        titleCell.fill = { type: 'pattern', pattern: 'solid', fgColor: { argb: 'FFFFFFFF' } };
        titleCell.border = { top: { style: 'thin', color: { argb: 'FFE2E8F0' } }, left: { style: 'thin', color: { argb: 'FFE2E8F0' } }, bottom: { style: 'thin', color: { argb: 'FFE2E8F0' } }, right: { style: 'thin', color: { argb: 'FFE2E8F0' } } };

        // Render stages with wrapping (max 8 per row)
        const ITEMS_PER_ROW = 8;
        const allIndicators = [...this.availableStages, { wirCode: 'EMPTY', displayName: 'Empty' }];
        let currentRow = startRow;
        let indicatorIndex = 0;

        while (indicatorIndex < allIndicators.length) {
            let col = 3; // Start after title
            const itemsInThisRow = Math.min(ITEMS_PER_ROW, allIndicators.length - indicatorIndex);

            for (let i = 0; i < itemsInThisRow; i++) {
                const indicator = allIndicators[indicatorIndex];
                const isEmpty = indicator.wirCode === 'EMPTY';
                const stageInfo = isEmpty ? null : getWIRStageInfo(indicator.wirCode);
                const bgColor = isEmpty ? 'F3F4F6' : this.getColorForStage(stageInfo?.colorClass || '');

                const cell = worksheet.getCell(currentRow, col);
                cell.value = indicator.displayName;
                cell.font = { name: 'Calibri', size: 9, color: { argb: isEmpty ? 'FF64748B' : 'FF475569' } };
                cell.alignment = { horizontal: 'center', vertical: 'middle', wrapText: true };
                cell.fill = { type: 'pattern', pattern: 'solid', fgColor: { argb: 'FF' + bgColor } };
                cell.border = { top: { style: 'thin', color: { argb: 'FFE0E0E0' } }, left: { style: 'thin', color: { argb: 'FFE0E0E0' } }, bottom: { style: 'thin', color: { argb: 'FFE0E0E0' } }, right: { style: 'thin', color: { argb: 'FFE0E0E0' } } };
                worksheet.getColumn(col).width = 18; // Wider columns for better text wrapping

                col++;
                indicatorIndex++;
            }

            worksheet.getRow(currentRow).height = 30; // Taller rows to accommodate wrapped text
            currentRow++;
        }

        return currentRow;
    }


    /**
     * Render ultra-clean factory layout matching the reference UI.
     * Structure: column-headers → top sections → horizontal aisle → bottom sections → column-headers
     */
    private async renderUltraCleanLayout(
        worksheet: ExcelJS.Worksheet,
        layoutConfig: any,
        startRow: number,
        startCol: number,
        cellSize: number,
        cellHeight: number,
        aisleWidth: number
    ): Promise<void> {
        const topLeft = layoutConfig.leftColumn.sections[0];
        const topRight = layoutConfig.rightColumn.sections[0];
        const bottomLeft = layoutConfig.leftColumn.sections[1];
        const bottomRight = layoutConfig.rightColumn.sections[1];

        // Total column span
        // Calculate based on parts if available, otherwise use section grid
        const leftParts = topLeft?.parts || [];
        const rightParts = topRight?.parts || [];
        
        let leftDataCols = 0;
        let rightDataCols = 0;
        
        if (leftParts.length > 0) {
            leftDataCols = leftParts[0]?.grid?.columns?.length || 0;
        } else {
            leftDataCols = topLeft?.grid?.columns?.length || 0;
        }
        
        if (rightParts.length > 0) {
            rightDataCols = rightParts[0]?.grid?.columns?.length || 0;
        } else {
            rightDataCols = topRight?.grid?.columns?.length || 0;
        }
        
        // Total width: bay(left) + left-data + aisle + right-data + bay(right) + right-part-label
        // Note: merge starts at startCol-1 (left-part-label), so +1 columns beyond the label
        const totalWidth = 1 + leftDataCols + aisleWidth + rightDataCols + 1 + 1;

        let currentRow = startRow;
        const currentCol = startCol;

        // Top column headers
        await this.addModernColumnHeaders(worksheet, currentRow, currentCol, topLeft, topRight, aisleWidth, cellHeight, true);
        currentRow++;

        // Top sections (all parts)
        if (topLeft || topRight) {
            currentRow = await this.renderModernSectionPair(
                worksheet, topLeft, topRight, currentRow, currentCol, cellHeight, aisleWidth, 'TOP'
            );
        }

        // Horizontal CENTRAL AISLE (spans across entire width including left part label)
        const aisleRow = currentRow;
        worksheet.getRow(aisleRow).height = Math.max(cellHeight, 24);
        worksheet.mergeCells(aisleRow, startCol - 1, aisleRow, startCol - 1 + totalWidth);
        const aisleCell = worksheet.getCell(aisleRow, startCol - 1);
        aisleCell.value = 'CENTRAL AISLE';
        aisleCell.font = { name: 'Calibri', size: 11, bold: true, color: { argb: 'FF475569' } };
        aisleCell.alignment = { horizontal: 'center', vertical: 'middle' };
        aisleCell.fill = { type: 'pattern', pattern: 'solid', fgColor: { argb: 'FFD1D5DB' } };
        aisleCell.border = {
            top: { style: 'medium', color: { argb: 'FF9CA3AF' } },
            bottom: { style: 'medium', color: { argb: 'FF9CA3AF' } },
            left: { style: 'thin', color: { argb: 'FFD1D5DB' } },
            right: { style: 'thin', color: { argb: 'FFD1D5DB' } }
        };
        currentRow++;

        // Bottom sections (all parts)
        if (bottomLeft || bottomRight) {
            currentRow = await this.renderModernSectionPair(
                worksheet, bottomLeft, bottomRight, currentRow, currentCol, cellHeight, aisleWidth, 'BOTTOM'
            );
        }

        // Bottom column headers — use bottom sections so frozen columns reflect the bottom section's data
        await this.addModernColumnHeaders(worksheet, currentRow, currentCol, bottomLeft || topLeft, bottomRight || topRight, aisleWidth, cellHeight, false);
    }

    /**
     * Render section pair with individual parts properly displayed.
     * Each section has multiple parts (e.g., 4 parts per section).
     * Returns the next available row number.
     */
    private async renderModernSectionPair(
        worksheet: ExcelJS.Worksheet,
        leftSection: any,
        rightSection: any,
        startRow: number,
        startCol: number,
        cellHeight: number,
        aisleWidth: number,
        partLabel: string = ''
    ): Promise<number> {
        let currentRow = startRow;

        const leftParts = leftSection?.parts || [];
        const rightParts = rightSection?.parts || [];
        const maxParts = Math.max(leftParts.length, rightParts.length);

        // If no parts, use fallback section grid rendering
        if (maxParts === 0) {
            return this.renderSectionPairFallback(worksheet, leftSection, rightSection, startRow, startCol, cellHeight, aisleWidth);
        }

        // Loop through each part index (0, 1, 2, 3 for 4 parts)
        for (let partIdx = 0; partIdx < maxParts; partIdx++) {
            const leftPart = leftParts[partIdx];
            const rightPart = rightParts[partIdx];

            // Add part divider between parts (not before first part)
            if (partIdx > 0) {
                currentRow = this.renderPartDividerExcel(worksheet, currentRow, startCol, leftPart, rightPart, aisleWidth, cellHeight);
            }

            // Render this part's grid
            currentRow = await this.renderPartPairExcel(
                worksheet,
                leftPart,
                rightPart,
                currentRow,
                startCol,
                cellHeight,
                aisleWidth
            );
        }

        return currentRow;
    }

    /**
     * Render a pair of parts (left and right) with their grids
     */
    private async renderPartPairExcel(
        worksheet: ExcelJS.Worksheet,
        leftPart: any,
        rightPart: any,
        startRow: number,
        startCol: number,
        cellHeight: number,
        aisleWidth: number
    ): Promise<number> {
        let currentRow = startRow;

        const leftPartGrid = leftPart?.grid;
        const rightPartGrid = rightPart?.grid;
        const maxRows = Math.max(leftPartGrid?.matrix?.length || 0, rightPartGrid?.matrix?.length || 0);

        const sectionFirstRow = currentRow;

        // Get column widths to determine part label position
        const leftDataCols = leftPartGrid?.columns?.length || 0;
        const rightDataCols = rightPartGrid?.columns?.length || 0;
        const leftPartLabelCol = startCol - 1; // Left part label before bay column
        // Right layout: ... right-data | right-bay | right-part-label
        const rightBayLabelCol = startCol + 1 + leftDataCols + aisleWidth + rightDataCols;
        const rightPartLabelCol = rightBayLabelCol + 1;

        const bayBorder = {
            top: { style: 'thin' as const, color: { argb: 'FFE2E8F0' } },
            left: { style: 'thin' as const, color: { argb: 'FFE2E8F0' } },
            bottom: { style: 'thin' as const, color: { argb: 'FFE2E8F0' } },
            right: { style: 'thin' as const, color: { argb: 'FFE2E8F0' } }
        };

        // Render each row in the part
        for (let rowIdx = 0; rowIdx < maxRows; rowIdx++) {
            let col = startCol;

            // Left bay label (B, C, D, etc.) — Finishing section, left side
            if (leftPartGrid && rowIdx < leftPartGrid.matrix.length) {
                const rowCells = leftPartGrid.matrix[rowIdx];
                const bayCell = worksheet.getCell(currentRow, col);
                bayCell.value = rowCells[0].bay;
                bayCell.font = { name: 'Calibri', size: 10, bold: true, color: { argb: 'FF475569' } };
                bayCell.alignment = { horizontal: 'center', vertical: 'middle' };
                bayCell.fill = { type: 'pattern', pattern: 'solid', fgColor: { argb: 'FFF1F5F9' } };
                bayCell.border = bayBorder;
                col++;

                // Left part data cells
                rowCells.forEach((cell: any) => {
                    this.renderModernDataCell(worksheet, currentRow, col, cell, false, leftPart?.partId ?? null);
                    col++;
                });
            } else {
                col += leftDataCols + 1;
            }

            // Vertical CENTRAL AISLE
            for (let i = 0; i < aisleWidth; i++) {
                const vAisleCell = worksheet.getCell(currentRow, col);
                vAisleCell.value = '';
                vAisleCell.fill = { type: 'pattern', pattern: 'solid', fgColor: { argb: 'FFD1D5DB' } };
                vAisleCell.border = {
                    top: { style: 'thin', color: { argb: 'FF9CA3AF' } },
                    bottom: { style: 'thin', color: { argb: 'FF9CA3AF' } },
                    left: { style: 'medium', color: { argb: 'FF9CA3AF' } },
                    right: { style: 'medium', color: { argb: 'FF9CA3AF' } }
                };
                col++;
            }

            // Right part data cells (display in same order as column headers: 12, 11, 10, ..., 1)
            if (rightPartGrid && rowIdx < rightPartGrid.matrix.length) {
                rightPartGrid.matrix[rowIdx].forEach((cell: any) => {
                    this.renderModernDataCell(worksheet, currentRow, col, cell, false, rightPart?.partId ?? null);
                    col++;
                });
            } else {
                col += rightDataCols;
            }

            // Right bay label — Assembly section, right side (mirrors left bay on opposite side)
            const rightBayCell = worksheet.getCell(currentRow, col);
            if (rightPartGrid && rowIdx < rightPartGrid.matrix.length) {
                rightBayCell.value = rightPartGrid.matrix[rowIdx][0]?.bay ?? '';
            } else {
                rightBayCell.value = '';
            }
            rightBayCell.font = { name: 'Calibri', size: 10, bold: true, color: { argb: 'FF475569' } };
            rightBayCell.alignment = { horizontal: 'center', vertical: 'middle' };
            rightBayCell.fill = { type: 'pattern', pattern: 'solid', fgColor: { argb: 'FFF1F5F9' } };
            rightBayCell.border = bayBorder;

            worksheet.getRow(currentRow).height = cellHeight;
            currentRow++;
        }

        // Add part labels from backend data (always show, use part name from backend)
        if (leftPart && currentRow > sectionFirstRow && leftPartLabelCol > 0) {
            worksheet.mergeCells(sectionFirstRow, leftPartLabelCol, currentRow - 1, leftPartLabelCol);
            const leftLabel = worksheet.getCell(sectionFirstRow, leftPartLabelCol);
            leftLabel.value = leftPart.partName || '';
            leftLabel.font = { name: 'Calibri', size: 8, bold: true, color: { argb: 'FF111827' } };
            leftLabel.alignment = { horizontal: 'center', vertical: 'middle', textRotation: 90 };
            leftLabel.fill = { type: 'pattern', pattern: 'solid', fgColor: { argb: 'FFFFFFFF' } };
            leftLabel.border = {
                top: { style: 'thin', color: { argb: 'FF111827' } },
                left: { style: 'thin', color: { argb: 'FF111827' } },
                bottom: { style: 'thin', color: { argb: 'FF111827' } },
                right: { style: 'thin', color: { argb: 'FF111827' } }
            };
            worksheet.getColumn(leftPartLabelCol).width = 3;
        }

        if (rightPart && currentRow > sectionFirstRow) {
            // Right bay label column — individual cells per row (NOT merged), set column width only
            worksheet.getColumn(rightBayLabelCol).width = 5;

            // Right part label column
            worksheet.mergeCells(sectionFirstRow, rightPartLabelCol, currentRow - 1, rightPartLabelCol);
            const rightLabel = worksheet.getCell(sectionFirstRow, rightPartLabelCol);
            rightLabel.value = rightPart.partName || '';
            rightLabel.font = { name: 'Calibri', size: 8, bold: true, color: { argb: 'FF111827' } };
            rightLabel.alignment = { horizontal: 'center', vertical: 'middle', textRotation: 90 };
            rightLabel.fill = { type: 'pattern', pattern: 'solid', fgColor: { argb: 'FFFFFFFF' } };
            rightLabel.border = {
                top: { style: 'thin', color: { argb: 'FF111827' } },
                left: { style: 'thin', color: { argb: 'FF111827' } },
                bottom: { style: 'thin', color: { argb: 'FF111827' } },
                right: { style: 'thin', color: { argb: 'FF111827' } }
            };
            worksheet.getColumn(rightPartLabelCol).width = 3;
        }

        return currentRow;
    }

    /**
     * Render part divider (red line between parts)
     */
    private renderPartDividerExcel(
        worksheet: ExcelJS.Worksheet,
        currentRow: number,
        startCol: number,
        leftPart: any,
        rightPart: any,
        aisleWidth: number,
        cellHeight: number
    ): number {
        const dividerRow = currentRow;
        worksheet.getRow(dividerRow).height = 4;

        const leftDataCols = leftPart?.grid?.columns?.length || 0;
        const rightDataCols = rightPart?.grid?.columns?.length || 0;
        // +1 left bay, +1 right bay (Assembly bay letters on the right side)
        const totalCols = 1 + leftDataCols + aisleWidth + rightDataCols + 1;

        let col = startCol;
        for (let i = 0; i < totalCols; i++) {
            const dc = worksheet.getCell(dividerRow, col);
            dc.value = '';
            dc.fill = { type: 'pattern', pattern: 'solid', fgColor: { argb: 'FF111827' } };
            dc.border = {
                top: { style: 'thin', color: { argb: 'FF374151' } },
                bottom: { style: 'thin', color: { argb: 'FF374151' } },
                left: { style: 'thin', color: { argb: 'FF111827' } },
                right: { style: 'thin', color: { argb: 'FF111827' } }
            };
            col++;
        }

        return currentRow + 1;
    }

    /**
     * Fallback rendering when sections don't have parts defined
     */
    private async renderSectionPairFallback(
        worksheet: ExcelJS.Worksheet,
        leftSection: any,
        rightSection: any,
        startRow: number,
        startCol: number,
        cellHeight: number,
        aisleWidth: number
    ): Promise<number> {
        let currentRow = startRow;

        const leftGrid = leftSection?.grid;
        const rightGrid = rightSection?.grid;
        const maxRows = Math.max(leftGrid?.matrix?.length || 0, rightGrid?.matrix?.length || 0);

        for (let rowIdx = 0; rowIdx < maxRows; rowIdx++) {
            let col = startCol;

            // Bay label
            if (leftGrid && rowIdx < leftGrid.matrix.length) {
                const rowCells = leftGrid.matrix[rowIdx];
                const bayCell = worksheet.getCell(currentRow, col);
                bayCell.value = rowCells[0].bay;
                bayCell.font = { name: 'Calibri', size: 10, bold: true, color: { argb: 'FF475569' } };
                bayCell.alignment = { horizontal: 'center', vertical: 'middle' };
                bayCell.fill = { type: 'pattern', pattern: 'solid', fgColor: { argb: 'FFF1F5F9' } };
                bayCell.border = {
                    top: { style: 'thin', color: { argb: 'FFE2E8F0' } },
                    left: { style: 'thin', color: { argb: 'FFE2E8F0' } },
                    bottom: { style: 'thin', color: { argb: 'FFE2E8F0' } },
                    right: { style: 'thin', color: { argb: 'FFE2E8F0' } }
                };
                col++;

                rowCells.forEach((cell: any) => {
                    this.renderModernDataCell(worksheet, currentRow, col, cell, false, null);
                    col++;
                });
            } else {
                col += (leftGrid?.columns?.length || 0) + 1;
            }

            // Vertical CENTRAL AISLE
            for (let i = 0; i < aisleWidth; i++) {
                const vAisleCell = worksheet.getCell(currentRow, col);
                vAisleCell.value = '';
                vAisleCell.fill = { type: 'pattern', pattern: 'solid', fgColor: { argb: 'FFD1D5DB' } };
                vAisleCell.border = {
                    top: { style: 'thin', color: { argb: 'FF9CA3AF' } },
                    bottom: { style: 'thin', color: { argb: 'FF9CA3AF' } },
                    left: { style: 'medium', color: { argb: 'FF9CA3AF' } },
                    right: { style: 'medium', color: { argb: 'FF9CA3AF' } }
                };
                col++;
            }

            // Right section data cells (display in order to match column headers)
            if (rightGrid && rowIdx < rightGrid.matrix.length) {
                rightGrid.matrix[rowIdx].forEach((cell: any) => {
                    this.renderModernDataCell(worksheet, currentRow, col, cell, false, null);
                    col++;
                });
            }

            worksheet.getRow(currentRow).height = cellHeight;
            currentRow++;
        }

        return currentRow;
    }

    /**
     * Add modern column headers with dark gray background.
     * Frozen columns (from backend freezingCellsMap) are highlighted with a lighter grey to match the UI.
     * Both sections display numbers right-to-left to match UI:
     * Finishing: 24, 23, 22, ... (right to left)
     * Assembly: 12, 11, 10, ..., 2, 1 (right to left)
     */
    private async addModernColumnHeaders(
        worksheet: ExcelJS.Worksheet,
        row: number,
        startCol: number,
        leftSection: any,
        rightSection: any,
        aisleWidth: number,
        cellHeight: number,
        isTop: boolean
    ): Promise<void> {
        let col = startCol;

        // Collect parts for dynamic frozen column detection
        const leftParts: any[] = leftSection?.parts || [];
        const rightParts: any[] = rightSection?.parts || [];

        // Left part label column header (before bay column)
        const leftPartHeaderCell = worksheet.getCell(row, col - 1);
        leftPartHeaderCell.value = '';
        leftPartHeaderCell.fill = { type: 'pattern', pattern: 'solid', fgColor: { argb: 'FF6B7280' } };
        leftPartHeaderCell.border = { top: { style: 'thin', color: { argb: 'FF4B5563' } }, left: { style: 'thin', color: { argb: 'FF4B5563' } }, bottom: { style: 'thin', color: { argb: 'FF4B5563' } }, right: { style: 'thin', color: { argb: 'FF4B5563' } } };

        // Bay column header (corner cell)
        const leftBayCell = worksheet.getCell(row, col);
        leftBayCell.value = '';
        leftBayCell.font = { name: 'Calibri', size: 8, bold: true, color: { argb: 'FFFFFFFF' } };
        leftBayCell.alignment = { horizontal: 'center', vertical: 'middle' };
        leftBayCell.fill = { type: 'pattern', pattern: 'solid', fgColor: { argb: 'FF6B7280' } };
        leftBayCell.border = { top: { style: 'thin', color: { argb: 'FF4B5563' } }, left: { style: 'thin', color: { argb: 'FF4B5563' } }, bottom: { style: 'thin', color: { argb: 'FF4B5563' } }, right: { style: 'thin', color: { argb: 'FF4B5563' } } };
        col++;

        // Get the grid from first part if parts exist, otherwise use section grid
        const leftGrid = leftParts.length > 0 
            ? leftParts[0].grid 
            : leftSection?.grid;

        // Left section column numbers — frozen columns highlighted dynamically
        if (leftGrid) {
            const headerBorder = { top: { style: 'thin' as const, color: { argb: 'FF4B5563' } }, left: { style: 'thin' as const, color: { argb: 'FF4B5563' } }, bottom: { style: 'thin' as const, color: { argb: 'FF4B5563' } }, right: { style: 'thin' as const, color: { argb: 'FF4B5563' } } };
            leftGrid.columns.forEach((colNum: string) => {
                const cell = worksheet.getCell(row, col);
                const isFrozenCol = leftParts.length > 0
                    ? this.isSectionColumnFrozen(leftParts, colNum)
                    : false;
                cell.value = parseInt(colNum);
                cell.numFmt = '0';
                cell.font = { name: 'Calibri', size: 9, bold: true, color: { argb: 'FFFFFFFF' } };
                cell.alignment = { horizontal: 'center', vertical: 'middle' };
                cell.fill = { type: 'pattern', pattern: 'solid', fgColor: { argb: isFrozenCol ? 'FF94A3B8' : 'FF6B7280' } };
                cell.border = headerBorder;
                col++;
            });
        }

        // Aisle column headers (vertical CENTRAL AISLE)
        for (let i = 0; i < aisleWidth; i++) {
            const aisleCell = worksheet.getCell(row, col);
            aisleCell.value = '';
            aisleCell.fill = { type: 'pattern', pattern: 'solid', fgColor: { argb: 'FFD1D5DB' } };
            aisleCell.border = {
                top: { style: 'thin', color: { argb: 'FF9CA3AF' } },
                bottom: { style: 'thin', color: { argb: 'FF9CA3AF' } },
                left: { style: 'medium', color: { argb: 'FF9CA3AF' } },
                right: { style: 'medium', color: { argb: 'FF9CA3AF' } }
            };
            col++;
        }

        // Get the grid from first part if parts exist, otherwise use section grid
        const rightGrid = rightParts.length > 0 
            ? rightParts[0].grid 
            : rightSection?.grid;

        // Right section column numbers — frozen columns highlighted dynamically
        if (rightGrid) {
            const headerBorder = { top: { style: 'thin' as const, color: { argb: 'FF4B5563' } }, left: { style: 'thin' as const, color: { argb: 'FF4B5563' } }, bottom: { style: 'thin' as const, color: { argb: 'FF4B5563' } }, right: { style: 'thin' as const, color: { argb: 'FF4B5563' } } };
            rightGrid.columns.forEach((colNum: string) => {
                const cell = worksheet.getCell(row, col);
                const isFrozenCol = rightParts.length > 0
                    ? this.isSectionColumnFrozen(rightParts, colNum)
                    : false;
                cell.value = parseInt(colNum);
                cell.numFmt = '0';
                cell.font = { name: 'Calibri', size: 9, bold: true, color: { argb: 'FFFFFFFF' } };
                cell.alignment = { horizontal: 'center', vertical: 'middle' };
                cell.fill = { type: 'pattern', pattern: 'solid', fgColor: { argb: isFrozenCol ? 'FF94A3B8' : 'FF6B7280' } };
                cell.border = headerBorder;
                col++;
            });
        }

        // Right bay corner cell (mirrors the left bay corner on the right side of Assembly)
        const rightBayCorner = worksheet.getCell(row, col);
        rightBayCorner.value = '';
        rightBayCorner.font = { name: 'Calibri', size: 8, bold: true, color: { argb: 'FFFFFFFF' } };
        rightBayCorner.alignment = { horizontal: 'center', vertical: 'middle' };
        rightBayCorner.fill = { type: 'pattern', pattern: 'solid', fgColor: { argb: 'FF6B7280' } };
        rightBayCorner.border = { top: { style: 'thin', color: { argb: 'FF4B5563' } }, left: { style: 'thin', color: { argb: 'FF4B5563' } }, bottom: { style: 'thin', color: { argb: 'FF4B5563' } }, right: { style: 'thin', color: { argb: 'FF4B5563' } } };
        col++;

        // Right part-label column header (empty corner cell matching the header bar)
        const partCorner = worksheet.getCell(row, col);
        partCorner.value = '';
        partCorner.fill = { type: 'pattern', pattern: 'solid', fgColor: { argb: 'FF6B7280' } };
        partCorner.border = { top: { style: 'thin', color: { argb: 'FF4B5563' } }, left: { style: 'thin', color: { argb: 'FF4B5563' } }, bottom: { style: 'thin', color: { argb: 'FF4B5563' } }, right: { style: 'thin', color: { argb: 'FF4B5563' } } };

        worksheet.getRow(row).height = 22;
    }

    /**
     * Render modern data cell - matches UI: box colors by stage, empty with light grid, frozen rows grey.
     * Uses dynamic freezingCellsMap to determine frozen rows per part.
     */
    private renderModernDataCell(
        worksheet: ExcelJS.Worksheet,
        row: number,
        col: number,
        cell: any,
        isConveyorColumn: boolean = false,
        partId: string | null = null
    ): void {
        const excelCell = worksheet.getCell(row, col);
        const gridBorder = { top: { style: 'thin' as const, color: { argb: 'FFE2E8F0' } }, left: { style: 'thin' as const, color: { argb: 'FFE2E8F0' } }, bottom: { style: 'thin' as const, color: { argb: 'FFE2E8F0' } }, right: { style: 'thin' as const, color: { argb: 'FFE2E8F0' } } };

        // Dynamic frozen row check: use freezingCellsMap from backend data
        const isFrozenRow = partId ? this.isRowFrozen(partId, cell.row) : false;

        // Frozen rows or conveyor columns - grey band like UI
        if (isConveyorColumn || isFrozenRow) {
            excelCell.value = '';
            excelCell.fill = { type: 'pattern', pattern: 'solid', fgColor: { argb: 'FFE5E7EB' } };
            excelCell.border = gridBorder;
            return;
        }

        if (cell.box) {
            const wirStage = this.getBoxWIRStage(cell.box);
            const colorClass = wirStage ? wirStage.colorClass : 'in-progress';
            const bgColor = this.getColorForStage(colorClass);
            const progressText = cell.box.progress != null && cell.box.progress !== undefined ? `\n${cell.box.progress}%` : '';
            excelCell.value = `${cell.box.code || ''}${progressText}`;
            excelCell.font = { name: 'Calibri', size: 9, bold: true, color: { argb: 'FF1E293B' } };
            excelCell.alignment = { horizontal: 'center', vertical: 'middle', wrapText: true };
            excelCell.fill = { type: 'pattern', pattern: 'solid', fgColor: { argb: 'FF' + bgColor } };
            excelCell.border = gridBorder;
            return;
        }

        if (!cell.hidden) {
            excelCell.value = '';
            excelCell.fill = { type: 'pattern', pattern: 'solid', fgColor: { argb: 'FFF8FAFC' } };
        }
        excelCell.border = gridBorder;
    }


    /**
     * Get exact RGB color codes from UI Business Flow Indicators
     * Matching the reference screenshots exactly
     */
    private getColorForStage(colorClass: string): string {
        const colorMap: { [key: string]: string } = {
            'wir-stage-1': '5DD4CB',      // Assembly Clearance - Cyan (from UI)
            'wir-stage-2': '6EE7B7',      // Mechanical Clearance - Green (from UI)
            'wir-stage-3': 'C17FFC',      // Stage 2 - Purple (from UI)
            'wir-stage-4': 'B3E855',      // Electrical Clearance - Lime Green (from UI)
            'wir-stage-5': 'FDE047',      // 2nd Fix Installation - Yellow (WIR-4)
            'wir-stage-6': 'FFA726',      // 3rd Fix Installation - Orange (WIR-5, from UI)
            'wir-stage-7': '7B9FF5',      // Readiness for Dispatch - Blue (from UI)
            'wir-stage-8': '818CF8',      // Stage 8 - Indigo
            'wir-stage-9': 'F48FB1',      // Completed - Pink (from UI)
            'wir-stage-10': 'F472B6',     // Stage 10 - Hot Pink
            'wir-stage-11': 'FB7185',     // Stage 11 - Rose
            'wir-stage-12': '86EFAC',     // Stage 12 - Mint
            'wir-completed': 'F48FB1',    // Completed - Pink (from UI)
            'empty': 'FFFFFF',            // Empty - Pure White
            'in-progress': 'BFDBFE',      // In Progress - Light Blue
            'completed': 'F48FB1'         // Completed - Pink
        };

        return colorMap[colorClass] || 'FFFFFF';
    }

    /**
     * Convert column number to Excel column letter (A, B, C, ..., AA, AB, ...)
     */
    private columnToLetter(col: number): string {
        let temp;
        let letter = '';
        while (col > 0) {
            temp = (col - 1) % 26;
            letter = String.fromCharCode(temp + 65) + letter;
            col = (col - temp - 1) / 26;
        }
        return letter;
    }

    /**
     * Export matrix layout to Excel (fallback for non-sectioned layouts)
     */
    private exportMatrixLayoutToExcel(): void {
        const sectionsToPrint = this.getSectionsForExport();
        const workbook = XLSX.utils.book_new();

        sectionsToPrint.forEach((sectionData, index) => {
            const section = sectionData.section;
            const grid = sectionData.grid;

            const data: any[] = [];

            // Add header row
            const headerRow: any = { 'Bay/Row': 'Bay/Row' };
            grid.columns.forEach((col: string) => {
                headerRow[col] = col;
            });
            data.push(headerRow);

            // Add data rows
            grid.matrix.forEach((rowCells: any[]) => {
                const row: any = { 'Bay/Row': rowCells[0].bay };
                rowCells.forEach((cell: any) => {
                    if (!cell.hidden) {
                        if (cell.box) {
                            row[cell.row] = `${cell.box.code}\n${cell.box.progress}%`;
                        } else {
                            row[cell.row] = '';
                        }
                    }
                });
                data.push(row);
            });

            const worksheet = XLSX.utils.json_to_sheet(data, { skipHeader: true });

            // Set column widths
            const columnWidths = [{ wch: 10 }];
            grid.columns.forEach(() => {
                columnWidths.push({ wch: 12 });
            });
            worksheet['!cols'] = columnWidths;

            // Set row heights
            worksheet['!rows'] = data.map(() => ({ hpt: 30 }));

            const sheetName = section?.sectionName || `Section ${index + 1}`;
            XLSX.utils.book_append_sheet(workbook, worksheet, sheetName.substring(0, 31));
        });

        const timestamp = new Date().toISOString().split('T')[0];
        const filename = `Factory_Layout_${this.factory?.factoryCode || 'Export'}_${timestamp}.xlsx`;

        XLSX.writeFile(workbook, filename);

        document.dispatchEvent(new CustomEvent('app-toast', {
            detail: {
                message: 'Factory layout exported successfully!',
                type: 'success'
            }
        }));
    }

    /**
     * Get sections for export/print based on user selection
     * Returns all sections if none are selected, or only selected sections
     */
    private getSectionsForExport(): Array<{ section: FactorySection | null; grid: any }> {
        const allSections = this.getSectionGridLayouts();

        // If no specific sections are selected, return all sections
        if (this.selectedSectionsForExport.length === 0) {
            return allSections;
        }

        // Return only selected sections
        return allSections.filter(sectionData => {
            if (sectionData.section && sectionData.section.sectionId) {
                return this.selectedSectionsForExport.includes(sectionData.section.sectionId);
            }
            return false;
        });
    }
}