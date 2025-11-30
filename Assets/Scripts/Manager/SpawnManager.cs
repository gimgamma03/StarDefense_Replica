using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private EnemyStageDataSO _enemyStageData;

    private EnemyWaveDataSO _currentEnemyWaveData;
    private int _waveIndex;

    // Start is called before the first frame update
    void Start()
    {
        _currentEnemyWaveData = _enemyStageData.waveData[0];

        StartCoroutine("SpawnEnemyInWave");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnEnemyInWave()
    {
        yield return new WaitForSeconds(1f);

        EnemyUnit enemyInfo;
        enemyInfo = _currentEnemyWaveData.enemyPrefab.GetComponent<EnemyUnit>();

        if (enemyInfo == null) 
        {
            Debug.Log("적 유닛 정보를 가져오는데 실패했습니다.");
            yield return null;
        }

        for (int i= 0; i < _currentEnemyWaveData.spawnCount; i++)
        {
            GameObject go = ObjectPool.Instance.SpawnFromPool(enemyInfo.enemyName, transform.position);

            yield return new WaitForSeconds(_currentEnemyWaveData.spawnInterval);
        }
    }
}
