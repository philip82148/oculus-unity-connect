using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HandController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI debugText;

    [SerializeField] private DenseSparseExpController denseSparseExpController;
    private bool isGrabbed = false;

    // Start is called before the first frame update
    void Start()
    {
        debugText.text = "shine";
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrabbed)
        {
            debugText.text = "Grabbed";

        }
        else
        {
            // debugText.text = "";
        }
    }
    private void OnTriggerStay(Collider otherObject)
    {
        // debugText.text = "Grabbed";
        PaletteObjectController paletteObjectController =
        otherObject.GetComponent<PaletteObjectController>();
        if (paletteObjectController == null)
        {
            return;
        }
        // if (!paletteObjectController.IsMove())
        // {
        //     return;
        // }


        // index fingerボタンにすると人差し指の位置が変わってしまうことがあるので変更するようにした
        if (OVRInput.GetDown(OVRInput.Button.One))
        {

            int index = paletteObjectController.GetIndex();
            denseSparseExpController.SetRejoinedIndex(index);
            isGrabbed = true;
        }
        else
        {
            isGrabbed = false;
        }

    }

}


