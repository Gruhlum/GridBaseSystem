using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
	[CreateAssetMenu(menuName = "HexTecGames/Grid/TileData")]
	public class TileData : ScriptableObject
	{
        public Sprite Sprite
        {
            get
            {
                return sprite;
            }
            private set
            {
                sprite = value;
            }
        }
        [SerializeField] private Sprite sprite;
    }
}