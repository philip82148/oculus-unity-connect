using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Oculus.Platform;
using TMPro;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    [Header("Controller Setting")]
    [SerializeField] private DrawExperienceController drawExperienceController;
    [SerializeField] private VRKeyboardExpController vRKeyboardExpController;
    [SerializeField] private SurgeryExpController surgeryExpController;
    [SerializeField] private ChaseGameController chaseGameController;

    [SerializeField] private float GAME_TIME = 90.0f;
    private const float countTime = 5.0f;

    [Header("UI Setting")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField]
    private TextMeshProUGUI descriptionText;
    [SerializeField] private GameObject countDownPanel;
    [SerializeField]
    private TextMeshProUGUI countDownText;
    [SerializeField]
    private GameObject gameFinishDiplayCanvas;

    private float remainingTime;
    private float countDownTime;
    private float consumedTime = 0;
    private bool isCount = false;
    private bool isGame = false;


    // Start is called before the first frame update
    void Start()
    {

        Initialize();

    }
    void Initialize()
    {
        remainingTime = GAME_TIME;
        countDownTime = countTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (isCount)
        {
            countDownTime -= Time.deltaTime;
            countDownText.text = "count " + countDownTime.ToString("f0");
            if (countDownTime <= 0.5f)
            {
                StartGame();
                countDownPanel.SetActive(false);
                isCount = false;
            }

        }


        if (isGame)
        {
            remainingTime -= Time.deltaTime;
            if (remainingTime <= 0)
            {
                EndGame();

            }
            consumedTime += Time.deltaTime;

        }
        if (timerText != null)
        {
            timerText.text = "Time:" + remainingTime.ToString("f1");
            // timerText.text = "Time:" + consumedTime.ToString("f1");
        }
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
        if (drawExperienceController != null)
        {
            drawExperienceController.CallGameStart();
        }
        if (vRKeyboardExpController != null)
        {
            vRKeyboardExpController.CallGameStart();
        }
        if (surgeryExpController != null)
        {
            surgeryExpController.CallGameStart();
        }
        if (chaseGameController != null)
        {
            chaseGameController.CallGameStart();
        }
    }

    public void EndGame()
    {
        isGame = false;
        if (gameFinishDiplayCanvas != null) gameFinishDiplayCanvas.SetActive(true);
        // descriptionText.text = "";
        if (drawExperienceController != null) drawExperienceController.EndGame();
    }

    public void SetRemainingTime(float remainingTime)
    {
        this.remainingTime = remainingTime;
    }
    // public void EndGame1()
    // {

    //     isGame = false;
    //     gameFinishDiplayCanvas.SetActive(true);
    //     descriptionText.text = "";

    // }
    public float GetConsumedTime()
    {
        return consumedTime;
    }
}
