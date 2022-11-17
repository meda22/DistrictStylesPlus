using ColossalFramework.UI;
using UnityEngine;

namespace DistrictStylesPlus.Code.GUI.DistrictStylePicker
{
    public class DSPickerTitleBar : UIPanel
    {

        private UISprite _icon;
        private UILabel _title;
        private UIButton _close;
        private UIDragHandle _drag;

        internal void Setup(string title)
        {
            width = parent.width;
            height = 40;
            isVisible = true;
            canFocus = true;
            isInteractive = true;
            relativePosition = Vector3.zero;

            _drag = AddUIComponent<UIDragHandle>();
            _drag.width = width - 50;
            _drag.height = height;
            _drag.relativePosition = Vector3.zero;
            _drag.target = parent;

            _icon = AddUIComponent<UISprite>();
            _icon.spriteName = "ToolbarIconZoomOutCity";
            _icon.relativePosition = new Vector3(10, 5);
            UIUtils.ResizeIcon(_icon, new Vector2(30, 30));
            _icon.relativePosition = new Vector3(10, 5);

            _title = AddUIComponent<UILabel>();
            _title.relativePosition = new Vector3(50, 13);
            _title.text = title;

            _close = AddUIComponent<UIButton>();
            _close.relativePosition = new Vector3(width - 35, 2);
            _close.normalBgSprite = "buttonclose";
            _close.hoveredBgSprite = "buttonclosehover";
            _close.pressedBgSprite = "buttonclosepressed";
            _close.eventClick += (component, param) =>
            {
                DistrictStylePickerPanelManager.Close();
            };
        }

        internal void ChangeTitle(string title)
        {
            if (_title != null)
            {
                _title.text = title;
            }
        }
        
    }
}