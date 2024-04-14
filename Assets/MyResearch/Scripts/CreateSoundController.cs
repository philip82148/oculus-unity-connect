
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

public class CreateSoundController : MonoBehaviour
{
    private const double DEFAULT_FREQUENCY = 440;
    private const double DEFAULT_GAIN = 0.05;

    public double frequencyCoefficient = 1.0;
    public double gainCoefficient = 1.0;
    public double panCoefficient = 0.0;

    [SerializeField] private double frequency = 440;
    [SerializeField] private double gain = 0.05;

    [SerializeField] private double pan = 0;

    public double increment;
    public double phase;
    private const double SAMPLING_FREQUENCY = 48000;

    private void Start()
    {
        // EditorApplication.Beep();
    }
    private void Update()
    {
        frequency = DEFAULT_FREQUENCY * frequencyCoefficient;
        gain = DEFAULT_GAIN * gainCoefficient;
        pan = 1.5 * panCoefficient;
    }
    private void OnAudioFilterRead(float[] data, int channels)
    {
        increment = frequency * 2 * Math.PI / SAMPLING_FREQUENCY;

        for (int i = 0; i < data.Length; i = i + channels)
        {
            phase = phase + increment;
            double sample = gain * Math.Sin(phase);

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

            if (phase > 2 * Math.PI) phase -= 2 * Math.PI; // 位相をリセット
        }
    }
}
