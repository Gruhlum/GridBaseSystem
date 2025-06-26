using System;
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


        public event Action<BaseGrid> OnCreated;


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
                Debug.LogError("No shape selected!");
                return;
            }
            List<Coord> coords = shape.GetCoords(center);
            GenerateTileObjects(coords);
            OnCreated?.Invoke(grid);
        }

        protected void GenerateTileObjects(List<Coord> coords)
        {
            if (defaultData == null)
            {
                Debug.LogError("No DefaultData!");
                return;
            }

            if (defaultData is not IGridObjectCreator creator)
            {
                Debug.LogError($"{defaultData} needs to inherit from {nameof(IGridObjectCreator)}!");
                return;
            }

            foreach (var coord in coords)
            {
                creator.CreateGridObject(grid, coord, 0);
            }
        }
    }
}