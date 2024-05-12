using HexTecGames.Basics.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
	public class PlaceableDisplayController : DisplayController<BaseTileObjectData>
	{
        [SerializeField] private GridPlacementController gridPlacementController = default;


        protected void Awake()
        {
            DisplayItems();
        }
        private void OnEnable()
        {
            gridPlacementController.OnSelectedObjectChanged += GridPlacementController_OnSelectedObjectChanged;
        }
        private void OnDisable()
        {
            gridPlacementController.OnSelectedObjectChanged -= GridPlacementController_OnSelectedObjectChanged;
        }
        private void GridPlacementController_OnSelectedObjectChanged(BaseTileObjectData data)
        {
            foreach (var display in displaySpawner.GetActiveBehaviours())
            {
                display.SetHighlight(data == display.Item);
            }
        }

        public override void DisplayClicked(Display<BaseTileObjectData> display)
        {
            base.DisplayClicked(display);
            gridPlacementController.SetSelectedObject(display.Item);
        }
    }
}