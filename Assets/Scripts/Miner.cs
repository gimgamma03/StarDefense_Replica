using System.Collections;
using UnityEngine;

public class Miner : MonoBehaviour
{
    private Transform nexus;
    private Transform mineral;

    private float moveSpeed = 2.0f;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        nexus = GameManager.Instance.nexus.transform;
        mineral = GameManager.Instance.mineral.transform;

        StartCoroutine(MiningRoutine());
    }

    IEnumerator MiningRoutine()
    {
        while (true)
        {
            // 미네랄로 이동
            yield return MoveTo(mineral.position);
            yield return new WaitForSeconds(1f);

            // 넥서스로 이동
            yield return MoveTo(nexus.position);
            yield return new WaitForSeconds(1f);

            // 자원 획득
            // GameManager.Instance.AddGold(10);
        }
    }

    IEnumerator MoveTo(Vector3 targetPos)
    {
        while (Vector3.Distance(transform.position, targetPos) > 0.05f)
        {
            // 이동
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

            // 좌우 방향 체크
            Vector3 diff = targetPos - transform.position;
            if (diff.x < 0)
            {
                spriteRenderer.flipX = true;  // 왼쪽
            }
            else if (diff.x > 0)
            {
                spriteRenderer.flipX = false; // 오른쪽
            }

            yield return null; // 다음 프레임까지 대기
        }
    }
}
