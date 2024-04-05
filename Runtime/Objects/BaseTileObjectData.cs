using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
	public abstract class BaseTileObjectData : ScriptableObject
	{
		public abstract List<Coord> GetCoords();
		public abstract Sprite GetSprite();
		public abstract bool IsDraggable
		{
			get;
		}
		public abstract bool IsWall
		{
			get;
		}
	}
}