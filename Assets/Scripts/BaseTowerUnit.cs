using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTowerUnit : MonoBehaviour, IAttackable
{
    public int grade;

    public float attackRange;
    public float attackDelay;
    public float attackDamage;
    public float lastAttackTime;
    public string towerName;

    public EnemyUnit _targetEnemy;
    List<EnemyUnit> enemyListInRange;


    private SpriteRenderer towerImageRenderer;
    private TowerRangeVisualizer _rangeVisualizer;

    private void Awake()
    {
        if (transform.childCount > 0)
        {
            var firstChild = transform.GetChild(0);
            towerImageRenderer = firstChild.GetComponent<SpriteRenderer>();
        }

        _rangeVisualizer = GetComponentInChildren<TowerRangeVisualizer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Initialize(TowerData towerData)
    {
        grade = towerData.grade;
        towerName = towerData.towerName;
        attackDelay = towerData.attackDelay;
        attackDamage = towerData.attackDamage;
        towerImageRenderer.sprite = towerData.towerImage;

        OffRangeVisualizer();
    }

    // Update is called once per frame
    void Update()
    {
        lastAttackTime += Time.deltaTime;

        if (lastAttackTime > attackDelay)
        {
            if (!_rangeVisualizer.gameObject.activeSelf)
            {
                return;
            }

            FindTarget();
            Attack();
            lastAttackTime = 0;
        }
    }

    private void OnEnable()
    {
        lastAttackTime = 0;
    }

    void OnDrawGizmos()
    {
        // 시작점: 오브젝트 위치
        Vector2 start = transform.position;

        // 끝점: 오브젝트 앞 방향으로 2만큼
        Vector2 end = start + (Vector2)transform.up * 2f;

        // Scene 뷰에 빨간색 선 그리기
        Gizmos.color = Color.red;
        Gizmos.DrawLine(start, end);
    }

    private void FindTarget()
    {
        if (_rangeVisualizer._enemiesInRange.Count == 0)
        {
            _targetEnemy = null;
            return;
        }

        float minDistance = float.MaxValue;

        foreach(EnemyUnit enemy in _rangeVisualizer._enemiesInRange)
        {
            if (enemy == null)
            {
                continue;
            }

            float distance = Vector2.Distance(transform.position, enemy.transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                _targetEnemy = enemy;
            }
        }

        return;
    }

    public void Attack()
    {
        if (_targetEnemy == null)
        {
            return;
        }

        GameObject tp = ObjectPool.Instance.SpawnFromPool("TowerProjectile", transform.position);

        if (tp == null)
        {
            return;
        }

        TowerProjectile towerProjectile = tp.GetComponent<TowerProjectile>();
        towerProjectile.Init(this);
    }

    public void OnRangeVisualizer()
    {
        _rangeVisualizer.OnVisual();
    }

    public void OffRangeVisualizer()
    {
        _rangeVisualizer.OffVisual();
    }

    public void AddEnemyInRange(EnemyUnit enemy)
    {
        if (!enemyListInRange.Contains(enemy))
        {
            enemyListInRange.Add(enemy);
        }
    }

    public void RemoveEnemyInRange(EnemyUnit enemy)
    {
        if (enemyListInRange.Contains(enemy))
        {
            enemyListInRange.Remove(enemy);
        }
    }
}
