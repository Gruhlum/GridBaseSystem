using System.Collections.Generic;
using System.Linq;
using HexTecGames.Basics;
using HexTecGames.GridBaseSystem;
using UnityEngine;

namespace HexTecGames.UI
{
    public class GridCoordinatesController : MonoBehaviour
    {
        [SerializeField] private Spawner<CoordDisplay> coordSpawner = default;

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
                grid.OnGridObjectAdded += Grid_OnTileAdded;
            }
        }
        private void RemoveEvents(BaseGrid grid)
        {
            if (grid != null)
            {
                grid.OnGridObjectAdded -= Grid_OnTileAdded;
            }
        }

        private void Grid_OnTileAdded(GridObject tile)
        {
            if (!IsActive)
            {
                return;
            }

            coordSpawner.Spawn().Setup(tile.Center, tile.GetWorldPosition());
        }

        public void ToggleCoordinates()
        {
            SetCoordinates(!IsActive);
        }
        public void SetCoordinates(bool active)
        {
            IsActive = active;
            Debug.Log(IsActive);
            if (IsActive)
            {
                List<GridObject> results = Grid.GetAllGridObjects();
                List<CoordDisplay> displays = coordSpawner.ReuseOrSpawn(results.Count(), false);

                for (int i = 0; i < displays.Count; i++)
                {
                    displays[i].Setup(results[i].Center, grid.CoordToWorldPosition(results[i].Center));
                }
                foreach (CoordDisplay display in displays)
                {
                    display.gameObject.SetActive(true);
                }
            }
            else coordSpawner.DeactivateAll();
        }
    }
}