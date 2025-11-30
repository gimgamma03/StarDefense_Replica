using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private EnemyDataSO _enemyData;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private EnemyStageDataSO _enemyStageData;

    private int _waveIndex = 0;
    private int _maxWaveCount => _enemyStageData.waveData.Count();

    void Start()
    {
        StartCoroutine(SpawnAllWaves());
    }

    IEnumerator SpawnAllWaves()
    {
        while (_waveIndex < _maxWaveCount)
        {
            // 현재 웨이브 데이터 가져오기
            EnemyWaveDataSO currentWave = _enemyStageData.waveData[_waveIndex];

            // UI 텍스트 업데이트
            waveText.text = $"Wave\n{_waveIndex + 1} / {_maxWaveCount}";

            // 웨이브 시작 전 대기
            yield return new WaitForSeconds(2f);

            // 에너미 소환
            yield return StartCoroutine(SpawnEnemyInWave(currentWave));

            // 웨이브 종료 후 대기
            yield return new WaitForSeconds(2f);

            _waveIndex++;
        }

        waveText.text = $"Wave\n완료!";
        Debug.Log("모든 웨이브 완료!");
    }

    IEnumerator SpawnEnemyInWave(EnemyWaveDataSO waveData)
    {
        for (int i = 0; i < waveData.spawnCount; i++)
        {
            EnemyData randomData = _enemyData.enemies[Random.Range(0, _enemyData.enemies.Count)];

            GameObject go = ObjectPool.Instance.SpawnFromPool("Enemy", transform.position);

            EnemyUnit unit = go.GetComponent<EnemyUnit>();
            if (unit != null)
            {
                unit.Initialize(randomData);
            }
            else
            {
                Debug.LogWarning("EnemyUnit 컴포넌트를 찾을 수 없습니다!");
            }

            yield return new WaitForSeconds(waveData.spawnInterval);
        }
    }
}
