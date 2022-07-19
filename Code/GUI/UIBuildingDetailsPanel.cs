using ColossalFramework.UI;
using UnityEngine;
using static DistrictStylesPlus.Code.GUI.DistrictStyleEditorPanel;

namespace DistrictStylesPlus.Code.GUI
{
    public class UIBuildingDetailsPanel : UIPanel
    {

        private UIBuildingPreview m_buildingPreview;
        // TODO: implement buildingOptions (if needed)
        //private UIBuildingOptions m_buildingOptions;
        
        public static UIBuildingDetailsPanel instance { get; private set; }
        
        public override void Start()
        {
            base.Start();

            instance = this;
            
            m_buildingPreview = AddUIComponent<UIBuildingPreview>();
            m_buildingPreview.width = width;
            m_buildingPreview.height = (height - Spacing) / 2;
            m_buildingPreview.relativePosition = Vector3.zero;
            
            // TODO: implement buildingOptions (if needed)
            // m_buildingOptions = AddUIComponent<UIBuildingOptions>();
            // m_buildingOptions.width = width;
            // m_buildingOptions.height = (height - Spacing) / 2 - 40;
            // m_buildingOptions.relativePosition = new Vector3(0, m_buildingPreview.height + Spacing);
            
            // TODO: will I need clone option?
        }
        
        internal void UpdateBuildingInfo(BuildingInfo buildingInfo)
        {
            m_buildingPreview.Show(buildingInfo);
            // TODO: we will have to rewrite showing building options
            //m_buildingOptions.Show(item);
        }
    }
}