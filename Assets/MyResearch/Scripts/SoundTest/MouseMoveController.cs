using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MouseMoveController : MonoBehaviour
{

    private Vector3 mousePos, pos;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            mousePos = Input.mousePosition;
            Debug.Log(mousePos);
            pos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));
            Debug.Log(pos);
            this.transform.position = pos;
        }
    }
}
