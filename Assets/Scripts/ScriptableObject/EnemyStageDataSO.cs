using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "TowerDefense/StageData")]
public class EnemyStageDataSO : ScriptableObject
{
    public EnemyWaveDataSO[] waveData;
}
