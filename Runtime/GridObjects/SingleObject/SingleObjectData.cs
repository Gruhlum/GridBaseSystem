using HexTecGames.Basics;
using HexTecGames.GridBaseSystem.Generics;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    [CreateAssetMenu(fileName = "New SingleObject", menuName = "HexTecGames/Grid/SingleObjectData")]
    public class SingleObjectData : SingleObjectData<SingleObject, SingleObjectData, SingleObjectVisual>, IGridObjectCreator
    {
        public override int TotalRotations
        {
            get
            {
                return 4;
            }
        }
        public GridObject CreateGridObject(BaseGrid grid, Coord coord, int rotation, GridObjectSaveData saveData = null)
        {
            SingleObject obj = new SingleObject(this, coord, rotation, saveData);
            obj.AddToGrid(grid);
            return obj;
        }
    }
}