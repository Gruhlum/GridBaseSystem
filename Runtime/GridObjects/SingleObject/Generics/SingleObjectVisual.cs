using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public abstract class SingleObjectVisual<T, D, V> : GridObjectVisual2D<T, D, V>
        where T : SingleObject<T, D, V> where D : SingleObjectData<T, D, V> where V : SingleObjectVisual<T, D, V>
    {
        public override void Rotate(int rotation)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, rotation * 360f / Data.TotalRotations);
        }
    }
}