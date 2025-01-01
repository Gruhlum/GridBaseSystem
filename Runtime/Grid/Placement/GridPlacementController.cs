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
        [SerializeField] protected BaseGrid grid = default;
        [SerializeField] protected GridEventSystem gridEventSystem = default;
        [SerializeField] protected PlacementGhost ghost = default;
        [SerializeField] private MouseController mouseController = default;

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
                return gridEventSystem.MouseHoverCoord;
            }
        }

        public event Action<PlacementData> OnSelectedObjectChanged;
        public event Action<GridObject> OnObjectPlaced;
        public event Action<PreBuildInfo> OnBeforeBuild;

        private bool isDragging;
        private int currentRotation;
        private MouseInputData mouseInput = new MouseInputData();

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
            mouseController = FindObjectOfType<MouseController>();
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
        }
        private void OnDisable()
        {
            gridEventSystem.OnMouseHoverCoordChanged -= GridEventSystem_OnMouseHoverCoordChanged;
        }

        private void HandleMouseInput(int btn, ButtonType btnType)
        {
            //Left Down -> Try Build
            //Right Down -> Try clear selected -> Try Remove
            //Any Down -> Try Dragging
            //Any Up -> Stop Dragging

            if (btnType == ButtonType.Up)
            {
                isDragging = false;
            }
            if (btnType == ButtonType.Down)
            {
                isDragging = true;
                if (btn == 0)
                {
                    Build(HoverCoord);
                }
                else if (btn == 1)
                {
                    if (!AllowRemoval)
                    {
                        return;
                    }
                    if (SelectedPlacementData != null)
                    {
                        ClearSelectedObject();
                    }
                    else
                    {
                        RemoveNext(HoverCoord);
                    }
                }
            }
        }

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

        protected virtual void Update()
        {
            ghost.Show(!MouseController.IsPointerOverUI);

            if (MouseController.IsPointerOverUI)
            {
                return;
            }

            if (mouseInput.DetectMouseInput())
            {
                HandleMouseInput(mouseInput.button, mouseInput.type);
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

                CheckForValidTile(gridEventSystem.MouseHoverCoord);
            }
        }

        private void GridEventSystem_OnMouseHoverCoordChanged(Coord coord)
        {
            if (SelectedPlacementData != null)
            {
                CheckForValidTile(coord);
            }
            if (isDragging)
            {
                if (mouseInput.button == 0)
                {
                    Build(coord);
                }
                else if (AllowRemoval && mouseInput.button == 1)
                {
                    RemoveNext(coord);
                }
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
            GridObject tileObject = GenerateObject(coord);
            //Debug.Log("Placing Object: " + tileObject.Name + " at: " + coord.ToString());
            OnObjectPlaced?.Invoke(tileObject);
            //ghost.UpdatePlacementArea();
        }
        public void ClearSelectedObject()
        {
            SelectedPlacementData = null;
            isDragging = false;
            ghost.Deactivate();
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

        protected GridObject GenerateObject(Coord coord)
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