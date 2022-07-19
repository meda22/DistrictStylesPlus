using System.Linq;
using ColossalFramework;

namespace DistrictStylesPlus.Code.Utils
{
    public static class DSPDistrictStylesUtils
    {

        /// <summary>
        /// Get list of short names (without package name) of Styles.
        /// </summary>
        /// <returns>Array of district style names</returns>
        internal static string[] getDistrictStyleShortNames()
        {
            return Singleton<DistrictManager>.instance.m_Styles.Select(style => style.Name).ToArray();
        }
        
    }
}