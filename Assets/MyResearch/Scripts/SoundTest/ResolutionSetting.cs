using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ResolutionSetting : MonoBehaviour
{
    [SerializeField] private CreateSoundController createSoundController;
    [SerializeField] private ResolutionExpController resolutionExpController;
    private float minFrequency = 220;
    private float maxFrequency = 660;
    private int frequencyCount;

    private float minAmplitude = 0.2f;
    private float maxAmplitude = 1f;
    private int amplitudeCount;

    private int panCount;

    private int tmpFrequencyIndex = 0;
    private int tmpAmplitudeIndex = 0;
    private int tmpPanIndex = 0;

    [SerializeField] private bool isExp = false;


    // Start is called before the first frame update
    void Start()
    {
        frequencyCount = resolutionExpController.GetFrequencyResolutionCount() - 1;
        amplitudeCount = resolutionExpController.GetAmplitudeResolutionCount() - 1;
        panCount = resolutionExpController.GetPanResolutionCount() - 1;

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ReflectAudioSetting(int frequencyIndex, int amplitudeIndex, int panIndex, ExpSetting expSetting)
    {


        tmpFrequencyIndex = frequencyIndex;
        tmpAmplitudeIndex = amplitudeIndex;
        tmpPanIndex = panIndex;
        if (expSetting == ExpSetting.Both)
        {
            FrequencySetting();
            AmplitudeSetting();
        }
        else if (expSetting == ExpSetting.Amplitude)
        {
            AmplitudeSetting();
            createSoundController.SetFrequencySelf((minFrequency + maxFrequency) / 2);

        }
        else if (expSetting == ExpSetting.Frequency)
        {
            FrequencySetting();
            createSoundController.SetAmplitude(0.5f);
        }
        else if (expSetting == ExpSetting.Pan)
        {
            createSoundController.SetFrequencySelf((minFrequency + maxFrequency) / 2);
            createSoundController.SetAmplitude(0.5f);
            PanSetting();
        }
        else if (expSetting == ExpSetting.All)
        {
            FrequencySetting();
            AmplitudeSetting();
            PanSetting();
        }


    }


    private void FrequencySetting()
    {
        float frequency = 0;
        if (isExp == false)
        {
            frequency = minFrequency + (maxFrequency - minFrequency) * tmpFrequencyIndex / frequencyCount;
            createSoundController.SetFrequencySelf(frequency);
        }
        else
        {
            // 周波数を指数的に変化させる
            float frequencyRatio = maxFrequency / minFrequency;
            float frequencyExponent = tmpFrequencyIndex / (float)frequencyCount;
            frequency = minFrequency * Mathf.Pow(frequencyRatio, frequencyExponent);
            createSoundController.SetFrequencySelf(frequency);
        }
    }

    private void AmplitudeSetting()
    {
        float amplitude = 0;
        if (isExp == false)
        {

            amplitude = minAmplitude + (maxAmplitude - minAmplitude) * tmpAmplitudeIndex / amplitudeCount;
            createSoundController.SetAmplitude(amplitude);
        }
        else
        {
            // 振幅を指数的に変化させる
            float amplitudeRatio = maxAmplitude / minAmplitude;
            float amplitudeExponent = tmpAmplitudeIndex / (float)amplitudeCount;
            amplitude = minAmplitude * Mathf.Pow(amplitudeRatio, amplitudeExponent);
            createSoundController.SetAmplitude(amplitude);
        }
    }
    private void PanSetting()
    {

        float pan = (float)2 / panCount * tmpPanIndex - 1;
        createSoundController.SetPan(pan);
    }


    public void SetCount(int frequencyCount, int amplitudeCount)
    {
        this.frequencyCount = frequencyCount - 1;
        this.amplitudeCount = amplitudeCount - 1;
    }
}
