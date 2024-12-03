
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform player;       // プレイヤーのTransform
    [SerializeField] private Transform escapePoint;  // 逃げる目的地
    [SerializeField] private float fleeDistance = 0.5f; // 逃げ始める距離
    [SerializeField] private EnemyController enemyController;

    [SerializeField] private NavMeshAgent agent;
    private float speed = 4.0f;
    [SerializeField] private int flag = 0;

    void Start()
    {
        // NavMeshAgentの速度を設定
        speed = enemyController.GetSpeed();
        agent.speed = speed;
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < fleeDistance)
        {
            // 逃げる目的地に向かって移動
            Vector3 targetPosition = escapePoint.position;
            targetPosition.y = transform.position.y; // Y座標を固定

            NavMeshPath path = new NavMeshPath();
            if (NavMesh.CalculatePath(transform.position, targetPosition, NavMesh.AllAreas, path))
            {
                agent.SetDestination(targetPosition);

                flag = 1;
            }
            else
            {
                // 経路が見つからない場合、その場に留まる
                agent.SetDestination(transform.position);
                flag = 3;
            }

            if (agent.pathStatus == NavMeshPathStatus.PathComplete)
            {
                flag = 1; // 経路が正常に見つかった
            }
            else if (agent.pathStatus == NavMeshPathStatus.PathPartial)
            {
                flag = 2; // 経路が部分的に見つかった
                Debug.LogWarning("経路が部分的にしか見つかりませんでした");
            }
            else if (agent.pathStatus == NavMeshPathStatus.PathInvalid)
            {
                flag = 3; // 経路が見つからなかった
                Debug.LogWarning("経路が見つかりませんでした");
            }
        }
        else
        {
            flag = 4;
            // 逃げる必要がない場合、その場に留まる
            agent.SetDestination(transform.position);
        }
    }
    public void SetSpeed(float speed)
    {
        agent.speed = speed;
        this.speed = speed;
    }
}
