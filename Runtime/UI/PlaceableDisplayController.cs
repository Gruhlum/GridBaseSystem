using HexTecGames.Basics.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
	public class PlaceableDisplayController : DisplayController<GridObjectData>
	{
        [SerializeField] private GridPlacementController gridPlacementController = default;

        void Start()
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
        private void GridPlacementController_OnSelectedObjectChanged(GridObjectData data)
        {
            foreach (var display in displaySpawner.GetActiveBehaviours())
            {
                display.SetHighlight(data == display.Item);
            }
        }

        public override void DisplayClicked(Display<GridObjectData> display)
        {
            base.DisplayClicked(display);
            gridPlacementController.SetSelectedObject(display.Item);
        }
    }
}