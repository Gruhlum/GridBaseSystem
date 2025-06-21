using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;

namespace HexTecGames.GridBaseSystem
{
    public abstract class GridObjectData<T, D, V, S> : GridObjectData
        where T : GridObject<T, D, V, S> where D : GridObjectData<T, D, V, S> where V : GridObjectVisual<T, D, V, S> where S : GridObjectSaveData<T, D, V, S>
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

        private MultiSpawner spawner = new MultiSpawner();

        public virtual GridObjectVisual CreateVisual(T t, BaseGrid grid)
        {
            V visual = spawner.Spawn(VisualPrefab);
            SetupVisual(visual, t, grid);
            return visual;
        }
        public sealed override GridObjectVisual CreateVisual(GridObject obj, BaseGrid grid)
        {
            return CreateVisual(obj as T, grid);
        }
        protected virtual void SetupVisual(V visual, T tileObj, BaseGrid grid)
        {
            visual.Setup(tileObj, grid);
        }
    }
}