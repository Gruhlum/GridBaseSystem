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
                return false;
            }
        }

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

        [SerializeField]  protected List<SpriteData> spriteDatas = new List<SpriteData>();

        public int TotalSprites
        {
            get
            {
                return spriteDatas.Count;
            }
        }

        public override Sprite Sprite
        {
            get
            {
                return spriteDatas[0].sprite;
            }
        }

        public SoundClipBase PlacementSound
        {
            get
            {
                return placementSound;
            }
        }
        [SerializeField] private SoundClipBase placementSound = default;

        public abstract bool IsValidCoord(Coord coord, BaseGrid grid);
        public abstract bool IsValidPlacement(Coord coord, BaseGrid grid, int rotation);

        public abstract GridObject CreateObject(Coord center, BaseGrid grid);
        public virtual SpriteData GetSpriteData(int rotation)
        {
            return spriteDatas[rotation % spriteDatas.Count];
        }
        public virtual Sprite GetSprite(Coord coord, BaseGrid grid, int rotation)
        {
            rotation %= spriteDatas.Count;
            return spriteDatas[rotation].sprite;
        }
    }
}