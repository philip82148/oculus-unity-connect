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


    void Start()
    {

    }

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

            Debug.Log("grabbed Object" + grabbedObject.name);
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
            grabbedObject = null;
            if (isGrabbed)
            {
                PaletteObjectController paletteObject = grabbedObject.GetComponent<PaletteObjectController>();
                grabbedObject.transform.position = paletteObject.defaultPosition;
                // grabbedObject
                isGrabbed = false;
            }
        }
    }

    // 物体を追従させる
    private void MoveGrabbedObject()
    {
        grabbedObject.transform.position = this.gameObject.transform.position;
    }



}
