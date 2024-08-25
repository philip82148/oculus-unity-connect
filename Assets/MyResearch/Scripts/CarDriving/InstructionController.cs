using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mono.Reflection;
using Unity.VisualScripting;

public class InstructionController : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI instructionText;
    [SerializeField]
    private TextMeshProUGUI intervalCountText;


    public static readonly string[] instructions =
         {

        "shift to first gear",
        "shift to second gear",
        "shift to third gear",
        "shift to fourth gear",
        "Activate the winker",
        "Activate the wipers"

        };
    private float[] instructionIntervalTime = { 5.0f, 8.0f };
    private float inInstructionTime = 2.0f;
    private static string tmpInstruction;
    private float instructionInterval = 5.0f;
    private float tmpInInstructionTime = 2.0f;

    private static bool isInInstruction = false;

    // Start is called before the first frame update
    void Start()
    {
        instructionText.text = "";
        SetInstructionInterval();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInInstruction)
        {
            instructionInterval -= Time.deltaTime;
            if (instructionInterval <= 0)
            {
                SetInstructionInterval();
                SetInstruction();
            }
            intervalCountText.text = "interval:" + instructionInterval.ToString("F1");
            intervalCountText.color = Color.green;
        }
        else
        {
            tmpInInstructionTime -= Time.deltaTime;
            instructionText.text = tmpInstruction;
            if (tmpInInstructionTime <= 0)
            {
                ResetNonInstruction();
            }
            intervalCountText.text = "count down:" + tmpInInstructionTime.ToString("F1");
            intervalCountText.color = Color.red;

        }
    }


    public void ResetNonInstruction()
    {
        isInInstruction = false;
        tmpInInstructionTime = inInstructionTime;
        instructionText.text = "";
    }


    private void SetInstructionInterval()
    {
        instructionInterval = Random.Range(instructionIntervalTime[0], instructionIntervalTime[1]);
    }
    private void SetInstruction()
    {
        tmpInstruction = instructions[Random.Range(0, instructions.Length)];
        isInInstruction = true;
    }


    public static string GetTmpInstruction()
    {
        return tmpInstruction;
    }

    public static bool GetTmpIsInInstruction()
    {
        return isInInstruction;
    }




}
