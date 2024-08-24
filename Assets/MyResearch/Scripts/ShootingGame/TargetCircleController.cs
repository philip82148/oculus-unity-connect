using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCircleController : MonoBehaviour
{
    [SerializeField]
    private Renderer targetRenderer;

    [SerializeField]
    private ShotTargetController shotTargetController;

    private Color defaultColor = Color.white;
    private Color inOffenseTimeColor = Color.red;

    private float waitTime = 8.0f;
    private float tmpWaitingTime = 0f;


    private float inAttackTime = 1.0f;
    private float tmpInAttackTime = 0;



    private bool isAttack = false;
    private bool isHit = false;


    void Update()
    {
        if (!isAttack)
        {
            targetRenderer.material.color = defaultColor;
            if (waitTime <= tmpWaitingTime)
            {
                isAttack = true;
                tmpWaitingTime = 0;
            }
            tmpWaitingTime += Time.deltaTime;
            isHit = false;
        }
        else
        {
            if (!isHit)
            {
                targetRenderer.material.color = inOffenseTimeColor;
            }
            if (inAttackTime <= tmpInAttackTime)
            {
                isAttack = false;
                tmpInAttackTime = 0;
            }
            tmpInAttackTime += Time.deltaTime;

        }
    }



    void OnTriggerEnter(Collider collider)
    {

        if (isAttack)
        {
            targetRenderer.material.color = Color.green;
            isHit = true;
            shotTargetController.CallShot();
        }
    }
}
