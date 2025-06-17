using HexTecGames.Basics;
using HexTecGames.Basics.UI;
using HexTecGames.SoundSystem;
using System.Collections;
using System.Collections.Generic;
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

        public abstract bool IsValidCoord(BaseGrid grid, Coord coord, int rotation = 0);
        public abstract List<BoolCoord> GetNormalizedValidCoords(BaseGrid grid, Coord center, int rotation);
        public  GridObjectVisual CreateVisual()
        {
            return CreateVisual(null, null);
        }
        public abstract GridObjectVisual CreateVisual(GridObject obj, BaseGrid grid);
    }
}