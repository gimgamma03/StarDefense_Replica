using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : Singleton<GameManager>
{
    public int gold = 0;
    public int life = 10;

    public WayPoint wayPoint;
    private Tilemap tilemap;
    public GameObject nexus;
    public GameObject mineral;
    private BaseTowerUnit beforeSearchTower;

    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI lifeText;
    [SerializeField] private RectTransform gameoverUI;
    [SerializeField] private RectTransform combineTowerButton;

    public int Gold
    {
        get
        {
            return gold;
        }

        set
        {
            gold = value;
            goldText.text = $"골드 : {gold}";
        }
    }

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
            Time.timeScale = 0.01f;
            gameoverUI.gameObject.SetActive(true);
        }

        lifeText.text = $"Life\n{life} / {10}";
    }

    public void AddMiner()
    {
        GameObject go = ObjectPool.Instance.SpawnFromPool("Miner", nexus.transform.position);
        float yRandom = Random.Range(-0.2f, 0.2f);
        go.transform.position += new Vector3(0, yRandom, 0);

        Gold--;
    }
}
