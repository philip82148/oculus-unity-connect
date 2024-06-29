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


public class ExperienceController : MonoBehaviour
{

    [Header("OVR Input Information")]
    [SerializeField] private GameObject indexFinger;


    [Header("target place text")]

    [SerializeField] private List<GameObject> targetPlaceList;
    [SerializeField] private int targetPlaceTextIndex = 0;

    [Header("main setting")]

    [SerializeField] private CreateSoundController createSoundController;

    private bool isMeasuring = false;


    [SerializeField]
    private Vector3
         rightControllerPosition;
    [SerializeField] private Vector3 diff;

    private string filePath;
    private bool isSound = true;


    [Header("Controller Setting")]
    [SerializeField] private AudioController audioController;
    [SerializeField] private DataLoggerController dataLoggerController;

    [SerializeField] private TimeLoggerController timeLoggerController;


    [SerializeField] private bool isAmplitudeInversion = false;

    [Header("parameter setting")]

    [SerializeField]
    private int whichAudioParameter = 0;
    [SerializeField]
    private double requiredLength = 0.15;
    [Header("Subject Name")]
    [SerializeField] private string subjectName = "高原陽太";





    void Start()
    {

        if (whichAudioParameter == -1)
        {
            isSound = false;
        }
    }


    public void Initialize()
    {

        audioController.isAmplitudeInversion = isAmplitudeInversion;
        filePath = SetupFilePath(0);
        dataLoggerController.Initialize(filePath);
        timeLoggerController.Initialize(SetupFilePath(1));

    }

    private string SetupFilePath(int isTime)
    {
        string dateTime = DateTime.Now.ToString("yyyyMMddHHmmss");

        string folder = $"C:\\Users\\takaharayota\\Research\\Exp1-data\\{subjectName}\\{whichAudioParameter}";

        Directory.CreateDirectory(folder);

        string fileName;

        if (isTime == 0)

        {
            fileName = $"{isSound}_{targetPlaceTextIndex}_{whichAudioParameter}_{dateTime}.txt";
        }
        else
        {
            folder = $"C:\\Users\\takaharayota\\Research\\Exp1-data\\{subjectName}\\times";
            Directory.CreateDirectory(folder);

            fileName = $"{isSound}_time_{targetPlaceTextIndex}_{whichAudioParameter}.txt";
        }
        return System.IO.Path.Combine(folder, fileName);
    }


    public void StartMeasurement(int count)
    {
        dataLoggerController.CountAdd(count);

        isMeasuring = true;

    }
    public void EndMeasurement()
    {
        isMeasuring = false;
    }

    public void EndTimeMeasurement(float time)
    {

        timeLoggerController.WriteTimeInformation(time);
    }

    void Update()
    {
        // rightControllerPosition = GetRightControllerPosition();
        rightControllerPosition = GetRightIndexFingerPosition();
        if (isSound)
        {

            CheckIfAudioEnabled();
        }


        if (isMeasuring)
        {
            Debug.Log("start data measurement");
            WriteInformation();
        }

    }


    private void CheckIfAudioEnabled()
    {

        bool isAudioFlag = false;



        Vector3 targetPosition = targetPlaceList[targetPlaceTextIndex].transform.position;
        if ((Mathf.Abs(rightControllerPosition.x - targetPosition.x) < requiredLength) &&
        (Mathf.Abs(rightControllerPosition.y - targetPosition.y) < requiredLength)
    && (Mathf.Abs(rightControllerPosition.z - targetPosition.z) < requiredLength))
        {
            isAudioFlag = true;
            AudioSetting(targetPlaceTextIndex);


        }

        if (isAudioFlag)
        {
            createSoundController.EnableAudio();
        }
        else
        {
            createSoundController.DisableAudio();
        }
    }




    private void WriteInformation()
    {
        dataLoggerController.WriteInformation(rightControllerPosition);

    }

    public void WriteTimeInformation(double time)
    {
        timeLoggerController.WriteTimeInformation(time);


    }

    private Vector3 GetRightIndexFingerPosition()
    {

        return indexFinger.transform.position;

    }

    private Vector3 CalculateControllerPositionAndTextDiff(int index)
    {

        diff = rightControllerPosition - targetPlaceList[index].transform.position;
        return diff;
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
        else if (whichAudioParameter == -1)
        {
            isSound = false;
        }



    }
    public void SetTargetPlaceIndex(int placeIndex)
    {
        targetPlaceTextIndex = placeIndex;

    }
    public void SetTargetPlaceChange(int placeIndex)
    {
        SetTargetPlaceIndex(placeIndex);
        dataLoggerController.ReflectPlaceChange(targetPlaceTextIndex);
        timeLoggerController.ReflectPlaceChange(targetPlaceTextIndex);
    }





    private void OnDestroy()
    {
        dataLoggerController.Close();
        timeLoggerController.Close();
    }







}
