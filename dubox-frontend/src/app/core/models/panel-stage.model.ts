export enum PanelStage {
  NotStarted = 0,
  MoldPreparation = 1,         // 1️⃣ Mold Preparation
  ReinforcementSetup = 2,       // 2️⃣ Reinforcement Setup
  ConcreteCasting = 3,          // 3️⃣ Concrete Casting
  CuringAndDemolding = 4        // 4️⃣ Curing & Demolding
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
    stage: PanelStage.ReinforcementSetup,
    emoji: '2️⃣',
    label: 'Reinforcement Setup',
    description: 'Install steel reinforcement and fixtures'
  },
  {
    stage: PanelStage.ConcreteCasting,
    emoji: '3️⃣',
    label: 'Concrete Casting',
    description: 'Pour and compact concrete'
  },
  {
    stage: PanelStage.CuringAndDemolding,
    emoji: '4️⃣',
    label: 'Curing & Demolding',
    description: 'Cure concrete and remove from mold'
  }
];

