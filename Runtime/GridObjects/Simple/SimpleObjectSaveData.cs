namespace HexTecGames.GridBaseSystem
{
    [System.Serializable]
    public class SimpleObjectSaveData : SingleObjectSaveData<SimpleObject, SimpleObjectData, SimpleObjectVisual>
    {
        public SimpleObjectSaveData(SimpleObject tileObject) : base(tileObject)
        {
        }
    }
}