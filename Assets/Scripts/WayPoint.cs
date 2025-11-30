using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    private Transform wayPoint;
    public List<Transform> wayPointList = new List<Transform>();

    private void Awake()
    {
        wayPoint = transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform child in transform)
        {
            wayPointList.Add(child);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<Transform> GetWayPointList()
    {
        return wayPointList;
    }
}
