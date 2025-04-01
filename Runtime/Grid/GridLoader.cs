using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public class GridLoader : MonoBehaviour
    {
        [SerializeField] private BaseGrid grid = default;
        [Space]
        [SerializeField] private List<TileData> allTileDatas = default;
        [SerializeField] private List<TileObjectData> allTileObjectDatas = default;
        [Space]
        [SerializeField] private SavedGridData gridToLoad = default;

        private void Start()
        {
            if (gridToLoad != null)
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
                TileData tileData = allTileDatas.Find(x => x.name == saveData.dataName);
                results.Add(tileData.CreateObject(grid, saveData.position));
            }
            return results;
        }
        private void LoadTileObjects(List<TileObjectSaveData> saveDatas)
        {
            foreach (var data in saveDatas)
            {
                var result = allTileObjectDatas.Find(x => x.name == data.dataName);
                if (result == null)
                {
                    Debug.Log("Could not find TileObjectData with name: " + data.dataName);
                    continue;
                }
                var obj = result.CreateObject(grid, data.position, data.rotation);

                if (data.customSaveData != null)
                {
                    obj.LoadCustomSaveData(data.customSaveData);
                }
                grid.AddTileObject(obj);
            }
        }
    }
}