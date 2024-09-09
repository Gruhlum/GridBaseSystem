using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public class TileObjectPlacementController : GridPlacementController
    {
        public TileObjectData SelectedTileObjectData
        {
            get
            {
                return selectedTileObjectData;
            }
            private set
            {
                if (selectedTileObjectData == value)
                {
                    return;
                }
                selectedTileObjectData = value;
            }
        }
        private TileObjectData selectedTileObjectData;

        public override GridObjectData SelectedObject
        {
            get
            {
                return SelectedTileObjectData;
            }
        }

        private int currentRotation;

        protected override void Update()
        {
            base.Update();
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (SelectedTileObjectData == null)
                {
                    return;
                }
                if (Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift))
                {
                    currentRotation--;
                }
                else currentRotation++;

                ghost.Rotate(currentRotation);

                CheckForValidTile(gridEventSystem.MouseHoverCoord);
            }
        }
        protected override bool IsValidCoord(Coord coord)
        {
            if (!SelectedTileObjectData.IsValidPlacement(coord, grid, currentRotation))
            {
                return false;
            }
            return true;
        }
        public void SetSelectedObject(TileObjectData data)
        {
            if (data == null)
            {
                ClearSelectedObject();
                return;
            }

            SelectedTileObjectData = data;
            ghost.Activate(data, HoverCoord);
            ResetRotation();
        }
        private void ResetRotation()
        {
            currentRotation = 0;
            ghost.Rotate(0);
        }

        protected override void ClearSelected()
        {
            SelectedTileObjectData = null;
        }

        protected override GridObject GenerateObject(Coord coord)
        {
            TileObject tileObject = SelectedTileObjectData.CreateObject(coord, grid) as TileObject;
            grid.AddTileObject(tileObject);
            return tileObject;
        }
    }
}