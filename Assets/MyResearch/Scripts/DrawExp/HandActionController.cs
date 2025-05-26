using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Scripting;

public class HandActionController : MonoBehaviour
{


    [SerializeField] private TextMeshProUGUI debugText;
    private GameObject closestObject;
    private float closestDistance = Mathf.Infinity;
    // private GameObject closestObject;

    private bool isGrabbed = false;




    // Update is called once per frame
    void Update()
    {
        if (isGrabbed)
        {
            debugText.text = "Grabbed";
            MoveGrabbedObject();
        }
        else
        {
            debugText.text = "";
        }
    }



    private void OnTriggerStay(Collider otherObject)
    {
        PaletteObjectController paletteObjectController =
        otherObject.GetComponent<PaletteObjectController>();
        if (paletteObjectController == null)
        {
            return;
        }
        if (!paletteObjectController.IsMove())
        {
            return;
        }

        // index fingerボタンにすると人差し指の位置が変わってしまうことがあるので変更するようにした
        if (OVRInput.Get(OVRInput.Button.One))
        {

            float tmpDistance = Vector3.Distance(otherObject.transform.position, this.transform.position);
            if (tmpDistance <= closestDistance)
            {
                closestObject = otherObject.gameObject;
                closestDistance = tmpDistance;

            }
            if (closestObject == otherObject.gameObject)
            {

                Renderer objectRenderer = otherObject.gameObject.GetComponent<Renderer>();
                if (objectRenderer != null)
                {
                    objectRenderer.material.color = Color.green;
                    isGrabbed = true;
                }
            }

        }
        else
        {

            isGrabbed = false;

            if (paletteObjectController != null)
            {
                paletteObjectController.SetDefaultPosition();
                paletteObjectController.SetDefaultColor();

            }

        }
    }
    private void OnTriggerExit(Collider otherObject)

    {
        PaletteObjectController paletteObjectController =
        otherObject.GetComponent<PaletteObjectController>();
        if (paletteObjectController != null)
        {
            paletteObjectController.SetDefaultPosition();
            paletteObjectController.SetDefaultColor();

        }
        if (otherObject.gameObject == closestObject)
        {
            closestObject = null;
            closestDistance = Mathf.Infinity;

        }
    }

    // 物体を追従させる
    private void MoveGrabbedObject()
    {
        if (closestObject != null)
        {
            closestObject.transform.position = this.gameObject.transform.position;
        }
    }





}
