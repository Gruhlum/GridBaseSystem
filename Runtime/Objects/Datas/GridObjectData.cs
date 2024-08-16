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

        public List<Sprite> sprites = new List<Sprite>();

        public int totalRotations;
        public override Sprite Sprite
        {
            get
            {
				return sprites[0];
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

		public abstract bool IsValidCoord(Coord coord, BaseGrid grid, int rotation);
		public abstract GridObject CreateObject(Coord center, BaseGrid grid);

		//public Sprite GetRotationSprite(int index)
		//{
		//	return sprites[index];
		//}
        public virtual Sprite GetSprite(Coord coord, BaseGrid grid)
		{
			return Sprite;
		}
    }
}