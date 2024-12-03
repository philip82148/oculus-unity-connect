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

    private const double SAMPLING_FREQUENCY = 48000;
    private bool isChangedByCoefficient = false;

    // 波形の種類を指定
    public enum WaveType { Sine, Triangle, Sawtooth, Square }
    private WaveType waveType = WaveType.Sine;

    private List<double> frequencies = new List<double>();
    private List<double> gains = new List<double>();
    private List<double> pans = new List<double>();
    private List<double> phases = new List<double>();

    // private bool isOverlapped = false;

    // フラグを追加して、使用するOnAudioFilterReadのバージョンを切り替える
    [SerializeField] private bool useOverlappedMethod = false;

    // 倍音用の変数（元の単一音声処理で使用）
    private double phase = 0.0;
    // private double phaseHarmonic1 = 0.0;
    // private double phaseHarmonic2 = 0.0;
    private double increment = 0.0;


    private void Start()
    {
        if (isChangedByCoefficient)
            frequency = DEFAULT_FREQUENCY * frequencyCoefficient;
    }

    private void Update()
    {
        if (isChangedByCoefficient)
            frequency = DEFAULT_FREQUENCY * frequencyCoefficient;

        if (!isSound)
        {
            gain = 0;
            for (int i = 0; i < gains.Count; i++)
                gains[i] = 0;
        }
    }

    private void OnAudioFilterRead(float[] data, int channels)
    {
        if (useOverlappedMethod)
        {
            // オーバーラップした音声処理の実装

            // 合計ゲインを計算
            double totalGain = 0;
            for (int i = 0; i < gains.Count; i++)
            {
                totalGain += gains[i];
            }

            // 合計ゲインが1.0を超える場合、ゲインをスケールダウン
            if (totalGain > 1.0)
            {
                double scale = 1.0 / totalGain;
                for (int i = 0; i < gains.Count; i++)
                {
                    gains[i] *= scale;
                }
            }

            for (int i = 0; i < data.Length; i += channels)
            {
                double sampleLeft = 0;
                double sampleRight = 0;

                for (int j = 0; j < frequencies.Count; j++)
                {
                    double freq = frequencies[j];
                    double increment = freq * 2 * Math.PI / SAMPLING_FREQUENCY;

                    // フェーズの更新
                    phases[j] += increment;

                    // 波形の生成
                    double waveSample = gains[j] * GenerateWave(phases[j], waveType);

                    // パンニングの適用（標準的な計算方法）
                    double panValue = (pans[j] + 1) / 2; // -1から1を0から1に変換
                    double panLeft = Math.Cos(panValue * 0.5 * Math.PI);
                    double panRight = Math.Sin(panValue * 0.5 * Math.PI);

                    sampleLeft += waveSample * panLeft;
                    sampleRight += waveSample * panRight;

                    // フェーズのリセット
                    if (phases[j] > 2 * Math.PI)
                        phases[j] -= 2 * Math.PI;
                }

                // サンプル値のクリッピング処理
                sampleLeft = Math.Max(-1.0, Math.Min(1.0, sampleLeft));
                sampleRight = Math.Max(-1.0, Math.Min(1.0, sampleRight));

                // data配列への書き込み
                if (channels == 2)
                {
                    data[i] = (float)sampleLeft;
                    data[i + 1] = (float)sampleRight;
                }
                else
                {
                    data[i] = (float)((sampleLeft + sampleRight) * 0.5);
                }
            }
        }
        else
        {
            // 元の単一音声処理の実装
            increment = frequency * 2 * Math.PI / SAMPLING_FREQUENCY;

            for (int i = 0; i < data.Length; i += channels)
            {
                phase += increment;

                // 基本音
                double sample = gain * GenerateWave(phase, waveType);

                // パンニングの適用（標準的な計算方法）
                double panValue = (pan + 1) / 2; // -1から1を0から1に変換
                double panLeft = Math.Cos(panValue * 0.5 * Math.PI);
                double panRight = Math.Sin(panValue * 0.5 * Math.PI);

                double sampleLeft = sample * panLeft;
                double sampleRight = sample * panRight;

                // サンプル値のクリッピング処理
                sampleLeft = Math.Max(-1.0, Math.Min(1.0, sampleLeft));
                sampleRight = Math.Max(-1.0, Math.Min(1.0, sampleRight));

                // data配列への書き込み
                if (channels == 2)
                {
                    data[i] = (float)sampleLeft;
                    data[i + 1] = (float)sampleRight;
                }
                else
                {
                    data[i] = (float)((sampleLeft + sampleRight) * 0.5);
                }
                // フェーズの更新
                // phase += increment;
                // フェーズのリセット
                if (phase > 2 * Math.PI) phase -= 2 * Math.PI;

            }
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
        if (useOverlappedMethod && gains.Count > 0)
            return this.gains[0];
        else
            return this.gain;
    }

    public double GetTmpFrequency()
    {
        if (useOverlappedMethod && frequencies.Count > 0)
            return this.frequencies[0];
        else
            return this.frequency;
    }

    public double GetTmpPan()
    {
        if (useOverlappedMethod && pans.Count > 0)
            return this.pans[0];
        else
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

    public void SetFrequencies(List<double> frequencies)
    {
        this.frequencies = frequencies;
        EnsureListSizes();
    }

    public void SetAmplitudes(List<double> amplitudes)
    {
        this.gains = amplitudes;
        EnsureListSizes();
    }

    public void SetPans(List<double> pans)
    {
        this.pans = pans;
        EnsureListSizes();
    }

    private void EnsureListSizes()
    {
        int targetCount = frequencies.Count;

        // phasesリストの調整
        while (phases.Count < targetCount)
            phases.Add(0);
        while (phases.Count > targetCount)
            phases.RemoveAt(phases.Count - 1);

        // gainsリストの調整
        while (gains.Count < targetCount)
            gains.Add(0);
        while (gains.Count > targetCount)
            gains.RemoveAt(gains.Count - 1);

        // pansリストの調整
        while (pans.Count < targetCount)
            pans.Add(0);
        while (pans.Count > targetCount)
            pans.RemoveAt(pans.Count - 1);
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
