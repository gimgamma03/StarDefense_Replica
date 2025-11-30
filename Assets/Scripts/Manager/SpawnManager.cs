using UnityEngine;
using System.Collections;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class SpawnManager : Singleton<SpawnManager>
{
    [SerializeField] private EnemyDataSO _enemyData;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private GameObject bountyEnemyPrefab;
    [SerializeField] private EnemyStageDataSO _enemyStageData;

    private int _waveIndex = 0;
    private int _maxWaveCount => _enemyStageData.waveDatas.Count;

    public static List<EnemyUnit> ActiveEnemies = new List<EnemyUnit>();

    void Start()
    {
        StartCoroutine(SpawnAllWaves());
    }

    IEnumerator SpawnAllWaves()
    {
        while (_waveIndex < _maxWaveCount)
        {
            EnemyWaveData currentWave = _enemyStageData.waveDatas[_waveIndex];
            waveText.text = $"Wave\n{_waveIndex + 1} / {_maxWaveCount}";

            yield return StartCoroutine(SpawnEnemyInWave(currentWave));

            // 모든 적이 죽을 때까지 기다림
            yield return new WaitUntil(() => ActiveEnemies.Count == 0);

            _waveIndex++;
        }

        waveText.text = $"Wave\n완료!";
        Debug.Log("모든 웨이브 완료!");
    }

    IEnumerator SpawnEnemyInWave(EnemyWaveData waveData)
    {
        yield return new WaitForSeconds(1.0f);

        for (int i = 0; i < waveData.spawnCount; i++)
        {
            EnemyData randomData = _enemyData.enemies[Random.Range(0, _enemyData.enemies.Count)];
            GameObject go = ObjectPool.Instance.SpawnFromPool("Enemy", transform.position);

            EnemyUnit unit = go.GetComponent<EnemyUnit>();
            if (unit != null)
            {
                unit.Initialize(randomData);
                ActiveEnemies.Add(unit);
                unit.OnDeath += HandleEnemyDeath;
            }

            yield return new WaitForSeconds(waveData.spawnInterval);
        }
    }

    public void SpawnBountyEnemy()
    {
        GameObject go = ObjectPool.Instance.SpawnFromPool("BountyEnemy", transform.position);
        BountyEnemyUnit unit = go.GetComponent<BountyEnemyUnit>();

        if (unit != null)
        {
            unit.Initialize();
            ActiveEnemies.Add(unit);
            unit.OnDeath += HandleEnemyDeath;
        }
        else
        {
            Debug.LogError("BountyEnemyUnit 컴포넌트를 찾을 수 없습니다!");
        }
    }

    private void HandleEnemyDeath(EnemyUnit unit)
    {
        ActiveEnemies.Remove(unit);
        unit.OnDeath -= HandleEnemyDeath;
    }
}
