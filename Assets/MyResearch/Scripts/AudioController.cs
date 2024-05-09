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
    [SerializeField] private float amplitudeCoefficient = 0.5f;
    [SerializeField] private float amplitude;
    [SerializeField] private float frequencyCoefficient = 2.0f;
    [SerializeField] private float frequency;
    [SerializeField] private float panCoefficient = 15.0f;
    [SerializeField] private float pan;
    [SerializeField] public bool isAmplitudeInversion = false;

    [Header("scaling factor")]
    [SerializeField] float amplitudeScalingFactor = 1.0f; // この値を増減させて、影響度を調整
    [SerializeField] float frequencyScalingFactor = 1.0f; // この値を増減させて、影響度を調整


    // Start is called before the first frame update
    void Start()
    {
        ReflectAudioSettings();
        audioSource.loop = true;
        audioSource.Play();
        ChangeSpatialBlend();
    }


    public void SetAudioSetting(Vector3 controllerPosition)
    {



        if (isAmplitudeInversion)
        {
            amplitude = 0.5f + amplitudeScalingFactor * controllerPosition.z;
        }
        else
        {
            amplitude = 1.0f - amplitudeScalingFactor * controllerPosition.z;
        }


        frequency = 0.5f + frequencyScalingFactor * controllerPosition.y;
        pan = controllerPosition.x;


        // amplitude = 0.5f;
        // frequency = 0.5f;
        // pan = controllerPosition.x;

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
