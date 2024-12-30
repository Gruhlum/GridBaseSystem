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

        [SerializeField] private TileVisual defaultVisual = default;
        [SerializeField] private MultiSpawner spawner = default;

        public event Action<TileVisual> OnCoordDisplaySpawned;


        private void Reset()
        {
            if (transform.parent != null)
            {
                grid = GetComponentInParent<BaseGrid>();
            }
            if (spawner != null)
            {
                spawner.Parent = transform;
            }
           
        }

        private void OnEnable()
        {
            if (grid != null)
            {
                grid.OnTileAdded += Grid_OnTileAdded;
                grid.OnGridGenerated += Grid_OnGridGenerated;
            }
        }

        private void OnDisable()
        {
            if (grid != null)
            {
                grid.OnTileAdded -= Grid_OnTileAdded;
                grid.OnGridGenerated -= Grid_OnGridGenerated;
            }
        }
        protected void SpawnTileDisplay(Tile tile)
        {
            TileVisual display;
            if (tile.Data.VisualPrefab != null)
            {
                display = spawner.Spawn(tile.Data.VisualPrefab);
            }
            else display = spawner.Spawn(defaultVisual);

            SetupCoordDisplay(display, tile);
            OnCoordDisplaySpawned?.Invoke(display);
        }
        protected virtual void SetupCoordDisplay(TileVisual display, Tile tile)
        {
            display.Setup(tile);
        }

        private void Grid_OnTileAdded(Tile tile)
        {
            SpawnTileDisplay(tile);
            
        }
        private void Grid_OnGridGenerated()
        {
            SpawnTileDisplays(grid.GetTiles());
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