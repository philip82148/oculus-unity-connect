using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CarDrivingMainController : MonoBehaviour
{
    [SerializeField] private InstructionController instructionController;
    [SerializeField]
    private TextMeshProUGUI successDisplayText;


    void Start()
    {
        successDisplayText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(InstructionController.GetTmpIsInInstruction());
        if (InstructionController.GetTmpIsInInstruction())
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (InstructionController.GetTmpInstruction() == InstructionController.instructions[0])
                {
                    ProcessCorrectAnswer();
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (InstructionController.GetTmpInstruction() == InstructionController.instructions[1])
                {
                    ProcessCorrectAnswer();
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                if (InstructionController.GetTmpInstruction() == InstructionController.instructions[2])
                {
                    ProcessCorrectAnswer();
                }
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                if (InstructionController.GetTmpInstruction() == InstructionController.instructions[3])
                {
                    ProcessCorrectAnswer();
                }
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (InstructionController.GetTmpInstruction() == InstructionController.instructions[4])
                {
                    ProcessCorrectAnswer();
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (InstructionController.GetTmpInstruction() == InstructionController.instructions[5])
                {
                    ProcessCorrectAnswer();
                }
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
