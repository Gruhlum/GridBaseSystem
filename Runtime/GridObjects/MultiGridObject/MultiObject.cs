using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    [System.Serializable]
    public abstract class MultiObject<T, D, V, S> : GridObject<T, D, V, S>
        where T : MultiObject<T, D, V, S> where D : MultiObjectData<T, D, V, S> where V : MultiObjectVisual<T, D, V, S> where S : MultiObjectSaveData<T, D, V, S>
    {
        private HashSet<CoordData> coordDatas = new HashSet<CoordData>();

        protected MultiObject(D data, BaseGrid grid, Coord center, int rotation = 0) : base(data, grid, center, rotation)
        {
        }

        protected override void RemoveFromGrid()
        {
            Grid.RemoveGridObject(coordDatas, this);
        }
        protected override void AddToGrid(BaseGrid grid)
        {
            coordDatas = BaseData.GetNormalizedCoordDatas(Center, Rotation);
            grid.AddGridObject(coordDatas, this);
        }
        protected override void Move(Coord currentCoord, Coord targetCoord)
        {
            HashSet<CoordData> newCoords = BaseData.GetNormalizedCoordDatas(targetCoord, Rotation);
            HashSet<CoordData> dataToRemove = new HashSet<CoordData>(coordDatas);
            HashSet<CoordData> dataToAdd = new HashSet<CoordData>();

            foreach (var data in newCoords)
            {
                if (dataToRemove.Contains(data))
                {
                    dataToRemove.Remove(data);
                }
                else dataToAdd.Add(data);
            }

            Center = targetCoord;
            coordDatas = newCoords;
            Grid.MoveGridObject(dataToRemove, dataToAdd, this);
        }
    }
}