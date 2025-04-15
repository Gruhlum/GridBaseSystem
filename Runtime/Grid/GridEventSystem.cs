using System;
using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
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

        [SerializeField] private bool showDebugs = default;

        public event Action<Coord> OnMouseHoverCoordChanged;
        public event Action<Coord, int> OnMouseClicked;
        public event Action<Coord, int> OnDraggingMoved;

        private bool isDragging;
        private int lastBtn;

        private void Reset()
        {
            eventSys = FindObjectOfType<EventSystem>();
            grid = GetComponentInParent<BaseGrid>();
        }

        private void Start()
        {
            mouseHoverCoord = grid.MousePositionToCoord();
            mouseHoverCoord.isValid = true;
        }

        private void Update()
        {
            //if (eventSys != null && eventSys.IsPointerOverGameObject())
            //{
            //    mouseHoverCoord.isValid = false;
            //    OnMouseHoverCoordChanged?.Invoke(mouseHoverCoord);
            //    return;
            //}

            if (MouseController.IsPointerOverUI)
            {
                isDragging = false;
                return;
            }

            MouseOverCheck();

            if (Input.GetMouseButtonDown(0))
            {
                OnMouseClicked?.Invoke(mouseHoverCoord, 0);
                isDragging = true;
                lastBtn = 0;

            }
            else if (Input.GetMouseButtonDown(1))
            {
                OnMouseClicked?.Invoke(mouseHoverCoord, 1);
                isDragging = true;
                lastBtn = 1;
            }
            else if (Input.GetMouseButtonDown(2))
            {
                OnMouseClicked?.Invoke(mouseHoverCoord, 2);
                isDragging = true;
                lastBtn = 2;
            }
            if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(2))
            {
                isDragging = false;
            }
        }

        private void MouseOverCheck()
        {
            Coord coord = grid.MousePositionToCoord();
            
            if (mouseHoverCoord == coord)
            {
                return;
            }
            if (showDebugs)
            {
                Debug.Log("Current Position: " + coord.ToString());
            }
            
            mouseHoverCoord.isValid = grid.DoesTileExist(coord);
            mouseHoverCoord.Set(coord);
            OnMouseHoverCoordChanged?.Invoke(mouseHoverCoord);
            if (isDragging)
            {
                OnDraggingMoved?.Invoke(mouseHoverCoord, lastBtn);
            }
        }
        public Vector3 MouseHoverToWorldPoint()
        {
            return grid.CoordToWorldPoint(mouseHoverCoord);
        }
    }
}