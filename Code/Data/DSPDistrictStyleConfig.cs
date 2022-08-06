using System.Collections.Generic;
using HarmonyLib;

namespace DistrictStylesPlus.Code.Data
{
    internal static class DSPDistrictStyleConfig
    {

        private const int MaxDistrictCount = DistrictManager.MAX_DISTRICT_COUNT;

        private static Dictionary<int, HashSet<ushort>> _districtStyleMap = new Dictionary<int, HashSet<ushort>>();

        internal static void addStyleToDistrict(int districtId, ushort styleId)
        {
            var districtStyles = _districtStyleMap.GetValueSafe(districtId);
            var styleAdded = districtStyles.Add(styleId);

            if (styleAdded)
            {
                _districtStyleMap[districtId] = districtStyles;
            }
        }

        internal static void removeStyleFromDistrict(int districtId, ushort styleId)
        {
            var districtStyles = _districtStyleMap.GetValueSafe(districtId);

            // no styles used in district => nothing to remove
            if (districtStyles.Count <= 0) return;
            
            var styleRemoved = districtStyles.Remove(styleId);
            if (styleRemoved)
            {
                _districtStyleMap[districtId] = districtStyles;
            }
        }
    }
}