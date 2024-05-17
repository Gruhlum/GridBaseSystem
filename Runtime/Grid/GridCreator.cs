using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public class GridCreator : MonoBehaviour
    {
        [SerializeField] private BaseGrid grid = default;

        public bool createOnStart = true;
        public int width = 20;
        public int height = 20;

        [SerializeField] private List<TileData> allTileDatas = default;
        [SerializeField] private SavedGridData gridToLoad = default;

        void Start()
        {
            if (createOnStart)
            {
                if (gridToLoad != null)
                {
                    grid.GenerateGrid(GenerateTiles(gridToLoad.SavedGrid));
                }
                else grid.GenerateGrid(GenerateRect(width, height, allTileDatas[0]));
            }
        }
        private List<Tile> GenerateRect(int width, int height, TileData tileData)
        {
            List<Tile> tiles = new List<Tile>();
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {                  
                    tiles.Add((Tile)tileData.CreateObject(new Coord(x, y), grid));
                }
            }
            return tiles;
        }
        private List<Tile> GenerateTiles(SavedGrid savedGrid)
        {
            List<Tile> results = new List<Tile>();
            foreach (var saveData in savedGrid.saveDatas)
            {
                TileData tileData = allTileDatas.Find(x => x.name == saveData.dataName);
                results.Add((Tile)tileData.CreateObject(saveData.position, grid));
            }
            return results;
        }
    }
}