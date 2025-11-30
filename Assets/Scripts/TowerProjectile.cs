using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerProjectile : MonoBehaviour
{
    public float speed = 5f;
    private float damage = 1.0f;

    private Vector2 targetPosition;

    public void CommanderProjectile(Commander commander)
    {
        damage = commander.attackDamage;
        speed = commander.projectileSpeed;
        targetPosition = commander._targetEnemy.transform.position;
    }

    public void TowerProejctile(BaseTowerUnit tower)
    {
        damage = tower.attackDamage;
        targetPosition = tower._targetEnemy.transform.position;
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
            ObjectPool.Instance.ReturnToPool(this.gameObject, "Projectile");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            EnemyUnit enemyUnit = collision.GetComponent<EnemyUnit>();
            enemyUnit.TakeDamage(5.0f);
            ObjectPool.Instance.ReturnToPool(this.gameObject, "Projectile");
        }
    }
}
