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
        amplitudeText.text = "amp:" + createSoundController.gain.ToString("f2");
        frequencyText.text = "fre:" + createSoundController.frequency.ToString("f2");
        panText.text = "pan:" + createSoundController.pan.ToString("f2");

    }
}
