using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public abstract class SingleObjectVisual<T, D, V, S> : GridObjectVisual2D<T, D, V, S>
        where T : SingleObject<T, D, V, S> where D : SingleObjectData<T, D, V, S> where V : SingleObjectVisual<T, D, V, S> where S : SingleObjectSaveData<T, D, V, S>
    {

    }
}