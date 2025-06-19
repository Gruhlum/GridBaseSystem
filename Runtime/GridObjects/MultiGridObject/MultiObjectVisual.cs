using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public abstract class MultiObjectVisual<T, D, V, S> : GridObjectVisual<T, D, V, S>
        where T : MultiObject<T, D, V, S> where D : MultiObjectData<T, D, V, S> where V : MultiObjectVisual<T, D, V, S> where S : MultiObjectSaveData<T, D, V, S>
    {

    }
}