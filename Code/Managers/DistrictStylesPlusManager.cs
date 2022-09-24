using System.Collections.Generic;
using ColossalFramework;

namespace DistrictStylesPlus.Code.Managers
{
    public class DistrictStylesPlusManager : Singleton<DistrictStylesPlusManager>
    {

        /// <summary>
        /// List of all growable assets in game
        /// </summary>
        internal readonly List<BuildingInfo> BuildingInfoList = new List<BuildingInfo>();

        public static readonly HashSet<ushort> KeepSameAppearanceBuildingIds = new HashSet<ushort>();
        
        /// <summary>
        /// Initial setup of BuildingThemeManager
        /// </summary>
        public void SetupDistrictStylesPlusManager()
        {
            SetupBuildingInfoList();
            DSPDistrictStylePackageManager.AddEmptyEnabledStylesToGame();
            DSPDistrictStylePackageManager.LoadVanillaBuildingsToStyles();
            DSPBuildingManager.instance.RefreshStylesInBuildingManager();
        }
        
        private void SetupBuildingInfoList()
        {
            for (uint i = 0; i < PrefabCollection<BuildingInfo>.PrefabCount(); i++)
            {
                var buildingInfo = PrefabCollection<BuildingInfo>.GetPrefab(i);

                if (buildingInfo == null) continue;

                var privateServiceIndex = ItemClass.GetPrivateServiceIndex(buildingInfo.GetService());

                // if building is not growable asset do nothing
                if (privateServiceIndex == -1) continue;

                BuildingInfoList.Add(buildingInfo);
            }
        }
    }
}