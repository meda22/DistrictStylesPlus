using System;
using ColossalFramework;
using ColossalFramework.Packaging;
using DistrictStylesPlus.Code.Managers;
using DistrictStylesPlus.Code.Utils;

namespace DistrictStylesPlus.Code.BuildingThemesImport
{
    public static class BuildingThemesImportManager
    {
        
        internal static void ImportBuildingThemes(string btXmlFilePath)
        {
            
            Logging.InfoLog($"Get Building Themes configuration from {btXmlFilePath}");

            var btConfiguration = GetBuildingThemesConfiguration(btXmlFilePath);
            
            Logging.InfoLog("Building Themes configuration loaded. Let's try to import it.");

            if (btConfiguration.themes?.Count > 0)
            {
                foreach (var theme in btConfiguration.themes)
                {
                    var districtStyleMetaData = GetDistrictStyleMetaData(theme.name);
                    if (theme.buildings?.Count <= 0) continue; // nothing to do because there are no buildings in theme
                    var dsAssetList = new string[theme.buildings.Count];
                    for (int i = 0; i < theme.buildings.Count; i++)
                    {
                        var assetName = DSPDistrictStylePackageManager.GetPackageAssetName(theme.buildings[i].name);
                        dsAssetList[i] = assetName;
                    }

                    districtStyleMetaData.assets = dsAssetList;
                    
                    PackageManager.DisableEvents();
                    
                    StylesHelper.SaveStyle(districtStyleMetaData, districtStyleMetaData.name, true);
                    
                    PackageManager.EnabledEvents();
                    PackageManager.ForcePackagesChanged();
                }
            }
            else
            {
                Logging.InfoLog("Building Themes configuration was empty - nothing to import.");
            }

        }

        private static DistrictStyleMetaData GetDistrictStyleMetaData(string themeName)
        {
            return new DistrictStyleMetaData
            {
                assets = new string[0],
                builtin = false,
                name = themeName,
                timeStamp = DateTime.Now,
                steamTags = new string[1] {"District Style"}
            };
        }

        private static BTConfiguration GetBuildingThemesConfiguration(string btXmlFilePath)
        {
            try
            {
                if (btXmlFilePath.IsNullOrWhiteSpace())
                    throw new ArgumentException("Path to Building Themes xml is empty!");

                var btConfiguration = BTConfiguration.Deserialize(btXmlFilePath);

                if (btConfiguration == null) throw new Exception("Configuration is empty or malformed!");

                return btConfiguration;
            }
            catch (Exception e)
            {
                Logging.LogException(e, "Exception"); //TODO: better error handling - show message to player
                throw e;
            }
        }

    }
}