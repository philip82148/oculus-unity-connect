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
    private float amplitude;

    private float frequency;

    private float pan;
    [SerializeField] public bool isAmplitudeInversion = false;
    [Header("subtle")]
    [SerializeField] private float yOffset = 0.0f;
    [SerializeField] private float zOffset = -0.01f;


    const float discreteAmplitudeScalingFactor = 2.0f; // この値を増減させて、影響度を調整
    const float discreteFrequencyScalingFactor = 2.0f; // この値を増減させて、影響度を調整
    const float amplitudeScalingFactor = 12.0f; // この値を増減させて、影響度を調整
    const float frequencyScalingFactor = 4.0f; // この値を増減させて、影響度を調整
    [SerializeField] float frequencyCoefficient = 4.0f;


    private const float discreteFactor = 0.01f;
    private const float initialAmplitude = 2.0f;


    void Start()
    {

        ReflectAudioSettings();
        audioSource.loop = true;
        audioSource.Play();
        ChangeSpatialBlend();
    }



    public void SetContinuousAudioSetting(Vector3 targetDiff)
    {

        amplitude = initialAmplitude * (1 - amplitudeScalingFactor * targetDiff.z);
        frequency = 1.0f + frequencyScalingFactor * targetDiff.y;
        pan = 10 * targetDiff.x;

        ReflectAudioSettings();

    }

    public void SetDiscreteAudioSetting(Vector3 targetDiff)
    {


        float discreteX = Mathf.Floor(targetDiff.x / discreteFactor) * discreteFactor;
        float discreteY = Mathf.Floor(targetDiff.y / discreteFactor) * discreteFactor;
        float discreteZ = Mathf.Floor(targetDiff.z / discreteFactor) * discreteFactor;


        amplitude = initialAmplitude * (1 - amplitudeScalingFactor * targetDiff.z);
        frequency = 1.0f + frequencyScalingFactor * discreteY;
        pan = 10 * discreteX;



        ReflectAudioSettings();

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



    public void SetAudioSettingOnlyAmplitudeWithTargetText(Vector3 targetDiff)
    {
        float zOffset = -0.1f;
        float discreteZ = Mathf.Floor((targetDiff.z + zOffset) / discreteFactor) * discreteFactor;
        amplitude = Mathf.Pow(discreteAmplitudeScalingFactor, -discreteZ / discreteFactor);

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
        // frequency が負の値にならないようにチェック
        if (amplitude < 0)
        {
            amplitude = 0;
        }
        // frequency が負の値にならないようにチェック
        if (frequency < 0)
        {
            frequency = 0;
        }

        soundController.SetAmplitude(amplitude);
        soundController.frequencyCoefficient = frequency;
        soundController.SetPan(pan);
    }

    private void ChangeSpatialBlend()
    {
        audioSource.spatialBlend = spatialBlend;
    }
}
