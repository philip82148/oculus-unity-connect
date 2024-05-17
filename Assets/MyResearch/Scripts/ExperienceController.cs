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
// using System.Diagnostics;
// using System.Diagnostics;



public class ExperienceController : MonoBehaviour
{


    [Header("target place text")]
    [SerializeField] private Vector3 targetPosition;
    [SerializeField]

    private TextMeshPro targetPlaceText;


    [Header("main setting")]

    [SerializeField] private CreateSoundController createSoundController;

    private bool isMeasuring = false;


    [SerializeField]
    private Vector3
         rightControllerPosition;

    private Vector3
    previousRightControllerPosition;
    private float movementSpeed;
    // private Vector3 acceleration;
    private string filePath;



    [SerializeField] private bool isSound = true;
    [SerializeField] private int experienceCount = 1;

    // [SerializeField] private bool isMeasuringEveryInformation = false;

    [Header("Controller Setting")]
    [SerializeField] private AudioController audioController;
    [SerializeField] private DataLoggerController dataLoggerController;

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

        filePath = SetupFilePath();
        dataLoggerController.Initialize(filePath);


        previousRightControllerPosition = GetRightControllerPosition();


    }

    private string SetupFilePath()
    {
        string dateTime = DateTime.Now.ToString("yyyyMMddHHmmss");
        string directory = isSound ? $"{experienceCount}exp_withsound" : $"{experienceCount}exp_withoutsound";
        string folder = $"C:\\Users\\takaharayota\\Research\\data\\0516\\{directory}";

        Directory.CreateDirectory(folder);
        Debug.Log("create folder");

        string fileName = $"OpenBrushData_{dateTime}_{whichAudioParameter}.txt";

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

    void Update()
    {
        rightControllerPosition = GetRightControllerPosition();
        if (isSound)
        {
            AudioSetting();
        }
        CheckIfAudioEnabled();

        previousRightControllerPosition = rightControllerPosition;

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
        // double requiredLength = 0.15;

        // Debug.Log("math abs: " + Mathf.Abs(rightControllerPosition.x - targetPlaceText.transform.position.x));
        targetPosition = targetPlaceText.transform.localPosition;
        if ((Mathf.Abs(rightControllerPosition.x - targetPlaceText.transform.position.x) < requiredLength) &&
        (Mathf.Abs(rightControllerPosition.y - targetPlaceText.transform.position.y) < requiredLength)
    && (Mathf.Abs(rightControllerPosition.z - targetPlaceText.transform.position.z) < requiredLength))
        {
            createSoundController.EnableAudio();
        }
        else
        {
            // createSoundController.EnableAudio();
            createSoundController.DisableAudio();
        }
    }




    private void WriteInformation()
    {
        dataLoggerController.WriteInformation(rightControllerPosition);

    }

    private Vector3 GetRightControllerPosition()
    {
        Vector3 controllerPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);

        return controllerPosition;

    }


    private void AudioSetting()
    {
        if (whichAudioParameter == 0)
        {

            audioController.SetAudioSetting(rightControllerPosition);
        }
        else if (whichAudioParameter == 1)
        {
            audioController.SetAudioSettingOnlyAmplitude(rightControllerPosition);
        }
        else if (whichAudioParameter == 2)
        {
            audioController.SetAudioSettingOnlyFrequency(rightControllerPosition);
        }
        else if (whichAudioParameter == 3)
        {
            audioController.SetAudioSettingOnlyPan(rightControllerPosition);
        }
        // audioController.SetAudioSettingWithPolar(rightControllerPosition);
        // audioController.SetAudioSettingWithWeberFechner(rightControllerPosition);

    }



    private void OnDestroy()
    {
        dataLoggerController.Close();
    }







}
