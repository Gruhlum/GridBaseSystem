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
			for (int i = 0; i < neighbours.Count; i++)
			{
				neighbours[i] -= coord;
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