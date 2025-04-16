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

        public Coord MouseCoord
        {
            get
            {
                return mouseCoord;
            }
        }
        private Coord mouseCoord;

        public Coord LastMouseCoord
        {
            get
            {
                return lastMouseCoord;
            }
            private set
            {
                lastMouseCoord = value;
            }
        }
        private Coord lastMouseCoord;


        [SerializeField] private bool showDebugs = default;
        [Tooltip("Should overstepped tiles be detected")]
        [SerializeField] private bool continousDetection = true;

        public event Action<Coord> OnMouseHoverCoordChanged;
        public event Action<Coord, int> OnMouseClicked;
        public event Action<Coord, int> OnDraggingMoved;

        private bool isDragging;
        private int lastBtn;
        private int dragDirection = -1;

        private void Reset()
        {
            grid = GetComponentInParent<BaseGrid>();
        }

        private void Start()
        {
            mouseCoord = grid.MousePositionToCoord();
            mouseCoord.isValid = true;
        }

        private void DetectDirection()
        {
            dragDirection = grid.GetDirection(MouseCoord, lastMouseCoord);
        }

        private void Update()
        {
            if (MouseController.IsPointerOverUI)
            {
                isDragging = false;
                return;
            }

            MouseOverCheck();

            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                dragDirection = -1;
            }

            if (Input.GetMouseButtonDown(0))
            {
                OnMouseClicked?.Invoke(mouseCoord, 0);
                isDragging = true;
                lastBtn = 0;

            }
            else if (Input.GetMouseButtonDown(1))
            {
                OnMouseClicked?.Invoke(mouseCoord, 1);
                isDragging = true;
                lastBtn = 1;
            }
            else if (Input.GetMouseButtonDown(2))
            {
                OnMouseClicked?.Invoke(mouseCoord, 2);
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

            if (dragDirection >= 0)
            {
                coord = grid.GetClosestCoordInLine(MouseCoord, coord, dragDirection);
            }


            if (mouseCoord == coord)
            {
                return;
            }

            if (continousDetection)
            {
                if (grid.GetDistance(coord, mouseCoord) > 1)
                {
                    List<Coord> results = grid.GetLine(coord, mouseCoord);
                    foreach (var result in results)
                    {
                        SetCurrentMouseCoord(result);
                    }
                }
            }

            if (showDebugs)
            {
                Debug.Log("Current Position: " + coord.ToString());
            }

            mouseCoord.isValid = grid.DoesTileExist(coord);
            SetCurrentMouseCoord(coord);
        }

        private void SetCurrentMouseCoord(Coord coord)
        {
            LastMouseCoord = mouseCoord;
            mouseCoord.Set(coord);
            OnMouseHoverCoordChanged?.Invoke(mouseCoord);
            if (isDragging)
            {
                OnDraggingMoved?.Invoke(mouseCoord, lastBtn);
                if (dragDirection < 0 && Input.GetKey(KeyCode.LeftControl))
                {
                    DetectDirection();
                }
            }
        }

        public Vector3 MouseHoverToWorldPoint()
        {
            return grid.CoordToWorldPoint(mouseCoord);
        }
    }
}