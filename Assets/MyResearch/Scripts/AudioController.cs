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

public class AudioController : MonoBehaviour
{

    [Header("audio source setting")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] CreateSoundController soundController;
    [SerializeField] float spatialBlend = 1.0f;
    [SerializeField] private float amplitudeCoefficient = 5.0f;
    [SerializeField] private float amplitude;
    [SerializeField] private float frequencyCoefficient = 2.0f;
    [SerializeField] private float frequency;
    [SerializeField] private float panCoefficient = 10.0f;
    [SerializeField] private float pan;
    [SerializeField] public bool isAmplitudeInversion = false;

    [Header("scaling factor")]
    [SerializeField] float amplitudeScalingFactor = 1.0f; // この値を増減させて、影響度を調整
    [SerializeField] float frequencyScalingFactor = 1.0f; // この値を増減させて、影響度を調整


    // Start is called before the first frame update
    void Start()
    {
        soundController.gainCoefficient = amplitude * amplitudeCoefficient;
        soundController.frequencyCoefficient = frequency * frequencyCoefficient;
        soundController.panCoefficient = pan * panCoefficient;
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

        // Debug.Log("ans" + amplitude * amplitudeCoefficient);
        soundController.gainCoefficient = amplitude * amplitudeCoefficient;
        soundController.frequencyCoefficient = frequency * frequencyCoefficient;
        soundController.panCoefficient = pan * panCoefficient;

    }

    private void ChangeSpatialBlend()
    {
        audioSource.spatialBlend = spatialBlend;
    }
}
