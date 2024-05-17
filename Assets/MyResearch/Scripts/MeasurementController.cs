using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeasurementController : MonoBehaviour
{

    [SerializeField]
    private FingerPaint fingerPaint;

    // [SerializeField] private GetHandInformation handInformation;
    [SerializeField]
    private ExperienceController experienceController;


    [SerializeField] private bool isMeasuring;
    [SerializeField] private int count = 0;


    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Three))
        {

            if (!isMeasuring)
            {

                Debug.Log("Measurement and drawing started");
                count++;
                experienceController.StartMeasurement(count);
                fingerPaint.StartDrawing(); // Start drawing
                // handInformation.StartMeasurement(); // Start hand information recording
                isMeasuring = true;
            }
        }
        else if (OVRInput.GetUp(OVRInput.Button.Three))
        {
            isMeasuring = false;

            Debug.Log("Measurement and drawing stopped");
            experienceController.EndMeasurement();
            fingerPaint.StopDrawing(); // Stop drawing
            // handInformation.EndMeasurement(); // Stop hand information recording

        }
    }

}
