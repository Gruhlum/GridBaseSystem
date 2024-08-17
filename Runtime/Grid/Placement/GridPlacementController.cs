using HexTecGames.Basics;
using HexTecGames.SoundSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public class GridPlacementController : MonoBehaviour
    {
        [SerializeField] private BaseGrid grid = default;
        [SerializeField] private PlacementGhost ghost = default;
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

        [SerializeField] private SoundClipBase errorSound = default;
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

        public Coord HoverCoord
        {
            get
            {
                return gridEventSystem.MouseHoverCoord;
            }
        }

        public event Action<GridObjectData> OnSelectedObjectChanged;
        public event Action<GridObject> OnObjectPlaced;

        private bool isDragging;
        private int lastMouseBtn;

        private int currentRotation;

        [SerializeField] private bool allowRemoval = default;

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
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (SelectedObject == null)
                {
                    return;
                }
                if (Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift))
                {
                    currentRotation--;
                }
                else currentRotation++;

                if (currentRotation >= SelectedObject.totalRotations)
                {
                    currentRotation = 0;
                }
                else if (currentRotation < 0)
                {
                    currentRotation = SelectedObject.totalRotations - 1;
                }
                ghost.Rotate(currentRotation);
                CheckForValidTile(gridEventSystem.MouseHoverCoord);
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
                else if (allowRemoval) grid.RemoveGridObject(coord);
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
        private void CheckForValidTile(Coord coord)
        {
            if (SelectedObject != null)
            {
                ghost.Activate(coord);

                if (isDragging && lastMouseBtn == 0)
                {
                    Build(coord);
                }
            }
        }
        private void GridEventSystem_OnMouseHoverCoordChanged(Coord coord)
        {
            if (SelectedObject != null)
            {
                CheckForValidTile(coord);
            }
            else
            {
                if (isDragging && lastMouseBtn == 1)
                {
                    if (SelectedObject != null)
                    {
                        ClearSelectedObject();
                    }
                    else if (allowRemoval) grid.RemoveGridObject(coord);
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
            if (!SelectedObject.IsValidPlacement(coord, grid, currentRotation))
            {
                errorSound?.Play();
                return;
            }
            if (SelectedObject is ICost cost)
            {
                if (!cost.IsAffordable(resourceC.GetResources()))
                {
                    errorSound?.Play();
                    return;
                }
                else cost.SubtractResources(resourceC.GetResources());
            }
            GridObject tileObject = SelectedObject.CreateObject(coord, grid);
            tileObject.Rotation = currentRotation;
            if (SelectedObject.PlacementSound != null)
            {
                SelectedObject.PlacementSound.Play();
            }
            grid.AddGridObject(tileObject);
            OnObjectPlaced?.Invoke(tileObject);
            ghost.UpdatePlacementArea();
        }
        public void ClearSelectedObject()
        {
            SelectedObject = null;
            isDragging = false;
            ghost.Deactivate();
        }
        public void SetSelectedObject(GridObjectData data)
        {
            ResetRotation();
            SelectedObject = data;
            ghost.Activate(data, HoverCoord);
        }

        private void ResetRotation()
        {
            currentRotation = 0;
            ghost.Rotate(0);
        }
    }
}