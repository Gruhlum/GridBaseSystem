using HexTecGames.Basics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
	public class TileObjectVisualizer : MonoBehaviour
	{
        [SerializeField] private Spawner<TileObjectDisplay> srSpawner = default;
        [SerializeField] private BaseGrid grid = default;

        private List<TileObjectDisplay> activeDisplays = new List<TileObjectDisplay>();


        void Reset()
        {
            grid = GetComponentInParent<BaseGrid>();
            if (srSpawner == null)
            {
                srSpawner = new Spawner<TileObjectDisplay>();
            }
            srSpawner.Parent = transform;
        }
        private void OnEnable()
        {
            grid.OnTileObjectAdded += Grid_OnTileObjectAdded;
            grid.OnTileObjectRemoved += Grid_OnTileObjectRemoved;
            grid.OnTileObjectMoved += Grid_OnTileObjectMoved;
        }
        private void OnDisable()
        {
            grid.OnTileObjectAdded -= Grid_OnTileObjectAdded;
            grid.OnTileObjectRemoved -= Grid_OnTileObjectRemoved;
            grid.OnTileObjectMoved -= Grid_OnTileObjectMoved;
        }

        private void Grid_OnTileObjectMoved(TileObject obj)
        {
            
        }

        private void Grid_OnTileObjectRemoved(TileObject obj)
        {
        }

        public void RemoveDisplay(TileObjectDisplay display)
        {
            activeDisplays.Remove(display);
        }

        private void Grid_OnTileObjectAdded(TileObject obj)
        {
            TileObjectDisplay display = srSpawner.Spawn();
            activeDisplays.Add(display);
            display.Setup(obj, this);
        }
    }
}