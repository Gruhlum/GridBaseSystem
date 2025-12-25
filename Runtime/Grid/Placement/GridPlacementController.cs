using System;
using System.Collections;
using HexTecGames.Basics;
using HexTecGames.SoundSystem;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public class GridPlacementController : AdvancedBehaviour
    {
        [SerializeField] protected BaseGrid grid = default;
        [SerializeField] protected GridEventSystem gridEventSystem = default;
        [SerializeField] protected PlacementGhost ghost = default;
        //[SerializeField] private MouseController mouseController = default;

        //[Header("Settings")]
        //[SerializeField] private bool dropSelectedAfterBuild = default;
        //[SerializeField][DrawIf("dropSelectedAfterBuild", false)] private bool allowBuildHoldingDown = default;
        //[SerializeField] private bool allowRemoving = default;
        //[SerializeField][DrawIf("allowRemoving", true)] private bool allowRemoveHoldingDown = default;
        //[SerializeField][DrawIf("allowRemoving", true)] private bool allowOverwriting = default;

        [SerializeField] private SoundClipBase errorSound = default;

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
                OnSelectedObjectChanged?.Invoke(selectedPlacementData);
            }
        }
        private PlacementData selectedPlacementData;

        public Coord HoverCoord
        {
            get
            {
                return gridEventSystem.MouseCoord;
            }
        }

        public event Action<PlacementData> OnSelectedObjectChanged;
        public event Action<GridObject> OnObjectPlaced;
        public event Action<PreBuildInfo> OnBeforeBuild;


        private MultiSpawner spawner = new MultiSpawner();

        private int currentRotation;
        private int currentRemovalIndex;
        public bool AllowRemoval
        {
            get
            {
                return allowRemoval;
            }
            set
            {
                allowRemoval = value;
            }
        }
        [SerializeField] private bool allowRemoval = default;


        protected override void Reset()
        {
            base.Reset();
            grid = transform.GetComponentInParent<BaseGrid>();
            ghost = transform.GetComponentInChildren<PlacementGhost>();

            if (grid != null)
            {
                gridEventSystem = grid.transform.GetComponentInChildren<GridEventSystem>();
            }
        }

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
        protected virtual void Update()
        {
            ghost.Show(!MouseController.PointerOverUI);

            if (MouseController.PointerOverUI)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                if (Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift))
                {
                    RotateSelectedObject(-1);
                }
                else RotateSelectedObject(1);
            }
        }

        private void RotateSelectedObject(int change)
        {
            if (SelectedPlacementData == null)
            {
                return;
            }
            if (selectedPlacementData.Data.TotalRotations <= 1)
            {
                return;
            }
            currentRotation += change;

            currentRotation %= selectedPlacementData.Data.TotalRotations;
            ghost.UpdatePlacementArea(gridEventSystem.MouseCoord, currentRotation);
        }

        private void GridEventSystem_OnMouseClicked(Coord coord, int btn)
        {
            if (btn == 0)
            {
                Build(HoverCoord);
            }
            else if (btn == 1)
            {
                if (SelectedPlacementData != null)
                {
                    ClearSelectedPlacementData();
                }
                else if (AllowRemoval)
                {
                    RemoveNext(HoverCoord);
                }
            }
        }
        private void GridEventSystem_OnMouseHoverCoordChanged(Coord coord)
        {
            if (SelectedPlacementData != null)
            {
                ghost.UpdatePlacementArea(coord, currentRotation);
            }
            if (gridEventSystem.IsDragging)
            {
                if (SelectedPlacementData != null && SelectedPlacementData.IsDraggable && gridEventSystem.LastMouseButton == 0)
                {
                    Build(coord);
                }
                else if (AllowRemoval && gridEventSystem.LastMouseButton == 1)
                {
                    RemoveNext(currentRemovalIndex, coord);
                }
            }
        }

        private void RemoveNext(int layer, Coord coord)
        {
            GridObject result = grid.GetGridObject(layer, coord);
            if (result != null)
            {
                result.Remove();
            }
        }
        private void RemoveNext(Coord coord)
        {
            GridObject result = grid.GetGridObject(coord);
            if (result != null)
            {
                currentRemovalIndex = result.Layer;
                result.Remove();
            }
        }

        public virtual void Build(Coord coord)
        {
            if (SelectedPlacementData == null)
            {
                return;
            }

            if (!SelectedPlacementData.Data.IsValidPlacement(grid, coord, currentRotation))
            {
                errorSound?.Play();
                return;
            }

            PreBuildInfo info = new PreBuildInfo();
            OnBeforeBuild?.Invoke(info);
            if (info.IsBlocked)
            {
                errorSound?.Play();
                info.PrintBlockReasons();
                return;
            }


            if (SelectedPlacementData.PlacementSound != null)
            {
                SelectedPlacementData.PlacementSound.Play();
            }

            GridObject tileObject = CreateGridObject(coord, currentRotation);
            OnObjectPlaced?.Invoke(tileObject);
        }
        public void ClearSelectedPlacementData()
        {
            SelectedPlacementData = null;
            ghost.Deactivate();
        }

        public void SetSelectedObject(PlacementData placementData)
        {
            if (!gameObject.activeSelf)
            {
                return;
            }
            if (placementData == null)
            {
                ClearSelectedPlacementData();
                return;
            }

            SelectedPlacementData = placementData;
            GridObjectVisual visual = SpawnVisual(placementData);
            ghost.Activate(placementData, visual, HoverCoord);
            ResetRotation();
        }

        private GridObjectVisual SpawnVisual(PlacementData placementData)
        {
            var visual = spawner.Spawn(placementData.Data.GetVisualPrefab());
            visual.SetData(placementData.Data);
            return visual;
        }

        private void ResetRotation()
        {
            currentRotation = 0;
            ghost.UpdatePlacementArea(gridEventSystem.MouseCoord, currentRotation);
        }

        protected GridObject CreateGridObject(Coord coord, int rotation)
        {
            if (SelectedPlacementData.Data is IGridObjectCreator creator)
            {
                GridObject tileObj = creator.CreateGridObject(grid, coord, rotation);
                return tileObj;
            }
            else
            {
                Debug.Log($"{SelectedPlacementData.Data} needs to inherit from {nameof(IGridObjectCreator)}!");

                return null;
            }
        }
    }
}