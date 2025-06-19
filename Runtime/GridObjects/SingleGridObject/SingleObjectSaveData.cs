using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    [System.Serializable]
    public abstract class SingleObjectSaveData<T, D, V, S> : GridObjectSaveData<T, D, V, S>
        where T : SingleObject<T, D, V, S> where D : SingleObjectData<T, D, V, S> where V : SingleObjectVisual<T, D, V, S> where S : SingleObjectSaveData<T, D, V, S>
    {
        protected SingleObjectSaveData(T tileObject) : base(tileObject)
        {
        }
    }
}