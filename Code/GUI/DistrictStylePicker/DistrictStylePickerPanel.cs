using System;
using System.Collections.Generic;
using System.Linq;
using ColossalFramework.UI;
using DistrictStylesPlus.Code.Managers;
using DistrictStylesPlus.Code.Utils;
using UnityEngine;

namespace DistrictStylesPlus.Code.GUI.DistrictStylePicker
{
    public class DistrictStylePickerPanel : UIPanel
    {

        private const float PanelWidth = 400;
        private const float PanelHeight = 500;
        private const float Spacing = 5;
        private const float TitleHeight = 40;

        private static GameObject _gameObject;

        public static DistrictStylePickerPanel instance { get; private set; }

        private UITitleBar _pickerTitleBar;
        private DSPickerStyleListPanel _styleListPanel;
        private UIButton _saveButton;

        private byte m_selectedDistrictId;
        private HashSet<string> m_selectedStyles = new HashSet<string>();

        internal static void Initialize()
        {
            // instance already exists -> do nothing
            if (instance != null) return;

            try
            {
                _gameObject = new GameObject("DSPDistrictStylePickerPanel");
                _gameObject.transform.parent = UIView.GetAView().transform;

                instance = _gameObject.AddComponent<DistrictStylePickerPanel>();
                instance.transform.parent = _gameObject.transform.parent;

                instance.DrawDistrictStylePickerPanel();
                _gameObject.SetActive(false);
            }
            catch (Exception e)
            {
                Logging.LogException(e, "Exception when initializing DistrictStylePickerPanel");
            }
        }

        public static void Destroy()
        {
            try
            {
                if (_gameObject != null)
                    GameObject.Destroy(_gameObject);
            }
            catch (Exception e)
            {
                // Catching any exception to not block the unloading process of other mods
                Logging.LogException(e, "Exception when destroying DistrictStylePickerPanel");
            }
        }

        internal void Toggle(byte districtId)
        {
            if (isVisible)
            {
                _pickerTitleBar.Title = "District Style Picker";
                m_selectedStyles = new HashSet<string>();
                _gameObject.SetActive(false);
                instance.Hide();
                Logging.DebugLog($"Hide DS picker for district id {districtId}");
            }
            else
            {
                _gameObject.SetActive(true);
                instance.Show(true);
                m_selectedDistrictId = districtId;
                _pickerTitleBar.Title = $"District Style Picker (districtId: {m_selectedDistrictId})";
                m_selectedStyles = DSPTransientStyleManager.GetSelectedStylesForDistrict(m_selectedDistrictId);
                RefreshStyleSelectData();
                Logging.DebugLog($"Show DS picker for district id {m_selectedDistrictId}");
            }
        }

        private void DrawDistrictStylePickerPanel()
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
            _pickerTitleBar = AddUIComponent(typeof(UITitleBar)) as UITitleBar;
            if (_pickerTitleBar != null) {
                _pickerTitleBar.Title = "District Style Picker";
                _pickerTitleBar.IconSprite = "ToolbarIconZoomOutCity";
            }
            else
            {
                Logging.ErrorLog("Cannot create title bar for district style picker!");
            }
        }

        private void SetupStyleListPanel()
        {
            _styleListPanel = AddUIComponent<DSPickerStyleListPanel>();
            _styleListPanel.width = PanelWidth;
            _styleListPanel.height = PanelHeight - 50;
            _styleListPanel.relativePosition = new Vector3(Spacing, TitleHeight + Spacing);
        }

        private void SetupSaveButton()
        {
            _saveButton = UIUtils.CreateButton(this);
            _saveButton.text = "Save";
            _saveButton.relativePosition =
                new Vector3(Spacing, TitleHeight + Spacing + PanelHeight - 50 + Spacing * 2);

            _saveButton.eventClick += (component, param) =>
            {
                Logging.DebugLog($"save button clicked {m_selectedDistrictId} " +
                                 $"and styles {string.Join(", ", m_selectedStyles.ToArray())}");
                DSPTransientStyleManager.SetSelectedStylesForDistrict(m_selectedDistrictId, m_selectedStyles);
                instance.Hide();
            };
            
        }

        internal HashSet<string> GetSelectedStyles()
        {
            return m_selectedStyles;
        }

        internal void AddStyleToSelected(DistrictStyle districtStyle)
        {
            m_selectedStyles.Add(districtStyle.FullName);
        }

        internal void RemoveStyleFromSelected(DistrictStyle districtStyle)
        {
            m_selectedStyles.Remove(districtStyle.FullName);
        }

        private void RefreshStyleSelectData()
        {
            if (DSPickerStyleListPanel.instance == null) return; // nothing to refresh
            
            DSPickerStyleListPanel.instance.RefreshStoredDistrictStyles();
            DSPickerStyleListPanel.instance.RefreshPickerStyleSelect();
        }
    }
}