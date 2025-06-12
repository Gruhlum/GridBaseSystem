using HexTecGames.Basics.UI;
using HexTecGames.HotkeySystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public class PlaceableDisplayController : PreSeededDisplayController<PlaceableDisplay, PlacementData>
    {
        [SerializeField] private GridPlacementController tileObjPlacementController = default;
        [SerializeField] private HotkeyController hotkeyController = default;

        protected override void Reset()
        {
            base.Reset();
            tileObjPlacementController = FindObjectOfType<GridPlacementController>();
        }
        protected void Start()
        {
            if (displays.Count > 0)
            {
                displays[0].DisplayClicked();
            }
        }

        protected override void SetupDisplay(PlaceableDisplay display, PlacementData item)
        {
            base.SetupDisplay(display, item);
            hotkeyController.AddNextNumberHotkey(display.DisplayClicked);
        }

        protected void OnEnable()
        {
            tileObjPlacementController.OnSelectedObjectChanged += TileObjPlacementController_OnSelectedObjectChanged;
        }
        protected void OnDisable()
        {
            tileObjPlacementController.OnSelectedObjectChanged -= TileObjPlacementController_OnSelectedObjectChanged;
        }
        protected override void Display_OnDisplayClicked(PlaceableDisplay display)
        {
            base.Display_OnDisplayClicked(display);
            if (tileObjPlacementController.SelectedPlacementData == display.Item)
            {
                tileObjPlacementController.ClearSelectedPlacementData();
            }
            else tileObjPlacementController.SetSelectedObject(display.Item);
        }
        protected void TileObjPlacementController_OnSelectedObjectChanged(PlacementData data)
        {
            foreach (var display in displays)
            {
                if (display.gameObject.activeSelf)
                {
                    display.SetHighlight(data == display.Item);
                }
            }
        }
    }
}