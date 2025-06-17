using HexTecGames.Basics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public class TileObjectVisualizer : MonoBehaviour
    {
        [SerializeField] private BaseGrid grid = default;
        [SerializeField] private TileObjectVisualBase defaultVisual = default;
        [SerializeField] private Transform visualParent = default;

        private HashSet<GridObjectVisual> activeDisplays = new HashSet<GridObjectVisual>();


        //public event Action<TileObjectVisual> OnVisualSpawned;


        protected void Reset()
        {
            grid = GetComponentInParent<BaseGrid>();
        }
        protected void OnEnable()
        {
            grid.OnTileObjectAdded += Grid_OnTileObjectAdded;
        }
        protected void OnDisable()
        {
            grid.OnTileObjectAdded -= Grid_OnTileObjectAdded;
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

        public void RemoveDisplay(TileObjectVisualBase visual)
        {
            activeDisplays.Remove(visual);
        }

        private void Grid_OnTileObjectAdded(TileObjectBase tileObject)
        {
            GridObjectVisual visual = tileObject.CreateVisual(grid);
            visual.transform.SetParent(visualParent);
            activeDisplays.Add(visual);
        }
    }
}