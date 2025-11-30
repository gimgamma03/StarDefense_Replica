using UnityEngine;

public class BountyEnemyUnit : EnemyUnit
{
    public int bountyGold = 500; // 잡으면 주는 보상 골드

    public void Initialize()
    {
        hp = maxHp;
        targetListIndex = 0;
        wayPointList = GameManager.Instance.wayPoint.GetWayPointList();

        SetMoveTarget();
    }

    public override void Die()
    {
        base.Die(); // 기본 EnemyUnit의 Die 처리 (풀 반환, 리스트 제거 등)
        //GameManager.Instance.AddGold(bountyGold); // 보상 지급
        Debug.Log($"{enemyName} 처치! 현상금 {bountyGold} 골드 획득!");
    }
}
