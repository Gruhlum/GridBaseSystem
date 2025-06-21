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


        //public void SortObjectsByPosition()
        //{
        //    tileSaveDatas = tileSaveDatas.OrderBy(x => x.position).ToList();
        //    tileObjects = tileObjects.OrderBy(x => x.position).ToList();
        //}
        //public void CenterTiles()
        //{
        //    int offsetX = (tileSaveDatas.Max(coord => coord.position.x) + tileSaveDatas.Min(coord => coord.position.x)) / 2;
        //    int offsetY = (tileSaveDatas.Max(coord => coord.position.y) + tileSaveDatas.Min(coord => coord.position.y)) / 2;

        //    if (offsetX == 0 && offsetY == 0)
        //    {
        //        Debug.Log("Already centered");
        //        return;
        //    }

        //    foreach (var saveData in tileSaveDatas)
        //    {
        //        saveData.position -= new Coord(offsetX, offsetY);
        //    }
        //}
    }
}