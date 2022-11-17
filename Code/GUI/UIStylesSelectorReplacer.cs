using ColossalFramework;
using ColossalFramework.UI;
using DistrictStylesPlus.Code.GUI.DistrictStylePicker;
using DistrictStylesPlus.Code.Utils;
using HarmonyLib;
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
            
            var stylePickerButton = UIUtils.CreateButton(parent);
            stylePickerButton.name = "cityStylesPicker";
            stylePickerButton.text = "STYLES";
            stylePickerButton.relativePosition = new Vector3(300, 0, 0);
            stylePickerButton.eventClick += (component, clickEvent) =>
            {
                if (!clickEvent.used)
                {
                    DistrictStylePickerPanelManager.Open(0);
                }
            };
        }

        public static void AddStylePickerToDistrictPanel()
        {
            var districtInfoPanel = GameObject.Find("(Library) DistrictWorldInfoPanel").GetComponent<DistrictWorldInfoPanel>();
            var policiesButton = districtInfoPanel.Find("PoliciesButton").GetComponent<UIButton>();
            var parent = policiesButton.parent;
            
            var stylePickerButton = UIUtils.CreateButton(parent);
            stylePickerButton.name = "districtStylesPicker";
            stylePickerButton.text = "STYLES";
            stylePickerButton.relativePosition = new Vector3(220, 0, 0);
            stylePickerButton.eventClick += (component, clickEvent) =>
            {
                if (clickEvent.used) return;
                var currentInstanceID = WorldInfoPanel.GetCurrentInstanceID();
                DistrictStylePickerPanelManager.Open(currentInstanceID.District);
            };
            
            var uiDropDown = UIView.Find<UIDropDown>("StyleDropdown");
            uiDropDown.Hide();
        }

    }
}