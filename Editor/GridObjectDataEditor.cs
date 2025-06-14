using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace HexTecGames.GridBaseSystem
{
    [CustomEditor(typeof(GridObjectDataBase), isFallback = true), CanEditMultipleObjects]
    public class GridObjectDataEditor : Editor
    {
    	public override void OnInspectorGUI()
    	{
    		base.OnInspectorGUI();
    	}
    }
}