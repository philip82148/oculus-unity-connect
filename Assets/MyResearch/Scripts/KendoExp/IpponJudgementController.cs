using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IpponJudgementController : MonoBehaviour
{

    [Header("UI Setting")]
    [SerializeField] private TextMeshProUGUI judgementText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void judgeIppon()
    {
        judgementText.text = "Ippon";
    }
    public void judgeNotIppon()
    {
        judgementText.text = "Not Ippon";
    }
}
