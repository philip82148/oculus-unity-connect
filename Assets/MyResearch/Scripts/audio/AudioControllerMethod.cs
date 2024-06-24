using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine;
using System.IO;
using UnityEngine.UI;


using TMPro;




public class AudioControllerMethod : MonoBehaviour
{


    [Header("audio source setting")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] CreateSoundController soundController;
    [SerializeField] float spatialBlend = 1.0f;
    [SerializeField] private const float amplitudeCoefficient = 1.0f;
    [SerializeField] private float amplitude;
    private const float frequencyCoefficient = 4.0f;
    [SerializeField] private float frequency;
    [SerializeField] private const float panCoefficient = 1.0f;
    [SerializeField] private float pan;
    [SerializeField] public bool isAmplitudeInversion = false;

    [Header("scaling factor")]
    [SerializeField] float amplitudeScalingFactor = 1.0f; // この値を増減させて、影響度を調整
    [SerializeField] float frequencyScalingFactor = 2.0f; // この値を増減させて、影響度を調整
    [SerializeField] float controllerYPosition;

    private float discreteFactor = 0.01f;
    // private float discreteFactor = 0.01f;




    void Start()
    {
        Initialize();
        ReflectAudioSettings();
        audioSource.loop = true;
        audioSource.Play();
        ChangeSpatialBlend();
    }

    void Initialize()
    {
        amplitude = 1.0f;
        frequency = 0.5f;
        pan = 0;


    }


    public void SetAudioSetting(Vector3 controllerPosition)
    {



        amplitude = 1.0f - amplitudeScalingFactor * controllerPosition.z;
        // }


        frequency = 0.5f + frequencyScalingFactor * controllerPosition.y;
        pan = controllerPosition.x;

        ReflectAudioSettings();

    }


    public void SetAudioSettingWithTargetText(Vector3 targetDiff)
    {
        float zOffset = -0.1f;
        float discreteZ = Mathf.Floor((targetDiff.z + zOffset) / discreteFactor) * discreteFactor;
        amplitude = Mathf.Pow(2.0f, -discreteZ / discreteFactor);




        float discreteY = Mathf.Floor((targetDiff.y) / discreteFactor) * discreteFactor;
        // 1cm 増えるごとに 1.5 倍になるための計算
        frequency = Mathf.Pow(frequencyScalingFactor, discreteY * 100);  // controllerPosition.y がメートル単位なので、100 を掛けてセンチメートル単位に変換

        // frequency が負の値にならないようにチェック
        if (frequency < 0)
        {
            frequency = 0;
        }


        double diffX = targetDiff.x * 1000;
        if (-5 < diffX && diffX < 5)
        {
            Debug.Log("-10 < diffX && diffX < 10");
            pan = 0;
        }
        else if (-30 < diffX && diffX <= -5)
        {
            Debug.Log("-30 < diffX && diffX <= -10");
            pan = -1.0f;
        }
        else if (5 <= diffX && diffX < 30)
        {
            Debug.Log("diffX <= 10 && diffX < 30");
            pan = 1.0f;
        }
        else
        {
            amplitude = 0;
        }

        ReflectAudioSettings();

    }


    public void SetAudioSettingWithTargetText2(Vector3 targetDiff)
    {
        float yOffset = 0.15f;
        float zOffset = 0.0f;
        float discreteZ = Mathf.Floor((targetDiff.z + zOffset) / discreteFactor) * discreteFactor + zOffset;
        float discreteY = Mathf.Floor((targetDiff.y + yOffset - 0.25f) / discreteFactor) * discreteFactor + yOffset;

        amplitude = Mathf.Pow(2.0f, discreteY / discreteFactor);
        // 1cm 増えるごとに 1.5 倍になるための計算
        frequency = Mathf.Pow(frequencyScalingFactor, discreteZ * 100);  // controllerPosition.y がメートル単位なので、100 を掛けてセンチメートル単位に変換

        // frequency が負の値にならないようにチェック
        if (frequency < 0)
        {
            frequency = 0;
        }


        double diffX = targetDiff.x * 1000;
        if (-5 < diffX && diffX < 5)
        {
            Debug.Log("-10 < diffX && diffX < 10");
            pan = 0;
        }
        else if (-30 < diffX && diffX <= -5)
        {
            Debug.Log("-30 < diffX && diffX <= -10");
            pan = -0.5f;
        }
        else if (5 <= diffX && diffX < 30)
        {
            Debug.Log("diffX <= 10 && diffX < 30");
            pan = 0.5f;
        }
        else
        {
            amplitude = 0;
        }

        ReflectAudioSettings();

    }





