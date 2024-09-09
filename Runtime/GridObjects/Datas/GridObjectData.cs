using HexTecGames.Basics.UI;
using HexTecGames.SoundSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public abstract class GridObjectData : DisplayableScriptableObject
    {
        public abstract bool IsDraggable
        {
            get;
        }
        public virtual bool IsReplaceable
        {
            get
            {
                return isReplaceable;
            }
            private set
            {
                isReplaceable = value;
            }
        }
        [SerializeField] private bool isReplaceable;

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

        public SoundClipBase PlacementSound
        {
            get
            {
                return placementSound;
            }
        }
        [SerializeField] private SoundClipBase placementSound = default;

        //public virtual bool IsValidCoord(Coord coord, BaseGrid grid)
        //{
        //    return true;
        //}
        //public abstract bool IsValidPlacement(Coord coord, BaseGrid grid);

        public abstract GridObject CreateObject(Coord center, BaseGrid grid);
        //public abstract SpriteData GetSpriteData(int rotation)
        //{
        //    return spriteDatas[];
        //}
    }
}