using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SpatialTracking;

public class AudioSettingController : MonoBehaviour
{
    [Header("Controller Setting")]
    [SerializeField] private AudioController audioController;
    [SerializeField] private CreateSoundController createSoundController;
    [Header("OVR Input Information")]
    [SerializeField] private GameObject indexFinger;

    [Header("target place text")]
    [SerializeField] private GameObject centralSoundObject;

    [Header("Parameter Setting")]
    [SerializeField] private int whichAudioParameter = 0;

    [Header("Debug Text")]
    [SerializeField] private TextMeshProUGUI debugText;

    // これぐらいが一番音として聞こえやすいのでは
    private const double requiredLength = 0.06;
    private const double zRequiredLength = 0.06;

    private bool isSound = true;

    void Start()
    {
        IsSound();
    }

    void Update()
    {
        if (isSound)
        {
            CheckIfAudioEnabled();
        }
    }

    private void CheckIfAudioEnabled()
    {


        // debugText.text = IsInSoundTeritory().ToString() + centralSoundObject.name;
        if (IsInSoundTeritory())
        {
            AudioSetting();
            createSoundController.EnableAudio();
        }
        else
        {
            createSoundController.DisableAudio();
        }


    }




    private Vector3 CalculateControllerPositionAndTextDiff()
    {

        Vector3 diff = GetRightIndexFingerPosition() - centralSoundObject.transform.position;
        return diff;
    }

    public Vector3 GetRightIndexFingerPosition()
    {
        return indexFinger.transform.position;
    }
    private void AudioSetting()
    {
        Vector3 positionDiff = CalculateControllerPositionAndTextDiff();

        if (whichAudioParameter == 0)
        {

            audioController.SetDiscreteExponentAudioSettingWithTargetText(positionDiff);

        }
        else if (whichAudioParameter == 1)
        {

            audioController.SetContinuousExponentAudioSettingWithTargetText(positionDiff);
        }
        else if (whichAudioParameter == 2)
        {
            audioController.SetContinuousAudioSetting(positionDiff);
        }
        else if (whichAudioParameter == 3)
        {
            audioController.SetDiscreteAudioSetting(positionDiff);
        }
        else if (whichAudioParameter == -1)
        {
            IsSound();
        }



    }


    private bool IsInSoundTeritory()
    {
        Vector3 rightControllerPosition = GetRightIndexFingerPosition();
        Vector3 targetPosition = centralSoundObject.transform.position;
        if ((Mathf.Abs(rightControllerPosition.x - targetPosition.x) < requiredLength) &&
        (Mathf.Abs(rightControllerPosition.y - targetPosition.y) < requiredLength)
    && (Mathf.Abs(rightControllerPosition.z - targetPosition.z) < zRequiredLength))
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    private void IsSound()
    {
        if (whichAudioParameter == -1)
        {
            isSound = false;
        }

    }
    public int GetWhichAudioParameter()
    {
        return whichAudioParameter;
    }
}
