using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;


[CreateAssetMenu(fileName = "_NavMeshRangeMove", menuName = "UnitState/NavMeshRangeMove")]
public class NavMeshRangeMove : UnitStateNavMeshMove
{

    private bool TryAttackTower()
    {
        float distanceToTarget = _nearestTower.GetDistance(_unit.transform.position);
        if (distanceToTarget <= _unit.parametres.startAttackDistance)
        {

            
            return true;
        }
        return false;
    }

    protected override bool TryFindTarget(out UnitStateType changeType)
    {
        if (TryAttackTower()) { changeType = UnitStateType.Attack; return true; } 
        if (TryChaseUnit()) { changeType = UnitStateType.Chase; return true; }

        changeType = UnitStateType.None;
        return false;
    }

    private bool TryChaseUnit()
    {
        if (!MapInfo.Instance.TryGetNearestUnit(_unit.transform.position, _targetIsEnemy, out Unit enemy, out float distance))
            return false;

        if (_unit.parametres.startChaseDistance >= distance)
        {
            
            return true;
        }

        return false;
    }


}
