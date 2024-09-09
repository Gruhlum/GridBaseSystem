using HexTecGames.Basics;
using HexTecGames.SoundSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public abstract class GridPlacementController : MonoBehaviour
    {
        [SerializeField] protected BaseGrid grid = default;
        [SerializeField] protected GridEventSystem gridEventSystem = default;
        [SerializeField] protected PlacementGhost ghost = default;

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

        public abstract GridObjectData SelectedObject
        {
            get;
        }

        public Coord HoverCoord
        {
            get
            {
                return gridEventSystem.MouseHoverCoord;
            }
        }

        public event Action<GridObjectData> OnSelectedObjectChanged;
        public event Action<GridObject> OnObjectPlaced;
        public event Action<PreBuildInfo> OnBeforeBuild;

        private bool isDragging;
        private int lastMouseBtn;

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
                else if (AllowRemoval)
                {
                    grid.RemoveGridObject(coord);
                }
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
        protected void CheckForValidTile(Coord coord)
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

        protected abstract bool IsValidCoord(Coord coord);
        public virtual void Build(Coord coord)
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
           

            if (SelectedObject.PlacementSound != null)
            {
                SelectedObject.PlacementSound.Play();
            }
            StartCoroutine(BuildDelayed(coord));
        }

        protected abstract GridObject GenerateObject(Coord coord);

        private IEnumerator BuildDelayed(Coord coord)
        {
            yield return null;
            GridObject tileObject = GenerateObject(coord);
            OnObjectPlaced?.Invoke(tileObject);
            //ghost.UpdatePlacementArea();
        }
        protected abstract void ClearSelected();
        public void ClearSelectedObject()
        {
            ClearSelected();
            isDragging = false;
            ghost.Deactivate();
        }
    }
}