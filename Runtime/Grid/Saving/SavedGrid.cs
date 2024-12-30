using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
	[System.Serializable]
	public class SavedGrid
	{
		public List<TileSaveData> tileSaveDatas = new List<TileSaveData>();
		public List<TileObjectSaveData> tileObjects = new List<TileObjectSaveData>();
		public SavedGrid(List<Tile> tiles, List<TileObject> objects)
		{
			foreach (var tile in tiles)
			{
				tileSaveDatas.Add(new TileSaveData(tile));
			}
			foreach (var obj in objects)
			{
				tileObjects.Add(new TileObjectSaveData(obj));
            }
		}

		public void CenterTiles()
		{
			int offsetX = (tileSaveDatas.Max(coord => coord.position.x) + tileSaveDatas.Min(coord => coord.position.x)) / 2;
			int offsetY = (tileSaveDatas.Max(coord => coord.position.y) + tileSaveDatas.Min(coord => coord.position.y)) / 2;

			if (offsetX == 0 && offsetY == 0)
			{
				Debug.Log("Already centered");
				return;
			}

			foreach (var saveData in tileSaveDatas)
			{
				saveData.position -= new Coord(offsetX, offsetY);
			}
		}
	}
}