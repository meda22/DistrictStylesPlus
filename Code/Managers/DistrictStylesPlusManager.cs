using ColossalFramework;

namespace DistrictStylesPlus.Code.Managers
{
    public class DistrictStylesPlusManager : Singleton<DistrictStylesPlusManager>
    {
        /// <summary>
        /// Initial setup of BuildingThemeManager
        /// </summary>
        public void SetupDistrictStylesPlusManager()
        {
            DSPDistrictStylePackageManager.AddEmptyEnabledStylesToGame();
            DSPDistrictStylePackageManager.LoadVanillaBuildingsToStyles();
            SimulationManager.instance.AddAction(() => DSPBuildingManager.instance.RefreshStylesInBuildingManager());
        }
        
        internal static bool IsPrefabGrowable(BuildingInfo buildingInfo)
        {
            var prefabAI = buildingInfo.GetAI();
            return prefabAI != null 
                   && !prefabAI.ToString().Contains("PloppableRICO.Ploppable")
                   && (prefabAI is CommercialBuildingAI || prefabAI is ResidentialBuildingAI ||
                       prefabAI is IndustrialBuildingAI || prefabAI is IndustrialExtractorAI ||
                       prefabAI is LivestockExtractorAI || prefabAI is OfficeBuildingAI);
        }
    }
}