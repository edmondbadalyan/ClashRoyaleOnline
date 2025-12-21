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
    private void Start()
    {
        SubscribeOnDestroy(_playerWalkingUnits);
        SubscribeOnDestroy(_enemyWalkingUnits);
        SubscribeOnDestroy(_playerTowers);
        SubscribeOnDestroy(_enemyTowers);
    }
    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
        
    }


     public void AddUnit(Unit unit)
     {
        List<Unit> units;
        if (unit._isEnemy) units = unit.parametres.isFlying ? _enemyFlyingUnits : _enemyWalkingUnits;
        else units = unit.parametres.isFlying ? _playerFlyingUnits : _playerWalkingUnits;
        AddObjectToList(units, unit);
     }





    

    [SerializeField] private List<Tower> _playerTowers = new();
    [SerializeField] private List<Tower> _enemyTowers = new(); 

    [SerializeField] private List<Unit> _playerWalkingUnits = new();
    [SerializeField] private List<Unit> _enemyWalkingUnits = new(); 

    [SerializeField] private List<Unit> _playerFlyingUnits = new();
    [SerializeField] private List<Unit> _enemyFlyingUnits = new();

    public bool TryGetNearestUnit(in Vector3 currentPosition, bool enemy, out Unit unit, out float distance)
    {
        TryGetNearestFlyingUnit(currentPosition, enemy, out Unit flyingUnit, out float flyingDistance);
        TryGetNearestWalkingUnit(currentPosition, enemy, out Unit walkingUnit, out float walkingDistance);
        
        if (flyingDistance < walkingDistance) {
            unit = flyingUnit;
            distance = flyingDistance;
            return true;
        }
        unit = walkingUnit;
        distance = walkingDistance;
        return true;
    }

    public bool TryGetNearestFlyingUnit(in Vector3 currentPosition, bool enemy, out Unit unit, out float distance)
    {
        List<Unit> units = enemy ? _enemyFlyingUnits : _playerFlyingUnits;  
        unit = GetNearest(currentPosition, units, out distance);
        return unit;
    }
    public bool TryGetNearestWalkingUnit(in Vector3 currentPosition, bool enemy, out Unit unit, out float distance)
    {
        List<Unit> units = enemy ? _enemyWalkingUnits : _playerWalkingUnits;  
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

    private void SubscribeOnDestroy<T>(List<T> objects) where T : Idestroy
    {
        for (int i = 0; i < objects.Count; i++)
        {
            T obj = objects[i];
            obj.onDestroy += RemoveandUnsubscribe;

            void RemoveandUnsubscribe(){
                RemoveObjectFromList(objects, obj);
                obj.onDestroy -= RemoveandUnsubscribe;
            };
        }
    }
    private void RemoveObjectFromList<T>(List<T> objects, T obj) where T : Idestroy
    {
        if (objects.Contains(obj))
        {
            objects.Remove(obj);
            
        }
    }
    private void AddObjectToList<T>(List<T> objects, T obj) where T : Idestroy
    {
        objects.Add(obj);
        obj.onDestroy += RemoveandUnsubscribe;
        void RemoveandUnsubscribe(){
            RemoveObjectFromList(objects, obj);
            obj.onDestroy -= RemoveandUnsubscribe;
        };
    }
}
