#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Underneath
{
    public class BuildPreprocessor : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;
		    const string Fill = "Fill";
		    const string SearchPhrase = "t:scriptableObject database";

        public void OnPreprocessBuild(BuildReport report)
        {
            FillDatabases();
        }
        
        void FillDatabases()
        {
            var guids = AssetDatabase.FindAssets(SearchPhrase);
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadMainAssetAtPath(path);
                if (asset == null) continue;

                var type = asset.GetType();
                // Find the ScriptableDatabase<T> base type
                var baseType = type;
                while (baseType != null && (!baseType.IsGenericType || baseType.GetGenericTypeDefinition() != typeof(ScriptableDatabase<>)))
                {
                    baseType = baseType.BaseType;
                }

                if (baseType != null)
                {
                    var itemType = baseType.GetGenericArguments()[0];
                    var fillMethod = baseType.GetMethod(Fill, BindingFlags.Public | BindingFlags.Instance);
                    fillMethod?.Invoke(asset, new object[] { itemType });
                    Debug.Log($"Filled database: {asset.name} with type {itemType.Name}");
                }
            }
        }
    }
}
#endif
