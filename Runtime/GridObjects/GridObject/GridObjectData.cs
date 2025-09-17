using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public abstract class GridObjectData<T, D, V> : GridObjectData
        where T : GridObject<T, D, V> where D : GridObjectData<T, D, V> where V : GridObjectVisual<T, D, V>
    {
        public V VisualPrefab
        {
            get
            {
                return visualPrefab;
            }
            private set
            {
                visualPrefab = value;
            }
        }
        [SerializeField] private V visualPrefab;


        public override GridObjectVisual GetVisualPrefab()
        {
            return VisualPrefab;
        }
        protected virtual void SetupVisual(V visual, T tileObj, BaseGrid grid)
        {
            visual.Setup(tileObj, grid);
        }
    }
}