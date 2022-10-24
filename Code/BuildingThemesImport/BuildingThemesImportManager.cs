using System;
using System.IO;
using System.Text;
using ColossalFramework;
using ColossalFramework.Packaging;
using ColossalFramework.UI;
using DistrictStylesPlus.Code.Managers;
using DistrictStylesPlus.Code.Utils;

namespace DistrictStylesPlus.Code.BuildingThemesImport
{
    public static class BuildingThemesImportManager
    {
        
        internal static void ImportBuildingThemes(string btXmlFilePath)
        {
            Logging.InfoLog($"Get Building Themes configuration from {btXmlFilePath}");
            
            var btConfiguration = GetBuildingThemesConfiguration(btXmlFilePath, out var readConfigMessage);
            
            Logging.InfoLog("Building Themes configuration loaded. Let's try to import it.");

            if (btConfiguration == null)
            {
                ShowResults(readConfigMessage, false);
                return;
            }

            var resultMessage = new StringBuilder();

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
                    resultMessage.AppendLine("Theme imported: " + theme.name);
                }
                ShowResults(resultMessage.ToString(), true);
            }
            else
            {
                Logging.InfoLog("Building Themes configuration was empty - nothing to import.");
                ShowResults("No themes found in BuildingThemes.xml file.", false);
            }
        }

        private static void ShowResults(string message, bool success)
        {
            var panel = UIView.library.ShowModal<ExceptionPanel>("ExceptionPanel");
            var title = success ? "Import was successful" : "Import failed";
            panel.SetMessage(title, message, !success);
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

        private static BTConfiguration GetBuildingThemesConfiguration(string btXmlFilePath, out string message)
        {
            try
            {
                if (btXmlFilePath.IsNullOrWhiteSpace())
                    throw new ArgumentException("Path to BuildingThemes.xml file is empty!");
                
                if (!File.Exists(btXmlFilePath))
                    throw new ArgumentException($"Configuration file {btXmlFilePath} does not exist!");

                var btConfiguration = BTConfiguration.Deserialize(btXmlFilePath);

                if (btConfiguration == null) throw new Exception("Configuration is empty or malformed!");

                message = "BuildingThemes.xml file loaded successfully.";
                return btConfiguration;
            }
            catch (Exception e)
            {
                Logging.LogException(e, "Exception");
                message = e.Message;
                return null;
            }
        }

    }
}