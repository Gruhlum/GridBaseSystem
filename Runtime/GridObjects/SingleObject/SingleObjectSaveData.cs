
using HexTecGames.GridBaseSystem.Generics;

namespace HexTecGames.GridBaseSystem
{
    [System.Serializable]
    public class SingleObjectSaveData : SingleObjectSaveData<SingleObject, SingleObjectData, SingleObjectVisual>
    {
        public SingleObjectSaveData(SingleObject tileObject) : base(tileObject)
        {
        }
    }
}