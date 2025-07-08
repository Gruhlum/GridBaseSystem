//using UnityEditor;
//using UnityEngine;
//using EditorUtility = HexTecGames.Basics.Editor.EditorUtility;

//namespace HexTecGames.GridBaseSystem.Editor
//{
//    [CustomPropertyDrawer(typeof(CoordData))]
//    public class CoordDataDrawer : PropertyDrawer
//    {
//        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
//        {
//            label = EditorGUI.BeginProperty(pos, label, prop);
//            Rect contentRect = EditorGUI.PrefixLabel(pos, GUIUtility.GetControlID(FocusType.Passive), label);
//            GUIContent[] labels = new[] { new GUIContent("Layer"), new GUIContent("X"), new GUIContent("Y") };
//            SerializedProperty[] properties = new[] { prop.FindPropertyRelative("layer"),
//                prop.FindPropertyRelative("coord").FindPropertyRelative("x"), prop.FindPropertyRelative("coord").FindPropertyRelative("y") };
//            EditorUtility.DrawMultiplePropertyFields(contentRect, labels, properties);

//            EditorGUI.EndProperty();
//        }
//    }
//}