using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using HexTecGames.Basics.UI;
using TMPro;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public class GridCoordDisplay : Display<GridCoordDisplay, Tile>
    {
        [SerializeField] private TMP_Text textGUI = default;

        protected override void DrawItem(Tile tile)
        {
            textGUI.text = $"{tile.Center.x},{tile.Center.y}";
            transform.localPosition = tile.GetWorldPosition();
        }
        protected override void OnDisable()
        {
            RemoveEvents(Item);
            base.OnDisable();
        }
        public override void Deactivate()
        {
            RemoveEvents(Item);
            base.Deactivate();
        }

        public override void SetItem(Tile item, bool activate = true)
        {
            base.SetItem(item, activate);
            AddEvents(item);
        }
        private void AddEvents(Tile tile)
        {
            if (tile == null)
            {
                return;
            }
            tile.OnRemoved += Item_OnRemoved;
            tile.OnMoved += Item_OnMoved;
        }
        private void RemoveEvents(Tile tile)
        {
            if (tile == null)
            {
                return;
            }
            tile.OnRemoved -= Item_OnRemoved;
            tile.OnMoved -= Item_OnMoved;
        }
        private void Item_OnMoved(GridObject obj, Coord old, Coord current)
        {
            DrawItem(Item);
        }

        private void Item_OnRemoved(GridObject obj)
        {
            Deactivate();
        }
    }
}