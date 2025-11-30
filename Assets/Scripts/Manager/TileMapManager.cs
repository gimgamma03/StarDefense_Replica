using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class TileMapManager : Singleton<TileMapManager>
{
    public Tilemap tilemap;

    [SerializeField] private TileBase fixTile;
    [SerializeField] private TileBase normalTile;

    [SerializeField] private RectTransform fixTileButton;
    [SerializeField] private RectTransform spawnTowerButton;

    private TileBase _currentSearchTile;
    private Vector3Int _currentCellPos;

    //타일 별 타워 설치 여부 저장 데이터
    Dictionary<Vector3Int, bool> towerPlaced = new Dictionary<Vector3Int, bool>();

    private void Update()
    {
        
    }

    //타일 수리
    public void FixTile()
    {
        if (_currentSearchTile == fixTile)
        {
            tilemap.SetTile(_currentCellPos, normalTile); // 셀 교체

            fixTileButton.gameObject.SetActive(false);
        }
    }

    //ig
    public void SpawnTower()
    {
        // 이미 설치된 셀인지 확인
        if (towerPlaced.ContainsKey(_currentCellPos) && towerPlaced[_currentCellPos])
        {
            Debug.Log("이미 타워가 설치된 셀입니다!");
            return;
        }

        // TowerManager에 소환 요청
        TowerManager.Instance.SpawnTower(_currentCellPos, 1);

        // 설치 여부 기록
        towerPlaced[_currentCellPos] = true;

        // UI 버튼 숨기기
        spawnTowerButton.gameObject.SetActive(false);
    }


    public bool SearchTile(TileBase tile, Vector2 mousePos)
    {
        Vector3Int cellPos = tilemap.WorldToCell(mousePos);
        _currentSearchTile = tile;
        _currentCellPos = cellPos;

        // 이미 설치된 셀인지 확인
        if (towerPlaced.ContainsKey(_currentCellPos) && towerPlaced[_currentCellPos])
        {
            return true;
        }

        if (_currentSearchTile == fixTile)
        {
            fixTileButton.gameObject.SetActive(true);
            fixTileButton.transform.position = mousePos;
            spawnTowerButton.gameObject.SetActive(false);

            return false;
        }
        else if (_currentSearchTile == normalTile)
        {
            spawnTowerButton.gameObject.SetActive(true);
            spawnTowerButton.transform.position = mousePos;
            fixTileButton.gameObject.SetActive(false);

            return false;
        }

        fixTileButton.gameObject.SetActive(false);
        spawnTowerButton.gameObject.SetActive(false);

        return false;
    }

    public void RemoveTowerAtCell(Vector3Int cellPos)
    {
        // 설치 여부 확인
        if (towerPlaced.ContainsKey(cellPos) && towerPlaced[cellPos])
        {
            // TowerManager에 실제 타워 제거 요청
            TowerManager.Instance.RemoveTower(cellPos);

            // 설치 여부 갱신
            towerPlaced[cellPos] = false;

            Debug.Log($"타일 {cellPos}의 타워 제거 완료!");
        }
        else
        {
            Debug.Log("이 셀에는 제거할 타워가 없습니다!");
        }
    }

    public void SetTowerPlaced(Vector3Int cellPos, bool placed)
    {
        towerPlaced[cellPos] = placed;
    }
}
