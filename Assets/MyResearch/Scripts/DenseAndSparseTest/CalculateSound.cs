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
    private double centralFrequency;
    private double calculatedFrequency;
    [SerializeField] private double soundLength = 1.0f;
    [SerializeField]
    private TextMeshProUGUI debugText;

    private float yDiff = 0;


    void Start()
    {
        centralFrequency = (minFrequency + maxFrequency) / 2;
        // createSoundController.SetAmplitude(0);
        CalculateSoundLength();
        Debug.Log("sound length:" + soundLength);
    }

    void Update()
    {
        CalculateSoundLength();
        debugText.text = calculatedFrequency.ToString("f2");

    }
    public void SetYDiff(float yDiff)
    {
        this.yDiff = yDiff;
        CalculateFrequency();
        SetFrequency();
    }
    public void SetInitial()
    {
        createSoundController.SetFrequencySelf(0);
        createSoundController.SetAmplitude(0);
    }


    private void CalculateFrequency()
    {
        // calculatedFrequency = centralFrequency - yDiff / soundLength * (maxFrequency - minFrequency) / 2;
        calculatedFrequency = minFrequency + (maxFrequency - minFrequency) / soundLength * (yDiff + soundLength / 2);
    }
    private void CalculateExponentialFrequency()
    {
        float t = yDiff / (float)soundLength;
        float frequencyRatio = minFrequency / (float)maxFrequency;
        calculatedFrequency = minFrequency * Mathf.Pow(frequencyRatio, t);
    }
    private void SetFrequency()
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
            soundLength = ((denseSparseExpController.GetObjectCount() - 1) * denseSparseExpController.GetInterval() + 2 *
            calculateDistance.GetRequiredLength()) / 2;
        }
    }
}
