using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform player;       // プレイヤーのTransform
    [SerializeField] private List<Transform> escapePoints; // 複数の逃げ地点を保持
    [SerializeField] private float fleeDistance = 0.5f; // 逃げ始める距離
    [SerializeField] private EnemyController enemyController;
    [SerializeField] private ChaseGameController chaseGameController;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private float goalDistance;
    private float speed = 4.0f;
    [SerializeField] private int flag = 0;

    [SerializeField] private bool useEscapePoint = true; // trueならescapePointめぐり、falseならランダム逃走

    // escape point巡回用
    private int currentEscapeIndex = 0;

    // ランダム逃走用パラメータ
    [SerializeField] private float wanderRadius = 2f;     // NavMesh上でサンプルする半径
    [SerializeField] private float wanderInterval = 3f;   // 次のランダムポイントを探すまでの間隔
    private float nextWanderTime = 0f;
    private Vector3 currentWanderTarget;
    private bool isGame = true;

    void Start()
    {
        speed = enemyController.GetSpeed();
        agent.speed = speed;
        agent.updateRotation = false;
        currentWanderTarget = transform.position; // 初期

        // 初回、最初のescape pointへ設定
        if (escapePoints != null && escapePoints.Count > 0)
        {
            SetNextEscapePoint();
        }
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        // 敵が常にプレイヤーを注視（後ろ向きで逃げるため）
        transform.LookAt(player);

        if (distance < fleeDistance)
        {
            // 距離が近い: 逃げる処理
            if (useEscapePoint && escapePoints != null && escapePoints.Count > 0)
            {
                // escapePointを巡回しながら逃げる
                FleeBetweenEscapePoints();
            }
            else
            {
                // // 迷路内をランダムに逃げ回る場合
                // RandomFlee();
                // 経路が見つからない場合、その場に留まる
                agent.SetDestination(transform.position);
                flag = 5;
            }
        }
        else
        {
            // 距離が十分ある：その場に留まる
            flag = 4;
            agent.SetDestination(transform.position);
        }

        // escape pointに近づいたら次のポイントへ移動指示を出す処理も可能
        if (useEscapePoint && escapePoints.Count > 0)
        {
            if (!agent.pathPending && agent.remainingDistance < 2f)
            {
                // 現在のescape pointに到達したら次へ
                SetNextEscapePoint();
            }
        }

        Vector3 lastPosition = escapePoints[escapePoints.Count - 1].position;
        goalDistance = Mathf.Sqrt((this.transform.position.x - lastPosition.x) * (this.transform.position.x - lastPosition.x)
                                   + (this.transform.position.z - lastPosition.z) * (this.transform.position.z - lastPosition.z));
        if (goalDistance < 2f && isGame)
        {
            isGame = false;
            EndGame();
        }
    }


    private void EndGame()
    {
        chaseGameController.EndGame();
    }

    void FleeBetweenEscapePoints()
    {
        // 現在指定中のescape pointへ移動するだけ
        // 到達時はUpdate()でSetNextEscapePoint()を呼ぶ
        if (agent.pathStatus == NavMeshPathStatus.PathComplete)
        {
            flag = 1;
        }
        else if (agent.pathStatus == NavMeshPathStatus.PathPartial)
        {
            flag = 2;
            Debug.LogWarning("経路が部分的にしか見つかりませんでした");
        }
        else if (agent.pathStatus == NavMeshPathStatus.PathInvalid)
        {
            flag = 3;
            Debug.LogWarning("経路が見つかりませんでした");
        }
    }

    void SetNextEscapePoint()
    {
        // 次のescape pointを決定（巡回）
        currentEscapeIndex = (currentEscapeIndex + 1) % escapePoints.Count;
        Transform targetPoint = escapePoints[currentEscapeIndex];

        NavMeshPath path = new NavMeshPath();
        Vector3 targetPosition = targetPoint.position;
        targetPosition.y = transform.position.y;

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
    }

    void RandomFlee()
    {
        flag = 1; // ランダム逃走中

        // プレイヤーから敵へのベクトル
        Vector3 awayFromPlayerDir = (transform.position - player.position).normalized;

        if (Time.time > nextWanderTime || agent.remainingDistance < 0.1f)
        {
            Vector3 basePos = transform.position + awayFromPlayerDir * (wanderRadius * 0.5f);
            int maxAttempts = 5;
            NavMeshHit hit;
            Vector3 candidatePos = transform.position;

            for (int i = 0; i < maxAttempts; i++)
            {
                Vector3 randomOffset = Random.insideUnitSphere * wanderRadius;
                Vector3 attemptPos = basePos + randomOffset;

                if (NavMesh.SamplePosition(attemptPos, out hit, wanderRadius, NavMesh.AllAreas))
                {
                    Vector3 enemyToPlayer = (player.position - transform.position).normalized;
                    Vector3 enemyToCandidate = (hit.position - transform.position).normalized;

                    float dot = Vector3.Dot(enemyToPlayer, enemyToCandidate);
                    if (dot < 0)
                    {
                        candidatePos = hit.position;
                        break;
                    }
                }
            }

            agent.SetDestination(candidatePos);
            currentWanderTarget = candidatePos;
            nextWanderTime = Time.time + wanderInterval;
        }
    }

    public void SetSpeed(float speed)
    {
        agent.speed = speed;
        this.speed = speed;
    }

    public void SetUseEscapePoint(bool useEscape)
    {
        useEscapePoint = useEscape;
    }
}
