using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(MazeSpawner))]
public class MazeSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MazeSpawner mazeSpawner = (MazeSpawner)target;

        if (GUILayout.Button("迷路を生成"))
        {
            mazeSpawner.GenerateMazeInEditor();
            Debug.Log("迷路を生成しました");
        }
        if (GUILayout.Button("Prefabとして保存"))
        {
            string localPath = "Assets/MyResearch/Prefab/MazePrefab.prefab";
            localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

            PrefabUtility.SaveAsPrefabAsset(mazeSpawner.gameObject, localPath);
            Debug.Log("迷路をPrefabとして保存しました: " + localPath);
        }  // **迷路を削除するボタンを追加**
        if (GUILayout.Button("迷路を削除"))
        {
            mazeSpawner.DeleteMaze();
            Debug.Log("迷路を削除しました");
        }
    }
}
