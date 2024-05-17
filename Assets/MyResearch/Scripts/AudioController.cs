using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Text;
using TMPro;
using System;
using Microsoft.Win32.SafeHandles;
using System.Runtime.Serialization;
using System.Runtime.ConstrainedExecution;

public class AudioController : MonoBehaviour
{


    [Header("audio source setting")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] CreateSoundController soundController;
    [SerializeField] float spatialBlend = 1.0f;
    [SerializeField] private const float amplitudeCoefficient = 0.5f;
    [SerializeField] private float amplitude;
    private const float frequencyCoefficient = 4.0f;
    [SerializeField] private float frequency;
    [SerializeField] private const float panCoefficient = 15.0f;
    [SerializeField] private float pan;
    [SerializeField] public bool isAmplitudeInversion = false;

    [Header("scaling factor")]
    [SerializeField] float amplitudeScalingFactor = 1.0f; // この値を増減させて、影響度を調整
    [SerializeField] float frequencyScalingFactor = 1.0f; // この値を増減させて、影響度を調整
    [SerializeField] float controllerYPosition;

    private float discreteFactor = 0.005f;


    void Start()
    {
        ReflectAudioSettings();
        audioSource.loop = true;
        audioSource.Play();
        ChangeSpatialBlend();
    }


    public void SetAudioSetting(Vector3 controllerPosition)
    {



        // if (isAmplitudeInversion)
        // {
        //     amplitude = 0.5f + amplitudeScalingFactor * controllerPosition.z;
        // }
        // else
        // {
        amplitude = 1.0f - amplitudeScalingFactor * controllerPosition.z;
        // }


        frequency = 0.5f + frequencyScalingFactor * controllerPosition.y;
        pan = controllerPosition.x;


        // amplitude = 0.5f;
        // frequency = 0.5f;
        // pan = controllerPosition.x;

        ReflectAudioSettings();

    }
    public void SetAudioSettingOnlyAmplitude(Vector3 controllerPosition)
    {
        float discreteZ = Mathf.Floor(controllerPosition.z / discreteFactor) * discreteFactor;
        // amplitude = 1.0f - amplitudeScalingFactor * controllerPosition.z;
        amplitude = 1.0f - amplitudeScalingFactor * discreteZ;
        frequency = 0.5f;
        pan = 0;

        ReflectAudioSettings();

    }


    // public void SetAudioSettingOnlyFrequency(Vector3 controllerPosition)
    // {

    //     float discreteY = Mathf.Floor((controllerPosition.y + 0.7f) / discreteFactor) * discreteFactor;
    //     controllerYPosition = controllerPosition.y;


    //     amplitude = 0.25f;
    //     frequency = frequencyScalingFactor * discreteY;
    //     if (frequency < 0)
    //     {
    //         frequency = 0;
    //     }

    //     pan = 0;

    //     ReflectAudioSettings();

    // }


    public void SetAudioSettingOnlyFrequency(Vector3 controllerPosition)
    {
        // frequency の基本値（controllerPosition.y が 0 のときの値）
        float baseFrequency = 1.0f;  // 適切な基本周波数を設定する
        amplitude = 0.25f;
        float discreteY = Mathf.Floor((controllerPosition.y + 0.05f - 0.25f) / discreteFactor) * discreteFactor + 0.05f;
        // 1cm 増えるごとに 1.5 倍になるための計算
        frequency = baseFrequency * Mathf.Pow(frequencyScalingFactor, discreteY * 100);  // controllerPosition.y がメートル単位なので、100 を掛けてセンチメートル単位に変換
        controllerYPosition = controllerPosition.y;
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
        amplitude = 1.0f;
        frequency = 0.5f;
        pan = 0;

        ReflectAudioSettings();

    }








    public void SetAudioSettingWithPolar(Vector3 controllerPosition)
    {
        float radius = MathF.Sqrt(controllerPosition.x * controllerPosition.x + controllerPosition.z * controllerPosition.z + controllerPosition.y * controllerPosition.y);
        float azimuth = MathF.Atan2(MathF.Abs(controllerPosition.z), MathF.Abs(controllerPosition.x));
        float elevation = MathF.Asin(controllerPosition.y / radius) / 90;
        amplitude = 1.0f - amplitudeScalingFactor * radius;



        frequency = 0.5f + frequencyScalingFactor * controllerPosition.y;
        pan = controllerPosition.x;


        ReflectAudioSettings();
    }

    public void SetAudioSettingWithWeberFechner(Vector3 controllerPosition)
    {
        float amplitudeStimulus = MathF.Max(0.01f, controllerPosition.z);
        float frequencyStimulus = MathF.Max(0.01f, controllerPosition.y);

        amplitude = 1.0f - amplitudeScalingFactor * Mathf.Log10(amplitudeStimulus + 1);
        frequency = 0.5f + frequencyScalingFactor * Mathf.Log10(frequencyStimulus + 1);
        pan = controllerPosition.x;

        ReflectAudioSettings();




    }

    private void ReflectAudioSettings()
    {
        soundController.gain = amplitude * amplitudeCoefficient;
        soundController.frequencyCoefficient = frequency * frequencyCoefficient;
        soundController.pan = pan * panCoefficient;
    }

    private void ChangeSpatialBlend()
    {
        audioSource.spatialBlend = spatialBlend;
    }
}
