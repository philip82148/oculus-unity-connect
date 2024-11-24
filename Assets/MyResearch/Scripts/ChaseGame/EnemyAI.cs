using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform player; // プレイヤーのTransform
    [SerializeField] private float fleeDistance = 0.5f; // 逃げ始める距離
    [SerializeField] private float safeDistance = 1f; // 安全と判断する距離
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float speed = 2.0f;
    [SerializeField] private Vector3 dirToPlayer;
    [SerializeField] private int flag = 0;
    private Vector3 destination;

    // Start is called before the first frame update
    void Start()
    {
        destination = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance < fleeDistance)
        {   // プレイヤーから遠ざかる方向を計算
            dirToPlayer = transform.position - player.position;
            Vector3 fleePosition = transform.position + dirToPlayer.normalized * safeDistance;

            NavMeshPath path = new NavMeshPath();
            if (NavMesh.CalculatePath(transform.position, fleePosition, NavMesh.AllAreas, path))
            {
                agent.SetDestination(path.corners[path.corners.Length - 1] * speed);
                flag = 1;
            }
            else
            {
                // 経路が見つからない場合、ランダムな方向に逃げる
                agent.SetDestination(GetRandomPoint() * speed);

            }

        }
        else
        {
            flag = 4;
            agent.SetDestination(transform.position);
        }
    }
    Vector3 GetRandomPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * safeDistance;
        randomDirection += transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, safeDistance, NavMesh.AllAreas))
        {
            flag = 2;
            return hit.position;
        }
        else
        {
            flag = 3;
            return transform.position;
        }
    }
}
