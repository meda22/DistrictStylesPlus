using System;
using ColossalFramework.UI;
using DistrictStylesPlus.Code.Utils;
using UnityEngine;

namespace DistrictStylesPlus.Code.GUI
{
    public class MainButtonPanel : UIPanel
    {

        private UIButton m_mainButton;
        private UIPanel m_mainButtonPanel;

        private static GameObject _gameObject;
        public static MainButtonPanel Instance { get; private set; }

        /// <summary>
        /// Initialize District Style Editor.
        /// </summary>
        internal static void Initialize()
        {
            if (Instance != null) return;

            try
            {
                _gameObject = new GameObject("DSPMainButtonPanel");
                _gameObject.transform.parent = UIView.GetAView().transform;
                Instance = _gameObject.AddComponent<MainButtonPanel>();
                Instance.DrawMainButtonPanel();
                _gameObject.SetActive(false);
            }
            catch (Exception e)
            {
                Logging.LogException(e, "Exception when initializing DSPMainButtonPanel");
            }
        }
        
        /// <summary>
        /// Destroy gameObject of District Styles Editor
        /// </summary>
        public static void Destroy()
        {
            try
            {
                if (_gameObject != null)
                    GameObject.Destroy(_gameObject);
            }
            catch (Exception e)
            {
                Logging.LogException(e, "Exception when destroying DSPMainButtonPanel");
            }
        }

        private void DrawMainButtonPanel()
        {
            try
            {
                m_mainButtonPanel = UIUtils.CreatePanel("DSPMainButtonPanel");
                m_mainButtonPanel.zOrder = 25;
                m_mainButtonPanel.size = new Vector2(50, 50);
                m_mainButtonPanel.relativePosition = new Vector2(800, 0);
                Logging.DebugLog("DSPMainButtonPanel has been created.");

                m_mainButton = 
                    UIUtils.CreateButton(m_mainButtonPanel, "DSPMainButton", "DSPE", "Open DSP Editor");
                m_mainButton.size = new Vector2(43, 43);
                m_mainButton.relativePosition = new Vector2(0, 0);

                m_mainButton.eventClick += (component, clickEvent) =>
                {
                    if (!clickEvent.used)
                    {
                        DistrictStyleEditorPanel.Instance.Toggle();
                    }
                };
                
                Logging.DebugLog("DSPMainButton has been created.");
            }
            catch (Exception e)
            {
                Logging.LogException(e, "Exception when drawing DSPMainButtonPanel");
            }
        }
    }
}