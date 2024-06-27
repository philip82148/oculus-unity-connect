using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction.Editor.BuildingBlocks;
using UnityEngine;
using UnityEngine.Scripting;

public class HandActionController : MonoBehaviour
{


    [SerializeField] private GameObject[] paletteObjects;
    [SerializeField] private GameObject grabbedObject;

    private bool isGrabbed = false;




    // Update is called once per frame
    void Update()
    {
        if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
        {

        }

    }

    private void CheckObjectsCollider()
    {
        for (int i = 0; i < paletteObjects.Length; i++)
        {


        }

    }

    private void OnTriggerStay(Collider otherObject)
    {

        if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
        {
            grabbedObject = otherObject.gameObject;


            Renderer objectRenderer = otherObject.gameObject.GetComponent<Renderer>();
            if (objectRenderer != null)
            {
                objectRenderer.material.color = Color.green;
                MoveGrabbedObject();
                isGrabbed = true;
            }

        }
        else
        {

            if (isGrabbed)
            {
                PaletteObjectController paletteObject = grabbedObject.GetComponent<PaletteObjectController>();
                if (paletteObject != null)
                {
                    grabbedObject.transform.position = paletteObject.GetDefaultPoisition();
                }
                isGrabbed = false;
            }
            grabbedObject = null;
        }
    }
    private void OnTriggerExit(Collider otherObject)

    {
        PaletteObjectController paletteObjectController =
        otherObject.GetComponent<PaletteObjectController>();
        if (paletteObjectController != null)
        {
            paletteObjectController.SetDefaultPosition();

        }
    }

    // 物体を追従させる
    private void MoveGrabbedObject()
    {
        grabbedObject.transform.position = this.gameObject.transform.position;
    }

    public void UngrabbeObject()
    {
        isGrabbed = false;
    }



}
