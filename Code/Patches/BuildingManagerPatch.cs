using System.Collections.Generic;
using System.Linq;
using ColossalFramework;
using DistrictStylesPlus.Code.Utils;
using HarmonyLib;

namespace DistrictStylesPlus.Code.Patches
{
    [HarmonyPatch]
    public class BuildingManagerPatch
    {

        [HarmonyPostfix]
        [HarmonyPatch(typeof(BuildingManager), "GetRandomBuildingInfo")]
        public static void GetRandomBuildingPostfix(
            ref BuildingInfo __result,
            ItemClass.Service service,
            ItemClass.SubService subService,
            int width,
            int length,
            BuildingInfo.ZoningMode zoningMode,
            int style)
        {
            if (ModSettings.checkServiceLevel) return; // we are using vanilla way - do nothing
            
            Logging.DebugLog($"Search for {service} - {subService} - {width} - {length} - {zoningMode}");
            
            if (__result == null) return; // result is null - do nothing
            
            if (style <= 0) return; // no specific style chosen - do nothing

            var districtStyle = Singleton<DistrictManager>.instance.m_Styles[style];
            
            Logging.DebugLog($"Chosen style: {districtStyle.Name} and chosen building: {__result.name}");
            
            var affectedServices = (HashSet<int>) AccessTools
                .Field(districtStyle.GetType(), "m_AffectedServices")
                .GetValue(districtStyle);

            var indexRangeStart = DistrictStyle.GetServiceLevelIndex(service, subService, ItemClass.Level.Level1);
            var indexRangeEnd = DistrictStyle.GetServiceLevelIndex(service, subService, ItemClass.Level.Level5);

            var affectsZoneType = affectedServices.Any(index => index >= indexRangeStart && index <= indexRangeEnd);
            
            // if district affects zone type but does not contains given building, do not allow to spawn it.
            if (affectsZoneType && !districtStyle.Contains(__result))
            {
                Logging.DebugLog($"FORBID!!! Chosen style: {districtStyle.Name} and chosen building: {__result.name}");
                __result = null;
                return;
            }
            
            Logging.DebugLog($"ALLOW!!! Chosen style: {districtStyle.Name} and chosen building: {__result.name}");
        }
        
    }
}