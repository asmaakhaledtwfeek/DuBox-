namespace Dubox.Domain.Enums
{
    public enum PanelStageEnum
    {
        NotStarted = 0,
        MoldPreparation = 1,                  // 1️⃣ Mold Preparation
        Initial = 2,                          // 2️⃣ Initial
        MEPInsertsInstallation = 3,           // 3️⃣ MEP Inserts / Embedded Items Installation
        ReinforcementSetup = 4,               // 4️⃣ Reinforcement Setup
        ConcreteCasting = 5,                  // 5️⃣ Concrete Casting
        SurfaceFinishing = 6,                 // 6️⃣ Surface Finishing
        CuringAndDemolding = 7                // 7️⃣ Curing & Demolding (Final Stage)
    }
}

