using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : MonoBehaviour, IMovable, IDamageable
{
    //인스펙터 확인용 퍼블릭
    public List<Transform> wayPointList;
    public Transform targetPos;
    public int targetListIndex = 0;


    Vector2 moveDirection;

    public float hp;
    public float maxHp;
    public float moveSpeed;
    public string enemyName;

    public event Action<EnemyUnit> OnDeath;
    private EnemyData enemyData;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Move();
    }

    public void Initialize(EnemyData data)
    {
        enemyData = data;

        maxHp = enemyData.maxHp;
        moveSpeed = enemyData.moveSpeed;
        enemyName = enemyData.enemyName;
        spriteRenderer.sprite = enemyData.enemySprite;

        hp = maxHp;
        targetListIndex = 0;
        wayPointList = GameManager.Instance.wayPoint.GetWayPointList();

        SetMoveTarget();
    }

    private void SetMoveTarget()
    {
        targetPos = wayPointList[targetListIndex];

        Vector3 diff = targetPos.position - transform.position;

        if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
        {
            //좌우 방향
            moveDirection = new Vector3(Mathf.Sign(diff.x), 0, 0);

            //스프라이트 좌우 반전
            spriteRenderer.flipX = (diff.x < 0); // 왼쪽이면 flipX = true
        }
        else
        {
            //상하 방향
            moveDirection = new Vector3(0, Mathf.Sign(diff.y), 0);
        }
    }


    public void Move()
    {
        Vector2 pos = transform.position;
        pos += moveDirection * moveSpeed * Time.deltaTime;
        transform.position = pos;

        if (Vector2.Distance(transform.position, targetPos.position) < 0.1f)
        {
            targetListIndex++;
            if (targetListIndex < wayPointList.Count)
                SetMoveTarget();
        }
    }

    public void TakeDamage(float amount)
    {
        hp -= amount;

        if (hp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        OnDeath?.Invoke(this);
        ObjectPool.Instance.ReturnToPool(this.gameObject, "Enemy");

        if (!SpawnManager.ActiveEnemies.Contains(this))
        {
            SpawnManager.ActiveEnemies.Remove(this);
        }
    }


}
