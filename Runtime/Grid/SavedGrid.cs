using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

		public void CenterTiles()
		{
			int offsetX = (saveDatas.Max(coord => coord.position.x) + saveDatas.Min(coord => coord.position.x)) / 2;
			int offsetY = (saveDatas.Max(coord => coord.position.y) + saveDatas.Min(coord => coord.position.y)) / 2;

			if (offsetX == 0 && offsetY == 0)
			{
				Debug.Log("Already centered");
				return;
			}

			foreach (var saveData in saveDatas)
			{
				saveData.position -= new Coord(offsetX, offsetY);
			}
		}
	}
}