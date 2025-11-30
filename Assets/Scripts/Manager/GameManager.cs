using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : Singleton<GameManager>
{
    public int life = 10;
    public WayPoint wayPoint;
    private Tilemap tilemap;
    private BaseTowerUnit beforeSearchTower;

    [SerializeField] private TextMeshProUGUI lifeText;
    [SerializeField] private RectTransform combineTowerButton;

    // Start is called before the first frame update
    void Start()
    {
        tilemap = TileMapManager.Instance.tilemap;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 왼쪽 클릭
        {
            ClickMouse();
        }
    }

    private void ClickMouse()
    {
        if (beforeSearchTower != null)
        {
            beforeSearchTower.OffRangeVisualizer();
        }

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPos = tilemap.WorldToCell(mousePos);

        // 셀에 있는 타일 가져오기
        TileBase clickedTile = tilemap.GetTile(cellPos);

        if (clickedTile != null)
        {
            combineTowerButton.gameObject.SetActive(false);

            bool haveTowerThisCell = TileMapManager.Instance.SearchTile(clickedTile, mousePos);

            if (!haveTowerThisCell)
            {
                return;
            }
        }

        int layerMask = LayerMask.GetMask("Tower");
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, layerMask);

        if (hit.collider != null)
        {
            SelectTower(hit.transform.gameObject.GetComponent<BaseTowerUnit>());
        }
    }

    void SelectTower(BaseTowerUnit tower)
    {
        if (tower != null)
        {
            beforeSearchTower = tower;

            tower.OnRangeVisualizer();

            combineTowerButton.gameObject.SetActive(true);
            combineTowerButton.transform.position = tower.gameObject.transform.position;
            combineTowerButton.transform.position += new Vector3(0, 0.6f, 0);
        }
    }

    public void OnClickCombineTower()
    {
        if (beforeSearchTower != null)
        {
            TowerManager.Instance.CombineTower(beforeSearchTower);
            combineTowerButton.gameObject.SetActive(false);
        }
    }

    public void OnEnemyReachedGoal()
    {
        life--;

        if (life == 0)
        {
            //Game Over
        }

        lifeText.text = $"Life\n{life} / {10}";
    }
}
