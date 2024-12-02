using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using TMPro;
using UnityEngine;

public class PaletteObjectController : MonoBehaviour
{
    [SerializeField] private Vector3 defaultPosition = new Vector3(-0.25f, -0.1f, 0f);
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Renderer thisRenderer;
    [SerializeField] private int index = 0;
    [SerializeField] private bool isMove = false;

    private int xIndex = 0;
    private int yIndex = 0;
    private int zIndex = 0;

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
    public void SetIndex(int index)
    {
        this.index = index;
    }
    public bool IsMove()
    {
        return isMove;
    }

    public void SetIndexes(int xIndex, int yIndex, int zIndex)
    {
        this.xIndex = xIndex;
        this.yIndex = yIndex;
        this.zIndex = zIndex;


        if (zIndex == 0)
        {
            thisRenderer.material.color = Color.red;
        }
        else if (zIndex == 1)
        {
            thisRenderer.material.color = Color.black;
        }
        else if (zIndex == 2)
        {

            thisRenderer.material.color = Color.green;
        }


    }
}