using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 moveDirection;
    private float speed;
    [SerializeField] private GameObject explosionEffect; // 爆発エフェクトのプレハブ
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
}
