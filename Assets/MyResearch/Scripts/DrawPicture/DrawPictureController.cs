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
    [SerializeField] private DataLoggerController dataLoggerController;

    [SerializeField]
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
        if (OVRInput.GetDown(OVRInput.Button.Three))
        {
            if (isMeasuring == false)
            {
                isMeasuring = true;
            }
        }
        if (isMeasuring)
        {
            WriteInformation();
        }


    }

    private string SetupFilePath()
    {
        string dateTime = DateTime.Now.ToString("yyyyMMddHHmmss");

        string folder = $"C:\\Users\\takaharayota\\Research\\Exp-Draw-Picture\\";
        string fileName = $"{dateTime}.txt";
        return System.IO.Path.Combine(folder, fileName);
    }


    private void WriteInformation()
    {
        dataLoggerController.WriteInformation(rightControllerPosition);

    }

}
