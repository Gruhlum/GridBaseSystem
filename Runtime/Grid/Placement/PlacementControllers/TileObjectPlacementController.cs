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
                if (SelectedPlacementData == null)
                {
                    return null;
                }
                return SelectedPlacementData.data as TileObjectData;
            }
        }

        public PlacementData SelectedPlacementData
        {
            get
            {
                return selectedPlacementData;
            }
            private set
            {
                if (selectedPlacementData == value)
                {
                    return;
                }
                selectedPlacementData = value;
            }
        }
        private PlacementData selectedPlacementData;

        public override PlacementData SelectedObject
        {
            get
            {
                return null; //TODO
            }
        }

        private int currentRotation;

        protected override void Update()
        {
            base.Update();
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (SelectedPlacementData == null)
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
        public void SetSelectedObject(PlacementData data)
        {
            if (data == null)
            {
                ClearSelectedObject();
                return;
            }

            SelectedPlacementData = data;
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
            SelectedPlacementData = null;
        }

        protected override GridObject GenerateObject(Coord coord)
        {
            TileObject tileObject = SelectedTileObjectData.CreateObject(grid, coord, currentRotation);
            grid.AddTileObject(tileObject);
            return tileObject;
        }
    }
}