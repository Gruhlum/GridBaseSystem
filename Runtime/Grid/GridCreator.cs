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
        [Space]
        [SerializeField] protected GridObjectData defaultData = default;
        [Space]
        [SerializeReference, SubclassSelector] Shape shape;



        private void OnValidate()
        {
            if (defaultData != null && defaultData is not IGridObjectCreator)
            {
                Debug.Log($"{defaultData} needs to inherit from {nameof(IGridObjectCreator)}!");
                defaultData = null;
            }
        }

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
            if (defaultData is not IGridObjectCreator creator)
            {
                Debug.Log($"{defaultData} needs to inherit from {nameof(IGridObjectCreator)}!");
                return;
            }

            foreach (var coord in coords)
            {
                creator.CreateGridObject(grid, coord, 0);
            }
        }
    }
}