using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
	[System.Serializable]
	public class TileSaveData
	{
		public Coord position;
		public string dataName;

		public TileSaveData(Tile tile)
		{
			position = tile.Center;
			dataName = tile.Data.name;
		}
	}
}