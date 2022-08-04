using System;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;
using DistrictStylesPlus.Code.Utils;

namespace DistrictStylesPlus.Code
{
    /// <summary>
    /// Holds global mod settings.
    /// </summary>
    public static class ModSettings
    {
        internal static bool enableDebugLog = false;
        internal static bool checkServiceLevel = true;
        internal static bool allowUpgradeSameAppearance = false;
    }
    

    /// <summary>
    /// Mapping class xml file for mod configuration.
    /// </summary>
    [XmlRoot("SettingsFile")]
    public class XMLSettingsFile
    {
        
        [XmlElement("enableDebugLog")]
        //[DefaultValue(false)]
        public bool enableDebugLog
        {
            get => ModSettings.enableDebugLog;
            set => ModSettings.enableDebugLog = value;
        }
        
        [XmlElement("checkServiceLevel")]
        //[DefaultValue(true)]
        public bool checkServiceLevel
        {
            get => ModSettings.checkServiceLevel;
            set => ModSettings.checkServiceLevel = value;
        }
        
        [XmlElement("allowUpgradeSameAppearance")]
        //[DefaultValue(false)]
        public bool allowUpgradeSameAppearance
        {
            get => ModSettings.allowUpgradeSameAppearance;
            set => ModSettings.allowUpgradeSameAppearance = value;
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