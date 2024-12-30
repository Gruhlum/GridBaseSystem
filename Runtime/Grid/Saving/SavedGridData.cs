using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
	[CreateAssetMenu(menuName = "HexTecGames/SavedGridData")]
	public class SavedGridData : ScriptableObject
	{
		public SavedGrid SavedGrid;

		[ContextMenu("Center Tiles")]
		public void CenterTiles()
		{
			SavedGrid.CenterTiles();
#if UNITY_EDITOR
			EditorUtility.SetDirty(this);
#endif
		}
	}
}