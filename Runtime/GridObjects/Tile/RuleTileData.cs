using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
	[CreateAssetMenu(menuName = "HexTecGames/Grid/RuleTileData")]
	public class RuleTileData : TileData
	{
		public List<RuleData> ruleDatas = new List<RuleData>();
		[SerializeField] private List<TileData> affectedTileDatas = default;

		public Sprite GetSprite(Coord coord, BaseGrid grid)
        {
            var neighbours = grid.GetNeighbourCoords(coord);

			for (int i = neighbours.Count - 1; i >= 0; i--)
			{
                Tile tile = grid.GetTile(neighbours[i]);

                if (tile == null)
                {
                    neighbours.RemoveAt(i);
                }
                else if (tile.Data != this && (affectedTileDatas == null || affectedTileDatas.Count == 0 || !affectedTileDatas.Contains(tile.Data)))
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
			//Debug.Log("Unable to find maching rule!");
			return null;
        }
    }
}