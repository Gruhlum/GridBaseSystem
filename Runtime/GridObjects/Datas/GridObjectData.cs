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

        //public abstract GridObject CreateObject(Coord center, BaseGrid grid);
        //public abstract GridObject CreateObject(Coord center, BaseGrid grid, int rotation);
        //public abstract SpriteData GetSpriteData(int rotation)
        //{
        //    return spriteDatas[];
        //}
    }
}