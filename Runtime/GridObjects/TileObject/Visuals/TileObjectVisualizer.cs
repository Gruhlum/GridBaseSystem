using HexTecGames.Basics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public class TileObjectVisualizer : MonoBehaviour
    {
        [SerializeField] private BaseGrid grid = default;
        [SerializeField] private TileObjectVisual defaultVisual = default;
        [SerializeField] private MultiSpawner spawner = default;

        private List<TileObjectVisual> activeDisplays = new List<TileObjectVisual>();

        public int SpawnIndex
        {
            get
            {
                return spawnIndex;
            }
            private set
            {
                spawnIndex = value;
            }
        }
        private int spawnIndex;


        protected void Reset()
        {
            grid = GetComponentInParent<BaseGrid>();
            if (spawner == null)
            {
                spawner = new MultiSpawner();
            }
            spawner.Parent = transform;
        }
        protected void OnEnable()
        {
            grid.OnTileObjectAdded += Grid_OnTileObjectAdded;
        }
        protected void OnDisable()
        {
            grid.OnTileObjectAdded -= Grid_OnTileObjectAdded;
        }

        public void RemoveDisplay(TileObjectVisual display)
        {
            activeDisplays.Remove(display);
        }

        private void Grid_OnTileObjectAdded(TileObject obj)
        {
            TileObjectVisual visual;
            if (obj.Data.VisualPrefab == null)
            {
                visual = spawner.Spawn(defaultVisual);
            }
            else visual = spawner.Spawn(obj.Data.VisualPrefab);
            activeDisplays.Add(visual);
            visual.Setup(obj, this, grid);
            SpawnIndex++;
        }
    }
}