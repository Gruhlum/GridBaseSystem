using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
	[System.Serializable]
	public class SavedGrid
	{
		public List<TileSaveData> saveDatas = new List<TileSaveData>();

		public SavedGrid(List<Tile> tiles)
		{
			foreach (var tile in tiles)
			{
				saveDatas.Add(new TileSaveData(tile));
			}
		}
	}
}