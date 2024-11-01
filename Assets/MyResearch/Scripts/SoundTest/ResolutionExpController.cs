using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

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


    [Header("UI Setting")]
    [SerializeField] private GameObject[] frequencyButtons;
    [SerializeField] private GameObject[] amplitudeButtons;
    [SerializeField] private string subjectName;
    private int frequencyResolutionIndex = 0;
    private int amplitudeResolutionIndex = 2;

    private bool isGameStart = false;
    private int score = 0;
    private int restCount = 10;
    private int frequencyResolutionCount = 5;
    private int amplitudeResolutionCount = 5;

    void Start()
    {
        InitializeButtonSetting();
        resolutionSetting.SetCount(frequencyResolutionCount, amplitudeResolutionCount);
        string folder = $"C:\\Users\\takaharayota\\Research\\Exp1-data\\{subjectName}\\times";
        Directory.CreateDirectory(folder);
        string fileName = $"time_{frequencyResolutionCount}_{amplitudeResolutionCount}_{expSetting}.txt";
        dataLoggerController.Initialize(System.IO.Path.Combine(folder, fileName));
    }

    // Update is called once per frame
    void Update()
    {
        debugText.text = restCount.ToString();
        scoreText.text = "score:" + score.ToString();
    }
    public void ReflectClickedIndex(int index, ResolutionType resolutionType)
    {
        Debug.Log("index:" + index);
        if (!isGameStart)
        {
            if (resolutionType == ResolutionType.Frequency) frequencyResolutionIndex = index;
            else if (resolutionType == ResolutionType.Amplitude) amplitudeResolutionIndex = index;
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

            }
            else if (expSetting == ExpSetting.Frequency)
            {

                if (frequencyResolutionCount <= i) { frequencyButtons[i].SetActive(false); }
            }
            else if (expSetting == ExpSetting.Amplitude)
            {
                frequencyButtons[i].SetActive(false);

            }

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
        }


    }


    public void AnswerSetting(int ansFrequencyIndex, int ansAmplitudeIndex)
    {
        dataLoggerController.WriteAnswer(ansFrequencyIndex, ansAmplitudeIndex, frequencyResolutionIndex, amplitudeResolutionIndex);
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
        resolutionSetting.ReflectAudioSetting(frequencyResolutionIndex, amplitudeResolutionIndex, expSetting);
        // resolutionSetting.ReflectExponentialAudioSetting(frequencyResolutionIndex, frequencyResolutionIndex);
    }
    private void SetNext()
    {
        frequencyResolutionIndex = Random.Range(0, frequencyResolutionCount);
        amplitudeResolutionIndex = Random.Range(0, amplitudeResolutionCount);
        // gameText.text = "next index:" + frequencyResolutionIndex.ToString("") + "";
        ReflectAudioSetting();
    }
    public void StartGame()
    {
        isGameStart = true;

        Debug.Log("start game");
        gameText.text = "start game"; SetNext();
    }
    public int GetFrequencyResolutionCount()
    {
        return frequencyResolutionCount;
    }

    private void OnDestroy()
    {
        dataLoggerController.Close();
    }
}



public enum ResolutionType
{
    None,
    Frequency,
    Amplitude
}
public enum ExpSetting
{
    Amplitude,
    Frequency,
    Both
}