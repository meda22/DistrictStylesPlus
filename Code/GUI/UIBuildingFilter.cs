﻿using ColossalFramework.PlatformServices;
using ColossalFramework.UI;
using UnityEngine;

namespace DistrictStylesPlus.Code.GUI
{
    public class UIBuildingFilter : UIPanel
    {
        public static UIBuildingFilter Instance { get; private set; }
        
        private const int NumOfCategories = 19; // 19 types after new Financial Districts DLC
        public UICheckBox[] zoningToggles;
        public UIButton allZones;
        public UIButton noZones;
        public UIDropDown origin;
        public UIDropDown status;
        public UIDropDown levelFilter;
        public UIDropDown sizeFilterX;
        public UIDropDown sizeFilterY;
        public UITextField nameFilter;
        public UITextField minHeightFilter;
        public UITextField maxHeightFilter;
        public UIDropDown orderBySelect;

        public int maxBuildingHeight
        {
            get
            {
                int.TryParse(maxHeightFilter.text, out var maxHeight);
                if (maxHeight == 0) maxHeight = int.MaxValue;
                return maxHeight;
            }
        }
        
        public int minBuildingHeight
        {
            get
            {
                int.TryParse(minHeightFilter.text, out var minHeight);
                if (minHeight == 0) minHeight = int.MinValue;
                return minHeight;
            }
        }


        public bool IsZoneSelected(Category zone)
        {
            return zoningToggles[(int)zone].isChecked;
        }

        public bool IsAllZoneSelected()
        {
            return zoningToggles[(int)Category.ResidentialLow].isChecked &&
                zoningToggles[(int)Category.ResidentialHigh].isChecked &&
                zoningToggles[(int)Category.ResidentialEco].isChecked &&
                zoningToggles[(int)Category.CommercialLow].isChecked &&
                zoningToggles[(int)Category.CommercialHigh].isChecked &&
                zoningToggles[(int)Category.CommercialLeisure].isChecked &&
                zoningToggles[(int)Category.CommercialTourism].isChecked &&
                zoningToggles[(int)Category.CommercialEco].isChecked &&
                zoningToggles[(int)Category.Industrial].isChecked &&
                zoningToggles[(int)Category.Farming].isChecked &&
                zoningToggles[(int)Category.Forestry].isChecked &&
                zoningToggles[(int)Category.Oil].isChecked &&
                zoningToggles[(int)Category.Ore].isChecked &&
                zoningToggles[(int)Category.Office].isChecked &&
                zoningToggles[(int)Category.OfficeHightech].isChecked &&
                zoningToggles[(int)Category.ResidentialWallToWall].isChecked &&
                zoningToggles[(int)Category.CommercialWallToWall].isChecked &&
                zoningToggles[(int)Category.OfficeWallToWall].isChecked &&
                zoningToggles[(int)Category.OfficeFinancial].isChecked;
        }

        public ItemClass.Level buildingLevel
        {
            get { return (ItemClass.Level)(levelFilter.selectedIndex - 1); }
        }

        public Vector2 BuildingSize =>
            sizeFilterX.selectedIndex == 0 ? 
                Vector2.zero : 
                new Vector2(sizeFilterX.selectedIndex, sizeFilterY.selectedIndex);

        public string buildingName
        {
            get { return nameFilter.text.Trim(); }
        }

        public Origin buildingOrigin
        {
            get { return (Origin)origin.selectedIndex; }
        }

        public Status buildingStatus
        {
            get { return (Status)status.selectedIndex; }
        }

        public int orderBy => orderBySelect.selectedIndex;

        public event PropertyChangedEventHandler<int> eventFilteringChanged;

