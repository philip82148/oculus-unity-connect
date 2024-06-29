using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MeasurementController : MonoBehaviour
{

    [SerializeField]
    private FingerPaint fingerPaint;


    [Header("Controller Setting")]
    [SerializeField]
    private ExperienceController experienceController;
    [SerializeField] private ProgressController progressController;
    [Header("Debug UI")]
    [SerializeField] private TextMeshProUGUI measurementText;



    private bool isMeasuring;
    private int count = 0;

    private float startTime;



    public void Initialize()
    {
        count = 0;

    }

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Three))
        {
            if (!isMeasuring)
            {

                Debug.Log("Measurement and Drawing Started");
                count++;
                experienceController.StartMeasurement(count);
                startTime = Time.time;
                isMeasuring = true;


            }
        }
        else if (OVRInput.GetDown(OVRInput.Button.Four))
        {
            if (isMeasuring)
            {
                float endTime = Time.time;
                float duration = endTime - startTime;
                experienceController.EndMeasurement();
                experienceController.WriteTimeInformation(duration);
                progressController.SubtractCount();
                Debug.Log("Measurement and drawing stopped:" + count);
                isMeasuring = false;

            }
        }
        if (isMeasuring)
        {
            measurementText.text = "Recording";
        }
        else
        {
            measurementText.text = "";
        }
    }


}
