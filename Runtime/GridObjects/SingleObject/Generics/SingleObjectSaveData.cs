namespace HexTecGames.GridBaseSystem.Generics
{
    [System.Serializable]
    public abstract class SingleObjectSaveData<T, D, V> : GridObjectSaveData<T, D, V>
        where T : SingleObject<T, D, V> where D : SingleObjectData<T, D, V> where V : SingleObjectVisual<T, D, V>
    {
        protected SingleObjectSaveData(T gridObject) : base(gridObject)
        {
        }
    }
}