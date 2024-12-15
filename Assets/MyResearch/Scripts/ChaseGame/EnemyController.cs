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
    private float switchInterval = 2f; // 状態を切り替える間隔（秒）
    private int targetColorIndex = 0;

    private Vector3 defaultPosition;

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
    private IEnumerator SetRandomColor()
    {
        while (true)
        {
            // ランダムに1つのオブジェクトをアクティブにし、他を非アクティブにする
            int randomIndex = Random.Range(0, colorList.Count);

            // for (int i = 0; i < colorList.Count; i++)
            // {
            //     targetObjects[i].SetActive(i == randomIndex); // 選ばれたオブジェクトだけアクティブ
            // }
            targetColorIndex = randomIndex;
            SetColor(colorList[randomIndex]);
            // 指定された間隔待つ
            yield return new WaitForSeconds(switchInterval);
        }
    }

    public float GetSpeed()
    {
        return this.speed;
    }
}
