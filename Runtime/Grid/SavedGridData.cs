using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
	[CreateAssetMenu(menuName = "HexTecGames/SavedGridData")]
	public class SavedGridData : ScriptableObject
	{
		public SavedGrid SavedGrid;
	}
}