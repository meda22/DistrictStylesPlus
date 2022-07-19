using System.Collections.Generic;
using System.Linq;
using ColossalFramework;
using ColossalFramework.UI;
using DistrictStylesPlus.Code.Managers;
using DistrictStylesPlus.Code.Utils;
using UnityEngine;
using static DistrictStylesPlus.Code.GUI.DistrictStyleEditorPanel;

namespace DistrictStylesPlus.Code.GUI
{
    public class UIBuildingSelectPanel : UIPanel
    {
        private UIFastList _buildingInfoSelect;
        private UILabel _includeLabel;
        private UIButton _includeAllButton;
        private UIButton _includeNoneButton;
        internal BuildingInfo SelectedBuildingInfo;

        public static UIBuildingSelectPanel Instance { get; private set; }

        public override void Start()
        {
            base.Start();

            Instance = this;
            
            SetupBuildingInfoSelect();
            SetupIncludeNoneButton();
            SetupIncludeAllButton();
            SetupIncludeLabel();
        }

        internal void RefreshBuildingInfoSelectList()
        {
            _buildingInfoSelect.Refresh();
        }

        internal void FilterAndRefreshBuildingInfoSelectList()
        {
            _buildingInfoSelect.rowsData =
                FilterBuildingInfoList(Singleton<DistrictStylesPlusManager>.instance.BuildingInfoList);
        }

        private void SetupBuildingInfoSelect()
        {
            _buildingInfoSelect = UIFastList.Create<UIBuildingInfoItem>(this);
            _buildingInfoSelect.backgroundSprite = "UnlockingPanel";
            _buildingInfoSelect.width = width;
            _buildingInfoSelect.height = height - 40;
            _buildingInfoSelect.canSelect = true;
            _buildingInfoSelect.rowHeight = 40;
            _buildingInfoSelect.autoHideScrollbar = true;
            _buildingInfoSelect.relativePosition = Vector3.zero;

            _buildingInfoSelect.rowsData =
                FilterBuildingInfoList(Singleton<DistrictStylesPlusManager>.instance.BuildingInfoList);
            
            _buildingInfoSelect.eventSelectedIndexChanged += (component, value) =>
            {
                if (value > -1) {
                    SelectedBuildingInfo = _buildingInfoSelect.selectedItem as BuildingInfo;
                }
            };

            // TODO: add event mouse leave
            _buildingInfoSelect.eventMouseLeave += (component, param) =>
            {
                if (SelectedBuildingInfo != null)
                    UIBuildingDetailsPanel.instance.UpdateBuildingInfo(SelectedBuildingInfo);
            };
        }

        // TODO: how to register right click
        // public override void Update()
        // {
        //     base.Update();
        //     
        //     Logging.DebugLog("Je tu CLICK!");
        //     if (Input.GetMouseButtonDown(1))
        //     {
        //         Logging.DebugLog("Je tu PRAVY Click.");
        //     }
        // }

        private FastList<object> FilterBuildingInfoList(List<BuildingInfo> buildingInfos)
        {
            var filteredData = new FastList<object>();
            var selectedDistrictStyle = UIDistrictStyleSelectPanel.SelectedDistrictStyle;
            
            for (var i = 0; i < buildingInfos.Count; i++)
            {
                BuildingInfo item = buildingInfos[i];

                // Origin
                if (UIBuildingFilter.Instance.buildingOrigin == Origin.Default && BuildingInfoHelper.IsCustomAsset(item.name)) continue;
                if (UIBuildingFilter.Instance.buildingOrigin == Origin.Custom && !BuildingInfoHelper.IsCustomAsset(item.name)) continue;
                // TODO: when cloning is implemented
                // if (buildingFilter.buildingOrigin == Origin.Cloned && !item.isCloned) continue;

                // Status
                if (selectedDistrictStyle != null)
                {
                    if (UIBuildingFilter.Instance.buildingStatus == Status.Included && !selectedDistrictStyle.Contains(item)) continue;
                    if (UIBuildingFilter.Instance.buildingStatus == Status.Excluded && selectedDistrictStyle.Contains(item)) continue;
                }

                // Level
                if (UIBuildingFilter.Instance.buildingLevel != ItemClass.Level.None 
                    && item.GetClassLevel() != UIBuildingFilter.Instance.buildingLevel) continue;

                // size
                Vector2 buildingSize = UIBuildingFilter.Instance.buildingSize;
                if (buildingSize != Vector2.zero 
                    && new Vector2(item.m_cellWidth, item.m_cellLength) != buildingSize) continue;

                // zone
                if (!UIBuildingFilter.Instance.IsAllZoneSelected())
                {
                    Category category = BuildingInfoHelper.GetBuildingCategory(item);
                    if (category == Category.None || !UIBuildingFilter.Instance.IsZoneSelected(category)) continue;
                }
                // Name
                if (!UIBuildingFilter.Instance.buildingName.IsNullOrWhiteSpace() 
                    && !item.name.ToLower().Contains(UIBuildingFilter.Instance.buildingName.ToLower())) continue;
                
                // height
                if (!(UIBuildingFilter.Instance.minBuildingHeight <= BuildingInfoHelper.GetBuildingHeight(item))) continue;
                if (!(UIBuildingFilter.Instance.maxBuildingHeight >= BuildingInfoHelper.GetBuildingHeight(item))) continue;

                filteredData.Add(item);
            }
            
            return filteredData;
        }

