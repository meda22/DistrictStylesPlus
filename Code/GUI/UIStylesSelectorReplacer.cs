using ColossalFramework;
using ColossalFramework.UI;
using UnityEngine;

namespace DistrictStylesPlus.Code.GUI
{
    public static class UIStylesSelectorReplacer
    {

        private const string CityStyleSelectorName = "CityStyleSelector";
        
        /// <summary>
        /// Add a Style drop-down to be able to select style for city (basically default style)
        /// </summary>
        public static void AddStyleSelectorToCityPanel()
        {
            if (GameObject.Find(CityStyleSelectorName) != null) return;

            var cityInfoPanel = GameObject.Find("(Library) CityInfoPanel").GetComponent<CityInfoPanel>();
            var policiesButton = cityInfoPanel.Find("PoliciesButton").GetComponent<UIButton>();
            var parent = policiesButton.parent;

            // TODO: solve positioning 
            var cityStyleSelector = UIUtils.CreatePlainDropDown(parent, CityStyleSelectorName);
            cityStyleSelector.relativePosition = new Vector3(200, 0, 0);
            cityStyleSelector.listPosition = UIDropDown.PopupListPosition.Above;

            if (cityStyleSelector != null)
            {
                cityStyleSelector.eventSelectedIndexChanged += OnStyleChanged;
            }
        }

        /// <summary>
        /// Set a new style to city.
        /// </summary>
        private static void OnStyleChanged(UIComponent c, int styleId)
        {
            byte districtId = 0; // 0 means city
            if (Singleton<DistrictManager>.exists)
            {
                Singleton<DistrictManager>.instance.m_districts.m_buffer[districtId].m_Style = (ushort)styleId;
            }
        }

        /// <summary>
        /// Populate style selector with actual existing styles.
        /// Mostly copied from DistrictWorldInfoPanel.OnSetTarget.
        /// TODO: is it possible to reuse UIComponents?
        /// TODO: clean up this method
        /// </summary>
        internal static void PopulateCityStyleSelector()
        {
            if (GameObject.Find(CityStyleSelectorName) == null) return;

            var cityStyleSelector = GameObject.Find(CityStyleSelectorName).GetComponent<UIDropDown>();
            
            var isEuTheme = false;
            if (Singleton<SimulationManager>.exists && Singleton<SimulationManager>.instance.m_metaData != null)
            {
                isEuTheme = Singleton<SimulationManager>.instance.m_metaData.m_environment == "Europe";
            }
            if (Singleton<DistrictManager>.exists && Singleton<DistrictManager>.instance.m_Styles != null)
            {
                if (Singleton<DistrictManager>.instance.m_Styles.Length == 0)
                {
                    cityStyleSelector.items = new string[1];
                    cityStyleSelector.items[0] = ColossalFramework.Globalization.Locale.Get("STYLES_DEFAULT");
                    cityStyleSelector.isEnabled = false;
                    cityStyleSelector.tooltip = ColossalFramework.Globalization.Locale.Get("STYLES_MISSING_TOOLTIP");
                }
                else
                {
                    string[] array = new string[1 + Singleton<DistrictManager>.instance.m_Styles.Length];
                    array[0] = ColossalFramework.Globalization.Locale.Get("STYLES_DEFAULT");
                    for (ushort num = 0; num < Singleton<DistrictManager>.instance.m_Styles.Length; num = (ushort)(num + 1))
                    {
                        DistrictStyle districtStyle = Singleton<DistrictManager>.instance.m_Styles[num];
                        string text = districtStyle.Name;
                        if (districtStyle.BuiltIn)
                        {
                            if (districtStyle.Name.Equals(DistrictStyle.kEuropeanStyleName))
                            {
                                text = ColossalFramework.Globalization.Locale.Get((!isEuTheme) ? "STYLES_EUROPEAN" : "STYLES_NORMAL");
                            }
                            else if (districtStyle.Name.Equals(DistrictStyle.kEuropeanSuburbiaStyleName))
                            {
                                text = ColossalFramework.Globalization.Locale.Get("STYLES_EUROPEANSUBURBIA");
                            }
                            else if (districtStyle.Name.Equals(DistrictStyle.kModderPack5StyleName))
                            {
                                text = ColossalFramework.Globalization.Locale.Get("STYLES_MODDERPACKFIVE");
                            }
                        }
                        array[1 + num] = text;
                    }
                    cityStyleSelector.items = array;
                    cityStyleSelector.selectedIndex = Singleton<DistrictManager>.instance.m_districts.m_buffer[0].m_Style;
                }
            }
        }
    }
}