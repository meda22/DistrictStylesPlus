using System;
using System.IO;
using System.Xml.Serialization;
using DistrictStylesPlus.Code.GUI;
using DistrictStylesPlus.Code.Utils;

namespace DistrictStylesPlus.Code
{
    /// <summary>
    /// Holds global mod settings.
    /// </summary>
    public static class ModSettings
    {
        internal static bool enableDebugLog { get; set; } = false;
        internal static bool checkServiceLevel { get; set; } = true;
        internal static float dsEditorButtonPosX { get; set; } = 0;
        internal static float dsEditorButtonPoxY { get; set; } = 0;

        private static bool _showDistrictStylesEditorButton = true;
        internal static bool showDistrictStylesEditorButton
        {
            get => _showDistrictStylesEditorButton;

            set
            {
                _showDistrictStylesEditorButton = value;

                // mod is not loaded, do nothing
                if (!Loading.IsModLoaded) return;
                
                if (value)
                {
                    DistrictStylesEditorButton.CreateDistrictStylesEditorButton();
                }
                else
                {
                    DistrictStylesEditorButton.DestroyDistrictStylesEditorButton();
                }
            }
        }
    }
    

    /// <summary>
    /// Mapping class xml file for mod configuration.
    ///
    /// TODO: set proper default values?
    /// </summary>
    [XmlRoot("SettingsFile")]
    public class XMLSettingsFile
    {
        
        [XmlElement("enableDebugLog")]
        public bool enableDebugLog
        {
            get => ModSettings.enableDebugLog;
            set => ModSettings.enableDebugLog = value;
        }
        
        [XmlElement("checkServiceLevel")]
        public bool checkServiceLevel
        {
            get => ModSettings.checkServiceLevel;
            set => ModSettings.checkServiceLevel = value;
        }

        [XmlElement("dsEditorButtonPosX")]
        public float dsEditorButtonPosX
        {
            get => ModSettings.dsEditorButtonPosX;
            set => ModSettings.dsEditorButtonPosX = value;
        }

        [XmlElement("dsEditorButtonPoxY")]
        public float dsEditorButtonPoxY
        {
            get => ModSettings.dsEditorButtonPoxY;
            set => ModSettings.dsEditorButtonPoxY = value;
        }
        
        [XmlElement("showDistrictStylesEditorButton")]
        public bool showDistrictStylesEditorButton
        {
            get => ModSettings.showDistrictStylesEditorButton;
            set => ModSettings.showDistrictStylesEditorButton = value;
        }

    }
    
    ///<summary>
    /// XML serialization/deserialization utilities class.
    /// </summary>
    internal static class SettingsUtils
    {
        internal static readonly string SettingsFileName = "DistrictStylesPlus.xml";


        /// <summary>
        /// Load settings from XML file.
        /// </summary>
        internal static void LoadSettings()
        {
            try
            {
                // Check to see if configuration file exists.
                if (File.Exists(SettingsFileName))
                {
                    // Read it.
                    using (StreamReader reader = new StreamReader(SettingsFileName))
                    {
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(XMLSettingsFile));
                        if (!(xmlSerializer.Deserialize(reader) is XMLSettingsFile xmlSettingsFile))
                        {
                            Logging.ErrorLog("couldn't deserialize settings file");
                        }
                    }
                }
                else
                {
                    Logging.InfoLog("no settings file found");
                }
            }
            catch (Exception e)
            {
                Logging.LogException(e, "exception reading XML settings file");
            }
        }


        /// <summary>
        /// Save settings to XML file.
        /// </summary>
        internal static void SaveSettings()
        {
            try
            {
                // Pretty straightforward.  Serialisation is within GBRSettingsFile class.
                using (StreamWriter writer = new StreamWriter(SettingsFileName))
                {
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(XMLSettingsFile));
                    xmlSerializer.Serialize(writer, new XMLSettingsFile());
                }
            }
            catch (Exception e)
            {
                Logging.LogException(e, "exception saving XML settings file");
            }
        }
    }
}