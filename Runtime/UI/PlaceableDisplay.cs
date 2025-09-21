using HexTecGames.Basics.UI;
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


        private void OnValidate()
        {
            background.color = backgroundColor;
        }

        protected override void DrawItem(PlacementData item)
        {
            if (nameGUI != null)
            {
                nameGUI.text = item.DisplayName;
            }
            if (img != null)
            {
                img.sprite = item.Icon;
                img.color = item.GetColor();
            }
        }
        public override void SetHighlight(bool active)
        {
            base.SetHighlight(active);
            background.color = active ? selectedColor : backgroundColor;
        }
    }
}