using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 moveDirection;
    private float speed;
    [SerializeField] private GameObject explosionEffect; // 爆発エフェクトのプレハブ
    [SerializeField] private Renderer thisRenderer;
    public void Initialize(Vector3 direction, float speed)
    {
        this.moveDirection = direction;
        this.speed = speed;
    }

    void Update()
    {
        // 毎フレーム移動する
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
    }
    void OnCollisionEnter(Collision collision)
    {

        Destroy(this.gameObject);
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(explosionEffect, 1f);
        }
        Debug.Log("当たった!");
    }
    void OnCollisionEnter2D(Collision2D collision) // 2D用に変更
    {
        // 衝突時に自身を破壊
        Destroy(this.gameObject);

        if (explosionEffect != null)
        {
            // 爆発エフェクトを生成
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        Debug.Log("circle当たった!");
    }

    public void SetColor(Color color)
    {
        thisRenderer.material.color = color;
    }
}
