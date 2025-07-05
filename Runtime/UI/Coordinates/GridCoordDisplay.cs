using HexTecGames.Basics;
using TMPro;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public class CoordDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text textGUI = default;

        public void Setup(Coord coord, Vector3 position)
        {
            textGUI.text = $"{coord.x},{coord.y}";
            transform.localPosition = position;
        }
    }
}