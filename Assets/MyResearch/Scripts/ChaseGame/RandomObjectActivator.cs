using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomObjectActivator : MonoBehaviour
{
    [SerializeField] private List<GameObject> targetObjects; // 操作するオブジェクトのリスト
    [SerializeField] private float switchInterval = 2f; // 状態を切り替える間隔（秒）

    void Start()
    {
        if (targetObjects == null || targetObjects.Count == 0)
        {
            Debug.LogError("対象のオブジェクトリストが空です。設定してください。");
            return;
        }

        // コルーチンを開始
        StartCoroutine(SwitchActiveStateRandomly());
    }

    private IEnumerator SwitchActiveStateRandomly()
    {
        while (true)
        {
            // ランダムに1つのオブジェクトをアクティブにし、他を非アクティブにする
            int randomIndex = Random.Range(0, targetObjects.Count);

            for (int i = 0; i < targetObjects.Count; i++)
            {
                targetObjects[i].SetActive(i == randomIndex); // 選ばれたオブジェクトだけアクティブ
            }

            // 指定された間隔待つ
            yield return new WaitForSeconds(switchInterval);
        }
    }
}
