using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSettingController : MonoBehaviour
{
    [Header("Controller Setting")]
    [SerializeField] private AudioController audioController;
    [SerializeField] private CreateSoundController createSoundController;
    [Header("OVR Input Information")]
    [SerializeField] private GameObject indexFinger;

    [Header("target place text")]
    [SerializeField] private List<GameObject> targetPlaceList;
    [Header("Parameter Setting")]

    private const double requiredLength = 0.1;
    private int whichAudioParameter = 0;
    private int targetPlaceIndex = 0;



    public void SetTargetPlaceIndex(int index)
    {
        targetPlaceIndex = index;
    }

    void Update()
    {
        CheckIfAudioEnabled();
    }

    private void CheckIfAudioEnabled()
    {

        Vector3 targetPosition = targetPlaceList[targetPlaceIndex].transform.position;
        if (IsInSoundTeritory())
        {
            createSoundController.EnableAudio();
        }
        else
        {
            createSoundController.DisableAudio();
        }


    }




    private Vector3 CalculateControllerPositionAndTextDiff(int index)
    {

        Vector3 diff = GetRightIndexFingerPosition() - targetPlaceList[index].transform.position;
        return diff;
    }

    private Vector3 GetRightIndexFingerPosition()
    {
        return indexFinger.transform.position;
    }
    private void AudioSetting(int index)
    {
        if (whichAudioParameter == 0)
        {

            audioController.SetDiscreteExponentAudioSettingWithTargetText(CalculateControllerPositionAndTextDiff(index));

        }
        else if (whichAudioParameter == 1)
        {

            audioController.SetContinuousExponentAudioSettingWithTargetText(CalculateControllerPositionAndTextDiff(index));
        }
        else if (whichAudioParameter == 2)
        {
            audioController.SetContinuousAudioSetting(CalculateControllerPositionAndTextDiff(index));
        }
        else if (whichAudioParameter == 3)
        {
            audioController.SetDiscreteAudioSetting(CalculateControllerPositionAndTextDiff(index));
        }



    }


    private bool IsInSoundTeritory()
    {
        Vector3 rightControllerPosition = GetRightIndexFingerPosition();
        Vector3 targetPosition = targetPlaceList[targetPlaceIndex].transform.position;
        if ((Mathf.Abs(rightControllerPosition.x - targetPosition.x) < requiredLength) &&
        (Mathf.Abs(rightControllerPosition.y - targetPosition.y) < requiredLength)
    && (Mathf.Abs(rightControllerPosition.z - targetPosition.z) < requiredLength))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
