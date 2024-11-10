using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CalculateSound : MonoBehaviour
{
    [SerializeField] private CreateSoundController createSoundController;
    [SerializeField] private DenseSparseExpController denseSparseExpController;
    [SerializeField] private CalculateDistance calculateDistance;



    private float minFrequency = 220f;
    private float maxFrequency = 660f;

    private float minAmplitude = 0.2f;
    private float maxAmplitude = 1.0f;
    private double calculatedAmplitude;
    private double calculatedFrequency;
    [SerializeField] private double soundLength = 1.0f;
    [SerializeField]
    private TextMeshProUGUI debugText;

    private float xDiff = 0;
    private float yDiff = 0;
    private float zDiff = 0;


    void Start()
    {
        // centralFrequency = (minFrequency + maxFrequency) / 2;
        // createSoundController.SetAmplitude(0);
        // CalculateSoundLength();
        CalculateExponentialFrequency();
        CalculateExponentialAmplitude();
        Debug.Log("sound length:" + soundLength);
    }

    void Update()
    {
        CalculateSoundLength();
        debugText.text = calculatedFrequency.ToString("f2");

    }
    public void SetCoordinateDiff(Vector3 diff)
    {
        this.yDiff = diff.y;
        this.zDiff = diff.z;
        this.xDiff = diff.x;

    }
    public void SetYDiff(float yDiff)
    {
        this.yDiff = yDiff;
        CalculateExponentialFrequency();

        SetFrequency();
    }
    public void SetZDiff(float zDiff)
    {
        this.zDiff = zDiff;
        CalculateExponentialAmplitude();

    }
    public void SetInitial()
    {
        createSoundController.SetFrequencySelf(0);
        createSoundController.SetAmplitude(0);
    }


    private void CalculateFrequency()
    {
        // calculatedFrequency = centralFrequency - yDiff / soundLength * (maxFrequency - minFrequency) / 2;
        calculatedFrequency = minFrequency + (maxFrequency - minFrequency) / (2 * soundLength) * (yDiff + soundLength);
    }
    private void CalculateExponentialFrequency()
    {
        double t = (yDiff + soundLength) / (2 * soundLength);
        float frequencyRatio = minFrequency / (float)maxFrequency;
        calculatedFrequency = minFrequency * Mathf.Pow(frequencyRatio, (float)t);
    }
    private void CalculateExponentialAmplitude()
    {
        double t = (zDiff + soundLength) / (2 * soundLength);
        float amplitudeRatio = minAmplitude / (float)maxAmplitude;
        calculatedAmplitude = minAmplitude * Mathf.Pow(amplitudeRatio, (float)t);

    }
    private void SetFrequency()
    {
        createSoundController.SetFrequencySelf((float)calculatedFrequency);
        createSoundController.SetAmplitude(0.5f);

    }
    private void SetAudio()
    {
        createSoundController.SetFrequencySelf((float)calculatedFrequency);
        createSoundController.SetAmplitude(0.5f);
    }

    private void CalculateSoundLength()
    {
        DenseOrSparse denseOrSparse = denseSparseExpController.GetDenseOrSparse();
        if (denseOrSparse == DenseOrSparse.Dense)
        {
            soundLength = calculateDistance.GetCentralRequiredLength();
        }
        else if (denseOrSparse == DenseOrSparse.Sparse)
        {
            soundLength = calculateDistance.GetRequiredLength();
        }
    }
}
