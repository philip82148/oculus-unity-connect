using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private Vector3 velocity;
    private float gravity = -0.981f / 3; // 重力加速度（適宜調整）
    [SerializeField] private GameObject explosionEffect; // 爆発エフェクトのプレハブ

    void Start()
    {
    }

    public void Initialize(Vector3 velocity)
    {
        this.velocity = velocity;
    }

    void Update()
    {
        // 重力を適用
        velocity.y += gravity * Time.deltaTime;

        // 毎フレーム移動する
        transform.Translate(velocity * Time.deltaTime, Space.World);
    }

    void OnCollisionEnter(Collision collision)
    {
        // 衝突時に自身を破壊
        Destroy(this.gameObject);
        if (explosionEffect != null)
        {
            // 爆発エフェクトを生成
            GameObject expl = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(expl, 0.5f);
        }
        Debug.Log("当たった!");
    }
}
