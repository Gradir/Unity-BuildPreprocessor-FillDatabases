using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public static class EditorScript
{
    [MenuItem("Tools/Create Missing ScriptableDatabases", false, 0)]
    public static void CreateMissingScriptables()
    {
        var scriptableTypes = GetAllScriptableObjectTypes().ToList();
        if (scriptableTypes.Count <= 0)
            return;
			
        for (var i = 0; i < scriptableTypes.Count; i++)
        {
            var found = AssetDatabase.FindAssets("t:" + scriptableTypes[i].Name);
            if (found.Length > 0)
                continue;
				
            Debug.Log("Creating " + scriptableTypes[i].Name);
            var ob = ScriptableObject.CreateInstance(scriptableTypes[i]);
            AssetDatabase.CreateAsset(ob, "Assets/Scriptables/" + scriptableTypes[i].Name + ".asset");
        }
    }

    static IEnumerable<Type> GetAllScriptableObjectTypes()
    {
        var scriptableDatabases =  AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.InheritsFrom(typeof(ScriptableDatabase<>)));
				
        return scriptableDatabases;
    }
}
#endif
