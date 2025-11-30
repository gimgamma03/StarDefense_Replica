using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRangeVisualizer : MonoBehaviour
{
    public List<EnemyUnit> _enemiesInRange;

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnVisual()
    {
        _spriteRenderer.enabled = true;
    }

    public void OffVisual()
    {
        _spriteRenderer.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.gameObject);

        if (collision.TryGetComponent<EnemyUnit>(out EnemyUnit enemyUnit)) 
        {
            if (!_enemiesInRange.Contains(enemyUnit))
            {
                _enemiesInRange.Add(enemyUnit);
                enemyUnit.OnDeath += RemoveEnemyInRange;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<EnemyUnit>(out EnemyUnit enemyUnit))
        {
            if (_enemiesInRange.Contains(enemyUnit))
            {
                _enemiesInRange.Remove(enemyUnit);
                enemyUnit.OnDeath -= RemoveEnemyInRange;
            }
        }
    }

    private void RemoveEnemyInRange(EnemyUnit dieEnemy)
    {
        if (_enemiesInRange.Contains(dieEnemy))
        {
            _enemiesInRange.Remove(dieEnemy);
        }
    }
}
