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
                grid.SetTiles(GenerateTiles(gridToLoad.SavedGrid));
                LoadTileObjects(gridToLoad.SavedGrid.tileObjects);
            }
        }

        private List<Tile> GenerateTiles(SavedGrid savedGrid)
        {
            List<Tile> results = new List<Tile>();
            foreach (var saveData in savedGrid.tileSaveDatas)
            {
                results.Add(saveData.tileData.CreateObject(grid, saveData.position, saveData.customSaveData));
            }
            return results;
        }
        private void LoadTileObjects(List<TileObjectSaveData> saveDatas)
        {
            foreach (var saveData in saveDatas)
            {
                var obj = saveData.tileObjectData.CreateObject(grid, saveData.position, saveData.rotation, saveData.customSaveData);
                grid.AddTileObject(obj);
            }
        }
    }
}