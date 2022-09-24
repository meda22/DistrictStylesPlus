using DistrictStylesPlus.Code.Managers;
using HarmonyLib;

namespace DistrictStylesPlus.Code.Patches
{
    
    [HarmonyPatch]
    public static class DistrictManagerPatch
    {
        
        /// <summary>
        /// Called when district is deleted. If so, check and remove transient style for given district.
        /// </summary>
        /// <param name="district"></param>
        [HarmonyPostfix]
        [HarmonyPatch(typeof(DistrictManager), "ReleaseDistrictImplementation")]
        public static void ReleaseDistrictImplementationPostfix(byte district)
        {
            if (district == 0) return; // not a district, do nothing
            
            var transientConfigs = DSPTransientStyleManager.GetTransientDistrictStyleConfigs();
            var transientExisted = transientConfigs.Remove(district);
            
            if (!transientExisted) return; // no transient style config, nothing to delete
            
            DSPTransientStyleManager.GetTransientStyleNames
                (district, out var transientStyleName, out var transientStyleFullName);
            var style = DSPDistrictStyleManager.GetDistrictStyleByFullName(transientStyleFullName);
            DSPDistrictStyleManager.DeleteDistrictStyle(style, true);
        }
        
    }
}