    public void SetAudioSettingOnlyAmplitude(Vector3 controllerPosition)
    {
        float discreteZ = Mathf.Floor(controllerPosition.z / discreteFactor) * discreteFactor;
        // amplitude = 1.0f - amplitudeScalingFactor * controllerPosition.z;
        amplitude = 1.0f - amplitudeScalingFactor * discreteZ;

        ReflectAudioSettings();

    }
    public void SetAudioSettingOnlyAmplitudeWithTargetText(Vector3 targetDiff)
    {
        float discreteZ = Mathf.Floor(targetDiff.z / discreteFactor) * discreteFactor;
        amplitude = Mathf.Pow(2.0f, -discreteZ / discreteFactor);


        ReflectAudioSettings();


    }


    // public void SetAudioSettingOnlyFrequency(Vector3 controllerPosition)
    // {

    //     float discreteY = Mathf.Floor((controllerPosition.y + 0.7f) / discreteFactor) * discreteFactor;
    //     controllerYPosition = controllerPosition.y;


    //     amplitude = 0.25f;
    //     frequency = frequencyScalingFactor * discreteY;
    //     if (frequency < 0)
    //     {
    //         frequency = 0;
    //     }

    //     pan = 0;

    //     ReflectAudioSettings();

    // }


    public void SetAudioSettingOnlyFrequency(Vector3 controllerPosition)
    {
        // frequency の基本値（controllerPosition.y が 0 のときの値）
        float baseFrequency = 1.0f;  // 適切な基本周波数を設定する
        amplitude = 0.25f;
        float discreteY = Mathf.Floor((controllerPosition.y + 0.05f - 0.25f) / discreteFactor) * discreteFactor + 0.05f;
        // 1cm 増えるごとに 1.5 倍になるための計算
        frequency = baseFrequency * Mathf.Pow(frequencyScalingFactor, discreteY * 100);  // controllerPosition.y がメートル単位なので、100 を掛けてセンチメートル単位に変換

        // frequency が負の値にならないようにチェック
        if (frequency < 0)
        {
            frequency = 0;
        }

        pan = 0;  // パンは変更しない

        ReflectAudioSettings();  // オーディオ設定を反映

    }

    public void SetAudioSettingOnlyPan(Vector3 controllerPosition)
    {


        ReflectAudioSettings();

    }


    public void SetAudioSettingOnlyPanWithTargetText(Vector3 targetDiff)
    {

        double diffX = targetDiff.x * 1000;
        if (-5 < diffX && diffX < 5)
        {
            Debug.Log("-10 < diffX && diffX < 10");
            pan = 0; amplitude = 1.0f;
        }
        else if (-30 < diffX && diffX <= -5)
        {
            Debug.Log("-30 < diffX && diffX <= -10");
            pan = -0.5f; amplitude = 1.0f;
        }
        else if (5 <= diffX && diffX < 30)
        {
            Debug.Log("diffX <= 10 && diffX < 30");
            pan = 0.5f; amplitude = 1.0f;
        }
        else
        {

            amplitude = 0;
        }
        // else if (-50 < diffX && diffX <= -30)
        // {
        //     pan = -1; amplitude = 1.0f;
        // }
        // else if (30 <= diffX && diffX < 50)
        // {
        //     pan = 1; amplitude = 1.0f;
        // }


        ReflectAudioSettings();
    }








    public void SetAudioSettingWithPolar(Vector3 controllerPosition)
    {
        float radius = Mathf.Sqrt(controllerPosition.x * controllerPosition.x + controllerPosition.z * controllerPosition.z + controllerPosition.y * controllerPosition.y);
        float azimuth = Mathf.Atan2(Mathf.Abs(controllerPosition.z), Mathf.Abs(controllerPosition.x));
        float elevation = Mathf.Asin(controllerPosition.y / radius) / 90;
        amplitude = 1.0f - amplitudeScalingFactor * radius;



        frequency = 0.5f + frequencyScalingFactor * controllerPosition.y;
        pan = controllerPosition.x;


        ReflectAudioSettings();
    }

    public void SetAudioSettingWithWeberFechner(Vector3 controllerPosition)
    {
        float amplitudeStimulus = Mathf.Max(0.01f, controllerPosition.z);
        float frequencyStimulus = Mathf.Max(0.01f, controllerPosition.y);

        amplitude = 1.0f - amplitudeScalingFactor * Mathf.Log10(amplitudeStimulus + 1);
        frequency = 0.5f + frequencyScalingFactor * Mathf.Log10(frequencyStimulus + 1);
        pan = controllerPosition.x;

        ReflectAudioSettings();




    }

    private void ReflectAudioSettings()
    {
        soundController.gain = amplitude * amplitudeCoefficient;
        soundController.frequencyCoefficient = frequency * frequencyCoefficient;
        soundController.pan = pan * panCoefficient;
    }

    private void ChangeSpatialBlend()
    {
        audioSource.spatialBlend = spatialBlend;
    }
}
