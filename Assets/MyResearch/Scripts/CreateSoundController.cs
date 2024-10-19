using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CreateSoundController : MonoBehaviour
{
    private const double DEFAULT_FREQUENCY = 440;
    public double frequencyCoefficient;

    private double frequency;
    private double gain = 1.0f;
    private double pan = 0;
    private bool isSound = true;

    public double increment;
    public double phase;
    private const double SAMPLING_FREQUENCY = 48000;
    [SerializeField] private float harmonic1Coefficient = 0.5f;
    [SerializeField] private float harmonic2Coefficient = 0.3f;

    // 倍音用の位相
    private double phaseHarmonic1;
    private double phaseHarmonic2;

    private void Start()
    {
        frequency = DEFAULT_FREQUENCY * frequencyCoefficient;
    }

    private void Update()
    {
        frequency = DEFAULT_FREQUENCY * frequencyCoefficient;

        if (!isSound)
        {
            gain = 0;
        }
    }

    private void OnAudioFilterRead(float[] data, int channels)
    {
        increment = frequency * 2 * Math.PI / SAMPLING_FREQUENCY;

        double harmonic1Frequency = frequency * 2; // 第1倍音 (2倍の周波数)
        double harmonic2Frequency = frequency * 3; // 第2倍音 (3倍の周波数)

        double incrementHarmonic1 = harmonic1Frequency * 2 * Math.PI / SAMPLING_FREQUENCY;
        double incrementHarmonic2 = harmonic2Frequency * 2 * Math.PI / SAMPLING_FREQUENCY;

        for (int i = 0; i < data.Length; i = i + channels)
        {
            phase += increment;
            phaseHarmonic1 += incrementHarmonic1;
            phaseHarmonic2 += incrementHarmonic2;

            // 基本音
            double sample = gain * Math.Sin(phase);

            // 第1倍音 (50%の振幅で加算)
            sample += gain * harmonic1Coefficient * Math.Sin(phaseHarmonic1);

            // 第2倍音 (30%の振幅で加算)
            sample += gain * harmonic2Coefficient * Math.Sin(phaseHarmonic2);

            float panLeft = 1.0f - (float)pan; // 左チャンネルのゲイン計算
            float panRight = 1.0f + (float)pan; // 右チャンネルのゲイン計算

            // パンニングを適用
            if (channels == 2)
            {
                data[i] = (float)(sample * panLeft * 0.5); // 左チャンネル
                data[i + 1] = (float)(sample * panRight * 0.5); // 右チャンネル
            }
            else
            {
                data[i] = (float)sample; // モノラルの場合
            }

            // 位相をリセット
            if (phase > 2 * Math.PI) phase -= 2 * Math.PI;
            if (phaseHarmonic1 > 2 * Math.PI) phaseHarmonic1 -= 2 * Math.PI;
            if (phaseHarmonic2 > 2 * Math.PI) phaseHarmonic2 -= 2 * Math.PI;
        }
    }

    public void EnableAudio()
    {
        isSound = true;
    }

    public void DisableAudio()
    {
        isSound = false;
    }

    public double GetTmpAmplitude()
    {
        return this.gain;
    }

    public double GetTmpFrequency()
    {
        return this.frequency;
    }

    public double GetTmpPan()
    {
        return this.pan;
    }

    public void SetAmplitude(double amplitude)
    {
        this.gain = amplitude;
    }

    public void SetPan(double pan)
    {
        this.pan = pan;
    }
}
