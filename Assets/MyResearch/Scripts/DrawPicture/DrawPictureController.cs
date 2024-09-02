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

public class DrawPictureController : MonoBehaviour
{


    [Header("Controller Setting")]
    [SerializeField] private AudioController audioController;
    [SerializeField] private FingerPaintController fingerPaintController;
    [SerializeField] private DataLoggerController dataLoggerController;
    [Header("OVR Input Information")]
    [SerializeField] private GameObject indexFinger;

    private Vector3
          rightControllerPosition;


    private bool isMeasuring = false;


    // Start is called before the first frame update
    void Start()
    {
        string fileName = SetupFilePath();
        dataLoggerController.Initialize(fileName);
    }

    // Update is called once per frame
    void Update()
    {
        rightControllerPosition = indexFinger.transform.position;
        if (OVRInput.Get(OVRInput.Button.One))
        {

            if (isMeasuring == false)
            {
                isMeasuring = true;
                fingerPaintController.StartDrawing();
            }
            WriteInformation();

        }
        else
        {
            if (isMeasuring)
            {
                isMeasuring = false;
                fingerPaintController.StopDrawing();
            }
        }


    }

    private string SetupFilePath()
    {
        string dateTime = DateTime.Now.ToString("yyyyMMddHHmmss");

        string folder = $"C:\\Users\\takaharayota\\Research\\Exp-Draw-Picture";
        Directory.CreateDirectory(folder);
        string fileName = $"{dateTime}.txt";
        return System.IO.Path.Combine(folder, fileName);
    }


    private void WriteInformation()
    {
        dataLoggerController.WriteInformation(rightControllerPosition);

    }

    private Vector3 GetRightIndexFingerPosition()
    {

        return indexFinger.transform.position;

    }


}
