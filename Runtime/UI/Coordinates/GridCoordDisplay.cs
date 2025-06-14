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

        protected override void AddEvents(Tile tile)
        {
            base.AddEvents(tile);
            tile.OnRemoved += Item_OnRemoved;
            tile.OnMoved += Item_OnMoved;
        }
        protected override void RemoveEvents(Tile tile)
        {
            base.RemoveEvents(tile);
            tile.OnRemoved -= Item_OnRemoved;
            tile.OnMoved -= Item_OnMoved;
        }
        private void Item_OnMoved(Tile obj, Coord old, Coord current)
        {
            DrawItem(Item);
        }

        private void Item_OnRemoved(Tile obj)
        {
            Deactivate();
        }
    }
}