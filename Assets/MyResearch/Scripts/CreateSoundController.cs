using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CreateSoundController : MonoBehaviour
{
    private const double DEFAULT_FREQUENCY = 440;
    public double frequencyCoefficient;

    [SerializeField] private double frequency;
    [SerializeField] private double gain = 1.0f;
    private double pan = 0;
    private bool isSound = true;

    public double increment;
    public double phase;
    private const double SAMPLING_FREQUENCY = 48000;
    [SerializeField] private float harmonic1Coefficient = 0f;
    [SerializeField] private float harmonic2Coefficient = 0f;
    [SerializeField] private bool isChangedByCoefficient = true;

    // 波形の種類を指定
    public enum WaveType { Sine, Triangle, Sawtooth, Square }
    public WaveType waveType = WaveType.Sine;

    // 倍音用の位相
    private double phaseHarmonic1;
    private double phaseHarmonic2;

    private void Start()
    {
        if (isChangedByCoefficient) frequency = DEFAULT_FREQUENCY * frequencyCoefficient;
    }

    private void Update()
    {
        if (isChangedByCoefficient) frequency = DEFAULT_FREQUENCY * frequencyCoefficient;

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
            double sample = gain * GenerateWave(phase, waveType);

            // 第1倍音 (50%の振幅で加算)
            sample += gain * harmonic1Coefficient * GenerateWave(phaseHarmonic1, waveType);

            // 第2倍音 (30%の振幅で加算)
            sample += gain * harmonic2Coefficient * GenerateWave(phaseHarmonic2, waveType);

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

    // 波形を生成する関数
    private double GenerateWave(double phase, WaveType waveType)
    {
        switch (waveType)
        {
            case WaveType.Sine:
                return Math.Sin(phase); // サイン波
            case WaveType.Triangle:
                return 2.0 * Math.Asin(Math.Sin(phase)) / Math.PI; // 三角波
            case WaveType.Sawtooth:
                return 2.0 * (phase / (2 * Math.PI) - Math.Floor(phase / (2 * Math.PI) + 0.5)); // ノコギリ波
            case WaveType.Square:
                return Math.Sign(Math.Sin(phase)); // 矩形波
            default:
                return Math.Sin(phase); // デフォルトはサイン波
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

    public void SetFrequencyCoefficient(double nextFrequencyCoefficient)
    {
        this.frequencyCoefficient = nextFrequencyCoefficient;
    }

    public void SetWaveType(WaveType nextWaveType)
    {
        this.waveType = nextWaveType;
    }

    public void SetFrequencySelf(float frequencySelf)
    {
        frequency = frequencySelf;
    }
}
