#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace UnityUtilitiesAndExtensions
{
    public static partial class EditorUtilities
    {
        public static T GetOrCreate<T>(string subDirectory = "") where T : ScriptableObject
        {
            var allInstances = GetAllInstances<T>();
            if (allInstances.Length == 0)
            {
                var newInstance = ScriptableObject.CreateInstance(typeof(T)) as T;
                AssetDatabase.CreateAsset(newInstance, string.Format("{0}.asset", Path.Combine("Assets", subDirectory, typeof(T).Name)));
                AssetDatabase.SaveAssets();
                return newInstance;
            }
            else if (allInstances.Length > 1)
            {
                Debug.LogError(string.Format("Sorry! This only supports 1 Bookmark asset. Please remove {0} of the ones logged below so that only 1 remains.", allInstances.Length - 1));
                for (var i = 0; i < allInstances.Length; i++)
                {
                    Debug.Log(allInstances[i].name, allInstances[i]);
                }
            }

            return allInstances[0];
        }

        public static T[] GetAllInstances<T>() where T : ScriptableObject
        {
            var guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T).Name));  //FindAssets uses tags check documentation for more info
            var a = new T[guids.Length];

            for (var i = 0; i < guids.Length; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[i]);
                a[i] = AssetDatabase.LoadAssetAtPath<T>(path);
            }

            return a;
        }

        public static void DrawLabelCenteredBold(string s)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.LabelField(s);
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }
    }
}
#endif
