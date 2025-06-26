using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    [System.Serializable]
    public abstract class MultiObject<T, D, V> : GridObject<T, D, V>
        where T : MultiObject<T, D, V> where D : MultiObjectData<T, D, V> where V : MultiObjectVisual<T, D, V>
    {
        private HashSet<CoordData> coordDatas = new HashSet<CoordData>();

        protected MultiObject(D data, Coord center, int rotation = 0, GridObjectSaveData saveData = null)
            : base(data, center, rotation, saveData)
        {
        }

        protected override void RemoveFromGrid()
        {
            Grid.RemoveGridObject(coordDatas, this);
        }
        protected override void AddObjectToGrid(BaseGrid grid)
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
            coordDatas = newCoords;
            Grid.MoveGridObject(dataToRemove, dataToAdd, this);
        }

        public HashSet<Coord> GetNeighbourCoords()
        {
            HashSet<Coord> results = new HashSet<Coord>();

            // Get neighbours of each coordData
            // Add them to a list if they are not a coordData or already in the list

            foreach (var coordData in coordDatas)
            {
                var neighbours = Grid.GetNeighbourCoords(coordData.coord);
                foreach (var neighbour in neighbours)
                {
                    if (results.Contains(neighbour))
                    {
                        continue;
                    }
                    if (coordDatas.Any(x => x.coord == neighbour))
                    {
                        continue;
                    }
                    results.Add(neighbour);
                }
            }
            return results;
        }
    }
}