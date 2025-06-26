using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public class GridLoader : MonoBehaviour
    {
        [SerializeField] private BaseGrid grid = default;
        [Space]
        [SerializeField] private SavedGridData gridToLoad = default;
        [SerializeField] private bool loadOnStart = true;

        private void Start()
        {
            if (gridToLoad != null && loadOnStart)
            {
                GenerateTiles(gridToLoad.SavedGrid);
            }
        }

        private void GenerateTiles(SavedGrid savedGrid)
        {
            foreach (var saveData in savedGrid.tileObjects)
            {
                GridObject result = saveData.CreateGridObject(grid);
            }
        }
    }
}