using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MSL
{

    public class EditorManager
    {

        [MenuItem("Assets/Create/ScriptableDataList")]
        public static MonsterDataList Create()
        {
            MonsterDataList assetClass = ScriptableObject.CreateInstance<MonsterDataList>();
            assetClass.dataList = new List<MonsterData>();
            AssetDatabase.CreateAsset(assetClass, "Assets/scriptableDataList.asset");
            AssetDatabase.SaveAssets();

            return assetClass;
        }

        public static MonsterDataList Create(string assetPath, string assetName)
        {
            MonsterDataList assetClass = ScriptableObject.CreateInstance<MonsterDataList>();
            assetClass.dataList = new List<MonsterData>();
            AssetDatabase.CreateAsset(assetClass, assetPath + assetName);
            AssetDatabase.SaveAssets();

            return assetClass;
        }

    }
}