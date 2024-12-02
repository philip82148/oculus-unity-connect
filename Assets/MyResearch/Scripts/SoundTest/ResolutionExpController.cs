using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class ResolutionExpController : MonoBehaviour
{
    [SerializeField] private ResolutionSetting resolutionSetting;
    [SerializeField] private TextMeshProUGUI debugText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI gameText;

    [SerializeField] private CreateSoundController soundController;

    [Header("Exp Setting")]
    [SerializeField]
    private ExpSetting expSetting = ExpSetting.Both;
    [SerializeField] private DataLoggerController dataLoggerController;
    [SerializeField] private Slider frequencySlider; // 周波数調整用のスライダー

    [Header("UI Setting")]
    [SerializeField] private GameObject[] frequencyButtons;
    [SerializeField] private GameObject[] amplitudeButtons;
    [SerializeField] private GameObject[] panButtons;
    [SerializeField] private string subjectName;
    private int frequencyResolutionIndex = 0;
    private int amplitudeResolutionIndex = 0;
    private int panResolutionIndex = 0;

    private bool isGameStart = false;
    private int score = 0;
    private int restCount = 30;
    [SerializeField] private int frequencyResolutionCount = 5;
    [SerializeField] private int amplitudeResolutionCount = 5;
    [SerializeField] private int panResolutionCount = 5;

    void Start()
    {
        string dateTime = System.DateTime.Now.ToString("yyyyMMddHHmmss");
        InitializeButtonSetting();
        resolutionSetting.SetCount(frequencyResolutionCount, amplitudeResolutionCount);
        string folder = $"C:\\Users\\takaharayota\\Research\\Semi-Exp2\\{subjectName}";
        Directory.CreateDirectory(folder);
        string fileName = $"{frequencyResolutionCount}_{amplitudeResolutionCount}_{panResolutionCount}_{expSetting}_{dateTime}.txt";
        dataLoggerController.Initialize(System.IO.Path.Combine(folder, fileName));
    }

    // Update is called once per frame
    void Update()
    {
        debugText.text = restCount.ToString();
        scoreText.text = "score:" + score.ToString();


        if (expSetting == ExpSetting.ManualFrequency)
        {
            ReflectFrequencyFromSlider();
        }
        else if (expSetting == ExpSetting.ManualAmplitude)
        {
            resolutionSetting.ReflectPacksan();
        }
    }
    public void ReflectClickedIndex(int index, ResolutionType resolutionType)
    {
        Debug.Log("index:" + index);
        if (!isGameStart)
        {
            if (resolutionType == ResolutionType.Frequency) frequencyResolutionIndex = index;
            else if (resolutionType == ResolutionType.Amplitude) amplitudeResolutionIndex = index;
            else if (resolutionType == ResolutionType.Pan) panResolutionIndex = index;
            ReflectAudioSetting();
        }

    }


    private void InitializeButtonSetting()
    {
        for (int i = 0; i < frequencyButtons.Length; i++)
        {
            if (expSetting == ExpSetting.Both && frequencyResolutionCount <= i)
            {

                frequencyButtons[i].SetActive(false);
                frequencySlider.gameObject.SetActive(false);

            }
            else if (expSetting == ExpSetting.Frequency)
            {

                if (frequencyResolutionCount <= i) { frequencyButtons[i].SetActive(false); }
                frequencySlider.gameObject.SetActive(false);
            }
            else if (expSetting == ExpSetting.Amplitude)
            {
                frequencyButtons[i].SetActive(false);
                frequencySlider.gameObject.SetActive(false);

            }
            else if (expSetting == ExpSetting.Pan)
            {
                frequencyButtons[i].SetActive(false);
                frequencySlider.gameObject.SetActive(false);
            }
            else if (expSetting == ExpSetting.All)
            {
                if (frequencyResolutionCount <= i) { frequencyButtons[i].SetActive(false); }
                frequencySlider.gameObject.SetActive(false);
            }
            else if (expSetting == ExpSetting.ManualFrequency)
            {
                foreach (var button in frequencyButtons)
                {
                    button.SetActive(false);
                }
                foreach (var button in amplitudeButtons)
                {
                    button.SetActive(false);
                }
                foreach (var button in panButtons)
                {
                    button.SetActive(false);
                }
                frequencySlider.gameObject.SetActive(true); // スライダーを表示
            }
            // else if (expSetting == ExpSetting.ManualAmplitude)
            // {
            //     foreach (var button in frequencyButtons)
            //     {
            //         button.SetActive(false);
            //     }
            //     foreach (var button in amplitudeButtons)
            //     {
            //         button.SetActive(false);
            //     }
            //     foreach (var button in panButtons)
            //     {
            //         button.SetActive(false);
            //     }
            //     frequencySlider.gameObject.SetActive(false); // スライダーを表示
            // }

        }
        for (int i = 0; i < amplitudeButtons.Length; i++)
        {
            if (expSetting == ExpSetting.Both && amplitudeResolutionCount <= i)
            {


                amplitudeButtons[i].SetActive(false);
            }
            else if (expSetting == ExpSetting.Frequency)
            {
                amplitudeButtons[i].SetActive(false);

            }
            else if (expSetting == ExpSetting.Amplitude)
            {
                frequencyButtons[i].SetActive(false);
                if (amplitudeResolutionCount <= i) amplitudeButtons[i].SetActive(false);
            }
            else if (expSetting == ExpSetting.Pan)
            {
                amplitudeButtons[i].SetActive(false);
            }
            else if (expSetting == ExpSetting.All)
            {
                if (amplitudeResolutionCount <= i) amplitudeButtons[i].SetActive(false);
            }
        }
        for (int i = 0; i < panButtons.Length; i++)
        {
            if (expSetting == ExpSetting.Both)
            {


                panButtons[i].SetActive(false);
            }
            else if (expSetting == ExpSetting.Frequency)
            {
                panButtons[i].SetActive(false);

            }
            else if (expSetting == ExpSetting.Amplitude)
            {
                panButtons[i].SetActive(false);
                // frequencyButtons[i].SetActive(false);
                // if (amplitudeResolutionCount <= i) amplitudeButtons[i].SetActive(false);
            }
            else if (expSetting == ExpSetting.Pan && panResolutionCount <= i)
            {
                panButtons[i].SetActive(false);
            }
            else if (expSetting == ExpSetting.All && panResolutionCount <= i) { panButtons[i].SetActive(false); }
        }


    }


    public void AnswerSetting(int ansFrequencyIndex, int ansAmplitudeIndex, int ansPanIndex)
    {
        dataLoggerController.WriteAnswer(ansFrequencyIndex, ansAmplitudeIndex, ansPanIndex, frequencyResolutionIndex, amplitudeResolutionIndex, panResolutionIndex);
        if (!isGameStart) return;
        if (expSetting == ExpSetting.Frequency && frequencyResolutionIndex == ansFrequencyIndex)
        {
            score += 1;
        }
        else if (expSetting == ExpSetting.Amplitude && amplitudeResolutionIndex == ansAmplitudeIndex)
        {
            score += 1;
        }
        else if (expSetting == ExpSetting.Both && frequencyResolutionIndex == ansFrequencyIndex && amplitudeResolutionIndex == ansAmplitudeIndex)
        {
            score += 1;
        }
        else if (expSetting == ExpSetting.Pan && panResolutionIndex == ansPanIndex)
        {
            score += 1;
        }
        else if (expSetting == ExpSetting.All && frequencyResolutionIndex == ansFrequencyIndex && amplitudeResolutionIndex == ansAmplitudeIndex && panResolutionIndex == ansPanIndex)
        {
            score += 1;
        }
        restCount -= 1;
        if (restCount <= 0)
        {
            gameText.text = "game finished" + score.ToString();
        }
        else
        {
            SetNext();
        }

    }

    private void ReflectAudioSetting()
    {
        resolutionSetting.ReflectAudioSetting(frequencyResolutionIndex, amplitudeResolutionIndex, panResolutionIndex, expSetting);
        // resolutionSetting.ReflectExponentialAudioSetting(frequencyResolutionIndex, frequencyResolutionIndex);
    }
    private int previousFrequencyResolutionIndex = -1;
    private int previousAmplitudeResolutionIndex = -1;
    private int previousPanResolutionIndex = -1;

    private void SetNext()
    {
        do
        {
            frequencyResolutionIndex = Random.Range(0, frequencyResolutionCount);
        } while (frequencyResolutionIndex == previousFrequencyResolutionIndex);

        do
        {
            amplitudeResolutionIndex = Random.Range(0, amplitudeResolutionCount);
        } while (amplitudeResolutionIndex == previousAmplitudeResolutionIndex);

        do
        {
            panResolutionIndex = Random.Range(0, panResolutionCount);
        } while (panResolutionIndex == previousPanResolutionIndex);

        // 前回のインデックスを更新
        previousFrequencyResolutionIndex = frequencyResolutionIndex;
        previousAmplitudeResolutionIndex = amplitudeResolutionIndex;
        previousPanResolutionIndex = panResolutionIndex;

        ReflectAudioSetting();
    }
    public void StartGame()
    {
        isGameStart = true;

        Debug.Log("start game");
        gameText.text = "start game"; SetNext();
    }

    public void ReflectFrequencyFromSlider()
    {
        float frequency = frequencySlider.value;
        resolutionSetting.SetFrequencyOnly(frequency);
    }

    public int GetFrequencyResolutionCount()
    {
        return frequencyResolutionCount;
    }
    public int GetAmplitudeResolutionCount()
    {
        return amplitudeResolutionCount;
    }
    public int GetPanResolutionCount()
    {
        return panResolutionCount;
    }
    private void OnDestroy()
    {
        dataLoggerController.Close();
    }
}

public enum ResokutionCount
{

}

public enum ResolutionType
{
    None,
    Frequency,
    Amplitude,
    Pan
}
public enum ExpSetting
{
    Amplitude,
    Frequency,
    Both,
    Pan,
    All,
    ManualFrequency,
    ManualAmplitude
}