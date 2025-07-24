using HexTecGames.Basics;
using HexTecGames.GridBaseSystem.Generics;

namespace HexTecGames.GridBaseSystem
{
    [System.Serializable]
    public class SingleObject : SingleObject<SingleObject, SingleObjectData, SingleObjectVisual>
    {
        public SingleObject(SingleObjectData data, Coord center, int rotation = 0, GridObjectSaveData saveData = null)
            : base(data, center, rotation, saveData)
        {
        }

        public override GridObjectSaveData GetSaveData()
        {
            return new SingleObjectSaveData(this);
        }


    }
}