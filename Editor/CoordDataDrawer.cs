using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using HexTecGames.Basics.Editor;
using EditorUtility = HexTecGames.Basics.Editor.EditorUtility;

namespace HexTecGames.GridBaseSystem.Editor
{
    [CustomPropertyDrawer(typeof(CoordData))]
    public class CoordDataDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            label = EditorGUI.BeginProperty(pos, label, prop);
            var contentRect = EditorGUI.PrefixLabel(pos, GUIUtility.GetControlID(FocusType.Passive), label);
            var labels = new[] { new GUIContent("Layer"), new GUIContent("X"), new GUIContent("Y") };
            var properties = new[] { prop.FindPropertyRelative("layer"), 
                prop.FindPropertyRelative("coord").FindPropertyRelative("x"), prop.FindPropertyRelative("coord").FindPropertyRelative("y") };
            EditorUtility.DrawMultiplePropertyFields(contentRect, labels, properties);

            EditorGUI.EndProperty();
        }
    }
}