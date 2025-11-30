using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : MonoBehaviour, IMovable
{
    //인스펙터 확인용 퍼블릭
    public List<Transform> wayPointList;
    public Transform targetPos;
    public int targetListIndex = 0;


    Vector2 moveDirection;


    public float moveSpeed;
    public string enemyName;

    public event Action OnDeath;


    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Move();
    }

    /*void OnEnable()
    {
        if (GameManager.Instance != null && GameManager.Instance.wayPoint != null)
        {
            wayPointList = GameManager.Instance.wayPoint.GetWayPointList();
            targetListIndex = 0;
            SetMoveTarget();
        }
    }*/

    public void Initialize()
    {
        wayPointList = GameManager.Instance.wayPoint.GetWayPointList();

        SetMoveTarget();
    }

    private void SetMoveTarget()
    {
        targetPos = wayPointList[targetListIndex];

        Vector3 diff = targetPos.position - transform.position;

        if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
        {
            // 좌우 방향
            moveDirection = new Vector3(Mathf.Sign(diff.x), 0, 0);
        }
        else
        {
            // 상하 방향
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

    public void Die()
    {
        OnDeath?.Invoke();
    }

}
