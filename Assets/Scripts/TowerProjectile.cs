using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerProjectile : MonoBehaviour
{
    private Vector2 targetPosition;
    public float speed = 5f;

    public void Init(Transform target)
    {
        targetPosition = target.position; // 발사 순간 위치만 저장
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            targetPosition,
            speed * Time.deltaTime
        );

        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            ObjectPool.Instance.ReturnToPool(this.gameObject, "TowerProjectile");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            //Debug.Log("적 맞춤");
            ObjectPool.Instance.ReturnToPool(this.gameObject, "TowerProjectile");
        }
    }
}
