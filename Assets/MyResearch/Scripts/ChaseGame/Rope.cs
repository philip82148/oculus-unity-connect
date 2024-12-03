using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    private Vector3 moveDirection;
    private float speed;
    [SerializeField] private GameObject explosionEffectPrefab; // 爆発エフェクトのプレハブ
    private GameObject explosionEffectInstance; // 実際に生成されたエフェクトのインスタンス

    public void Initialize(Vector3 direction, float speed)
    {
        this.moveDirection = direction;
        this.speed = speed;

        // プレハブからエフェクトを生成し、インスタンスを保持する
        explosionEffectInstance = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);

        // 一定時間後にインスタンスを破壊
        Destroy(explosionEffectInstance, 1.5f);
    }

    void Update()
    {
        // 毎フレーム移動する
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);

        // 爆発エフェクトが存在する場合は位置を更新
        if (explosionEffectInstance != null)
        {
            explosionEffectInstance.transform.position = this.transform.position;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // 自身を破壊する
        Destroy(this.gameObject);

        // デバッグ用ログ
        Debug.Log("当たった!");
    }
}
