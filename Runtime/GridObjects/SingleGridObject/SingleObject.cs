using HexTecGames.Basics;

namespace HexTecGames.GridBaseSystem
{
    [System.Serializable]
    public abstract class SingleObject<T, D, V> : GridObject<T, D, V>
        where T : SingleObject<T, D, V> where D : SingleObjectData<T, D, V> where V : SingleObjectVisual<T, D, V>
    {
        protected SingleObject(D data, Coord center, int rotation = 0, GridObjectSaveData saveData = null)
            : base(data, center, rotation, saveData)
        {
        }

        protected override void RemoveFromGrid()
        {
            Grid.RemoveGridObject(Layer, Center, this);
        }
        protected override void AddObjectToGrid(BaseGrid grid)
        {
            grid.AddGridObject(Layer, Center, this);
        }

        protected override void Move(Coord currentCoord, Coord targetCoord)
        {
            Grid.MoveGridObject(Layer, currentCoord, targetCoord, this);
        }
    }
}