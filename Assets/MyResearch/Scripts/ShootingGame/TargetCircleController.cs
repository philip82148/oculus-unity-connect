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

    private float[] waitTimeRange = { 3.0f, 8.0f };

    private float waitTime = 8.0f;
    private float tmpWaitingTime = 0f;


    private float inAttackTime = 1.0f;
    private float tmpInAttackTime = 0;



    private bool isAttack = false;
    private bool isHit = false;


    void Start()
    {
        RandomSelectWaitingTime();
    }


    void Update()
    {
        // this.gameObject.SetActive(isAttack);
        if (!isAttack)
        {
            // this.gameObject.SetActive(false);
            targetRenderer.material.color = defaultColor;
            if (waitTime <= tmpWaitingTime)
            {
                isAttack = true;
                tmpWaitingTime = 0;
                RandomSelectWaitingTime();
            }
            tmpWaitingTime += Time.deltaTime;
            isHit = false;
        }
        else
        {
            if (!isHit)
            {

                targetRenderer.material.color = inOffenseTimeColor;
                // this.gameObject.SetActive(true);
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

    private void RandomSelectWaitingTime()
    {
        waitTime = Random.Range(waitTimeRange[0], waitTimeRange[1] + 1);
    }
}
