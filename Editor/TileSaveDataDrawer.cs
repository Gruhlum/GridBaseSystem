using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace HexTecGames.GridBaseSystem
{
	[CustomPropertyDrawer(typeof(TileSaveData))]
	public class TileSaveDataDrawer : PropertyDrawer
	{
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement root = new VisualElement();
            PropertyField dataName = new PropertyField(property.FindPropertyRelative("dataName"));
            root.Add(dataName);
            PropertyField coord = new PropertyField(property.FindPropertyRelative("position"));
            root.Add(coord);
            //Coord coord = property.FindPropertyRelative("positon"). as Coord;
            //Vector2IntField coords = new Vector2IntField();

            return root;
        }
    }
}