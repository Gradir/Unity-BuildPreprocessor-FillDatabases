using UnityEditor;
using UnityEngine;

namespace Underneath
{
    [CreateAssetMenu(menuName = "Scriptable objects/Databases/Dialogue Database")]
    public class DialogueDatabase : ScriptableDatabase<Dialogue>
    {
        #if UNITY_EDITOR
        [ContextMenu("Fill")]
        public void ContextFill()
        {
            All.Clear();

            var guids = AssetDatabase.FindAssets("t:" + nameof(Dialogue));

            foreach (var iGuid in guids)
            {
                var realPath = AssetDatabase.GUIDToAssetPath(iGuid);
                All.Add((Dialogue)AssetDatabase.LoadAssetAtPath(realPath, typeof(Dialogue)));
            }
        }
        #endif
    }
}
