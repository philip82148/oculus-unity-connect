using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(VRKeyboardExpController))]
public class VRKeyboardControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        VRKeyboardExpController vRKeyboardExpController = (VRKeyboardExpController)target;
        if (GUILayout.Button("Keyboardを生成"))
        {
            // vRKeyboardExpController.CreateTargetObjectsIn3D();
            Debug.Log("Keyboardを生成しました");
        }
        if (GUILayout.Button("Prefabとして保存"))
        {
            string localPath = "Assets/MyResearch/Prefab/VRKeyboard.prefab";
            localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);

            PrefabUtility.SaveAsPrefabAsset(vRKeyboardExpController.gameObject, localPath);
            Debug.Log("KeyboardをPrefabとして保存しました: " + localPath);
        }  // **迷路を削除するボタンを追加**
        if (GUILayout.Button("Keyboardを削除"))
        {

            Debug.Log("迷路を削除しました");
        }
    }
}
