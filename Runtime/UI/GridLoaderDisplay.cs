using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using HexTecGames.GridBaseSystem;
using UnityEditor;
using System.IO;

namespace HexTecGames.GridBaseSystem.UI
{
    public class GridLoaderDisplay : AdvancedBehaviour
    {
        [SerializeField] private GridLoader gridLoader = default;
        [SerializeField] private TMP_Dropdown dropdown = default;
        [SerializeField, FolderPath] private string savedGridFolderPath = default;

        protected override void Reset()
        {
            base.Reset();
            gridLoader = FindObjectOfType<GridLoader>();
        }

        private void Start()
        {
            dropdown.ClearOptions();
            List<string> options = new List<string>();
            string[] guids = AssetDatabase.FindAssets("t:SavedGridData", new[] { savedGridFolderPath });

            foreach (var guid in guids)
            {
                var gridPath = AssetDatabase.GUIDToAssetPath(guid);
                options.Add(Path.GetFileNameWithoutExtension(gridPath));
            }
            dropdown.AddOptions(options);
        }

        public void LoadGrid()
        {
            string selectedLabel = dropdown.options[dropdown.value].text;
            string gridPath = $"{savedGridFolderPath}/{selectedLabel}.asset";
            var savedGridData = AssetDatabase.LoadAssetAtPath<SavedGridData>(gridPath);
            if (savedGridData == null)
            {
                Debug.Log($"Could not find grid with path: {gridPath}");
                return;
            }
            gridLoader.LoadGrid(savedGridData.SavedGrid);
        }
    }
}