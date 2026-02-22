export enum PanelStage {
  NotStarted = 0,
  MoldPreparation = 1,                  // 1️⃣ Mold Preparation
  Initial = 2,                          // 2️⃣ Initial
  MEPInsertsInstallation = 3,           // 3️⃣ MEP Inserts / Embedded Items Installation
  ReinforcementSetup = 4,               // 4️⃣ Reinforcement Setup
  ConcreteCasting = 5,                  // 5️⃣ Concrete Casting
  SurfaceFinishing = 6,                 // 6️⃣ Surface Finishing
  CuringAndDemolding = 7                // 7️⃣ Curing & Demolding
}

export interface PanelStageInfo {
  stage: PanelStage;
  emoji: string;
  label: string;
  description: string;
}

export const PANEL_STAGES: PanelStageInfo[] = [
  {
    stage: PanelStage.MoldPreparation,
    emoji: '1️⃣',
    label: 'Mold Preparation',
    description: 'Prepare and clean molds for casting'
  },
  {
    stage: PanelStage.Initial,
    emoji: '2️⃣',
    label: 'Initial',
    description: 'Initial stage - panel setup and preparation'
  },
  {
    stage: PanelStage.MEPInsertsInstallation,
    emoji: '3️⃣',
    label: 'MEP Inserts / Embedded Items Installation',
    description: 'Install MEP inserts and embedded items'
  },
  {
    stage: PanelStage.ReinforcementSetup,
    emoji: '4️⃣',
    label: 'Reinforcement Setup',
    description: 'Install steel reinforcement and fixtures'
  },
  {
    stage: PanelStage.ConcreteCasting,
    emoji: '5️⃣',
    label: 'Concrete Casting',
    description: 'Pour and compact concrete'
  },
  {
    stage: PanelStage.SurfaceFinishing,
    emoji: '6️⃣',
    label: 'Surface Finishing',
    description: 'Apply surface finishing and treatment'
  },
  {
    stage: PanelStage.CuringAndDemolding,
    emoji: '7️⃣',
    label: 'Curing & Demolding',
    description: 'Cure concrete and remove from mold'
  }
];

