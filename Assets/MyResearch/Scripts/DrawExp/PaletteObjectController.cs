using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaletteObjectController : MonoBehaviour
{
    [SerializeField] private Vector3 defaultPosition = new Vector3(-0.25f, -0.1f, 0f);
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Renderer thisRenderer;
    [SerializeField] private int index = 0;
    [SerializeField] private bool isMove = false;
    // Start is called before the first frame update
    void Start()
    {
        defaultPosition = this.transform.position;
        thisRenderer.material.color = defaultColor;
    }



    public void SetDefaultPosition()
    {
        this.transform.position = defaultPosition;
    }

    public void SetDefaultColor()
    {
        thisRenderer.material.color = defaultColor;
    }
    public int GetIndex()
    {
        return index;
    }
    public bool IsMove()
    {
        return isMove;
    }

}
