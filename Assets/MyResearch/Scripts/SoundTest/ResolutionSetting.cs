using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.AI;

public class ResolutionSetting : MonoBehaviour
{
    [SerializeField] private CreateSoundController createSoundController;
    [SerializeField] private ResolutionExpController resolutionExpController;
    [SerializeField] private float minFrequency;
    [SerializeField] private float maxFrequency;
    private int frequencyCount;

    private float minAmplitude = (float)1 / 16f;
    private float maxAmplitude = 1.0f;
    // private float amplitudeRange = 11f;
    private int amplitudeCount;

    private int panCount;

    private int tmpFrequencyIndex = 0;
    private int tmpAmplitudeIndex = 0;
    private int tmpPanIndex = 1;

    [SerializeField] private bool isExp = false;
    [SerializeField] private float frequencyExponent = 0;
    [SerializeField] private float dBReduction;
    [SerializeField] private float newAmplitude;


    // Start is called before the first frame update
    void Start()
    {
        // minAmplitude = maxAmplitude * Mathf.Pow(10.0f, -amplitudeRange / 20.0f);
        Debug.Log("min amplitude:" + minAmplitude);
        frequencyCount = resolutionExpController.GetFrequencyResolutionCount() - 1;
        amplitudeCount = resolutionExpController.GetAmplitudeResolutionCount() - 1;
        panCount = resolutionExpController.GetPanResolutionCount() - 1;

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ReflectPacksan()
    {
        createSoundController.SetFrequencySelf(440f);
        frequencyExponent = -1;
        float fixedAmplitude = CorrectLoudness(1);
        createSoundController.SetAmplitude(fixedAmplitude);
        createSoundController.SetPan(0);

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
            float frequencyRatio = maxFrequency / minFrequency;
            createSoundController.SetFrequencySelf(minFrequency * Mathf.Pow(frequencyRatio, 1 / 2f));

        }
        else if (expSetting == ExpSetting.Frequency)
        {
            FrequencySetting();
            createSoundController.SetAmplitude(CorrectLoudness(1 / 4f));
        }
        else if (expSetting == ExpSetting.Pan)
        {
            float frequencyRatio = maxFrequency / minFrequency;
            createSoundController.SetFrequencySelf(minFrequency * Mathf.Pow(frequencyRatio, 1 / 2f));
            createSoundController.SetAmplitude(CorrectLoudness(1 / 4f));
            PanSetting();
        }
        else if (expSetting == ExpSetting.All)
        {
            FrequencySetting();
            AmplitudeSetting();
            PanSetting();
        }



    }


    public void SetFrequencyOnly(float frequency)
    {
        createSoundController.SetAmplitude(1f);
        // createSoundController.SetPan(1.0f);
        createSoundController.SetFrequencySelf(frequency);
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
            frequencyExponent = tmpFrequencyIndex / (float)frequencyCount;
            frequency = minFrequency * Mathf.Pow(frequencyRatio, frequencyExponent);
            // createSoundController.SetFrequencySelf(440);
            createSoundController.SetFrequencySelf(frequency);
        }
    }

    private float CorrectLoudness(float originalAmplitude)
    {
        // float naze = 6;
        dBReduction = 6 + 6;
        if (frequencyExponent == 1) dBReduction += 2;
        else if (frequencyExponent == 0.75) dBReduction += 1;
        else if (frequencyExponent == 0.5) dBReduction += -3;
        else if (frequencyExponent == 0.25) dBReduction += -8;
        else if (frequencyExponent == 0) dBReduction += -12;
        newAmplitude = originalAmplitude * Mathf.Pow(10, -dBReduction / 20f);
        return newAmplitude;

        // return amplitude;
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
            Debug.Log("amplitude:" + amplitude);
            float fixedAmplitude = CorrectLoudness(amplitude);
            createSoundController.SetAmplitude(fixedAmplitude);
            // createSoundController.SetAmplitude(amplitude);
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
