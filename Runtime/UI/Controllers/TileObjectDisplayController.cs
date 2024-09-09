using HexTecGames.Basics.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public class TileObjectDisplayController : PlaceableDisplayController<TileObjectData>
    {
        [SerializeField] private TileObjectPlacementController gridPlacementController = default;

        private void OnEnable()
        {
            gridPlacementController.OnSelectedObjectChanged += GridPlacementController_OnSelectedObjectChanged;
        }
        private void OnDisable()
        {
            gridPlacementController.OnSelectedObjectChanged -= GridPlacementController_OnSelectedObjectChanged;
        }

        public override void DisplayClicked(Display<TileObjectData> display)
        {
            base.DisplayClicked(display);
            gridPlacementController.SetSelectedObject(display.Item);
        }
    }
}