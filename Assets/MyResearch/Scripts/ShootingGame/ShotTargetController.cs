using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotTargetController : MonoBehaviour
{
    [SerializeField] private Renderer targetRenderer;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("shot");
        // targetRenderer.material.color = Color.red;
    }
    void OnTriggerEnter(Collider collision)
    {
        Debug.Log("shot");
        // targetRenderer.material.color = Color.red;
    }
}
