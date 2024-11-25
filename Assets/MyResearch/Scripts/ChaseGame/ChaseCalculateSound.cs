using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseCalculateSound : MonoBehaviour
{
    [SerializeField] private CreateSoundController createSoundController;



    private float minFrequency = 200f;
    private float maxFrequency = 900f;
    private float amplitudeRange = 16.0f;

    private float minAmplitude = 1 / 16f;
    private float maxAmplitude = 1.0f;
    private double calculatedAmplitude;
    float fixedAmplitude;
    private double calculatedFrequency;
    private double calculatedPan;


    [SerializeField] private double soundLength = 0.15f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetCoordinateDiff(Vector3 diff)
    {
        // this.yDiff = diff.y;
        // this.zDiff = diff.z;
        // this.xDiff = diff.x;
        CalculateExponentialFrequency(diff.y);

        // CalculateExponentialAmplitude();
        // CalculateExponentialPan();
        // SetAudio();
        SetFrequency();
    }
    private void CalculateExponentialFrequency(float yDiff)
    {
        // double calculatedFrequency;

        double t = (yDiff + soundLength) / (2 * soundLength);
        double frequencyExponent = ConvertToDiscrete(t);
        float frequencyRatio = maxFrequency / (float)minFrequency;
        calculatedFrequency = minFrequency * Mathf.Pow(frequencyRatio, (float)frequencyExponent);

        // return calculatedFrequency;
    }
    // private double CalculateExponentialAmplitude(float zDiff)
    // {
    //     tZ = (zDiff + soundLength) / (2 * soundLength);
    //     double convertedT = ConvertToDiscrete(tZ);
    //     float amplitudeRatio = maxAmplitude / (float)minAmplitude;
    //     calculatedAmplitude = minAmplitude * Mathf.Pow(amplitudeRatio, 1 - (float)convertedT);
    //     fixedAmplitude = CorrectLoudness((float)calculatedAmplitude);
    //     return fixedAmplitude;

    // }
    // private double CalculateExponentialPan(float xDiff)
    // {
    //     // double calculatedPan;

    //     if (-soundLength < xDiff && xDiff < -objectLength) calculatedPan = -1;
    //     else if (-objectLength <= xDiff && xDiff <= objectLength) calculatedPan = 0;
    //     else if (objectLength < xDiff && xDiff < soundLength) calculatedPan = 1;
    //     else calculatedPan = 0;


    //     return calculatedPan;

    // }

    // private void SetAudio()
    // {
    //     createSoundController.SetFrequencySelf((float)calculatedFrequency);
    //     createSoundController.SetAmplitude((float)calculatedAmplitude);
    //     createSoundController.SetPan((float)calculatedPan);
    // }


    private double ConvertToDiscrete(double t)
    {
        if (0 <= t && t < 1.0 / 3.0) return 0;
        else if (1.0 / 3.0 <= t && t <= 2.0 / 3.0) return 0.5;
        else if (2.0 / 3.0 < t && t <= 1.0) return 1;
        else return 0;
    }
    private void SetFrequency()
    {
        createSoundController.SetFrequencySelf((float)calculatedFrequency);
        createSoundController.SetAmplitude(1.0f);

    }
    private void CalculateSoundLength()
    {

    }
}
