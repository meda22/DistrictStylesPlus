﻿using ColossalFramework.UI;
using DistrictStylesPlus.Code.Managers;
using UnityEngine;

namespace DistrictStylesPlus.Code.GUI.DistrictStylePicker
{
    public class DSPickerStyleListPanel : UIPanel
    {

        private UIFastList _styleSelect;

        public void Setup()
        {
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
            var styles = DSPDistrictStyleManager.GetStoredDistrictStyles();

            var resultData = new FastList<object>();

            if (styles.Count <= 0) return resultData;

            foreach (var style in styles)
            {
                resultData.Add(style);
            }

            return resultData;
        }
    }
}