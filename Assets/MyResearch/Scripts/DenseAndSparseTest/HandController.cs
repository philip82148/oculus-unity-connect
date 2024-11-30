using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HandController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI debugText;

    [SerializeField] private DenseSparseExpController denseSparseExpController;
    private bool isTouched = false;

    private int tmpIndex = 0;


    private void OnTriggerStay(Collider otherObject)
    {
        // debugText.text = "Grabbed";
        PaletteObjectController paletteObjectController =
        otherObject.GetComponent<PaletteObjectController>();
        if (paletteObjectController == null)
        {
            return;
        }

        tmpIndex = paletteObjectController.GetIndex();
        isTouched = true;

    }
    // private void OnTriggerStay(Collider other)
    // {
    //     isTouched = true;
    // }
    private void OnTriggerExit(Collider other)
    {
        isTouched = false;
    }
    public int GetIndex()
    {
        if (isTouched)
        {
            return tmpIndex;
        }
        else
        {
            return -1;
        }
    }
}