        public override void Start()
        {
            base.Start();

            Instance = this;

            // Zoning
            zoningToggles = new UICheckBox[NumOfCategories];
            for (int i = 0; i < NumOfCategories; i++)
            {
                zoningToggles[i] = UIUtils.CreateIconToggle(this, CategoryIcons.atlases[i], CategoryIcons.spriteNames[i], CategoryIcons.spriteNames[i] + "Disabled");
                zoningToggles[i].tooltip = CategoryIcons.tooltips[i];
                zoningToggles[i].relativePosition = new Vector3(35 * i, 0);
                zoningToggles[i].isChecked = true;
                zoningToggles[i].readOnly = true;
                zoningToggles[i].checkedBoxObject.isInteractive = false; // Don't eat my double click event please

                zoningToggles[i].eventClick += (c, p) =>
                {
                    ((UICheckBox)c).isChecked = !((UICheckBox)c).isChecked;
                    eventFilteringChanged(this, 0);
                };

                zoningToggles[i].eventDoubleClick += (c, p) =>
                {
                    for (int j = 0; j < NumOfCategories; j++)
                        zoningToggles[j].isChecked = false;
                    ((UICheckBox)c).isChecked = true;

                    eventFilteringChanged(this, 0);
                };
            }

            if (!PlatformService.IsDlcInstalled(SteamHelper.kAfterDLCAppID))
            {
                zoningToggles[(int) Category.CommercialLeisure].isVisible = false;
                zoningToggles[(int) Category.CommercialTourism].isVisible = false;
            }

            if (!PlatformService.IsDlcInstalled(SteamHelper.kGreenDLCAppID))
            {
                zoningToggles[(int)Category.ResidentialEco].isVisible = false;
                zoningToggles[(int)Category.CommercialEco].isVisible = false;
                zoningToggles[(int)Category.OfficeHightech].isVisible = false;
            }

            if (!PlatformService.IsDlcInstalled(SteamHelper.kPlazasAndPromenadesDLCAppID))
            {
                zoningToggles[(int)Category.ResidentialWallToWall].isVisible = false;
                zoningToggles[(int)Category.CommercialWallToWall].isVisible = false;
                zoningToggles[(int)Category.OfficeWallToWall].isVisible = false;
            }

            if (!PlatformService.IsDlcInstalled(SteamHelper.kFinancialDistrictsDLCAppID))
            {
                zoningToggles[(int)Category.OfficeFinancial].isVisible = false;
            }

            allZones = UIUtils.CreateButton(this);
            allZones.width = 40;
            allZones.text = "All";
            allZones.relativePosition = new Vector3(675, 5);

            allZones.eventClick += (c, p) =>
            {
                for (int i = 0; i < NumOfCategories; i++)
                {
                    zoningToggles[i].isChecked = true;
                }
                eventFilteringChanged(this, 0);
            };

            noZones = UIUtils.CreateButton(this);
            noZones.width = 55;
            noZones.text = "None";
            noZones.relativePosition = new Vector3(720, 5);

            noZones.eventClick += (c, p) =>
            {
                for (int i = 0; i < NumOfCategories; i++)
                {
                    zoningToggles[i].isChecked = false;
                }
                eventFilteringChanged(this, 0);
            };

            // Display
            UILabel display = AddUIComponent<UILabel>();
            display.textScale = 0.8f;
            display.padding = new RectOffset(0, 0, 8, 0);
            display.text = "Display: ";
            display.relativePosition = new Vector3(0, 40);

            origin = UIUtils.CreateDropDown(this);
            origin.width = 90;
            origin.AddItem("All");
            origin.AddItem("Default");
            origin.AddItem("Custom");
            origin.selectedIndex = 0;
            origin.relativePosition = new Vector3(display.relativePosition.x + display.width + 5, 40);

            origin.eventSelectedIndexChanged += (c, i) => eventFilteringChanged(this, 1);

            status = UIUtils.CreateDropDown(this);
            status.width = 90;
            status.AddItem("All");
            status.AddItem("Included");
            status.AddItem("Excluded");
            status.selectedIndex = 0;
            status.relativePosition = new Vector3(origin.relativePosition.x + origin.width + 5, 40);

            status.eventSelectedIndexChanged += (c, i) => eventFilteringChanged(this, 2);

            // Level
            UILabel levelLabel = AddUIComponent<UILabel>();
            levelLabel.textScale = 0.8f;
            levelLabel.padding = new RectOffset(0, 0, 8, 0);
            levelLabel.text = "Level: ";
            levelLabel.relativePosition = new Vector3(status.relativePosition.x + status.width + 10, 40);

            levelFilter = UIUtils.CreateDropDown(this);
            levelFilter.width = 55;
            levelFilter.AddItem("All");
            levelFilter.AddItem("1");
            levelFilter.AddItem("2");
            levelFilter.AddItem("3");
            levelFilter.AddItem("4");
            levelFilter.AddItem("5");
            levelFilter.selectedIndex = 0;
            levelFilter.relativePosition = new Vector3(levelLabel.relativePosition.x + levelLabel.width + 5, 40);

            levelFilter.eventSelectedIndexChanged += (c, i) => eventFilteringChanged(this, 3);

            // Size
            UILabel sizeLabel = AddUIComponent<UILabel>();
            sizeLabel.textScale = 0.8f;
            sizeLabel.padding = new RectOffset(0, 0, 8, 0);
            sizeLabel.text = "Size: ";
            sizeLabel.relativePosition = new Vector3(levelFilter.relativePosition.x + levelFilter.width + 10, 40);

            sizeFilterX = UIUtils.CreateDropDown(this);
            sizeFilterX.width = 55;
            sizeFilterX.AddItem("All");
            sizeFilterX.AddItem("1");
            sizeFilterX.AddItem("2");
            sizeFilterX.AddItem("3");
            sizeFilterX.AddItem("4");
            sizeFilterX.selectedIndex = 0;
            sizeFilterX.relativePosition = new Vector3(sizeLabel.relativePosition.x + sizeLabel.width + 5, 40);

            UILabel xLabel = AddUIComponent<UILabel>();
            xLabel.textScale = 0.8f;
            xLabel.padding = new RectOffset(0, 0, 8, 0);
            xLabel.text = "X";
            xLabel.isVisible = false;
            xLabel.relativePosition = new Vector3(sizeFilterX.relativePosition.x + sizeFilterX.width - 5, 40);

            sizeFilterY = UIUtils.CreateDropDown(this);
            sizeFilterY.width = 45;
            sizeFilterY.AddItem("All");
            sizeFilterY.AddItem("1");
            sizeFilterY.AddItem("2");
            sizeFilterY.AddItem("3");
            sizeFilterY.AddItem("4");
            sizeFilterY.selectedIndex = 0;
            sizeFilterY.isVisible = false;
            sizeFilterY.relativePosition = new Vector3(xLabel.relativePosition.x + xLabel.width + 5, 40);

            sizeFilterX.eventSelectedIndexChanged += (c, i) =>
            {
                if (i == 0)
                {
                    sizeFilterX.width = 55;
                    xLabel.isVisible = false;
                    sizeFilterY.selectedIndex = 0;
                    sizeFilterY.isVisible = false;
                }
                else
                {
                    sizeFilterX.width = 45;
                    xLabel.isVisible = true;
                    sizeFilterY.isVisible = true;
                }

                eventFilteringChanged(this, 4);
            };

            sizeFilterY.eventSelectedIndexChanged += (c, i) => eventFilteringChanged(this, 4);

            // Name filter
            UILabel nameLabel = AddUIComponent<UILabel>();
            nameLabel.textScale = 0.8f;
            nameLabel.padding = new RectOffset(0, 0, 8, 0);
            nameLabel.relativePosition = new Vector3(width - 250, 0);
            nameLabel.text = "Name: ";

            nameFilter = UIUtils.CreateTextField(this);
            nameFilter.width = 200;
            nameFilter.height = 30;
            nameFilter.padding = new RectOffset(6, 6, 6, 6);
            nameFilter.relativePosition = new Vector3(width - nameFilter.width, 0);

            nameFilter.eventTextChanged += (c, s) => eventFilteringChanged(this, 5);
            nameFilter.eventTextSubmitted += (c, s) => eventFilteringChanged(this, 5);
            
            // Height filter
            UILabel heightLabel = AddUIComponent<UILabel>();
            heightLabel.textScale = 0.8f;
            heightLabel.padding = new RectOffset(0, 0, 8, 0);
            heightLabel.relativePosition = new Vector3(width - 250, 40);
            heightLabel.text = "Height min-max: ";
            
            minHeightFilter = UIUtils.CreateTextField(this);
            minHeightFilter.width = 50;
            minHeightFilter.height = 30;
            minHeightFilter.padding = new RectOffset(6, 6, 6, 6);
            minHeightFilter.relativePosition = new Vector3(width - minHeightFilter.width - 60, 40);
            minHeightFilter.eventTextChanged += (c, s) => eventFilteringChanged(this, 6);
            minHeightFilter.eventTextSubmitted += (c, s) => eventFilteringChanged(this, 6);
            
            maxHeightFilter = UIUtils.CreateTextField(this);
            maxHeightFilter.width = 50;
            maxHeightFilter.height = 30;
            maxHeightFilter.padding = new RectOffset(6, 6, 6, 6);
            maxHeightFilter.relativePosition = new Vector3(width - minHeightFilter.width, 40);
            
            maxHeightFilter.eventTextChanged += (c, s) => eventFilteringChanged(this, 7);
            maxHeightFilter.eventTextSubmitted += (c, s) => eventFilteringChanged(this, 7);
            
            // Order By
            UILabel orderByLabel = AddUIComponent<UILabel>();
            orderByLabel.textScale = 0.8f;
            orderByLabel.padding = new RectOffset(0, 0, 8, 0);
            orderByLabel.text = "Order By: ";
            orderByLabel.relativePosition = new Vector3(sizeFilterY.relativePosition.x + 50, 40);

            orderBySelect = UIUtils.CreateDropDown(this);
            orderBySelect.width = 100;
            orderBySelect.AddItem("Name");
            orderBySelect.AddItem("Zone");
            orderBySelect.AddItem("Level");
            orderBySelect.AddItem("Size");
            orderBySelect.AddItem("Height");
            orderBySelect.selectedIndex = 0;
            
            orderBySelect.relativePosition = new Vector3(orderByLabel.relativePosition.x + orderByLabel.width + 5, 40);

            orderBySelect.eventSelectedIndexChanged += (c, i) => eventFilteringChanged(this, 8);
        }
    }
}