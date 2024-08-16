using HexTecGames.Basics.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
	public abstract class BaseTileObjectData : GridObjectData
    {
		public abstract List<Coord> GetCoords();
		public abstract Sprite GetSprite();

		public abstract bool IsWall
		{
			get;
		}

        public override Sprite Sprite
        {
            get
            {
                return GetSprite();
            }
        }

        public override GridObject CreateObject(Coord center, BaseGrid grid)
		{
			return new TileObject(center, grid, this);
		}
	}
}