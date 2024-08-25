using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GearTouchHandController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI successDisplayText;
    [SerializeField] private InstructionController instructionController;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnTriggerStay(Collider other)
    {
        GearPaletteController gearPaletteController = other.GetComponent<GearPaletteController>();
        if (gearPaletteController == null) return;

        if (OVRInput.Get(OVRInput.Button.One))
        {
            int targetGearNumber = gearPaletteController.GetGearNumber();
            int tmpInstructionIndex = InstructionController.GetTmpInstructionIndex();
            if (targetGearNumber == tmpInstructionIndex)
            {
                ProcessCorrectAnswer();
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
