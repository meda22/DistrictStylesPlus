using System;
using System.Collections.Generic;
using System.Linq;
using ColossalFramework.UI;
using DistrictStylesPlus.Code.Managers;
using DistrictStylesPlus.Code.Utils;
using UnityEngine;

namespace DistrictStylesPlus.Code.GUI.DistrictStylePicker
{

    /// <summary>
    /// Static manager of District Style Picker Panel. Based on Algernon's code in Realistic Population mod.
    /// </summary>
    public static class DistrictStylePickerPanelManager
    {

        private static GameObject _uiGameObject;
        private static DistrictStylePickerPanel _panel;
        public static DistrictStylePickerPanel panel => _panel;

        internal static void Open(byte districtId)
        {
            try
            {
                if (_uiGameObject == null)
                {
                    _uiGameObject = new GameObject("DSPDistrictStylePickerPanel");
                    _uiGameObject.transform.parent = UIView.GetAView().transform;

                    _panel = _uiGameObject.AddComponent<DistrictStylePickerPanel>();

                    panel.Setup();
                }

                panel.SetDistrictData(districtId);
                panel.Show(true);
            }
            catch (Exception e)
            {
                Logging.LogException(e, "Cannot open DSPDistrictStylePickerPanel");
            }
        }

        internal static void Close()
        {
            GameObject.Destroy(_panel);
            GameObject.Destroy(_uiGameObject);
            _panel = null;
            _uiGameObject = null;
        }

    }
    
    /// <summary>
    /// Class for UI of District Style Picker Panel.
    /// </summary>
    public class DistrictStylePickerPanel : UIPanel
    {

        private const float PanelWidth = 400;
        private const float PanelHeight = 500;
        private const float Spacing = 5;
        private const float TitleHeight = 40;
        
        private DSPickerTitleBar _pickerTitleBar;
        private DSPickerStyleListPanel _styleListPanel;
        private UIButton _saveButton;

        private byte _selectedDistrictId;
        private HashSet<string> _selectedStyles = new HashSet<string>();

        internal void SetDistrictData(byte districtId)
        {
            _selectedDistrictId = districtId;
            _pickerTitleBar.ChangeTitle($"District Style Picker (districtId: {_selectedDistrictId})");
            _selectedStyles = DSPTransientStyleManager.GetSelectedStylesForDistrict(_selectedDistrictId);
            RefreshStyleSelectData();
            Logging.DebugLog($"Show DS picker for district id {_selectedDistrictId}");
        }

        internal void Setup()
        {
            backgroundSprite = "UnlockingPanel2";
            isVisible = false;
            canFocus = true;
            isInteractive = true;

            width = Spacing + PanelWidth + Spacing;
            height = TitleHeight + PanelHeight + Spacing;
            
            relativePosition = new Vector3(
                Mathf.Floor((UIView.GetAView().fixedWidth - width) / 2),
                Mathf.Floor((UIView.GetAView().fixedHeight - height) / 2));
            
            SetupTitleBar();
            SetupStyleListPanel();
            SetupSaveButton();
        }

        private void SetupTitleBar()
        {
            _pickerTitleBar = AddUIComponent<DSPickerTitleBar>();
            _pickerTitleBar.Setup("District Style Picker");
        }

        private void SetupStyleListPanel()
        {
            _styleListPanel = AddUIComponent<DSPickerStyleListPanel>();
            _styleListPanel.width = PanelWidth;
            _styleListPanel.height = PanelHeight - 50;
            _styleListPanel.relativePosition = new Vector3(Spacing, TitleHeight + Spacing);
            _styleListPanel.Setup();
        }

        private void SetupSaveButton()
        {
            _saveButton = UIUtils.CreateButton(this);
            _saveButton.text = "Save";
            _saveButton.relativePosition =
                new Vector3(Spacing, TitleHeight + Spacing + PanelHeight - 50 + Spacing * 2);

            _saveButton.eventClick += (component, param) =>
            {
                Logging.DebugLog($"save button clicked {_selectedDistrictId} " +
                                 $"and styles {string.Join(", ", _selectedStyles.ToArray())}");
                DSPTransientStyleManager.SetSelectedStylesForDistrict(_selectedDistrictId, _selectedStyles);

                DistrictStylePickerPanelManager.Close();
            };
            
        }

        internal HashSet<string> GetSelectedStyles()
        {
            return _selectedStyles;
        }

        internal void AddStyleToSelected(DistrictStyle districtStyle)
        {
            _selectedStyles.Add(districtStyle.FullName);
        }

        internal void RemoveStyleFromSelected(DistrictStyle districtStyle)
        {
            _selectedStyles.Remove(districtStyle.FullName);
        }

        internal void RefreshPickerStyleSelect()
        {
            if (_styleListPanel != null) _styleListPanel.RefreshPickerStyleSelect();
        }

        private void RefreshStyleSelectData()
        {
            if (_styleListPanel == null) return;
            
            _styleListPanel.RefreshStoredDistrictStyles();
            _styleListPanel.RefreshPickerStyleSelect();
        }
    }
}