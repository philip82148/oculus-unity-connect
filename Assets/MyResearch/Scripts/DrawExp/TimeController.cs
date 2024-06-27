using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    [Header("Controller Setting")]
    [SerializeField] private DrawExperienceController drawExperienceController;

    [SerializeField] private const float gameTime = 10.0f;
    private const float countTime = 5.0f;

    [Header("UI Setting")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField]
    private TextMeshProUGUI descriptionText;
    [SerializeField] private GameObject countDownPanel;
    [SerializeField]
    private TextMeshProUGUI countDownText;

    private float remainingTime;
    private float countDownTime;
    private bool isCount = false;
    private bool isGame = false;


    // Start is called before the first frame update
    void Start()
    {
        Initialize();

    }
    void Initialize()
    {
        remainingTime = gameTime;
        countDownTime = countTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (isCount)
        {
            countDownTime -= Time.deltaTime;
            countDownText.text = countDownTime.ToString("f0");
            if (countDownTime <= 0)
            {
                StartGame();
                countDownPanel.SetActive(false);
                isCount = false;
            }

        }


        if (isGame)
        {
            remainingTime -= Time.deltaTime;

        }
        if (remainingTime <= 0)
        {
            remainingTime = 0;
            EndGame();
        }
        timerText.text = remainingTime.ToString("f1");
    }

    public void StartGameCountDown()
    {
        isCount = true;
        countDownPanel.SetActive(true);
    }

    public void StartGame()
    {
        isGame = true;
        Initialize();

    }
    public void EndGame()
    {
        timerText.text = "Game Finished";
        descriptionText.text = "";
    }

}
