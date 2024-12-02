using System.Collections;
using System.Collections.Generic;
using OVR.OpenVR;
using UnityEngine;

public class KeyboardHandController : MonoBehaviour
{
    [SerializeField]
    private VRKeyboardExpController vRKeyboardExpController;
    [SerializeField] private NumberKeyboard numberKeyboard;

    private bool isTouched = false;
    private int tmpIndex = 0;
    private string tmpAlphabet;



    private void OnTriggerStay(Collider otherObject)
    {
        KeyboardKey keyboardKey = otherObject.GetComponent<KeyboardKey>();
        if (keyboardKey == null)
        {
            return;
        }

        // tmpIndex = keyboardKey.GetIndex();
        tmpAlphabet = keyboardKey.GetAlphabet();
        isTouched = true;
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            keyboardKey.SetColor();
            numberKeyboard.OnKeyPressed(tmpAlphabet);

        }



    }
    private void OnTriggerExit(Collider other)
    {
        isTouched = false;
    }
    // public int GetIndex()
    // {
    //     if (isTouched)
    //     {
    //         return tmpIndex;
    //     }
    //     else
    //     {
    //         return -1;
    //     }
    // }

    public string GetAlphabet()
    {
        if (isTouched)
        {
            return tmpAlphabet;
        }
        else return "";
    }

}
