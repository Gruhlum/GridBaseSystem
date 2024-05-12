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


        public BaseTileObjectData SelectedObject
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
        private BaseTileObjectData selectedObject;

        public event Action<BaseTileObjectData> OnSelectedObjectChanged;
        public event Action<TileObject> OnObjectPlaced;

        private bool isDragging;

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
            if (isDragging && Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }
        }
        private void GridEventSystem_OnMouseClicked(Coord coord, int btn)
        {
            if (btn == 0)
            {
                Build(coord);
            }
            else if (btn == 1)
            {
                ClearSelectedObject();
            }
            if (SelectedObject != null)
            {
                if (SelectedObject.IsDraggable)
                {
                    isDragging = true;
                }
                else isDragging = false;
            }
        }

        private void GridEventSystem_OnMouseHoverCoordChanged(Coord coord)
        {
            if (SelectedObject != null)
            {
                if (coord.isValid == false)
                {
                    ghost.Deactivate();
                    return;
                }
                else ghost.Activate();

                ghost.SetPosition(grid.CoordToWorldPoint(coord));

                if (isDragging)
                {
                    Build(coord);
                }
            }
        }

        public void Build(Coord coord)
        {
            if (SelectedObject == null)
            {
                return;
            }
            if (!grid.IsTileEmpty(coord))
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
            TileObject tileObject = SelectedObject.CreateTileObject(coord, grid);
            grid.AddTileObject(tileObject);
            OnObjectPlaced?.Invoke(tileObject);
        }
        public void ClearSelectedObject()
        {
            SelectedObject = null;
            isDragging = false;
            ghost.Deactivate();
        }
        public void SetSelectedObject(BaseTileObjectData data)
        {
            SelectedObject = data;
            ghost.Activate(gridEventSystem.MouseHoverToWorldPoint(), data.GetSprite());           
        }
    }
}