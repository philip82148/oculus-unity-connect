using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControlledSonificationController : MonoBehaviour
{
    [SerializeField] private CreateSoundController createSoundController;
    [SerializeField] private GameObject targetObject;

    private enum SonificationStrategy
    {
        NoReference,
        WithReference,
        WithReferenceAndZoomEffect
    }

    private SonificationStrategy currentStrategy;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10f;

        Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        float distance = Mathf.Abs(worldMousePosition.x - targetObject.transform.position.x);
        float normalizedDistance = distance / 3;

        // switch (currentStrategy)
        // {
        //     case SonificationStrategy.NoReference:
        // }
        // ApplyNoReferenceStrategy(normalizedDistance);
        // ApplyWithReferenceStrategy(normalizedDistance);
        ApplyWithZoomEffectStrategy(normalizedDistance);
        // 物体の色を変えて距離の変化を可視化（オプション）
        // 物体の色を白から赤に変える（オプション）
        // 物体の色を赤から白に変化させる（オプション）
        Color startColor = Color.red;  // 開始色（赤）
        Color endColor = Color.white;  // 終了色（白）
        targetObject.GetComponent<Renderer>().material.color = Color.Lerp(startColor, endColor, normalizedDistance);

    }

    private void ApplyNoReferenceStrategy(float normalizedDistance)
    {
        float frequencyCoefficient = Mathf.Lerp(0.5f, 1.5f, 1 - normalizedDistance);
        createSoundController.SetFrequencyCoefficient(frequencyCoefficient);
    }   // 2. 参照付きの戦略: 目標に近づくと特定の周波数で参照音を再生
    private void ApplyWithReferenceStrategy(float normalizedDistance)
    {
        // 目標に近づいたときに特定の周波数で参照音を出す
        if (normalizedDistance < 0.1f)
        {
            createSoundController.frequencyCoefficient = 2.0f;  // 例えば、440Hzの参照音
        }
        else
        {
            createSoundController.frequencyCoefficient = Mathf.Lerp(0.5f, 1.5f, 1 - normalizedDistance);
        }
    }
    private void ApplyWithZoomEffectStrategy(float normalizedDistance)
    {
        // 目標に近づいたときに特定の周波数で参照音を出す
        if (normalizedDistance < 0.1f)
        {
            createSoundController.frequencyCoefficient = 2.0f;  // 例えば、440Hzの参照音
        }
        else if (normalizedDistance < 0.5f)
        {
            // normalizedDistanceの累乗変化を用いて、距離が小さいほど急激に変化
            float zoomFactor = Mathf.Pow(1.5f - normalizedDistance, 2);  // 累乗でズーム効果を強調

            // 基本周波数係数を累乗的に変化させる
            float baseFrequencyCoefficient = Mathf.Lerp(0.5f, 1.8f, zoomFactor);

            // 最終的な周波数係数を設定
            createSoundController.SetFrequencyCoefficient(baseFrequencyCoefficient);
        }
        else
        {
            createSoundController.frequencyCoefficient = Mathf.Lerp(0.5f, 1.5f, 1.5f - normalizedDistance);
        }
    }
}
