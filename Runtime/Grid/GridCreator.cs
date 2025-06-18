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
        [SerializeField] protected GridObjectDataBase defaultTileData = default;
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
            GenerateTileObjects(coords);
        }

        protected void GenerateTileObjects(List<Coord> coords)
        {
            foreach (var coord in coords)
            {
                defaultTileData.CreateGridObject(grid, coord, 0);
            }
        }
    }
}