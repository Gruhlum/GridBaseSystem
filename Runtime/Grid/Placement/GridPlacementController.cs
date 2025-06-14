using HexTecGames.Basics;
using HexTecGames.HotkeySystem;
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

        [Header("Placement")]
        [SerializeField] private TileHighlightSpawner highlightSpawner = default;
        //[SerializeField] private Color invalidLocationCol = Color.red;
        //[SerializeField] private Color validLocationCol = Color.green;

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
        public event Action<GridObjectBase> OnObjectPlaced;
        public event Action<PreBuildInfo> OnBeforeBuild;

        private int currentRotation;

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



        protected virtual void Reset()
        {
            grid = transform.GetComponentInParent<BaseGrid>();
            ghost = transform.GetComponentInChildren<PlacementGhost>();
            //mouseController = FindObjectOfType<MouseController>();
            if (grid != null)
            {
                gridEventSystem = grid.transform.GetComponentInChildren<GridEventSystem>();
            }
            if (highlightSpawner == null)
            {
                highlightSpawner = new TileHighlightSpawner();
            }
            highlightSpawner.Grid = grid;
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
            ghost.Show(!MouseController.IsPointerOverUI);

            if (MouseController.IsPointerOverUI)
            {
                return;
            }

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

                CheckForValidTile(gridEventSystem.MouseCoord);
            }
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
                CheckForValidTile(coord);
            }
            if (gridEventSystem.IsDragging)
            {
                if (gridEventSystem.LastMouseButton == 0)
                {
                    Build(coord);
                }

                else if (AllowRemoval && gridEventSystem.LastMouseButton == 1)
                {
                    RemoveNext(coord);
                }
            }
        }

        //private void HandleMouseInput(int btn, ButtonType btnType)
        //{
        //    //Left Down -> Try Build
        //    //Right Down -> Try clear selected -> Try Remove
        //    //Any Down -> Try Dragging
        //    //Any Up -> Stop Dragging

        //    
        //}

        private void RemoveNext(Coord coord)
        {
            var placementDatas = grid.GetTileObject(coord);
            if (placementDatas == null || placementDatas.Count <= 0)
            {
                if (grid.DoesTileExist(coord))
                {
                    grid.RemoveTile(coord);
                }
            }
            else
            {
                TileObject tileObj = placementDatas[0].tileObject;
                tileObj.Remove();
            }
        }
        protected void CheckForValidTile(Coord coord)
        {
            if (SelectedPlacementData != null)
            {
                ghost.Activate(coord);
            }
        }

        protected bool IsValidCoord(Coord coord)
        {
            return SelectedPlacementData.Data.IsValidCoord(grid, coord, currentRotation);
        }
        public virtual void Build(Coord coord)
        {
            if (SelectedPlacementData == null)
            {
                return;
            }

            if (!IsValidCoord(coord))
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
            ghost.Deactivate();
            StartCoroutine(BuildDelayed(coord));
        }

        private IEnumerator BuildDelayed(Coord coord)
        {
            yield return null;
            GridObjectBase tileObject = GenerateObject(coord);
            //Debug.Log("Placing Object: " + tileObject.Name + " at: " + coord.ToString());
            OnObjectPlaced?.Invoke(tileObject);
            //ghost.UpdatePlacementArea();
        }
        public void ClearSelectedPlacementData()
        {
            SelectedPlacementData = null;
            ghost.Deactivate();
        }

        public void SetSelectedObject(PlacementData data)
        {
            if (!gameObject.activeSelf)
            {
                return;
            }
            if (data == null)
            {
                ClearSelectedPlacementData();
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

        protected GridObjectBase GenerateObject(Coord coord)
        {
            if (SelectedPlacementData.Data is TileObjectData tileObjData)
            {
                TileObject tileObj = tileObjData.CreateObject(grid, coord, currentRotation);
                grid.AddTileObject(tileObj);
                return tileObj;
            }
            else if (SelectedPlacementData.Data is TileData tileData)
            {
                Tile tile = tileData.CreateObject(grid, coord);
                grid.AddTile(tile);
                return tile;
            }
            return null;
        }
    }
}