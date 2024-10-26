using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionSetting : MonoBehaviour
{
    [SerializeField] private CreateSoundController createSoundController;
    private float minFrequency = 220;
    private float maxFrequency = 440;
    private int frequencyCount = 5;

    private float minAmplitude = 0.5f;
    private float maxAmplitude = 1f;
    private int amplitudeCount = 3;

    private int tmpFrequencyIndex = 0;
    private int tmpAmplitudeIndex = 0;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ReflectAudioSetting(int frequencyIndex, int amplitudeIndex)
    {
        tmpFrequencyIndex = frequencyIndex;
        tmpAmplitudeIndex = amplitudeIndex;
        float frequency = minFrequency + (maxFrequency - minFrequency) * tmpFrequencyIndex / frequencyCount;
        createSoundController.SetFrequencySelf(frequency);
        float amplitude = minAmplitude + (maxAmplitude - minAmplitude) * tmpAmplitudeIndex / amplitudeCount;
        createSoundController.SetAmplitude(amplitude);
    }
}
