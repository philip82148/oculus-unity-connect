// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.AI;

// public class EnemyAI : MonoBehaviour
// {
//     [SerializeField] private Transform player; // プレイヤーのTransform
//     [SerializeField] private float fleeDistance = 0.5f; // 逃げ始める距離
//     [SerializeField] private float safeDistance = 1f; // 安全と判断する距離
//     [SerializeField] private NavMeshAgent agent;
//     [SerializeField] private float speed = 2.0f;
//     [SerializeField] private Vector3 dirToPlayer;
//     [SerializeField] private int flag = 0;
//     private Vector3 destination;

//     void Start()
//     {
//         // NavMeshAgentの速度を設定
//         agent.speed = speed;
//     }

//     void Update()
//     {
//         float distance = Vector3.Distance(transform.position, player.position);

//         if (distance < fleeDistance)
//         {
//             // プレイヤーから遠ざかる方向を計算（Y方向の移動を防ぐ）
//             dirToPlayer = transform.position - player.position;
//             dirToPlayer.y = 0;

//             Vector3 fleePosition = transform.position + dirToPlayer.normalized * safeDistance;
//             fleePosition.y = transform.position.y; // Y座標を固定

//             NavMeshPath path = new NavMeshPath();
//             if (NavMesh.CalculatePath(transform.position, fleePosition, NavMesh.AllAreas, path))
//             {
//                 Vector3 targetPosition = path.corners[path.corners.Length - 1];
//                 targetPosition.y = transform.position.y; // Y座標を固定

//                 agent.SetDestination(targetPosition);
//                 flag = 1;
//             }
//             else
//             {
//                 // 経路が見つからない場合、ランダムな方向に逃げる
//                 Vector3 randomPoint = GetRandomPoint();
//                 agent.SetDestination(randomPoint);
//             }
//         }
//         else
//         {
//             flag = 4;
//             // 現在位置に留まる（Y座標を固定）
//             Vector3 currentPosition = transform.position;
//             currentPosition.y = transform.position.y;
//             agent.SetDestination(currentPosition);
//         }
//     }

//     Vector3 GetRandomPoint()
//     {
//         // ランダムな方向を取得（Y方向の移動を防ぐ）
//         Vector3 randomDirection = Random.insideUnitSphere * safeDistance;
//         randomDirection.y = 0;

//         randomDirection += transform.position;

//         NavMeshHit hit;
//         if (NavMesh.SamplePosition(randomDirection, out hit, safeDistance, NavMesh.AllAreas))
//         {
//             Vector3 hitPosition = hit.position;
//             hitPosition.y = transform.position.y; // Y座標を固定

//             flag = 2;
//             return hitPosition;
//         }
//         else
//         {
//             flag = 3;
//             return transform.position;
//         }
//     }
// }
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform player;       // プレイヤーのTransform
    [SerializeField] private Transform escapePoint;  // 逃げる目的地
    [SerializeField] private float fleeDistance = 0.5f; // 逃げ始める距離

    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float speed = 2.0f;
    [SerializeField] private int flag = 0;

    void Start()
    {
        // NavMeshAgentの速度を設定
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
}
