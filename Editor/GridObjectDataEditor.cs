using UnityEditor;

namespace HexTecGames.GridBaseSystem
{
    [CustomEditor(typeof(GridObjectData), isFallback = true), CanEditMultipleObjects]
    public class GridObjectDataEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}