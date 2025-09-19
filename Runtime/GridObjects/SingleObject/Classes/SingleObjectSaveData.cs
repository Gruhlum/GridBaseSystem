
namespace HexTecGames.GridBaseSystem.Classes
{
    [System.Serializable]
    public class SingleObjectSaveData : SingleObjectSaveData<SingleObject, SingleObjectData, SingleObjectVisual>
    {
        public SingleObjectSaveData(SingleObject tileObject) : base(tileObject)
        {
        }
    }
}