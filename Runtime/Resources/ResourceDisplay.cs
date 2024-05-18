using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HexTecGames.GridBaseSystem
{
	public class ResourceDisplay : MonoBehaviour
	{
        [SerializeField] private TMP_Text textGUI = default;
        [SerializeField] private Image img = default;

        void Reset()
        {
            textGUI = GetComponentInChildren<TMP_Text>();
            img = GetComponentInChildren<Image>();
        }

        public void SetText(string text)
        {
            textGUI.text = text;
        }
        public void SetSprite(Sprite sprite)
        {
            img.sprite = sprite;
        }
        public void SetData(ResourceValue resourceValue)
        {
            SetData(resourceValue.Value.ToString(), resourceValue.Data.Icon);
        }
        public void SetData(string text, Sprite sprite)
        {
            SetText(text);
            SetSprite(sprite);
        }
    }
}