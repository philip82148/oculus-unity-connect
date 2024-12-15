using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageTextAnimation : MonoBehaviour
{
    [SerializeField] private float moveUpDistance = 1.0f;
    [SerializeField] private float duration = 1.0f;
    [SerializeField] private float fadeDuration = 1.0f;

    [SerializeField] private TextMeshPro textMesh;
    [SerializeField] private GameObject enemy;

    private Vector3 startPos;
    private float yDiff = 1.23f;
    private Vector3 endPos;
    private float elapsed = 0f;


    void Start()
    {
        yDiff = this.transform.position.y - enemy.transform.position.y;
        // 初期状態は非表示など必要ならここで設定
        gameObject.SetActive(false);
    }
    void Update()
    {
        startPos = enemy.transform.position + new Vector3(0, yDiff, 0);
    }

    public void StartAnimation(bool isColor)
    {
        // もし既にアニメーション中なら止める
        StopAllCoroutines();

        //elapsedをリセット
        elapsed = 0f;

        // 開始・終了位置を設定
        startPos = transform.position;
        endPos = transform.position + Vector3.up * moveUpDistance;

        // 初期設定
        gameObject.SetActive(true);
        if (isColor && textMesh != null)
        {
            textMesh.color = Color.red;
            SetAlpha(1f);
        }

        StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            // 位置をLerp
            transform.position = Vector3.Lerp(startPos, endPos, t);

            // フェード（alphaを下げる）
            float alpha = Mathf.Lerp(1f, 0f, t);
            SetAlpha(alpha);

            yield return null;
        }

        // アニメーション終了処理
        transform.position = startPos;
        gameObject.SetActive(false);
        if (textMesh != null)
        {
            textMesh.color = Color.white;
        }
    }

    void SetAlpha(float alpha)
    {
        if (textMesh != null)
        {
            Color c = textMesh.color;
            c.a = alpha;
            textMesh.color = c;
        }
    }
}
