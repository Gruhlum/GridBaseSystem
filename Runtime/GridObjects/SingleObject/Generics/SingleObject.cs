using HexTecGames.Basics;

namespace HexTecGames.GridBaseSystem
{
    [System.Serializable]
    public abstract class SingleObject<T, D, V> : GridObject<T, D, V>
        where T : SingleObject<T, D, V> where D : SingleObjectData<T, D, V> where V : SingleObjectVisual<T, D, V>
    {

        /// <summary>
        /// Does not belong to a specific Cell
        /// </summary>
        public virtual bool IsUnbound
        {
            get
            {
                return false;
            }
        }

        protected SingleObject(D data, Coord center, int rotation = 0, GridObjectSaveData saveData = null)
            : base(data, center, rotation, saveData)
        {
        }

        protected override void RemoveFromGrid()
        {
            if (IsUnbound)
            {
                Grid.RemoveGridObject(this);
            }
            else Grid.RemoveGridObject(Layer, Center, this);
        }
        protected override void AddObjectToGrid(BaseGrid grid)
        {
            if (IsUnbound)
            {
                grid.AddGridObject(this);
            }
            else grid.AddGridObject(Layer, Center, this);
        }

        protected override void Move(Coord currentCoord, Coord targetCoord)
        {
            if (IsUnbound)
            {
                Grid.MoveGridObject(this);
            }
            else Grid.MoveGridObject(Layer, currentCoord, targetCoord, this);
        }
    }
}