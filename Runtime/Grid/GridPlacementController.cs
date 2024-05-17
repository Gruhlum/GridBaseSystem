using HexTecGames.Basics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public class GridPlacementController : MonoBehaviour
    {
        [SerializeField] private BaseGrid grid = default;
        [SerializeField] private GhostObject ghost = default;
        [SerializeField] private GridEventSystem gridEventSystem = default;
        [SerializeField] private ResourceController resourceC = default;

        //[Header("Settings")]
        //[SerializeField] private bool dropSelectedAfterBuild = default;
        //[SerializeField][DrawIf("dropSelectedAfterBuild", false)] private bool allowBuildHoldingDown = default;
        //[SerializeField] private bool allowRemoving = default;
        //[SerializeField][DrawIf("allowRemoving", true)] private bool allowRemoveHoldingDown = default;
        //[SerializeField][DrawIf("allowRemoving", true)] private bool allowOverwriting = default;

        [Header("Placement")]
        [SerializeField] private TileHighlightSpawner highlightSpawner = default;
        //[SerializeField] private Color invalidLocationCol = Color.red;
        //[SerializeField] private Color validLocationCol = Color.green;


        public GridObjectData SelectedObject
        {
            get
            {
                return selectedObject;
            }
            private set
            {
                if (selectedObject == value)
                {
                    return;
                }
                selectedObject = value;
                OnSelectedObjectChanged?.Invoke(selectedObject);
            }
        }
        private GridObjectData selectedObject;

        public event Action<GridObjectData> OnSelectedObjectChanged;
        public event Action<GridObject> OnObjectPlaced;

        private bool isDragging;
        private int lastMouseBtn;

        private void OnEnable()
        {
            gridEventSystem.OnMouseHoverCoordChanged += GridEventSystem_OnMouseHoverCoordChanged;
            gridEventSystem.OnMouseClicked += GridEventSystem_OnMouseClicked;
        }
        private void OnDisable()
        {
            gridEventSystem.OnMouseHoverCoordChanged -= GridEventSystem_OnMouseHoverCoordChanged;
            gridEventSystem.OnMouseClicked -= GridEventSystem_OnMouseClicked;
        }
        private void Update()
        {
            if (isDragging && Input.GetMouseButtonUp(lastMouseBtn))
            {
                isDragging = false;
                lastMouseBtn = -1;
            }
        }
        private void GridEventSystem_OnMouseClicked(Coord coord, int btn)
        {
            lastMouseBtn = btn;
            if (btn == 0)
            {
                Build(coord);
            }
            else if (btn == 1)
            {
                if (SelectedObject != null)
                {
                    ClearSelectedObject();
                }
                else grid.RemoveGridObject(coord);
            }
            if (SelectedObject != null)
            {
                if (SelectedObject.IsDraggable)
                {
                    isDragging = true;
                }
                else isDragging = false;
            }
            else isDragging = true;
        }

        private void GridEventSystem_OnMouseHoverCoordChanged(Coord coord)
        {
            if (SelectedObject != null)
            {
                if (!SelectedObject.IsValidCoord(coord, grid))
                {
                    ghost.Deactivate();
                    return;
                }
                else ghost.Activate();

                ghost.SetPosition(grid.CoordToWorldPoint(coord));

                if (isDragging && lastMouseBtn == 0)
                {
                    Build(coord);
                }
            }
            else
            {
                if (isDragging && lastMouseBtn == 1)
                {
                    if (SelectedObject != null)
                    {
                        ClearSelectedObject();
                    }
                    else grid.RemoveGridObject(coord);
                }
            }
        }

        public void Build(Coord coord)
        {
            if (SelectedObject == null)
            {
                return;
            }
            Tile tile = grid.GetTile(coord);
            if (tile != null && SelectedObject == grid.GetTile(coord).Data)
            {
                return;
            }
            if (!SelectedObject.IsValidCoord(coord, grid))
            {
                return;
            }
            if (SelectedObject is ICost cost)
            {
                if (!cost.IsAffordable(resourceC.GetResources()))
                {
                    return;
                }
                else cost.SubtractResources(resourceC.GetResources());
            }
            GridObject tileObject = SelectedObject.CreateObject(coord, grid);
            if (SelectedObject.PlacementSound != null)
            {
                SelectedObject.PlacementSound.Play();
            }
            grid.AddGridObject(tileObject);
            OnObjectPlaced?.Invoke(tileObject);
        }
        public void ClearSelectedObject()
        {
            SelectedObject = null;
            isDragging = false;
            ghost.Deactivate();
        }
        public void SetSelectedObject(GridObjectData data)
        {
            SelectedObject = data;
            ghost.Activate(gridEventSystem.MouseHoverToWorldPoint(), data.Sprite);
        }
    }
}