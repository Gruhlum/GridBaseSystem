using HexTecGames.Basics;
using HexTecGames.Basics.UI;
using HexTecGames.SoundSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public abstract class GridObjectDataBase : ScriptableObject
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

        public abstract GridObjectBase CreateGridObject(BaseGrid grid, Coord target, int rotation);
        public GridObjectVisualBase CreateVisual()
        {
            return CreateVisual(null, null);
        }
        public abstract GridObjectVisualBase CreateVisual(GridObjectBase obj, BaseGrid grid);
    }
}