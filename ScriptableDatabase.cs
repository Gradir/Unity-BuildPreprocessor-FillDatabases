using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ScriptableDatabase<T> : ScriptableObject where T : ScriptableObject
{
    public bool ExcludeFromAutoFill;
    public List<T> All = new();

    public T GetByName(string n)
    {
        for (int i = 0; i < All.Count; i++)
        {
            var o = All[i];
            if (o.IsNull() || o == null)
                return null;
            if (o.name == n)
                return All[i];
        }

        return null;
    }

#if UNITY_EDITOR
    public virtual void Fill(System.Type type)
    {
        // Unfortunately due to polymorphism this method wouldn't appear in Unity Editor context menu
        // So the context code needs to be moved to inherited classes
        // We use this class in BeforeBuildPreprocessor, tho'

        if (ExcludeFromAutoFill)
            return;

        All.Clear();

        var guids = AssetDatabase.FindAssets("t:" + type.Name);

        foreach (var iGuid in guids)
        {
            var realPath = AssetDatabase.GUIDToAssetPath(iGuid);
            All.Add((T)AssetDatabase.LoadAssetAtPath(realPath, type));
        }

        EditorUtility.SetDirty(this);
    }
#endif
}
