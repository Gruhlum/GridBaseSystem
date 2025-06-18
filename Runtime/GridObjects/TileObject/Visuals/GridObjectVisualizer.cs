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
        [SerializeField] private GridObjectVisualBase defaultVisual = default;
        [SerializeField] private Transform visualParent = default;

        private HashSet<GridObjectVisualBase> activeDisplays = new HashSet<GridObjectVisualBase>();


        //public event Action<TileObjectVisual> OnVisualSpawned;


        protected void Reset()
        {
            grid = GetComponentInParent<BaseGrid>();
        }
        protected void OnEnable()
        {
            grid.OnGridObjectAdded += Grid_OnTileObjectAdded;
        }
        protected void OnDisable()
        {
            grid.OnGridObjectAdded -= Grid_OnTileObjectAdded;
        }


        //public T FindVisual<T>(T tileObject) where T : TileObjectVisualBase
        //{
        //    foreach (var display in activeDisplays)
        //    {
        //        if (display is T && display.GetTileObject() == tileObject)
        //        {
        //            return (T)display;
        //        }
        //    }
        //    return default;
        //}

        public void RemoveDisplay(GridObjectVisualBase visual)
        {
            activeDisplays.Remove(visual);
        }

        private void Grid_OnTileObjectAdded(GridObjectBase tileObject)
        {
            GridObjectVisualBase visual = tileObject.CreateVisual(grid);
            visual.transform.SetParent(visualParent);
            activeDisplays.Add(visual);
        }
    }
}