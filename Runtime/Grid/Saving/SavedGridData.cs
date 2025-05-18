using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    [CreateAssetMenu(menuName = "HexTecGames/Grid/SavedGridData")]
    public class SavedGridData : ScriptableObject
    {
        public SavedGrid SavedGrid;

#if UNITY_EDITOR
        [ContextMenu("Center Tiles")]
        public void CenterTiles()
        {
            SavedGrid.CenterTiles();
            Undo.RecordObject(this, "Center Tiles");
        }


        [ContextMenu("Sort Objects")]
        public void SortObjectsByPosition()
        {
            Undo.RecordObject(this, "Sort Objects");
            SavedGrid.SortObjectsByPosition();
        }
#endif
    }
}