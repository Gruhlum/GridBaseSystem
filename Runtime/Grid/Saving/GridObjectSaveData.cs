using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
	[System.Serializable]
	public class GridObjectSaveData
	{
		public Coord position;
		public string dataName;
		public CustomSaveData customSaveData;
		public GridObjectSaveData(GridObject gridObj)
		{
			position = gridObj.Center;
			dataName = gridObj.Name;
			customSaveData = gridObj.GetCustomSaveData();
		}
	}
}