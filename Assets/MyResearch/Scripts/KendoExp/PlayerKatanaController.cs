using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKatanaController : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 50f;
    [SerializeField] private float koteSpeed = 50f;
    [SerializeField] private float koteMoveSpeed = 5f;     // Kote動作の移動速度を調
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");

        if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickRight, OVRInput.Controller.LTouch))
        {
            this.transform.Rotate(0, Time.deltaTime * rotateSpeed, 0);
        }
        if (OVRInput.Get(OVRInput.Button.PrimaryThumbstickLeft, OVRInput.Controller.LTouch))
        {
            this.transform.Rotate(0, -Time.deltaTime * rotateSpeed, 0);
        }

        if (Input.GetKey(KeyCode.Space) || OVRInput.Get(OVRInput.Button.One))
        {
            // this.transform.Rotate(Time.deltaTime * koteSpeed, 0, 0);
            this.transform.Translate(0, 0, Time.deltaTime * koteMoveSpeed, Space.World);

        }
    }
}
