using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : Singleton<ObjectPool>
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
        public bool canExpand = true;
    }

    [SerializeField] private List<Pool> pools = new List<Pool>();
    private Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();

    //각각의 Pool의 Prefab 저장용
    private Dictionary<string, GameObject> prefabDictionary = new Dictionary<string, GameObject>();

    protected override void Awake()
    {
        base.Awake();
        InitializePools();
    }

    public bool HasPool(string tag)
    {
        return poolDictionary != null && poolDictionary.ContainsKey(tag);
    }

    private void InitializePools()
    {

        foreach (Pool pool in pools)
        {
            if (pool != null && pool.prefab != null)
            {
                CreatePool(pool);
            }
        }
    }

    private void CreatePool(Pool pool)
    {
        Queue<GameObject> objectPool = new Queue<GameObject>();

        for (int i = 0; i < pool.size; i++)
        {
            GameObject obj = CreateNewPoolObject(pool.prefab);
            objectPool.Enqueue(obj);
        }

        poolDictionary[pool.tag] = objectPool;
        prefabDictionary[pool.tag] = pool.prefab;
    }

    private GameObject CreateNewPoolObject(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab);

        // UI 요소인 경우 Canvas를 부모로 설정
        if (obj.GetComponent<RectTransform>() != null)
        {
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas != null)
            {
                obj.transform.SetParent(canvas.transform);

                obj.transform.localScale = Vector3.one;
            }
        }
        else if (prefab.transform.parent == null)
        {
            obj.transform.SetParent(transform);
        }
        else
        {
            obj.transform.SetParent(prefab.transform.parent);
        }

        obj.SetActive(false);
        return obj;
    }

    public GameObject SpawnFromPool(string tag, Vector3 position = default, Quaternion rotation = default)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with tag {tag} doesn't exist.");
            return null;
        }

        Queue<GameObject> pool = poolDictionary[tag];
        GameObject obj = null;

        if (pool.Count > 0)
        {
            obj = pool.Dequeue();
            pool.Enqueue(obj);

            if (obj.activeInHierarchy)
            {
                obj = TryExpandPool(tag);
            }
        }
        else
        {
            obj = TryExpandPool(tag);
        }

        if (obj != null)
        {
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.SetActive(true);
        }

        return obj;
    }


    //풀이 가득 찼을 때 혹은 모두 활성화 상태 일때 새로 생성하여 풀 확장
    private GameObject TryExpandPool(string tag)
    {

        Pool poolSettings = pools.Find(p => p.tag == tag);
        if (poolSettings != null && poolSettings.canExpand)
        {
            GameObject obj = CreateNewPoolObject(prefabDictionary[tag]);
            poolDictionary[tag].Enqueue(obj);
            return obj;
        }

        return null;

    }


    // 범용적인 프리팹 등록 메서드
    public void RegisterPrefab(string tag, GameObject prefab, int poolSize, bool canExpand = true)
    {
        // 이미 등록된 풀이 있다면 스킵
        if (poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Pool with tag {tag} already exists.");
            return;
        }

        Pool newPool = new Pool
        {
            tag = tag,
            prefab = prefab,
            size = poolSize,
            canExpand = canExpand
        };

        pools.Add(newPool);
        CreatePool(newPool);
    }

    // 오브젝트 반환 메서드
    public void ReturnToPool(GameObject obj, string tag)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogError($"Pool with tag {tag} doesn't exist.");
            return;
        }

        obj.SetActive(false);
    }

    // 특정 풀의 모든 오브젝트 비활성화
    public void DeactivateAll(string tag)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogError($"Pool with tag {tag} doesn't exist.");
            return;
        }

        Queue<GameObject> pool = poolDictionary[tag];
        foreach (GameObject obj in pool)
        {
            obj.SetActive(false);
        }
    }
}
