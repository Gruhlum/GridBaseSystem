using System;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public class GridLoader : MonoBehaviour
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
                GenerateTiles(gridToLoad.SavedGrid);
            }
        }

        private void GenerateTiles(SavedGrid savedGrid)
        {
            foreach (GridObjectSaveData saveData in savedGrid.tileObjects)
            {
                GridObject result = saveData.CreateGridObject(grid);
            }
            OnGridLoaded?.Invoke(grid);
        }
    }
}