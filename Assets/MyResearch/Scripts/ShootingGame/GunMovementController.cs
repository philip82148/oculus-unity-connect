using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunMovementController : MonoBehaviour
{

    private const float speed = 0.5f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += new Vector3(Input.GetAxis("Horizontal") * Time.deltaTime, 0f, 0f);
        this.transform.position += new Vector3(0f, Input.GetAxis("Vertical") * Time.deltaTime, 0f);

        if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickRight))
        {
            this.transform.position += new Vector3(speed * Time.deltaTime, 0f, 0f);
        }
        if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickLeft))
        {
            this.transform.position += new Vector3(-speed * Time.deltaTime, 0f, 0f);
        }
        if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickUp))
        {
            this.transform.position += new Vector3(0f, speed * Time.deltaTime, 0f);
        }
        if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickDown))
        {
            this.transform.position += new Vector3(0f, -speed * Time.deltaTime, 0f);
        }

    }
}
