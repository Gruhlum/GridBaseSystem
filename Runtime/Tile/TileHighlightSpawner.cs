using HexTecGames.Basics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
	[System.Serializable]
	public class TileHighlightSpawner : Spawner<TileHighlighter>
	{
		public override void DeactivateAll()
		{
            foreach (var behaviour in behaviours)
            {
                if (behaviour != null)
                {
                    behaviour.Deactivate();
                }
            }
        }
	}
}