using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    [System.Serializable]
    public class GridObjectSaveData
    {
        public Coord position;
        public int rotation;
        public GridObjectData data;

        public GridObjectSaveData(GridObject gridObj)
        {
            this.position = gridObj.Center;
            this.rotation = gridObj.Rotation;
            this.data = gridObj.BaseData;
        }

        public GridObject CreateGridObject(BaseGrid grid)
        {
            if (data is not IGridObjectCreator creator)
            {
                Debug.Log($"{data} needs to inherit from {nameof(IGridObjectCreator)}!");
                return null;
            }
            return creator.CreateGridObject(grid, position, rotation, this);
        }
    }
}