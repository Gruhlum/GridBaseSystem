using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
	[CreateAssetMenu(menuName = "HexTecGames/Grid/RuleTileData")]
	public class RuleTileData : TileData
	{
		public List<RuleData> ruleDatas = new List<RuleData>();

        public override Sprite GetSprite(Coord coord, BaseGrid grid)
        {
            var neighbours = grid.GetNeighbourCoords(coord);

			for (int i = neighbours.Count - 1; i >= 0; i--)
			{
                Tile tile = grid.GetTile(neighbours[i]);

                if (tile == null || tile.Data != this)
                {
                    neighbours.RemoveAt(i);
                }
                else neighbours[i] -= coord;
            }

			foreach (var ruleData in ruleDatas)
			{
				if (ruleData.Matches(neighbours))
				{
					return ruleData.sprite;
				}
			}
			Debug.Log("Unable to find maching rule! " + neighbours[0].ToString());
			return Sprite;
        }
    }
}