        private void SetupIncludeNoneButton()
        {
            _includeNoneButton = UIUtils.CreateButton(this);
            _includeNoneButton.width = 55;
            _includeNoneButton.text = "None";
            _includeNoneButton.relativePosition = new Vector3(width - _includeNoneButton.width, 
                _buildingInfoSelect.height + Spacing);
            
            _includeNoneButton.eventClick += (component, clickEvent) =>
            {
                if (clickEvent.used) return;
                
                Logging.DebugLog("Number of assets: " + _buildingInfoSelect.rowsData.m_size);
                var selectedDistrictStyle = UIDistrictStyleSelectPanel.GetSelectedDistrictStyle();
                
                if (selectedDistrictStyle == null || selectedDistrictStyle.BuiltIn) return;
                
                var buildingInfosToRemove = 
                    _buildingInfoSelect.rowsData.m_buffer.Cast<BuildingInfo>().ToList();
                
                Logging.DebugLog("Try to remove multiple buildings from style.");
                
                DSPDistrictStyleManager.RemoveBuildingInfoListFromStyle(buildingInfosToRemove, selectedDistrictStyle);
                
                // TODO: is it needed?
                UIDistrictStyleSelectPanel.Instance.RefreshDistrictStyleSelectList();
                Instance.RefreshBuildingInfoSelectList();
            };
        }

        private void SetupIncludeAllButton()
        {
            _includeAllButton = UIUtils.CreateButton(this);
            _includeAllButton.width = 55;
            _includeAllButton.text = "All";
            _includeAllButton.relativePosition = new Vector3(
                _includeNoneButton.relativePosition.x - _includeAllButton.width - Spacing, 
                _buildingInfoSelect.height + Spacing);
            
            _includeAllButton.eventClick += (component, clickEvent) =>
            {
                if (clickEvent.used) return;
                
                Logging.DebugLog("Number of assets: " + _buildingInfoSelect.rowsData.m_size);
                var selectedDistrictStyle = UIDistrictStyleSelectPanel.GetSelectedDistrictStyle();
                
                if (selectedDistrictStyle == null || selectedDistrictStyle.BuiltIn) return;
                
                var buildingInfosToInclude = 
                    _buildingInfoSelect.rowsData.m_buffer.Cast<BuildingInfo>().ToList();
                
                Logging.DebugLog("Try to add all buildings to style.");
                
                DSPDistrictStyleManager.AddBuildingInfoListToStyle(buildingInfosToInclude, selectedDistrictStyle);
                
                // TODO: is it needed?
                UIDistrictStyleSelectPanel.Instance.RefreshDistrictStyleSelectList();
                Instance.RefreshBuildingInfoSelectList();
            };
            
        }

        private void SetupIncludeLabel()
        {
            _includeLabel = AddUIComponent<UILabel>();
            _includeLabel.width = 100;
            _includeLabel.padding = new RectOffset(0, 0, 8, 0);
            _includeLabel.textScale = 0.8f;
            _includeLabel.text = "Include:";
            _includeLabel.relativePosition = new Vector3(
                _includeAllButton.relativePosition.x - _includeLabel.width - Spacing, 
                _buildingInfoSelect.height + Spacing);

        }
    }
}