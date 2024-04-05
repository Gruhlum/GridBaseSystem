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

        public IEnumerator SpawnHighlights(List<Vector3> positions, Color col, float delay = 0.02f)
        {          
            foreach (var pos in positions)
            {
                yield return new WaitForSeconds(delay);
                Spawn().Activate(pos, col);
            }
        }
        public IEnumerator SpawnHighlights(List<List<Vector3>> positions, float delay = 0.02f)
        {
            foreach (var pos in positions)
            {
                yield return SpawnHighlights(pos, new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 0.5f), delay);
            }
        }
    }
}