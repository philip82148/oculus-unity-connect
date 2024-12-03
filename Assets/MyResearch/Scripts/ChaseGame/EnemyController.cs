using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private ChaseGameController chaseGameController;
    [SerializeField] private EnemyAI enemyAI;
    private int hitPoint = 100;
    [SerializeField] private float speed;
    // Start is called before the first frame update
    void Start()
    {
        chaseGameController.SetHP(hitPoint);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter(Collision collision)
    {
        Bullet bullet = collision.gameObject.GetComponent<Bullet>();
        if (bullet != null)
        {
            // chaseGameController.AddScore();
            hitPoint -= 1;
            chaseGameController.SetHP(hitPoint);
        }
        else if (collision.gameObject.GetComponent<Bomb>() != null)
        {
            hitPoint -= 10;
            chaseGameController.SetHP(hitPoint);
        }
        else if (collision.gameObject.GetComponent<Rope>() != null)
        {
            this.speed = this.speed / 2;
            Debug.Log("hit water" + speed);
            enemyAI.SetSpeed(this.speed);

        }

    }


    public float GetSpeed()
    {
        return this.speed;
    }
}
