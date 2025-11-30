using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using System.Collections.Generic;

public class TowerManager : Singleton<TowerManager>
{
    [SerializeField] private TowerDatasSO towerDatas;   // SO 연결
    [SerializeField] private GameObject towerPrefab;    // 기본 타워 프리팹
    [SerializeField] private Tilemap tilemap;

    // 좌표별 타워 관리
    private Dictionary<Vector3Int, BaseTowerUnit> towersByCell = new Dictionary<Vector3Int, BaseTowerUnit>();
    // 등급별 타워 관리
    private Dictionary<int, List<BaseTowerUnit>> towersByGrade = new Dictionary<int, List<BaseTowerUnit>>();

    public void SpawnTower(Vector3Int cellPos, int grade)
    {
        // 이미 설치된 셀인지 확인
        if (towersByCell.ContainsKey(cellPos))
        {
            Debug.Log("이미 타워가 설치된 셀입니다!");
            return;
        }

        // 등급에 맞는 데이터 찾기
        TowerGradeData gradeData = towerDatas.towerGrades
            .FirstOrDefault(g => g.grade == grade);

        if (gradeData == null || gradeData.towers.Count == 0)
        {
            Debug.LogWarning($"등급 {grade} 타워 데이터 없음!");
            return;
        }

        // 랜덤으로 하나 선택
        TowerData selectedData = gradeData.towers[Random.Range(0, gradeData.towers.Count)];

        // 타일 중앙 좌표
        Vector3 worldPos = tilemap.GetCellCenterWorld(cellPos);

        // 프리팹 생성
        GameObject towerObj = Instantiate(towerPrefab, worldPos, Quaternion.identity);

        // TowerUnit 초기화
        BaseTowerUnit tower = towerObj.GetComponent<BaseTowerUnit>();
        if (tower != null)
        {
            tower.Initialize(selectedData);

            // 좌표 등록
            towersByCell[cellPos] = tower;

            // 등급 등록
            if (!towersByGrade.ContainsKey(grade))
                towersByGrade[grade] = new List<BaseTowerUnit>();

            towersByGrade[grade].Add(tower);

            // 타일 상태 갱신
            TileMapManager.Instance.SetTowerPlaced(cellPos, true);
        }
    }

    public void RemoveTower(Vector3Int cellPos)
    {
        if (!towersByCell.ContainsKey(cellPos)) return;

        BaseTowerUnit tower = towersByCell[cellPos];

        // 등급 리스트에서도 제거
        if (towersByGrade.ContainsKey(tower.grade))
            towersByGrade[tower.grade].Remove(tower);

        // 오브젝트 삭제
        Destroy(tower.gameObject);

        // 좌표 딕셔너리에서 제거
        towersByCell.Remove(cellPos);

        // 타일 상태 갱신
        TileMapManager.Instance.SetTowerPlaced(cellPos, false);

        Debug.Log($"타워 제거 완료: {cellPos}");
    }

    public void CombineTower(BaseTowerUnit baseTower)
    {
        string towerName = baseTower.towerName;
        int currentGrade = baseTower.grade;

        if (!towersByGrade.ContainsKey(currentGrade)) return;

        // 같은 이름의 타워들 찾기
        List<BaseTowerUnit> sameTowers = towersByGrade[currentGrade]
            .Where(t => t.towerName == towerName).ToList();

        if (sameTowers.Count < 2)
        {
            Debug.Log("조합할 수 있는 같은 종류의 타워가 2개 이상 필요합니다!");
            return;
        }

        // 선택된 타워 + 다른 하나
        BaseTowerUnit towerA = baseTower;
        BaseTowerUnit towerB = sameTowers.First(t => t != baseTower);

        Vector3Int cellA = towersByCell.First(kv => kv.Value == towerA).Key;
        Vector3Int cellB = towersByCell.First(kv => kv.Value == towerB).Key;

        // 두 타워 제거
        RemoveTower(cellA);
        RemoveTower(cellB);

        // 상위 등급 데이터 찾기
        TowerGradeData nextGradeData = towerDatas.towerGrades
            .FirstOrDefault(g => g.grade == currentGrade + 1);

        if (nextGradeData == null || nextGradeData.towers.Count == 0)
        {
            Debug.Log("상위 등급 타워 데이터 없음!");
            return;
        }

        // 상위 등급 타워 중 랜덤 하나 선택
        TowerData newTowerData = nextGradeData.towers[
            Random.Range(0, nextGradeData.towers.Count)
        ];

        // 두 타워 중 랜덤 위치 선택
        Vector3Int spawnCell = (Random.value > 0.5f) ? cellA : cellB;

        // 새 타워 소환
        SpawnTower(spawnCell, newTowerData.grade);

        Debug.Log($"{towerName} 타워가 {currentGrade}등급에서 {newTowerData.grade}등급으로 조합되었습니다!");
    }
}
