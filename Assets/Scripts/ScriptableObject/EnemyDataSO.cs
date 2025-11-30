using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "TowerDefense/EnemyData")]
public class EnemyDataSO : ScriptableObject
{
    public List<EnemyData> enemies;

    public GameObject prefab; // 실제 프리팹 연결
}

[System.Serializable]
public class EnemyData
{
    public string enemyName;
    public float maxHp;
    public float moveSpeed;
    public Sprite enemySprite;
}


