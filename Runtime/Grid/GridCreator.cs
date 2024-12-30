using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public class GridCreator : MonoBehaviour
    {
        [SerializeField] private BaseGrid grid = default;

        public bool createOnStart = true;
        [Space]
        public int width = 20;
        public int height = 20;
        public Coord center;
        [Space]
        [SerializeField] private List<TileData> allTileDatas = default;
        [SerializeField] private List<TileObjectData> allTileObjectDatas = default;
        [SerializeField] private SavedGridData gridToLoad = default;

        private void Start()
        {
            if (createOnStart)
            {
                if (gridToLoad != null)
                {
                    grid.GenerateGrid(GenerateTiles(gridToLoad.SavedGrid));
                    LoadTileObjects(gridToLoad.SavedGrid.tileObjects);
                }
                else grid.GenerateGrid(GenerateRect(width, height, allTileDatas[0]));
            }
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
        private List<Tile> GenerateRect(int width, int height, TileData tileData)
        {
            List<Tile> tiles = new List<Tile>();
            int startX = -(width / 2) + center.x;
            int startY = -(height / 2) + center.y;
            int endX = (width / 2) + center.x;
            int endY = (height / 2) + center.y;

            if (width % 2 != 0)
            {
                endX++;
            }
            if (height % 2 != 0)
            {
                endY++;
            }

            for (int x = startX; x < endX; x++)
            {
                for (int y = startY; y < endY; y++)
                {
                    tiles.Add((Tile)tileData.CreateObject(grid, new Coord(x, y)));
                }
            }
            return tiles;
        }
        private List<Tile> GenerateTiles(SavedGrid savedGrid)
        {
            List<Tile> results = new List<Tile>();
            foreach (var saveData in savedGrid.tileSaveDatas)
            {
                TileData tileData = allTileDatas.Find(x => x.name == saveData.dataName);
                results.Add((Tile)tileData.CreateObject(grid, saveData.position));
            }
            return results;
        }
    }
}