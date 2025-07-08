using System.Collections.Generic;
using System.Linq;
using HexTecGames.Basics;

namespace HexTecGames.GridBaseSystem
{
    [System.Serializable]
    public abstract class MultiObject<T, D, V> : GridObject<T, D, V>
        where T : MultiObject<T, D, V> where D : MultiObjectData<T, D, V> where V : MultiObjectVisual<T, D, V>
    {
        private Dictionary<int, HashSet<Coord>> coordDatas = new Dictionary<int, HashSet<Coord>>();

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
            //Dictionary<int, HashSet<Coord>> newCoords = BaseData.GetNormalizedCoordDatas(targetCoord, Rotation);
            //HashSet<Coord> dataToRemove = new HashSet<Coord>(coordDatas);
            //HashSet<Coord> dataToAdd = new HashSet<Coord>();

            //foreach (var data in newCoords)
            //{
            //    if (dataToRemove.Contains(data))
            //    {
            //        dataToRemove.Remove(data);
            //    }
            //    else dataToAdd.Add(data);
            //}
            //coordDatas = newCoords;
            //Grid.MoveGridObject(dataToRemove, dataToAdd, this);
        }

        public HashSet<Coord> GetNeighbourCoords()
        {
            HashSet<Coord> results = new HashSet<Coord>();

            // Get neighbours of each coordData
            // Add them to a list if they are not a coordData or already in the list

            foreach (var coordData in coordDatas)
            {
                foreach (var coord in coordData.Value)
                {
                    List<Coord> neighbours = Grid.GetNeighbourCoords(coord);
                    foreach (Coord neighbour in neighbours)
                    {
                        if (results.Contains(neighbour))
                        {
                            continue;
                        }
                        if (coordDatas[coordData.Key].Any(x => x == neighbour))
                        {
                            continue;
                        }
                        results.Add(neighbour);
                    }
                }
            }
            return results;
        }
    }
}