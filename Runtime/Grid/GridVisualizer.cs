using HexTecGames.Basics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public class TileVisualizer : MonoBehaviour
    {
        [SerializeField] protected BaseGrid grid = default;
        [SerializeField] protected Spawner<TileVisual> displaySpawner = default;

        public event Action<TileVisual> OnCoordDisplaySpawned;


        void Reset()
        {
            if (transform.parent != null)
            {
                grid = GetComponentInParent<BaseGrid>();
            }
            if (displaySpawner == null)
            {
                displaySpawner = new Spawner<TileVisual>();
            }
            displaySpawner.Parent = transform;
        }

        private void OnEnable()
        {
            if (grid != null)
            {
                grid.OnTileAdded += Grid_OnTileAdded;
                grid.OnTileRemoved += Grid_OnTileRemoved;
                grid.OnGridGenerated += Grid_OnGridGenerated;
            }
        }

        private void OnDisable()
        {
            if (grid != null)
            {
                grid.OnTileAdded -= Grid_OnTileAdded;
                grid.OnTileRemoved -= Grid_OnTileRemoved;
                grid.OnGridGenerated -= Grid_OnGridGenerated;
            }
        }
        protected void SpawnTileDisplay(Tile tile)
        {
            TileVisual display = displaySpawner.Spawn();
            SetupCoordDisplay(display, tile);
            OnCoordDisplaySpawned?.Invoke(display);
        }
        protected virtual void SetupCoordDisplay(TileVisual display, Tile tile)
        {
            display.Setup(tile);
        }
        private void Grid_OnTileRemoved(Tile tile)
        {
            var results = displaySpawner.GetActiveInstances();
            foreach (var result in results)
            {
                if (result.Tile == tile)
                {
                    result.gameObject.SetActive(false);
                    return;
                }
            }
            Debug.Log(tile.ToString());
        }

        private void Grid_OnTileAdded(Tile tile)
        {
            SpawnTileDisplay(tile);
            
        }
        private void Grid_OnGridGenerated()
        {
            SpawnTileDisplays(grid.GetAllTiles());
        }
        protected virtual void SpawnTileDisplays(List<Tile> tiles) 
        {
            foreach (var tile in tiles)
            {
                SpawnTileDisplay(tile);
            }
        }
    }
}