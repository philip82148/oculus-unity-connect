using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TargetDisplayTextController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI indexText;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetTargetXYZ(int x, int y, int z)
    {
        indexText.text = "左から:" + x.ToString() + "、高さ:" + (y + 1).ToString() + "奥行き:" + (z + 1).ToString();
    }
}
