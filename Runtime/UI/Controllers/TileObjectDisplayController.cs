using HexTecGames.Basics.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public class TileObjectDisplayController : PlaceableDisplayController<PlacementData>
    {
        [SerializeField] private TileObjectPlacementController tileObjPlacementController = default;


        protected override void Reset()
        {
            base.Reset();
            tileObjPlacementController = FindObjectOfType<TileObjectPlacementController>();
        }
        private void OnEnable()
        {
            tileObjPlacementController.OnSelectedObjectChanged += TileObjPlacementController_OnSelectedObjectChanged;
        }
        private void OnDisable()
        {
            tileObjPlacementController.OnSelectedObjectChanged -= TileObjPlacementController_OnSelectedObjectChanged;
        }

        public override void DisplayClicked(Display<PlacementData> display)
        {
            base.DisplayClicked(display);
            tileObjPlacementController.SetSelectedObject(display.Item);
        }
    }
}