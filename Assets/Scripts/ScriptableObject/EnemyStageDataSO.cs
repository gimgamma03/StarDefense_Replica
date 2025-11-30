using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "TowerDefense/StageData")]
public class EnemyStageDataSO : ScriptableObject
{
    public List<EnemyWaveData> waveDatas;
}

[System.Serializable]
public class EnemyWaveData
{
    public int spawnCount;
    public float spawnInterval;
}

