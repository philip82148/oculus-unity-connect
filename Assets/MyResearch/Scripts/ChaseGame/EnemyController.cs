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
    [SerializeField] private Renderer thisRenderer;
    [SerializeField] private List<Color> colorList;
    private float switchInterval = 2f; // 状態を切り替える間隔（秒）
    // Start is called before the first frame update
    void Start()
    {
        chaseGameController.SetHP(hitPoint);
        // コルーチンを開始
        StartCoroutine(SetRandomColor());
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
            hitPoint -= 1;
            chaseGameController.SetHP(hitPoint);
        }
        else if (collision.gameObject.GetComponent<Rope>() != null)
        {
            // this.speed = this.speed / 2;
            hitPoint -= 1;
            Debug.Log("hit water" + speed);
            enemyAI.SetSpeed(this.speed);

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
