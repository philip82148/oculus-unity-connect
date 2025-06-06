using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading;

public class ShootingExpController : MonoBehaviour
{
    [SerializeField] private GameObject countDownPanel;
    [SerializeField] private TextMeshProUGUI countDownText;
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private TextMeshProUGUI scoreText;


    private float countTime = 5.0f;
    private float gameTime = 60f;
    private bool isCountDown = false;
    private bool isGame = false;
    private int score = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Four))
        {
            isCountDown = true;
            countDownPanel.SetActive(true);
        }
        if (isCountDown)
        {
            countTime -= Time.deltaTime;
            countDownText.text = "count:" + countTime.ToString("F1");
        }
        if (countTime <= 0)
        {
            isCountDown = false;
            countDownPanel.SetActive(false);
            isGame = true;
        }
        if (isGame)
        {
            gameTime -= Time.deltaTime;
            countText.text = "" + gameTime.ToString("F1");
            if (gameTime <= 0)
            {

                countText.text = "game finished";
                isGame = false;
            }
        }
        scoreText.text = "score:" + score.ToString();
    }


    public void CallAddScore()
    {
        if (isGame)
        {

            score += 1;
        }
    }
}
