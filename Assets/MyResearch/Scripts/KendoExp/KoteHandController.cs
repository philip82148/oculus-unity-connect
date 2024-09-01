using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KoteHandController : MonoBehaviour
{
    [SerializeField] private IpponJudgementController ipponJudgementController;

    private void OnCollisionEnter(Collision other)
    {
        string objectName = other.gameObject.name;
        Debug.Log(objectName);
        if (objectName == "Kote")
        {
            ipponJudgementController.judgeIppon();
        }
        else
        {
            ipponJudgementController.judgeNotIppon();
        }

    }
}
