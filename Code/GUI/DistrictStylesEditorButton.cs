using ColossalFramework.UI;
using DistrictStylesPlus.Code.Utils;
using UnityEngine;

namespace DistrictStylesPlus.Code.GUI
{
    public class DistrictStylesEditorButton : UIButton
    {

        private const float ButtonSize = 36f;
        private static GameObject _gameObject;
        
        internal static DistrictStylesEditorButton Instance { get; private set; }

        public override void Start()
        {
            base.Start();
            
            // Instance setting
            name = "DistrictStylesEditorButton";
            Instance = this;
            
            // label, size and position
            textScale = 0.8f;
            text = "DSPE";
            tooltip = "District Styles Plus Editor";
            size = new Vector2(ButtonSize, ButtonSize);
            SetPosition();
            
            // button look
            normalBgSprite = "OptionBase";
            hoveredBgSprite = "OptionBaseHovered";
            pressedBgSprite = "OptionBasePressed";
            disabledBgSprite = "OptionBaseDisabled";
            
            // click event handler
            eventClicked += (component, clickEvent) =>
            {
                if (!clickEvent.used) DistrictStyleEditorPanel.Instance.Toggle();
            };
            
            // Add drag handle
            var dragHandle = this.AddUIComponent<UIDragHandle>();
            dragHandle.target = this;
            dragHandle.relativePosition = Vector3.zero;
            dragHandle.size = this.size;
            
            // Save new position when moved
            eventPositionChanged += PositionChanged;

        }

        internal static void CreateDistrictStylesEditorButton()
        {
            // do nothing if button is set to be not shown or if it is exist already
            if (!ModSettings.showDistrictStylesEditorButton || Instance != null) return;
            
            _gameObject = new GameObject("DistrictStylesEditorButton");
            _gameObject.transform.parent = UIView.GetAView().transform;
            _gameObject.AddComponent<DistrictStylesEditorButton>();
        }

        internal static void DestroyDistrictStylesEditorButton()
        {
            // Do nothing if button does not exist
            if (Instance == null) return;
            
            Destroy(Instance);
            Instance = null;
        }
        
        private void SetPosition()
        {
            relativePosition = new Vector2(200, 200);

            var screenSize = GetUIView().GetScreenResolution();
            if (ModSettings.dsEditorButtonPosX == 0 ||
                ModSettings.dsEditorButtonPoxY == 0 ||
                ModSettings.dsEditorButtonPosX > screenSize.x - this.height ||
                ModSettings.dsEditorButtonPoxY > screenSize.y - this.width)
            {
                relativePosition = new Vector2(200, 200);
            }
            else
            {
                absolutePosition = new Vector2(ModSettings.dsEditorButtonPosX, ModSettings.dsEditorButtonPoxY);
            }
        }

        protected override void OnResolutionChanged(Vector2 previousResolution, Vector2 currentResolution)
        {
            base.OnResolutionChanged(previousResolution, currentResolution);
            
            SetPosition();
        }
        
        private void PositionChanged(UIComponent component, Vector2 position)
        {
            Logging.InfoLog("Set new DSPE position.");
            ModSettings.dsEditorButtonPosX = this.absolutePosition.x;
            ModSettings.dsEditorButtonPoxY = this.absolutePosition.y;
            SettingsUtils.SaveSettings();
        }

    }
}