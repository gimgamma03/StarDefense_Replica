using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commander : MonoBehaviour, IAttackable
{
    public float attackDelay;      // 공격 간격
    public float attackDamage;    // 공격 데미지
    public float projectileSpeed = 4.5f;
    private float lastAttackTime = 0f;

    public EnemyUnit _targetEnemy;

    void Update()
    {
        lastAttackTime += Time.deltaTime;

        if (lastAttackTime >= attackDelay)
        {
            FindTarget();
            Attack();
            lastAttackTime = 0f;
        }
    }

    private void FindTarget()
    {
        if (SpawnManager.ActiveEnemies == null || SpawnManager.ActiveEnemies.Count == 0)
        {
            _targetEnemy = null;
            return;
        }

        float minDistance = float.MaxValue;
        EnemyUnit closestEnemy = null;

        foreach (EnemyUnit enemy in SpawnManager.ActiveEnemies)
        {
            if (enemy == null) continue;

            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestEnemy = enemy;
            }
        }

        _targetEnemy = closestEnemy;
    }

    public void Attack()
    {
        if (_targetEnemy == null || !_targetEnemy.gameObject.activeSelf) return;

        GameObject projectile = ObjectPool.Instance.SpawnFromPool("Projectile", transform.position);
        projectile.GetComponent<TowerProjectile>().CommanderProjectile(this);
    }

    // 골에 도달한 적 처리 (기존 코드 유지)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.CompareTag("Enemy"))
        {
            GameManager.Instance.OnEnemyReachedGoal();
            collision.GetComponent<EnemyUnit>().Die();
        }
    }
}
