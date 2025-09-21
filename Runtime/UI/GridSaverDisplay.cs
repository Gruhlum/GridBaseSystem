using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using HexTecGames.GridBaseSystem;

namespace HexTecGames.GridBaseSystem.UI
{    
    public class GridSaverDisplay : AdvancedBehaviour
    {
        [SerializeField] private TMP_InputField nameInputField = default;
        [SerializeField] private Toggle autoSaveToggle = default;
        [SerializeField] private GridSaver gridSaver = default;

        private const string AUTO_SAVE_TOGGLE_KEY = "AUTO_SAVE_TOGGLE";

        protected override void Reset()
        {
            base.Reset();
            gridSaver = FindObjectOfType<GridSaver>();
        }

        private void Start()
        {
            nameInputField.text = gridSaver.GridName;
            if (SaveSystem.LoadSettings(AUTO_SAVE_TOGGLE_KEY, out bool state))
            {
                autoSaveToggle.SetIsOnWithoutNotify(state);
                gridSaver.AutoSave = state;
            }
            else autoSaveToggle.SetIsOnWithoutNotify(gridSaver.AutoSave);
        }


        public void AutoSaveToggle_Changed(bool state)
        {
            gridSaver.AutoSave = state;
            SaveSystem.SaveSettings(AUTO_SAVE_TOGGLE_KEY, state);
        }
        public void NameInputField_TextChanged(string text)
        {
            gridSaver.GridName = text;
        }
        public void Save()
        {
            gridSaver.SaveGrid();
        }
    }
}