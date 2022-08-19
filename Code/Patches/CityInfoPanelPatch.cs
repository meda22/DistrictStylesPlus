using ColossalFramework;
using DistrictStylesPlus.Code.GUI;
using DistrictStylesPlus.Code.Utils;
using HarmonyLib;

namespace DistrictStylesPlus.Code.Patches
{
    [HarmonyPatch]
    public static class CityInfoPanelPatch
    {
        
        [HarmonyPrefix]
        [HarmonyPatch(typeof(CityInfoPanel), "OnVisibilityChanged")]
        public static void OnVisibilityChangedPrefix(bool visible)
        {
            Logging.DebugLog("Toggle City Panel!");

            if (!visible) return;
            
            if (Singleton<SimulationManager>.exists && Singleton<SimulationManager>.instance.m_metaData != null)
            {
                Logging.DebugLog("Populate City Styles...");
                UIStylesSelectorReplacer.PopulateCityStyleSelector();
            }
        }
        
    }
}