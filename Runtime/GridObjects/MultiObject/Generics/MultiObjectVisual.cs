using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public abstract class MultiObjectVisual<T, D, V> : GridObjectVisual2D<T, D, V>
        where T : MultiObject<T, D, V> where D : MultiObjectData<T, D, V> where V : MultiObjectVisual<T, D, V>
    {

        public override void Rotate(int rotation)
        {
            if (Data.sprites.Count > 1)
            {
                sr.sprite = Data.sprites[rotation % Data.sprites.Count];
            }
        }
    }
}