using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class AudioController : MonoBehaviour
{


    [Header("audio source setting")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] CreateSoundController soundController;
    [SerializeField] float spatialBlend = 1.0f;

    [SerializeField] private float amplitude;
    [SerializeField] private float frequencyCoefficient = 4.0f;
    [SerializeField] private float frequency;
    [SerializeField] private const float panCoefficient = 1.0f;
    [SerializeField] private float pan;
    [SerializeField] public bool isAmplitudeInversion = false;

    [Header("scaling factor")]
    [SerializeField] const float discreteAmplitudeScalingFactor = 2.0f; // この値を増減させて、影響度を調整
    [SerializeField] const float discreteFrequencyScalingFactor = 2.0f; // この値を増減させて、影響度を調整
    [SerializeField] const float amplitudeScalingFactor = 12.0f; // この値を増減させて、影響度を調整
    [SerializeField] const float frequencyScalingFactor = 4.0f; // この値を増減させて、影響度を調整


    private float discreteFactor = 0.01f;


    [Header("subtle")]
    [SerializeField] private float yOffset = 0.0f;
    [SerializeField] private float zOffset = 0f;





    void Start()
    {

        ReflectAudioSettings();
        audioSource.loop = true;
        audioSource.Play();
        ChangeSpatialBlend();
    }



    public void SetContinuousAudioSetting(Vector3 targetDiff)
    {

        amplitude = 0.5f - amplitudeScalingFactor * targetDiff.z;
        frequency = 1.0f + frequencyScalingFactor * targetDiff.y;
        pan = 100 / 10 * targetDiff.x;

        ReflectLinearAudioSettings();

    }

    public void SetDiscreteAudioSetting(Vector3 targetDiff)
    {


        float discreteX = Mathf.Floor(targetDiff.x / discreteFactor) * discreteFactor;
        float discreteY = Mathf.Floor(targetDiff.y / discreteFactor) * discreteFactor;
        float discreteZ = Mathf.Floor(targetDiff.z / discreteFactor) * discreteFactor;


        amplitude = 0.5f - amplitudeScalingFactor * discreteZ;
        frequency = 1.0f + frequencyScalingFactor * discreteY;
        pan = 10 * discreteX;

        ReflectLinearAudioSettings();

    }



    public void SetContinuousExponentAudioSettingWithTargetText(Vector3 targetDiff)
    {

        float adjustedZ = targetDiff.z + zOffset;
        amplitude = Mathf.Pow(discreteAmplitudeScalingFactor, -adjustedZ * 100);
        frequency = Mathf.Pow(discreteFrequencyScalingFactor, (targetDiff.y + yOffset) * 100);

        // frequency が負の値にならないようにチェック
        if (frequency < 0)
        {
            frequency = 0;
        }

        float diffX = targetDiff.x * 1000;
        if (-5 < diffX && diffX < 5)
        {
            Debug.Log("-10 < diffX && diffX < 10");
            pan = 0;
        }
        else if (-30 < diffX && diffX <= -5)
        {
            Debug.Log("-30 < diffX && diffX <= -10");
            pan = Mathf.Lerp(-1.0f, 0f, (diffX + 5) / 25.0f);
        }
        else if (5 <= diffX && diffX < 30)
        {
            Debug.Log("diffX <= 10 && diffX < 30");
            pan = Mathf.Lerp(0f, 1.0f, (diffX - 5) / 25.0f);
        }
        else
        {
            amplitude = 0;
        }

        ReflectAudioSettings();

    }


    public void SetDiscreteExponentAudioSettingWithTargetText(Vector3 targetDiff)
    {

        float discreteZ = Mathf.Floor((targetDiff.z + zOffset) / discreteFactor) * discreteFactor;
        amplitude = Mathf.Pow(discreteAmplitudeScalingFactor, -discreteZ / discreteFactor);




        float discreteY = Mathf.Floor((targetDiff.y + yOffset) / discreteFactor) * discreteFactor;
        // 1cm 増えるごとに 1.5 倍になるための計算
        frequency = Mathf.Pow(discreteFrequencyScalingFactor, discreteY * 100);  // controllerPosition.y がメートル単位なので、100 を掛けてセンチメートル単位に変換

        // frequency が負の値にならないようにチェック
        if (frequency < 0)
        {
            frequency = 0;
        }


        double diffX = targetDiff.x * 1000;
        if (-5 < diffX && diffX < 5)
        {
            Debug.Log("-10 < diffX && diffX < 10");
            pan = 0;
        }
        else if (-30 < diffX && diffX <= -5)
        {
            Debug.Log("-30 < diffX && diffX <= -10");
            pan = -1.0f;
        }
        else if (5 <= diffX && diffX < 30)
        {
            Debug.Log("diffX <= 10 && diffX < 30");
            pan = 1.0f;
        }
        else
        {
            amplitude = 0;
        }

        ReflectAudioSettings();

    }




    public void SetAudioSettingOnlyAmplitude(Vector3 controllerPosition)
    {
        float discreteZ = Mathf.Floor(controllerPosition.z / discreteFactor) * discreteFactor;
        // amplitude = 1.0f - amplitudeScalingFactor * controllerPosition.z;
        amplitude = 1.0f - amplitudeScalingFactor * discreteZ;

        ReflectAudioSettings();

    }
    public void SetAudioSettingOnlyAmplitudeWithTargetText(Vector3 targetDiff)
    {
        float zOffset = -0.1f;
        float discreteZ = Mathf.Floor((targetDiff.z + zOffset) / discreteFactor) * discreteFactor;
        amplitude = Mathf.Pow(discreteAmplitudeScalingFactor, -discreteZ / discreteFactor);

        ReflectAudioSettings();

    }





    public void SetAudioSettingOnlyFrequency(Vector3 controllerPosition)
    {
        // frequency の基本値（controllerPosition.y が 0 のときの値）
        float baseFrequency = 1.0f;  // 適切な基本周波数を設定する
        amplitude = 0.25f;
        float discreteY = Mathf.Floor((controllerPosition.y + 0.05f - 0.25f) / discreteFactor) * discreteFactor + 0.05f;
        // 1cm 増えるごとに 1.5 倍になるための計算
        frequency = baseFrequency * Mathf.Pow(discreteFrequencyScalingFactor, discreteY * 100);  // controllerPosition.y がメートル単位なので、100 を掛けてセンチメートル単位に変換

        // frequency が負の値にならないようにチェック
        if (frequency < 0)
        {
            frequency = 0;
        }

        pan = 0;  // パンは変更しない

        ReflectAudioSettings();  // オーディオ設定を反映

    }

    public void SetAudioSettingOnlyPan(Vector3 controllerPosition)
    {


        ReflectAudioSettings();

    }


    public void SetAudioSettingOnlyPanWithTargetText(Vector3 targetDiff)
    {

        double diffX = targetDiff.x * 1000;
        if (-5 < diffX && diffX < 5)
        {
            Debug.Log("-10 < diffX && diffX < 10");
            pan = 0; amplitude = 1.0f;
        }
        else if (-30 < diffX && diffX <= -5)
        {
            Debug.Log("-30 < diffX && diffX <= -10");
            pan = -0.5f; amplitude = 1.0f;
        }
        else if (5 <= diffX && diffX < 30)
        {
            Debug.Log("diffX <= 10 && diffX < 30");
            pan = 0.5f; amplitude = 1.0f;
        }
        else
        {

            amplitude = 0;
        }



        ReflectAudioSettings();
    }



    private void ReflectAudioSettings()
    {
        soundController.gain = amplitude;
        soundController.frequencyCoefficient = frequency * frequencyCoefficient;
        soundController.pan = pan;
    }
    private void ReflectLinearAudioSettings()
    {
        soundController.gain = amplitude;
        soundController.frequencyCoefficient = frequency;
        soundController.pan = pan;
    }

    private void ChangeSpatialBlend()
    {
        audioSource.spatialBlend = spatialBlend;
    }
}
