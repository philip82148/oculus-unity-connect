using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResolutionExpController : MonoBehaviour
{
    [SerializeField] private ResolutionSetting resolutionSetting;
    [SerializeField] private TextMeshProUGUI debugText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI gameText;

    [SerializeField] private CreateSoundController soundController;
    private int frequencyResolutionIndex = 0;
    private int amplitudeResolutionIndex = 5;

    private bool isGameStart = false;
    private int score = 0;
    private int restCount = 5;
    private int frequencyResolutionCount = 5;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        debugText.text = soundController.GetTmpFrequency().ToString("f2");
        scoreText.text = score.ToString();
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
        else
        {
            if (frequencyResolutionIndex == index)
            {
                score += 1;
            }
            restCount -= 1;
            if (restCount <= 0)
            {
                gameText.text = "game finished";
            }
            else
            {
                SetNextFrequency();
            }
        }
    }
    private void ReflectAudioSetting()
    {
        resolutionSetting.ReflectAudioSetting(frequencyResolutionIndex, amplitudeResolutionIndex);
    }
    private void SetNextFrequency()
    {
        frequencyResolutionIndex = Random.Range(0, frequencyResolutionCount);
        gameText.text = "next index:" + frequencyResolutionIndex.ToString("") + "";
        ReflectAudioSetting();
    }
    public void StartGame()
    {
        isGameStart = true;

        Debug.Log("start game");
        gameText.text = "start game"; SetNextFrequency();
    }
}



public enum ResolutionType
{
    None,
    Frequency,
    Amplitude
}