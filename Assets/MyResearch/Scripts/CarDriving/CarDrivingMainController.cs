using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CarDrivingMainController : MonoBehaviour
{
    [SerializeField] private InstructionController instructionController;
    [SerializeField]
    private TextMeshProUGUI successDisplayText;
    [SerializeField] private GameObject countDownPanel;
    [SerializeField] private TextMeshProUGUI countDownText;
    [SerializeField] private TextMeshProUGUI countText;

    private float countTime = 5.0f;
    private float gameTime = 20f;
    private bool isCountDown = false;
    private bool isGame = false;


    void Start()
    {
        successDisplayText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Four) || Input.GetKeyDown(KeyCode.Space))
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
    }


    private void ProcessCorrectAnswer()
    {

        instructionController.ResetNonInstruction();
        StartCoroutine(DisplaySuccessText());
    }

    private IEnumerator DisplaySuccessText()
    {
        successDisplayText.text = "success!";
        successDisplayText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        successDisplayText.gameObject.SetActive(false);
    }
}
