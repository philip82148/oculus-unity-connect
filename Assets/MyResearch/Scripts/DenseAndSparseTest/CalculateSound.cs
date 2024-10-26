using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CalculateSound : MonoBehaviour
{
    [SerializeField] private CreateSoundController createSoundController;



    private float minFrequency = 660f;
    private float maxFrequency = 220f;
    private float centralFrequency;
    private float calculatedFrequency;
    [SerializeField] private float soundLength = 1.0f;
    [SerializeField]
    private TextMeshProUGUI debugText;

    private float yDiff = 0;


    void Start()
    {
        centralFrequency = (minFrequency + maxFrequency) / 2;
        // createSoundController.SetAmplitude(0);

    }

    void Update()
    {

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
        calculatedFrequency = centralFrequency - yDiff / soundLength * (maxFrequency - minFrequency) / 2;
    }
    private void SetFrequency()
    {
        createSoundController.SetFrequencySelf(calculatedFrequency);
        createSoundController.SetAmplitude(0.5f);

    }
}
