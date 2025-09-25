using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public class GridObjectVisualizer : MonoBehaviour
    {
        [SerializeField] private BaseGrid grid = default;
        [SerializeField] private Transform visualParent = default;

        private Dictionary<int, Transform> layerParents = new Dictionary<int, Transform>();

        private HashSet<GridObjectVisual> activeDisplays = new HashSet<GridObjectVisual>();

        private MultiSpawner spawner = new MultiSpawner();

        //public event Action<TileObjectVisual> OnVisualSpawned;


        protected void Reset()
        {
            grid = GetComponentInParent<BaseGrid>();
        }
        protected void Awake()
        {
            grid.OnGridObjectAdded += Grid_OnTileObjectAdded;
        }

        protected void OnDestroy()
        {
            grid.OnGridObjectAdded -= Grid_OnTileObjectAdded;
            activeDisplays.Clear();
            layerParents.Clear();
        }

        public void RemoveDisplay(GridObjectVisual visual)
        {
            activeDisplays.Remove(visual);
        }

        private void Grid_OnTileObjectAdded(GridObject gridObject)
        {
            //Debug.Log($"{nameof(tileObject.Name)} {tileObject.Name}");
            var prefab = gridObject.BaseData.GetVisualPrefab();
            if (prefab == null)
            {
                Debug.Log("Prefab is null!");
                return;
            }
            GridObjectVisual visual = spawner.Spawn(prefab, false);
            visual.Setup(gridObject, grid);

            int layer = gridObject.BaseData.Layer;

            if (layerParents.TryGetValue(layer, out Transform parent))
            {
                visual.transform.SetParent(parent);
            }
            else
            {
                GameObject go = new GameObject($"Layer {layer}");
                go.transform.SetParent(visualParent);
                visual.transform.SetParent(go.transform);
                layerParents.Add(layer, go.transform);
            }

            activeDisplays.Add(visual);
        }

        public GridObjectVisual FindVisual(GridObject gridObj)
        {
            foreach (GridObjectVisual display in activeDisplays)
            {
                if (display.GridObject == gridObj)
                {
                    return display;
                }
            }
            return null;
        }
    }
}