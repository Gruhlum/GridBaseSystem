namespace HexTecGames.GridBaseSystem
{
    public abstract class MultiObjectVisual<T, D, V> : GridObjectVisual2D<T, D, V>
        where T : MultiObject<T, D, V> where D : MultiObjectData<T, D, V> where V : MultiObjectVisual<T, D, V>
    {

    }
}