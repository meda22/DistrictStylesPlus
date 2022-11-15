using System.Collections.Generic;
using System.Linq;
using ColossalFramework;
using ColossalFramework.UI;
using DistrictStylesPlus.Code.Managers;
using UnityEngine;

namespace DistrictStylesPlus.Code.GUI.DistrictStylePicker
{
    public class DSPickerStyleListPanel : UIPanel
    {

        private UIFastList _styleSelect;
        
        public static DSPickerStyleListPanel instance { get; private set; }

        public override void Start()
        {
            base.Start();

            instance = this;
            
            _styleSelect = UIFastList.Create<DSPickerStyleItem>(this);
            _styleSelect.backgroundSprite = "UnlockingPanel";
            _styleSelect.width = width;
            _styleSelect.height = height - 40;
            _styleSelect.canSelect = true;
            _styleSelect.rowHeight = 40;
            _styleSelect.autoHideScrollbar = true;
            _styleSelect.relativePosition = Vector3.zero;

            _styleSelect.rowsData = GetStoredDistrictStyles();
        }

        internal void RefreshPickerStyleSelect()
        {
            _styleSelect.Refresh();
        }

        internal void RefreshStoredDistrictStyles()
        {
            _styleSelect.rowsData = GetStoredDistrictStyles();
        }

        private FastList<object> GetStoredDistrictStyles()
        {
            var districtStyles = Singleton<DistrictManager>.instance.m_Styles;

            var resultData = new FastList<object>();
            if (districtStyles.Length <= 0) return resultData;
            
            foreach (var districtStyle in districtStyles)
            {
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