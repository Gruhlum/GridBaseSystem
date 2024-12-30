using HexTecGames.Basics.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public class TileDisplayController : PlaceableDisplayController<PlacementData>
    {
        [SerializeField] private TilePlacementController tilePlacementController = default;


        protected override void Reset()
        {
            base.Reset();
            tilePlacementController = FindObjectOfType<TilePlacementController>();
        }

        private void OnEnable()
        {
            tilePlacementController.OnSelectedObjectChanged += TileObjPlacementController_OnSelectedObjectChanged;
        }
        private void OnDisable()
        {
            tilePlacementController.OnSelectedObjectChanged -= TileObjPlacementController_OnSelectedObjectChanged;
        }

        public override void DisplayClicked(Display<PlacementData> display)
        {
            base.DisplayClicked(display);
            tilePlacementController.SetSelectedObject(display.Item);
        }
    }
}