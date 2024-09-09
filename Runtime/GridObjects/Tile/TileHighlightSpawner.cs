using HexTecGames.Basics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    [System.Serializable]
    public class TileHighlightSpawner : Spawner<TileHighlighter>
    {
        private Color StartColor
        {
            get
            {
                return Prefab.Color;
            }
        }

        public override void DeactivateAll()
        {
            foreach (var behaviour in items)
            {
                if (behaviour != null)
                {
                    behaviour.Deactivate();
                }
            }
        }
        public void SpawnHighlight(Coord coord, bool clear = true)
        {
            if (clear)
            {
                DeactivateAll();
            }
            Spawn().Activate(coord.ToWorldPosition());
        }
        public void SpawnHighlight(Coord coord, Color color, bool clear = true)
        {
            if (clear)
            {
                DeactivateAll();
            }
            Spawn().Activate(coord.ToWorldPosition(), color);
        }
        public void SpawnHighlights(List<Coord> coords, bool clear = true)
        {
            SpawnHighlights(coords, StartColor, clear);
        }
        public void SpawnHighlights(List<Coord> coords, Color color, bool clear = true)
        {
            SpawnHighlights(Coord.ToWorldPositions(coords), color, clear);
        }

        public void SpawnHighlights(List<Coord> coords, Color color, BaseGrid grid, bool clear = true)
        {
            SpawnHighlights(grid.CoordsToWorldPoint(coords), color, clear);
        }
        public void SpawnHighlights(List<Coord> coords, BaseGrid grid, bool clear = true)
        {
            SpawnHighlights(grid.CoordsToWorldPoint(coords), StartColor, clear);
        }
        public void SpawnHighlights(List<Tile> tiles, Color color, bool clear = true)
        {
            var results = new List<Vector3>();
            foreach (var tile in tiles)
            {
                results.Add(tile.GetWorldPosition());
            }
            SpawnHighlights(results, color, clear);
        }
        public void SpawnHighlights(List<Vector3> positions, Color color, bool clear = true)
        {
            if (clear)
            {
                DeactivateAll();
            }
            foreach (var position in positions)
            {
                Spawn().Activate(position, color);
            }
        }
        public void SpawnHighlights(List<Vector3> positions, bool clear = true)
        {
            SpawnHighlights(positions, StartColor, clear);
        }

        public IEnumerator SpawnHighlightsCoroutine(List<Vector3> positions, float delay = 0)
        {
            foreach (var pos in positions)
            {
                yield return new WaitForSeconds(delay);
                Spawn().Activate(pos);
            }
        }
        public IEnumerator SpawnHighlightsCoroutine(List<Vector3> positions, Color col, float delay = 0)
        {
            foreach (var pos in positions)
            {
                yield return new WaitForSeconds(delay);
                Spawn().Activate(pos, col);
            }
        }
        public IEnumerator SpawnHighlightsCoroutine(List<List<Vector3>> positions, float delay = 0)
        {
            foreach (var pos in positions)
            {
                yield return SpawnHighlightsCoroutine(pos, new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 0.5f), delay);
            }
        }
    }
}