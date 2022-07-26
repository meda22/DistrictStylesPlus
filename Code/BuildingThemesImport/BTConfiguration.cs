﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;
using System.IO;
using DistrictStylesPlus.Code.Utils;

namespace DistrictStylesPlus.Code.BuildingThemesImport
{
    [XmlRoot("Configuration")]
    public class BTConfiguration
    {
        public int version = 0;

        public bool UnlockPolicyPanel = true;

        public bool CreateBuildingDuplicates = false;

        [DefaultValue(true)]
        public bool ThemeValidityWarning = true;

        [XmlArray(ElementName = "Themes")]
        [XmlArrayItem(ElementName = "Theme")]
        public List<Theme> themes = new List<Theme>();

        public Theme getTheme(string name)
        {
            foreach (Theme theme in themes)
            {
                if (theme.name == name) return theme;
            }
            return null;
        }

        public class Theme
        {
            [XmlAttribute("name")]
            public string name;

            [XmlIgnoreAttribute]
            public bool isBuiltIn = false;

            [XmlAttribute("style-package"), DefaultValue(null)]
            public string stylePackage = null;

            [XmlArray(ElementName = "Buildings")]
            [XmlArrayItem(ElementName = "Building")]
            public List<Building> buildings = new List<Building>();

            public bool containsBuilding(string name)
            {
                foreach (Building building in buildings)
                {
                    if (building.name == name) return true;
                }
                return false;
            }

            public IEnumerable<Building> getVariations(string baseName)
            {
                return from building in buildings where building.baseName == baseName select building;
            }

            public Building getBuilding(string name)
            {
                foreach (Building building in buildings)
                {
                    if (building.name == name) return building;
                }
                return null;
            }
        }

        public class Building
        {
            [XmlAttribute("name")]
            public string name;

            [XmlIgnoreAttribute]
            public Building builtInBuilding = null;

            [XmlIgnoreAttribute()]
            public bool fromStyle = false;

            [XmlAttribute("level"), DefaultValue(-1)]
            public int level = -1;

            [XmlAttribute("upgrade-name"), DefaultValue(null)]
            public string upgradeName = null;

            [XmlAttribute("base-name"), DefaultValue(null)]
            public string baseName = null;

            [XmlAttribute("spawn-rate"), DefaultValue(10)]
            public int spawnRate = 10;

            [XmlAttribute("include"), DefaultValue(true)]
            public bool include = true;

            [XmlAttribute("dlc"), DefaultValue(null)]
            public string dlc = null;

            [XmlAttribute("environments"), DefaultValue(null)]
            public string environments = null;

            public bool Equals(Building other)
            {
                if (other == null) { return false; }
                if (object.ReferenceEquals(this, other)) { return true; }
                return this.name == other.name
                    && this.level == other.level
                    && this.upgradeName == other.upgradeName
                    && this.baseName == other.baseName
                    && this.spawnRate == other.spawnRate
                    && this.include == other.include;
            }

            public Building(string name)
            {
                this.name = name;
            }

            public Building(Building builtInBuilding)
            {
                this.builtInBuilding = builtInBuilding;

                this.name = builtInBuilding.name;
                this.level = builtInBuilding.level;
                this.upgradeName = builtInBuilding.upgradeName;
                this.baseName = builtInBuilding.baseName;
                this.spawnRate = builtInBuilding.spawnRate;
                this.include = builtInBuilding.include;
                this.dlc = builtInBuilding.dlc;
                this.environments = builtInBuilding.environments;
            }

            public Building()
            {
            }
        }

        public static BTConfiguration Deserialize(string filename)
        {

            if (!File.Exists(filename))
            {
                Logging.ErrorLog($"Configuration file {filename} does not exist!");
                return null;
            }

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(BTConfiguration));
            try
            {
                using (System.IO.StreamReader streamReader = new System.IO.StreamReader(filename))
                {
                    return (BTConfiguration)xmlSerializer.Deserialize(streamReader);
                }
            }
            catch (Exception e)
            {
                Logging.LogException(e, "Couldn't load configuration (XML malformed?)");
                throw e;
            }
        }

        public static void Serialize(string filename, BTConfiguration config)
        {
            var xmlSerializer = new XmlSerializer(typeof(BTConfiguration));
            try
            {
                using (System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(filename))
                {
                    var configCopy = new BTConfiguration();

                    configCopy.version = config.version;
                    configCopy.UnlockPolicyPanel = config.UnlockPolicyPanel;
                    configCopy.CreateBuildingDuplicates = config.CreateBuildingDuplicates;
                    configCopy.ThemeValidityWarning = config.ThemeValidityWarning;

                    foreach (var theme in config.themes)
                    {
                        var newTheme = new Theme
                        {
                            name = theme.name,
                            stylePackage = theme.stylePackage
                        };
                        foreach (var building in theme.buildings.Where(building =>
                            // a user-added building has to be included, or we don't need it in the config
                            (building.builtInBuilding == null && building.include)

                                // a built-in building that was modified by the user: Only add it to the config if the modification differs
                            || (building.builtInBuilding != null && !building.Equals(building.builtInBuilding))))
                        {
                            newTheme.buildings.Add(building);
                        }
                        if (!theme.isBuiltIn || newTheme.buildings.Count > 0)
                        {
                            configCopy.themes.Add(newTheme);
                        }
                    }

                    xmlSerializer.Serialize(streamWriter, configCopy);
                }
            }
            catch (Exception e)
            {
                Logging.LogException(e, "Couldn't create configuration file at \"" + Directory.GetCurrentDirectory() + "\"");
                throw e;
            }
        }
    }
}