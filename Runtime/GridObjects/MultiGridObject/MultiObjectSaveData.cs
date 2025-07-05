namespace HexTecGames.GridBaseSystem
{
    [System.Serializable]
    public abstract class MultiObjectSaveData<T, D, V> : GridObjectSaveData<T, D, V>
        where T : MultiObject<T, D, V> where D : MultiObjectData<T, D, V> where V : MultiObjectVisual<T, D, V>
    {
        protected MultiObjectSaveData(T tileObject) : base(tileObject)
        {
        }
    }
}