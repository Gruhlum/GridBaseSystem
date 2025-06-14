using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using HexTecGames.GridBaseSystem;
using UnityEngine;

namespace HexTecGames.UI
{
    public class GridCoordinatesController : MonoBehaviour
    {
        [SerializeField] private Spawner<GridCoordDisplay> coordSpawner = default;

        public BaseGrid Grid
        {
            get
            {
                return this.grid;
            }
            private set
            {
                this.grid = value;
            }
        }
        [SerializeField] private BaseGrid grid = default;
        public bool IsActive
        {
            get
            {
                return isActive;
            }
            private set
            {
                isActive = value;
            }
        }



        [SerializeField] private bool isActive;

        [SerializeField] private KeyCode toggleKeyCode = KeyCode.LeftAlt;


        private void Update()
        {
            if (toggleKeyCode != KeyCode.None && Input.GetKeyDown(toggleKeyCode))
            {
                ToggleCoordinates();
            }
        }

        private void Start()
        {
            AddEvents(Grid);

            if (IsActive)
            {
                SetCoordinates(true);
            }
        }

        public void SetGrid(BaseGrid grid)
        {
            RemoveEvents(this.Grid);
            this.Grid = grid;
            AddEvents(this.Grid);

            if (IsActive)
            {
                SetCoordinates(true);
            }
        }

        private void AddEvents(BaseGrid grid)
        {
            if (grid != null)
            {
                grid.OnTileAdded += Grid_OnTileAdded;
            }
        }
        private void RemoveEvents(BaseGrid grid)
        {
            if (grid != null)
            {
                grid.OnTileAdded -= Grid_OnTileAdded;
            }
        }    

        private void Grid_OnTileAdded(Tile tile)
        {
            if (!IsActive)
            {
                return;
            }

            coordSpawner.Spawn().SetItem(tile);
        }

        public void ToggleCoordinates()
        {
            SetCoordinates(!IsActive);
        }
        public void SetCoordinates(bool active)
        {
            IsActive = active;
            coordSpawner.DeactivateAll();
            if (IsActive)
            {
                var results = Grid.GetAllTiles();
                List<GridCoordDisplay> displays = new List<GridCoordDisplay>();
                foreach (var result in results)
                {
                    GridCoordDisplay display = coordSpawner.Spawn();
                    displays.Add(display);
                    display.SetItem(result, false);
                }
                foreach (var display in displays)
                {
                    display.gameObject.SetActive(true);
                }
            }
        }
    }
}