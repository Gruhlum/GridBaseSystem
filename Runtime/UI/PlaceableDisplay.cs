using HexTecGames.Basics.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HexTecGames.GridBaseSystem
{
    public class PlaceableDisplay : Display<PlaceableDisplay, PlacementData>
    {
        [Space]
        [SerializeField] private TMP_Text nameGUI = default;
        [SerializeField] private Image img = default;
        [SerializeField] private Image background = default;

        [SerializeField] private Color backgroundColor = Color.black;
        [SerializeField] private Color selectedColor = Color.green;

        private Coroutine hotkeyCoroutine;


        void OnValidate()
        {
            background.color = backgroundColor;
        }

        protected override void DrawItem(PlacementData item)
        {
            nameGUI.text = item.DisplayName;
            img.sprite = item.Icon;
            img.color = item.IconColor;

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
                    DisplayClicked();
                }
                yield return null;
            }
        }
    }
}