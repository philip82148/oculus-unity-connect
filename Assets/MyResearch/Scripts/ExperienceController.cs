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



public class ExperienceController : MonoBehaviour
{




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
        string fileName = $"OpenBrushData_{dateTime}.txt";

        return System.IO.Path.Combine($"C:\\Users\\takaharayota\\Research\\data\\0506\\{directory}", fileName);
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

        previousRightControllerPosition = rightControllerPosition;

        // Oculus Touchの右コントローラーのスティック入力を取得
        Vector2 stickInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, OVRInput.Controller.RTouch);


        if (isMeasuring)
        {
            Debug.Log("start data measurement");
            WriteInformation();
        }

    }




    private void WriteInformation()
    {
        dataLoggerController.WriteInformation(rightControllerPosition);

    }

    private Vector3 GetRightControllerPosition()
    {
        Vector3 controllerPosition = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
        Debug.Log("controller position" + controllerPosition);
        return controllerPosition;

    }
    private Vector3 GetPointerPosition()
    {
        return Input.mousePosition;

    }

    private void AudioSetting()
    {
        audioController.SetAudioSetting(rightControllerPosition);

    }



    private void OnDestroy()
    {
        dataLoggerController.Close();
    }







}
