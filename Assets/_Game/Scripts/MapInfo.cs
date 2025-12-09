using System.Collections.Generic;
using UnityEngine;

public class MapInfo : MonoBehaviour
{
    public static MapInfo Instance { get; private set; }


    private void Awake()
    {
        if (Instance) { Destroy(gameObject); return; }

        Instance = this;
    }
    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }


    [SerializeField] private List<Tower> _playerTowers = new();
    [SerializeField] private List<Tower> _enemyTowers = new(); 

    [SerializeField] private List<Unit> _playerUnits = new();
    [SerializeField] private List<Unit> _enemyUnits = new(); 

    public bool TryGetNearestUnit(in Vector3 currentPosition, bool enemy, out Unit unit, out float distance)
    {
        List<Unit> units = enemy ? _enemyUnits : _playerUnits;  
        unit = GetNearest(currentPosition, units, out distance);
        return unit;
    }
    public Tower GetNearestTower(in Vector3 currentPosition, bool enemy)
    {
        List<Tower> towers = enemy ? _enemyTowers : _playerTowers;


        
        return GetNearest(currentPosition, towers, out float distance);
    }

    public T GetNearest<T>( in Vector3 currentPosition, List<T> objects,out float distance) where T : MonoBehaviour
    {
        distance = float.MaxValue;
        if (objects.Count == 0) return null;
        distance = Vector3.Distance(currentPosition, objects[0].transform.position);
        T nearestObject = objects[0];
        for (int i = 1; i < objects.Count; i++) {
            float tempDistance = Vector3.Distance(currentPosition, objects[i].transform.position);
            if (tempDistance > distance) continue;
            nearestObject = objects[i];
            distance = tempDistance;
        }

        return nearestObject;
    }
}
