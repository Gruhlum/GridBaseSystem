using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public class GridEventSystem : MonoBehaviour
    {
        public BaseGrid Grid
        {
            get
            {
                return grid;
            }
            private set
            {
                grid = value;
            }
        }
        [SerializeField] private BaseGrid grid = default;

        public Coord MouseHoverCoord
        {
            get
            {
                return mouseHoverCoord;
            }
        }
        private Coord mouseHoverCoord;

        public event Action<Coord> OnMouseHoverCoordChanged;
        public event Action<Coord, int> OnMouseClicked;


        private void Start()
        {
            mouseHoverCoord = grid.MousePositionToCoord();
            mouseHoverCoord.isValid = false;
        }

        private void Update()
        {
            MouseOverCheck();
            if (Input.GetMouseButtonDown(0))
            {
                OnMouseClicked?.Invoke(mouseHoverCoord, 0);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                OnMouseClicked?.Invoke(mouseHoverCoord, 1);
            }
        }

        private void MouseOverCheck()
        {
            Coord coord = grid.MousePositionToCoord();
            
            if (mouseHoverCoord == coord)
            {
                return;
            }

            mouseHoverCoord.isValid = grid.DoesTileExist(coord);
            mouseHoverCoord.Set(coord);
            OnMouseHoverCoordChanged?.Invoke(mouseHoverCoord);
        }
        public Vector3 MouseHoverToWorldPoint()
        {
            return grid.CoordToWorldPoint(mouseHoverCoord);
        }
    }
}