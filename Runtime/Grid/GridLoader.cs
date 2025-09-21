using System;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public class GridLoader : AdvancedBehaviour
    {
        [SerializeField] private BaseGrid grid = default;
        [Space]
        [SerializeField] private SavedGridData gridToLoad = default;
        [SerializeField] private bool loadOnStart = true;

        public event Action<BaseGrid> OnGridLoaded;


        private void Start()
        {
            if (gridToLoad != null && loadOnStart)
            {
                LoadGrid(gridToLoad.SavedGrid);
            }
        }

        public void LoadGrid(SavedGrid savedGrid)
        {
            Debug.Log($"Loading grid with {savedGrid.tileObjects.Count} objects");
            grid.ClearAll();
            foreach (GridObjectSaveData saveData in savedGrid.tileObjects)
            {
                GridObject result = saveData.CreateGridObject(grid);
            }
            OnGridLoaded?.Invoke(grid);
        }
    }
}