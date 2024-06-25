using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaletteObjectController : MonoBehaviour
{
    public Vector3 defaultPosition = new Vector3(-0.25f, -0.1f, 0f);
    // Start is called before the first frame update
    void Start()
    {
        defaultPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
