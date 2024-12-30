using HexTecGames.Basics.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HexTecGames.GridBaseSystem
{
    public abstract class PlaceableDisplay<T> : Display<T> where T : PlacementData
    {
        [SerializeField] private TMP_Text nameGUI = default;
        [SerializeField] private Image img = default;
        [SerializeField] private Image background = default;

        [SerializeField] private Color backgroundColor = Color.black;
        [SerializeField] private Color selectedColor = Color.black;

        private Coroutine hotkeyCoroutine;

        protected override void DrawItem(T item)
        {
            nameGUI.text = item.DisplayName;
            img.sprite = item.Icon;
            //img.color = item.Color;
            if (hotkeyCoroutine != null)
            {
                StopCoroutine(hotkeyCoroutine);
            }
            if (item.Hotkey != KeyCode.None)
            {
                StartCoroutine(CheckForHotkey());
            }
        }
        public override void SetHighlight(bool active)
        {
            base.SetHighlight(active);
            background.color = active ? selectedColor : backgroundColor;
        }

        private IEnumerator CheckForHotkey()
        {
            while (true)
            {
                if (Input.GetKeyDown(Item.Hotkey))
                {
                    OnDisplayClicked();
                }
                yield return null;
            }
        }
    }
}