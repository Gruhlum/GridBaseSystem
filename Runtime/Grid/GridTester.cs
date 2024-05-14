using HexTecGames.Basics;
using HexTecGames.GridBaseSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public class GridTester : MonoBehaviour
    {
        [SerializeField] private Spawner<TileHighlighter> highlightSpawner = default;

        [SerializeField] private BaseGrid grid = default;
        [SerializeField] private float speed = 1;
        [SerializeField] private bool oneByOne = default;

        [SerializeField] private float range = 10;

        private void Start()
        {
            StartTest();
        }

        [ContextMenu("Start Test")]
        public void StartTest()
        {
            StartCoroutine(AnimateTest());
        }
        private IEnumerator AnimateTest()
        {
            yield return GetCoordByDistance();
            yield return new WaitForSeconds(1f / speed);
            yield return GetArea();
        }
        private IEnumerator GetCoordByDistance()
        {
            for (int i = 0; i < range; i++)
            {
                var results = grid.GetRing(grid.Center, i);
                grid.GetValidTiles(results);
                yield return DisplayResults(results);
            }
        }
        private IEnumerator GetArea()
        {
            for (int i = 0; i < range; i++)
            {
                var results = grid.GetArea(grid.Center, i);
                grid.GetValidTiles(results);
                yield return new WaitForSeconds(1.4f / speed);
                yield return DisplayResults(results);
            }
        }
        private IEnumerator DisplayResults(List<Coord> results)
        {
            foreach (var result in results)
            {
                var highlight = highlightSpawner.Spawn();
                StartCoroutine(highlight.ShowHighlight(grid.CoordToWorldPoint(result), 0, 0.8f / speed, 0.3f / speed, 0.3f / speed));
                if (oneByOne)
                {
                    yield return new WaitForSeconds(0.1f / speed);
                }
            }
            yield return new WaitForSeconds(0.4f / speed);
        }
    }
}