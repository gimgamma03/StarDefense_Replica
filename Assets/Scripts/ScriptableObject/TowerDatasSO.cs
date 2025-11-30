using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerDatas", menuName = "TowerDefense/TowerDatas")]
public class TowerDatasSO : ScriptableObject
{
    public List<TowerGradeData> towerGrades;
}

[System.Serializable]
public class TowerGradeData
{
    public int grade;                  // 1등급, 2등급...
    public List<TowerData> towers;     // 해당 등급의 타워 리스트
}

[System.Serializable]
public class TowerData
{
    public int grade;
    public string towerName;
    public float damage;
    public float range;
    public float attackDelay;
    public Sprite towerImage;
}
