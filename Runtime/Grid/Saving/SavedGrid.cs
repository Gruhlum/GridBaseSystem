using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    [System.Serializable]
    public class SavedGrid
    {
        [SerializeReference, SubclassSelector] public List<GridObjectSaveData> tileObjects = new List<GridObjectSaveData>();

        public SavedGrid(IEnumerable<GridObject> tiles)
        {
            foreach (var tile in tiles)
            {
                tileObjects.Add(tile.GetSaveData());
            }
        }


        public void SortObjectsByTypeAndPosition()
        {
            tileObjects = tileObjects.OrderBy(x => x.GetType().Name).ThenBy(x => x.position).ToList();
        }
        public void CenterTiles()
        {
            int offsetX = (tileObjects.Max(coord => coord.position.x) + tileObjects.Min(coord => coord.position.x)) / 2;
            int offsetY = (tileObjects.Max(coord => coord.position.y) + tileObjects.Min(coord => coord.position.y)) / 2;

            if (offsetX == 0 && offsetY == 0)
            {
                Debug.Log("Already centered");
                return;
            }

            foreach (var saveData in tileObjects)
            {
                saveData.position -= new Coord(offsetX, offsetY);
            }
        }
    }
}