using HexTecGames.Basics.UI;
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
		public abstract bool IsValidCoord(Coord coord, BaseGrid grid);
		public abstract GridObject CreateObject(Coord center, BaseGrid grid);

        public virtual Sprite GetSprite(Coord coord, BaseGrid grid)
		{
			return Sprite;
		}
    }
}