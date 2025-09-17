using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public abstract class MultiObjectVisual<T, D, V> : GridObjectVisual2D<T, D, V>
        where T : MultiObject<T, D, V> where D : MultiObjectData<T, D, V> where V : MultiObjectVisual<T, D, V>
    {
        [SerializeField] private List<Sprite> sprites = new List<Sprite>();

        public override int TotalRotations
        {
            get
            {
                return sprites.Count;
            }
        }

        public override void Rotate(int rotation)
        {
            if (sprites.Count > 1)
            {
                sr.sprite = sprites[rotation % sprites.Count];
            }
        }
    }
}