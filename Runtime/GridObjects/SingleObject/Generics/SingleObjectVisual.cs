using UnityEngine;

namespace HexTecGames.GridBaseSystem.Generics
{
    public abstract class SingleObjectVisual<T, D, V> : GridObjectVisual2D<T, D, V>
        where T : SingleObject<T, D, V> where D : SingleObjectData<T, D, V> where V : SingleObjectVisual<T, D, V>
    {
    }
}