using UnityEngine;

[CreateAssetMenu(fileName = "WaveData", menuName = "TowerDefense/WaveData")]
public class EnemyWaveDataSO : ScriptableObject
{
    public GameObject enemyPrefab;
    public int spawnCount;
    public float spawnInterval;
}



