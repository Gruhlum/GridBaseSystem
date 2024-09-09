using HexTecGames.Basics.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public class TileDisplayController : PlaceableDisplayController<TileData>
    {
        [SerializeField] private TilePlacementController gridPlacementController = default;

        private void OnEnable()
        {
            gridPlacementController.OnSelectedObjectChanged += GridPlacementController_OnSelectedObjectChanged;
        }
        private void OnDisable()
        {
            gridPlacementController.OnSelectedObjectChanged -= GridPlacementController_OnSelectedObjectChanged;
        }

        public override void DisplayClicked(Display<TileData> display)
        {
            base.DisplayClicked(display);
            gridPlacementController.SetSelectedObject(display.Item);
        }
    }
}