using HexTecGames.Basics;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public class GridSaver : MonoBehaviour
    {
        [SerializeField] private BaseGrid grid = default;

        public enum SaveMode { Local, Assets };

        [SerializeField] private bool autosave = true;
        public string GridName
        {
            get
            {
                return gridName;
            }
            set
            {
                gridName = value;
            }
        }
        [SerializeField] private string gridName = "Grid 1";

        [SerializeField] private SaveMode saveMode = SaveMode.Assets;

        public string AssetPath
        {
            get
            {
                return assetPath;
            }
            private set
            {
                assetPath = value;
            }
        }
        [SerializeField, DrawIf(nameof(saveMode), SaveMode.Assets)] private string assetPath = "ScriptableObjects/Grids";


        private void Reset()
        {
            grid = GetComponentInParent<BaseGrid>();
        }

        private void OnEnable()
        {
            Application.quitting += Application_quitting;
        }
        void OnDisable()
        {
            Application.quitting -= Application_quitting;
        }
        private void Application_quitting()
        {
            if (autosave)
            {
                SaveGrid();
            }
        }

        [ContextMenu("Save Grid")]
        public void SaveGrid()
        {
            if (!Application.isPlaying)
            {
                Debug.Log("Can't save in edit mode");
                return;
            }

            SavedGrid savedGrid = new SavedGrid(grid.GetAllTiles(), grid.GetAllTileObjects());

            switch (saveMode)
            {
                case SaveMode.Local:
                    Debug.Log("Mode not implemented");
                    break;
                case SaveMode.Assets:
                    SaveInAssets(savedGrid);
                    break;
                default:
                    Debug.Log("Mode not implemented");
                    break;
            }
        }

        private void SaveInAssets(SavedGrid savedGrid)
        {
#if UNITY_EDITOR
            SavedGridData savedGridData = ScriptableObject.CreateInstance<SavedGridData>();
            savedGridData.SavedGrid = savedGrid;
            string path = Path.Combine(Application.dataPath, assetPath);
            Directory.CreateDirectory(path);
            AssetDatabase.CreateAsset(savedGridData, Path.Combine("Assets", assetPath, GridName + ".asset"));
#else
            Debug.Log("Not possible to save in Assets in a build");
#endif
        }
        //private bool AssetAlreadyExist(string name)
        //{
        //    string[] results = AssetDatabase.FindAssets(name, new string[] { assetPath });
        //    if (results.Length == 0)
        //    {
        //        return false;
        //    }
        //    return true;
        //}
    }
}