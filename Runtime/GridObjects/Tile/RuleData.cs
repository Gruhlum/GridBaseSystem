using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
	[System.Serializable]
	public class RuleData
	{
		public List<Coord> neighbours = new List<Coord>();

		public Sprite sprite;

		public bool Matches(List<Coord> coords)
		{
			if (neighbours.Count != coords.Count)
			{
				return false;
			}
			foreach (var coord in coords)
			{
				if (!neighbours.Any(x => x == coord))
				{
					return false;
				}
			}
			return true;
		}
	}
}