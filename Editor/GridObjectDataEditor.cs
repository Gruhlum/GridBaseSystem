using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace HexTecGames.GridBaseSystem
{
    [CustomEditor(typeof(GridObjectData), isFallback = true), CanEditMultipleObjects]
    public class GridObjectDataEditor : UnityEditor.Editor
    {
    	public override void OnInspectorGUI()
    	{
    		base.OnInspectorGUI();
    	}
    }
}