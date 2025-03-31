using HexTecGames.Basics.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public class PlaceableDisplayController : DisplayController<PlaceableDisplay, PlacementData>
    {
        [SerializeField] private GridPlacementController tileObjPlacementController = default;

        protected override void Reset()
        {
            base.Reset();
            tileObjPlacementController = FindObjectOfType<GridPlacementController>();
        }
        protected void Start()
        {
            DisplayItems();
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
            tileObjPlacementController.SetSelectedObject(display.Item);
        }
        protected void TileObjPlacementController_OnSelectedObjectChanged(PlacementData data)
        {
            foreach (var display in displaySpawner)
            {
                if (display.gameObject.activeSelf)
                {
                    display.SetHighlight(data == display.Item);
                }                
            }
        }
    }
}