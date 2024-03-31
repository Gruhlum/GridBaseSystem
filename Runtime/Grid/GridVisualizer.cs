using HexTecGames.Basics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public class GridVisualizer : MonoBehaviour
    {
        [SerializeField] protected BaseGrid grid = default;
        [SerializeField] protected Spawner<CoordDisplay> displaySpawner = default;

        public event Action<CoordDisplay> OnCoordDisplaySpawned;



        protected void SpawnCoordDisplay(Coord coord)
        {
            CoordDisplay display = displaySpawner.Spawn();
            SetupCoordDisplay(display, coord);
            OnCoordDisplaySpawned?.Invoke(display);
        }
        protected virtual void SetupCoordDisplay(CoordDisplay display, Coord coord)
        {
            display.Setup(coord, grid.CoordToWorldPoint(coord));
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

        private void Grid_OnTileRemoved(Coord coord)
        {
            SpawnCoordDisplay(coord);
        }

        private void Grid_OnTileAdded(Coord coord)
        {
            var results = displaySpawner.GetActiveBehaviours();
            foreach (var result in results)
            {
                if (result.Coord == coord)
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
            SpawnCoordDisplays(grid.Coordinates);
        }
        protected virtual void SpawnCoordDisplays(Coord[,] list)
        {
            foreach (var coord in list)
            {
                SpawnCoordDisplay(coord);
            }
        }
    }
}