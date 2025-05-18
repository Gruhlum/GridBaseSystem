using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using HexTecGames.GridBaseSystem.Shapes;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public class GridCreator : MonoBehaviour
    {
        [SerializeField] protected BaseGrid grid = default;

        public bool createOnStart = true;
        [Space]
        public Coord center;
        [SerializeField] protected TileData defaultTileData = default;
        [SerializeReference, SubclassSelector] Shape shape;


        protected virtual void Start()
        {
            if (createOnStart)
            {
                GenerateTiles();
            }
        }

        private void GenerateTiles()
        {
            if (shape == null)
            {
                Debug.Log("No shape selected!");
                return;
            }
            List<Coord> coords = shape.GetCoords(center);
            List<Tile> tiles = GenerateTiles(coords);
            grid.SetTiles(tiles);
        }

        protected List<Tile> GenerateTiles(List<Coord> coords)
        {
            List<Tile> tiles = new List<Tile>();
            foreach (var coord in coords)
            {
                tiles.Add(defaultTileData.CreateObject(grid, coord));
            }
            return tiles;
        }
    }
}