using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplaySoundValueConrtoller : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI amplitudeText;
    [SerializeField] private TextMeshProUGUI frequencyText;
    [SerializeField] private TextMeshProUGUI panText;
    [SerializeField] private CreateSoundController createSoundController;


    // Update is called once per frame
    void Update()
    {
        amplitudeText.text = "amp:" + createSoundController.GetTmpAmplitude().ToString("f2");
        frequencyText.text = "fre:" + createSoundController.GetTmpFrequency().ToString("f2");
        panText.text = "pan:" + createSoundController.GetTmpPan().ToString("f2");

    }
}
