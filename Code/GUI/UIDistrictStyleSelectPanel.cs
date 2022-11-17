using ColossalFramework;
using ColossalFramework.UI;
using DistrictStylesPlus.Code.Managers;
using UnityEngine;
using static DistrictStylesPlus.Code.GUI.DistrictStyleEditorPanel;

namespace DistrictStylesPlus.Code.GUI
{
    public class UIDistrictStyleSelectPanel : UIPanel
    {
        private UIFastList _districtStyleSelect;
        private UIButton _addStyle;
        private UIButton _removeStyle;
        internal static DistrictStyle SelectedDistrictStyle;
        
        public static UIDistrictStyleSelectPanel Instance { get; private set; }
        
        public override void Start()
        {
            base.Start();

            Instance = this;
            
            SetupDistrictStyleSelect();
            SetupAddStyleButton();
            SetupRemoveStyleButton();
        }
        
        internal void RefreshDistrictStyleSelect()
        {
            _districtStyleSelect.rowsData = GetStoredDistrictStyles();
        }

        private void SetupAddStyleButton()
        {
            _addStyle = UIUtils.CreateButton(this);
            _addStyle.width = (width - Spacing) / 2;
            _addStyle.text = "New Style";
            _addStyle.relativePosition = new Vector3(0, _districtStyleSelect.height + Spacing);

            _addStyle.eventClick += (c, p) =>
            {
                UIView.PushModal(UINewStyleModal.Instance);
                UINewStyleModal.Instance.Show(true);
            };
        }
        
        private void SetupRemoveStyleButton()
        {
            _removeStyle = UIUtils.CreateButton(this);
            _removeStyle.width = (width - Spacing) / 2;
            _removeStyle.text = "Delete Style";
            _removeStyle.isEnabled = false;
            _removeStyle.relativePosition = new Vector3(width - _removeStyle.width,
                _districtStyleSelect.height + Spacing);

            _removeStyle.eventClick += (component, param) =>
            {
                ConfirmPanel.ShowModal("Delete District Style", 
                    $"Are you sure you want to delete '{SelectedDistrictStyle.Name}' style?",
                    (uiComponent, result) =>
                    {
                        if (result == 1) DSPDistrictStyleManager.DeleteDistrictStyle(SelectedDistrictStyle, false);
                        SelectedDistrictStyle = null;
                        UIBuildingSelectPanel.Instance.RefreshBuildingInfoSelectList();
                        RefreshDistrictStyleSelect();
                        component.isEnabled = false;
                    });
            };
        }

        private void SetupDistrictStyleSelect()
        {
            _districtStyleSelect = UIFastList.Create<UIDistrictStyleItem>(this);
            _districtStyleSelect.backgroundSprite = "UnlockingPanel";
            _districtStyleSelect.width = width;
            _districtStyleSelect.height = height - 40;
            _districtStyleSelect.canSelect = true;
            _districtStyleSelect.rowHeight = 40;
            _districtStyleSelect.autoHideScrollbar = true;
            _districtStyleSelect.relativePosition = Vector3.zero;

            _districtStyleSelect.rowsData = GetStoredDistrictStyles();

            _districtStyleSelect.eventSelectedIndexChanged += (component, value) =>
            {
                if (value == -1) return;
                
                SelectedDistrictStyle = _districtStyleSelect.selectedItem as DistrictStyle;
                _removeStyle.isEnabled = !SelectedDistrictStyle?.BuiltIn ?? false;

                UIBuildingSelectPanel.Instance.FilterAndRefreshBuildingInfoSelectList();
                UIBuildingSelectPanel.Instance.RefreshBuildingInfoSelectList();
            };
        }
        
        public static DistrictStyle GetSelectedDistrictStyle()
        {
            return SelectedDistrictStyle;
        }

        internal void RefreshDistrictStyleSelectList()
        {
            _districtStyleSelect.Refresh();
        }

        private FastList<object> GetStoredDistrictStyles()
        {
            var districtStyles = Singleton<DistrictManager>.instance.m_Styles;
            
            var resultData = new FastList<object>();
            if (districtStyles.Length <= 0) return resultData;
            
            foreach (var districtStyle in districtStyles)
            {
                // transient style for district should not be shown
                if (districtStyle.PackageName.Equals(DSPTransientStyleManager.TransientStylePackage)) continue;

                // styles for not owned DLCs or CCP should not be shown
                if (districtStyle.Name.Equals(DistrictStyle.kEuropeanStyleName) 
                    && !SteamHelper.IsDLCOwned(SteamHelper.DLC.ModderPack3))
                    continue;
                if (districtStyle.Name.Equals(DistrictStyle.kModderPack5StyleName) 
                    && !SteamHelper.IsDLCOwned(SteamHelper.DLC.ModderPack5))
                    continue;
                if (districtStyle.Name.Equals(DistrictStyle.kModderPack11StyleName) 
                    && !SteamHelper.IsDLCOwned(SteamHelper.DLC.ModderPack11))
                    continue;
                if (districtStyle.Name.Equals(DistrictStyle.kModderPack14StyleName) 
                    && !SteamHelper.IsDLCOwned(SteamHelper.DLC.ModderPack14))
                    continue;

                resultData.Add(districtStyle);
            }
            
            return resultData;
        }
        
    }
}