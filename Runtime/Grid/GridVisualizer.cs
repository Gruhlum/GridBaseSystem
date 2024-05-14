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
        [SerializeField] protected Spawner<TileDisplay> displaySpawner = default;

        public event Action<TileDisplay> OnCoordDisplaySpawned;


        void Reset()
        {
            if (transform.parent != null)
            {
                grid = GetComponentInParent<BaseGrid>();
            }
            if (displaySpawner == null)
            {
                displaySpawner = new Spawner<TileDisplay>();
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
            TileDisplay display = displaySpawner.Spawn();
            SetupCoordDisplay(display, tile);
            OnCoordDisplaySpawned?.Invoke(display);
        }
        protected virtual void SetupCoordDisplay(TileDisplay display, Tile tile)
        {
            display.Setup(tile);
        }
        private void Grid_OnTileRemoved(Tile tile)
        {
            SpawnTileDisplay(tile);
        }

        private void Grid_OnTileAdded(Tile tile)
        {
            var results = displaySpawner.GetActiveBehaviours();
            foreach (var result in results)
            {
                if (result.Tile == tile)
                {
                    result.gameObject.SetActive(false);
                    return;
                }
            }
        }
        private void Grid_OnGridGenerated()
        {
            if (grid.Coordinates == null)
            {
                return;
            }
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