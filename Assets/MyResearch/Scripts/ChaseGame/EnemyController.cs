using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private ChaseGameController chaseGameController;
    [SerializeField] private EnemyAI enemyAI;
    [SerializeField] private DamageTextAnimation damageTextAnimation;
    private const int DEFAULT_HIT_POINT = 100;
    private int hitPoint = 100;
    private float DEFAULT_SPEED;
    [SerializeField] private float speed;
    [SerializeField] private Renderer thisRenderer;
    [SerializeField] private List<Color> colorList;
    [SerializeField] private List<GameObject> targetList;
    private float switchInterval = 15f; // 状態を切り替える間隔（秒）
    private int targetColorIndex = 0;

    private Vector3 defaultPosition;

    [Header("Attack Setting")]
    [SerializeField] private GameObject bombPrefab; // 敵が投擲するBombプレハブ
    [SerializeField] private Transform playerTransform;

    [SerializeField] private float attackInterval = 15f;  // 攻撃間隔（秒）
    [SerializeField] private float bombInitialSpeed = 1.0f; // 爆弾の初速
    private float nextAttackTime = 0f; // 次の攻撃時間
    private bool canAttack = true;

    // Start is called before the first frame update
    void Start()
    {
        defaultPosition = this.transform.position;
        DEFAULT_SPEED = speed;

        chaseGameController.SetHP(hitPoint);
        // コルーチンを開始
        StartCoroutine(SetRandomColor());
    }

    public void Initialize()
    {
        this.transform.position = defaultPosition;
        hitPoint = DEFAULT_HIT_POINT;
        speed = DEFAULT_SPEED;

    }

    // Update is called once per frame
    void Update()
    {
        HandleAttack();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Bullet bullet = collision.gameObject.GetComponent<Bullet>();
        if (bullet != null)
        {
            // chaseGameController.AddScore();
            hitPoint -= 1;
            chaseGameController.SetHP(hitPoint);
            if (targetColorIndex == 0)
            {
                damageTextAnimation.StartAnimation(true);
            }
            else
            {
                damageTextAnimation.StartAnimation(false);
            }
        }
        else if (collision.gameObject.GetComponent<Bomb>() != null)
        {
            hitPoint -= 1;
            chaseGameController.SetHP(hitPoint);
            if (targetColorIndex == 1)
            {
                damageTextAnimation.StartAnimation(true);
            }
            else
            {
                damageTextAnimation.StartAnimation(false);
            }
        }
        else if (collision.gameObject.GetComponent<Rope>() != null)
        {
            // this.speed = this.speed / 2;
            hitPoint -= 1;
            Debug.Log("hit water" + speed);
            // enemyAI.SetSpeed(this.speed);
            if (targetColorIndex == 2)
            {
                damageTextAnimation.StartAnimation(true);
            }
            else
            {
                damageTextAnimation.StartAnimation(false);
            }
        }

    }

    public void SetColor(Color color)
    {
        thisRenderer.material.color = color;
    }

    public void SetTargetActivation(int randomIndex)
    {
        for (int i = 0; i < targetList.Count; i++)
        {
            if (i == randomIndex)
            {
                targetList[i].gameObject.SetActive(true);
            }
            else
            {
                targetList[i].gameObject.SetActive(false);
            }
        }

    }
    private IEnumerator SetRandomColor()
    {
        while (true)
        {
            int randomIndex;
            // ランダムに1つのオブジェクトをアクティブにし、他を非アクティブにする
            do
            {
                randomIndex = Random.Range(0, colorList.Count);
            } while (randomIndex == targetColorIndex);


            // for (int i = 0; i < colorList.Count; i++)
            // {
            //     targetObjects[i].SetActive(i == randomIndex); // 選ばれたオブジェクトだけアクティブ
            // }
            targetColorIndex = randomIndex;
            SetColor(colorList[randomIndex]);
            SetTargetActivation(randomIndex);
            targetColorIndex = randomIndex;
            // 指定された間隔待つ
            yield return new WaitForSeconds(switchInterval);
        }
    }
    private void HandleAttack()
    {
        if (!canAttack) return;

        if (Time.time > nextAttackTime && playerTransform != null)
        {
            // プレイヤーへ攻撃
            AttackPlayer();
            nextAttackTime = Time.time + attackInterval;
        }
    }
    private void AttackPlayer()
    {
        if (bombPrefab == null) return;

        // 敵→プレイヤーへ向かう方向を計算
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        // 初速をかける
        Vector3 bombVelocity = direction * bombInitialSpeed;

        // 爆弾生成
        GameObject bombObj = Instantiate(bombPrefab, transform.position + Vector3.up * 1.0f, Quaternion.identity);
        Bomb bomb = bombObj.GetComponent<Bomb>();
        if (bomb != null)
        {
            bomb.Initialize(bombVelocity);
        }
    }

    public void EnableAttack(bool enable)
    {
        canAttack = enable;
    }

    public float GetSpeed()
    {
        return this.speed;
    }
}
