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
    [SerializeField] private Vector3 targetPosition;
    [SerializeField]

    private List<GameObject> targetPlaceList;
    [SerializeField] private int targetPlaceTextIndex = 0;



    [Header("main setting")]

    [SerializeField] private CreateSoundController createSoundController;

    private bool isMeasuring = false;


    [SerializeField]
    private Vector3
         rightControllerPosition;
    [SerializeField] private Vector3 diff;


    private float movementSpeed;

    private string filePath;



    [SerializeField] private bool isSound = true;
    [SerializeField] private int experienceCount = 1;


    [Header("Controller Setting")]
    [SerializeField] private AudioController audioController;
    [SerializeField] private DataLoggerController dataLoggerController;

    [SerializeField] private TimeLoggerController timeLoggerController;
    [SerializeField] private ProgressController counterController;

    [SerializeField] private bool isAmplitudeInversion = false;

    [Header("parameter setting")]

    [SerializeField]
    private int whichAudioParameter = 0;
    [SerializeField]
    private double requiredLength = 0.15;




    void Start()
    {
        Initialize();



    }

    void Initialize()
    {
        audioController.isAmplitudeInversion = isAmplitudeInversion;

        filePath = SetupFilePath(0);
        dataLoggerController.Initialize(filePath);

        timeLoggerController.Initialize(SetupFilePath(1));





    }

    private string SetupFilePath(int way)
    {
        string dateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
        string directory = isSound ? $"{experienceCount}exp_withsound" : $"{experienceCount}exp_withoutsound";
        string folder = $"C:\\Users\\takaharayota\\Research\\data\\0623\\{directory}\\{whichAudioParameter}";

        Directory.CreateDirectory(folder);
        Debug.Log("create folder");

        string fileName;

        // string filePlaceTextName;
        // if (targetPlaceTextIndex == 0) filePlaceTextName = "(0.25,-0.1,0.25)";
        // else if (targetPlaceTextIndex == 1) filePlaceTextName = "(0.25,0.15,0.25)";
        // else if (targetPlaceTextIndex == 2) filePlaceTextName = "(0.25,-0.1,0.5)";
        // else if (targetPlaceTextIndex == 3) filePlaceTextName = "(0.25,0.15,0.5)";
        // else if (targetPlaceTextIndex == 4) filePlaceTextName = "(0.25,-0.1,0.75)";
        // else filePlaceTextName = "(0.25,0.15,0.75)";

        if (way == 0)

        {
            fileName = $"{targetPlaceTextIndex}_{whichAudioParameter}_{dateTime}.txt";
        }
        else
        {
            folder = $"C:\\Users\\takaharayota\\Research\\data\\0623\\times\\{directory}\\{whichAudioParameter}";
            Directory.CreateDirectory(folder);

            fileName = $"time_{targetPlaceTextIndex}_{whichAudioParameter}.txt";
        }
        return System.IO.Path.Combine(folder, fileName);
    }


    public void StartMeasurement(int count)
    {
        dataLoggerController.CountAdd(count);
        counterController.SubtractCount();

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


        // Oculus Touchの右コントローラーのスティック入力を取得
        Vector2 stickInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);


        if (isMeasuring)
        {
            Debug.Log("start data measurement");
            WriteInformation();
        }

    }


    private void CheckIfAudioEnabled()
    {

        bool isAudioFlag = false;

        for (int i = 0; i < targetPlaceList.Count; i++)
        {

            targetPosition = targetPlaceList[i].transform.position;
            if ((Mathf.Abs(rightControllerPosition.x - targetPosition.x) < requiredLength) &&
            (Mathf.Abs(rightControllerPosition.y - targetPosition.y) < requiredLength)
        && (Mathf.Abs(rightControllerPosition.z - targetPosition.z) < requiredLength))
            {
                isAudioFlag = true;
                AudioSetting(i);

                break;
            }
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




    private Vector3 GetRightControllerPosition()
    {
        Vector3 controllerPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);

        return controllerPosition;

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



    }



    private void OnDestroy()
    {
        dataLoggerController.Close();
        timeLoggerController.Close();
    }







}
