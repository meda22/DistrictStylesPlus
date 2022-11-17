using ColossalFramework.UI;
using DistrictStylesPlus.Code.Utils;
using UnityEngine;

namespace DistrictStylesPlus.Code.GUI.DistrictStylePicker
{
    public class DSPickerStyleItem : UIPanel, UIFastListRow
    {

        private UICheckBox _name;
        private UIPanel _background;
        private DistrictStyle _districtStyle;
        
        public void Display(object data, bool isRowOdd)
        {

            _districtStyle = data as DistrictStyle;
            SetupControls();

            if (_districtStyle == null) return;
            
            _name.text = _districtStyle.Name; 
            _name.label.textColor = new Color32(255, 255, 255, 255);
            _name.label.isInteractive = false;
            _name.isChecked = IsSelectedStyle(_districtStyle.FullName);
        }

        public void Select(bool isRowOdd)
        {
            // do nothing
        }

        public void Deselect(bool isRowOdd)
        {
            // do nothing
        }

        private bool IsSelectedStyle(string fullName)
        {
            var selectedStyleFullNames = DistrictStylePickerPanelManager.panel.GetSelectedStyles();
            return selectedStyleFullNames.Contains(fullName);
        }

        private void SetupControls()
        {
            if (_name != null) return;

            isVisible = true;
            canFocus = true;
            isInteractive = true;
            width = parent.width;
            height = 40;
            
            SetupBackgroundPanel();
            SetupNameCheckbox();
        }

        private void SetupNameCheckbox()
        {
            _name = UIUtils.CreateCheckBox(this);
            _name.width = 40;
            _name.clipChildren = false;
            _name.relativePosition = new Vector3(5, 13);

            _name.eventCheckChanged += (component, state) =>
            {
                if (state)
                {
                    Logging.DebugLog($"Select DS {_districtStyle.FullName}");
                    DistrictStylePickerPanelManager.panel.AddStyleToSelected(_districtStyle);
                }
                else
                {
                    Logging.DebugLog($"Remove DS {_districtStyle.FullName}");
                    DistrictStylePickerPanelManager.panel.RemoveStyleFromSelected(_districtStyle);
                }
                
                DistrictStylePickerPanelManager.panel.RefreshPickerStyleSelect();
            };
        }
        
        private void SetupBackgroundPanel()
        {
            if (_background != null) return;
            
            _background = AddUIComponent<UIPanel>();
            _background.width = width;
            _background.height = 40;
            _background.relativePosition = Vector2.zero;
            _background.zOrder = 0;
        }
    }
}