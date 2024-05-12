using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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

        [SerializeField] private EventSystem eventSys = default;

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


        void Reset()
        {
            eventSys = FindObjectOfType<EventSystem>();
            grid = GetComponentInParent<BaseGrid>();
        }

        private void Start()
        {
            mouseHoverCoord = grid.MousePositionToCoord();
            mouseHoverCoord.isValid = false;
        }

        private void Update()
        {
            //if (eventSys != null && eventSys.IsPointerOverGameObject())
            //{
            //    mouseHoverCoord.isValid = false;
            //    OnMouseHoverCoordChanged?.Invoke(mouseHoverCoord);
            //    return;
            //}
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