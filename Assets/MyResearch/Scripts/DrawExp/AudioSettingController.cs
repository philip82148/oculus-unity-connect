using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    [SerializeField] private int whichAudioParameter = 0;

    [Header("Debug Text")]
    [SerializeField] private TextMeshProUGUI debugText;

    private const double requiredLength = 0.1;

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
        debugText.text = IsInSoundTeritory().ToString();
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




    private Vector3 CalculateControllerPositionAndTextDiff(int index)
    {

        Vector3 diff = GetRightIndexFingerPosition() - targetPlaceList[index].transform.position;
        return diff;
    }

    private Vector3 GetRightIndexFingerPosition()
    {
        return indexFinger.transform.position;
    }
    private void AudioSetting()
    {
        if (whichAudioParameter == 0)
        {

            audioController.SetDiscreteExponentAudioSettingWithTargetText(CalculateControllerPositionAndTextDiff(targetPlaceIndex));

        }
        else if (whichAudioParameter == 1)
        {

            audioController.SetContinuousExponentAudioSettingWithTargetText(CalculateControllerPositionAndTextDiff(targetPlaceIndex));
        }
        else if (whichAudioParameter == 2)
        {
            audioController.SetContinuousAudioSetting(CalculateControllerPositionAndTextDiff(targetPlaceIndex));
        }
        else if (whichAudioParameter == 3)
        {
            audioController.SetDiscreteAudioSetting(CalculateControllerPositionAndTextDiff(targetPlaceIndex));
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
