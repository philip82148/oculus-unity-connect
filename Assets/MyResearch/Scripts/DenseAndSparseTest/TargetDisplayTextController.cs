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
        indexText.text = "x:" + x.ToString() + "y:" + y.ToString() + "z:" + z.ToString();
    }
}
