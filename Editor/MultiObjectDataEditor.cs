using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using HexTecGames.GridBaseSystem;
using System.Linq;
using HexTecGames.Basics;

namespace HexTecGames.GridHexSystem.Editor
{
    [CustomEditor(typeof(MultiObjectData))]
    public class MultiObjectDataEditor : UnityEditor.Editor
    {
        public Texture2D normalSprite = default;
        public Texture2D activeSprite = default;

        public Texture2D normalCenterSprite = default;
        public Texture2D activeCenterSprite = default;

        public GridType gridType;


        public override void OnInspectorGUI()
        {
            GenerateButtonGrid(24);

            base.OnInspectorGUI();

        }

        private Texture2D GetTexture(int x, int y, bool isActive)
        {
            if (x == 0 && y == 0)
            {
                if (isActive)
                {
                    return activeCenterSprite;
                }
                else return normalCenterSprite;
            }
            else
            {
                if (isActive)
                {
                    return activeSprite;
                }
                else return normalSprite;
            }
        }

        // Make a grid for every layer
        // button to add a layer


        private void GenerateButtonGrid(float btnSize)
        {
            MultiObjectData data = (MultiObjectData)target;
            if (data.coordDatas == null)
            {
                data.LoadDictionary();
                if (data.coordDatas == null)
                {
                    data.coordDatas = new Dictionary<int, HashSet<Coord>>();
                }
            }
            if (data.coordDatas.Count == 0)
            {
                data.coordDatas.Add(0, new HashSet<Coord>() { Coord.zero });
            }

            GUILayout.BeginVertical();
            float totalWidth = 0;
            float totalHeight = 0;
            foreach (var coordData in data.coordDatas)
            {
                HashSet<Coord> hashSet = coordData.Value;

                if (hashSet.Count <= 0)
                {
                    hashSet.Add(Coord.zero);
                }

                int startX = Mathf.Min(-1, hashSet.Min(coord => coord.x) - 1);
                int startY = Mathf.Min(-1, hashSet.Min(coord => coord.y) - 1);
                int endX = Mathf.Max(1, hashSet.Max(coord => coord.x) + 1);
                int endY = Mathf.Max(1, hashSet.Max(coord => coord.y) + 1);

                int columns = endX - startX + 1;
                int rows = endY - startY + 1;

                float padding = 4;
                float gridWidth = columns * (btnSize + 8) + padding;
                float gridHeight = rows * (btnSize + 1) + padding;

                int lastInput = EditorGUI.IntField(new Rect(totalWidth, 0, btnSize * columns / 2, 20), coordData.Key);
                if (lastInput != coordData.Key)
                {
                    if (data.coordDatas.ContainsKey(lastInput))
                    {
                        Debug.Log($"Key {lastInput} exists already!");
                        return;
                    }
                    data.coordDatas.Remove(coordData.Key);
                    data.coordDatas.Add(lastInput, coordData.Value);
                    data.SerializeDictionary();
                    return;
                }
                bool deleteBtn = EditorGUI.Toggle(new Rect(totalWidth + btnSize * columns / 2 + 8, 0, 20, 20), false);
                if (deleteBtn)
                {
                    data.coordDatas.Remove(coordData.Key);
                    data.SerializeDictionary();
                    return;
                }
                //GUILayout.Space(20);
                GUIStyle style = new GUIStyle("DD ItemStyle");
                GUILayout.BeginArea(new Rect(4 + totalWidth, 24, gridWidth, gridHeight));
                GUILayout.BeginHorizontal(GUILayout.Width(gridWidth), GUILayout.Height(btnSize));
                totalWidth += gridWidth;
                if (totalHeight < gridHeight)
                {
                    totalHeight = gridHeight;
                }

                for (int x = startX; x <= endX; x++)
                {
                    GUILayout.BeginVertical(GUILayout.Width(btnSize), GUILayout.Height(gridHeight));
                    for (int y = startY; y <= endY; y++)
                    {
                        bool isActive = data.HasCoordData(coordData.Key, x, y);

                        Texture2D texture = GetTexture(x, y, isActive);

                        var btnClick = GUILayout.Button(texture, style, GUILayout.Width(btnSize), GUILayout.Height(btnSize));
                        if (btnClick)
                        {
                            Coord coord = new Coord(x, y);
                            if (isActive)
                            {
                                hashSet.RemoveWhere(x => x == coord);
                            }
                            else hashSet.Add(coord);

                            data.SerializeDictionary();
                        }
                    }
                    GUILayout.EndVertical();
                }

                GUILayout.EndHorizontal();
                GUILayout.EndArea();
            }
            if (EditorGUI.Toggle(new Rect(totalWidth, 0, 20, 20), false))
            {
                data.coordDatas.Add(GetEmptyLayer(data), new HashSet<Coord>() { Coord.zero });
                data.SerializeDictionary();
                return;
            }
            GUILayout.EndVertical();
            GUILayout.Space(totalHeight + 10);
            //serializedObject.ApplyModifiedProperties();
            //EditorUtility.SetDirty(data);
        }


        private int GetEmptyLayer(MultiObjectData data)
        {
            for (int i = 0; i < Mathf.Infinity; i++)
            {
                if (!data.coordDatas.ContainsKey(i))
                {
                    return i;
                }
            }
            return 0;
        }
    }
}