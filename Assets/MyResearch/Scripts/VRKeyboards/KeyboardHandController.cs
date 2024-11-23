using System.Collections;
using System.Collections.Generic;
using OVR.OpenVR;
using UnityEngine;

public class KeyboardHandController : MonoBehaviour
{
    [SerializeField]
    private VRKeyboardExpController vRKeyboardExpController;

    private bool isTouched = false;
    private int tmpIndex = 0;



    private void OnTriggerStay(Collider otherObject)
    {
        KeyboardKey keyboardKey = otherObject.GetComponent<KeyboardKey>();
        if (keyboardKey == null)
        {
            return;
        }

        tmpIndex = keyboardKey.GetIndex();
        isTouched = true;
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            keyboardKey.SetColor();
        }



    }
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
