using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    [System.Serializable]
    public class TileHighlightSpawner : Spawner<TileHighlighter>
    {
        public BaseGrid Grid
        {
            get
            {
                return this.grid;
            }
            set
            {
                this.grid = value;
            }
        }
        [SerializeField, Space] private BaseGrid grid = default;
        private Color StartColor
        {
            get
            {
                return Prefab.Color;
            }
        }

        public override void DeactivateAll()
        {
            foreach (TileHighlighter behaviour in items)
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
            Spawn().Activate(Grid.CoordToWorldPosition(coord));
        }
        public void SpawnHighlight(Coord coord, Color color, bool clear = true)
        {
            if (clear)
            {
                DeactivateAll();
            }
            Spawn().Activate(Grid.CoordToWorldPosition(coord), color);
        }
        public void SpawnHighlights(List<Coord> coords, bool clear = true)
        {
            SpawnHighlights(coords, StartColor, clear);
        }
        public void SpawnHighlights(List<Coord> coords, Color color, bool clear = true)
        {
            SpawnHighlights(Grid.CoordsToWorldPositions(coords), color, clear);
        }
        public void SpawnHighlights(List<Coord> coords, Color color, BaseGrid grid, bool clear = true)
        {
            SpawnHighlights(grid.CoordsToWorldPositions(coords), color, clear);
        }
        public void SpawnHighlights(List<Coord> coords, BaseGrid grid, bool clear = true)
        {
            SpawnHighlights(grid.CoordsToWorldPositions(coords), StartColor, clear);
        }

        public void SpawnHighlights(List<Vector3> positions, Color color, bool clear = true)
        {
            if (clear)
            {
                DeactivateAll();
            }
            foreach (Vector3 position in positions)
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
            foreach (Vector3 pos in positions)
            {
                yield return new WaitForSeconds(delay);
                Spawn().Activate(pos);
            }
        }
        public IEnumerator SpawnHighlightsCoroutine(List<Vector3> positions, Color col, float delay = 0)
        {
            foreach (Vector3 pos in positions)
            {
                yield return new WaitForSeconds(delay);
                Spawn().Activate(pos, col);
            }
        }
        public IEnumerator SpawnHighlightsCoroutine(List<List<Vector3>> positions, float delay = 0)
        {
            foreach (List<Vector3> pos in positions)
            {
                yield return SpawnHighlightsCoroutine(pos, new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 0.5f), delay);
            }
        }
    }
}