using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
	[CreateAssetMenu(menuName = "HexTecGames/TileObjectData")]
	public class TileObjectData : BaseTileObjectData
	{
        public Sprite sprite;

        public override List<Coord> GetCoords()
        {
            return new() { Coord.zero };
        }

        public override Sprite GetSprite()
        {
            return sprite;
        }
        public override bool IsDragable()
        {
            return true;
        }
    }
}