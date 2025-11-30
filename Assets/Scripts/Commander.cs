using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commander : MonoBehaviour, IAttackable
{
    public float attackDelay;         // 공격 간격
    public float attackDamage;        // 공격 데미지
    public float projectileSpeed = 4.5f;
    private float lastAttackTime = 0f;

    public EnemyUnit _targetEnemy;

    void Update()
    {
        // 적이 없으면 공격 루프 중단
        if (SpawnManager.ActiveEnemies == null || SpawnManager.ActiveEnemies.Count == 0)
            return;

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
        _targetEnemy = null;

        if (SpawnManager.ActiveEnemies == null || SpawnManager.ActiveEnemies.Count == 0)
            return;

        float minDistance = float.MaxValue;
        EnemyUnit closestEnemy = null;

        foreach (EnemyUnit enemy in SpawnManager.ActiveEnemies)
        {
            if (enemy == null || !enemy.gameObject.activeSelf) continue;

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
        if (_targetEnemy == null || !_targetEnemy.gameObject.activeSelf)
            return;

        GameObject projectile = ObjectPool.Instance.SpawnFromPool("Projectile", transform.position);
        projectile.GetComponent<TowerProjectile>().CommanderProjectile(this);
    }

    // 골에 도달한 적 처리
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.gameObject.CompareTag("Enemy"))
        {
            GameManager.Instance.OnEnemyReachedGoal();
            collision.GetComponent<EnemyUnit>().Die();
        }
    }
}
