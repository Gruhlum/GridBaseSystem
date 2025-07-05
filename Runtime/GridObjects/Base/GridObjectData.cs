using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public abstract class GridObjectData : ScriptableObject
    {
        public Color Color
        {
            get
            {
                return color;
            }
            private set
            {
                color = value;
            }
        }
        [SerializeField] private Color color = Color.white;

        public abstract int Layer
        {
            get;
        }

        public abstract bool IsValidPlacement(BaseGrid grid, Coord target, int rotation);
        public abstract HashSet<CoordData> GetNormalizedCoordDatas(Coord target, int rotation);
        public abstract List<BoolCoord> GetNormalizedValidCoords(BaseGrid grid, Coord target, int rotation);

        //public abstract GridObject CreateGridObject(BaseGrid grid, Coord target, int rotation, GridObjectSaveData saveData = null);
        public GridObjectVisual CreateVisual()
        {
            return CreateVisual(null, null);
        }
        public abstract GridObjectVisual CreateVisual(GridObject obj, BaseGrid grid);
    }
}