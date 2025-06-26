using HexTecGames.Basics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public class GridObjectVisualizer : MonoBehaviour
    {
        [SerializeField] private BaseGrid grid = default;
        [SerializeField] private Transform visualParent = default;

        private Dictionary<int, Transform> layerParents = new Dictionary<int, Transform>();

        private HashSet<GridObjectVisual> activeDisplays = new HashSet<GridObjectVisual>();


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
        }

        public void RemoveDisplay(GridObjectVisual visual)
        {
            activeDisplays.Remove(visual);
        }

        private void Grid_OnTileObjectAdded(GridObject tileObject)
        {
            //Debug.Log($"{nameof(tileObject.Name)} {tileObject.Name}");
            GridObjectVisual visual = tileObject.CreateVisual(grid);

            int layer = tileObject.BaseData.Layer;

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
            throw new NotImplementedException();
        }
    }
}