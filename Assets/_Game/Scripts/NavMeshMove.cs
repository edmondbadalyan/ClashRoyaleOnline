using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;


[CreateAssetMenu(fileName = "_NavMeshMove",menuName = "UnitState/NavMeshMove")]
public class NavMeshMove : UnitState
{
    
    private NavMeshAgent _agent;
    private Tower _nearestTower;
    private bool _targetIsEnemy;

    public override void Constructor(Unit unit)
    {
        base.Constructor(unit);

        _targetIsEnemy = _unit._isEnemy;
        _agent = _unit.GetComponent<NavMeshAgent>();
        if (_agent == null) Debug.LogWarning($"�� ��������� {unit.name}, ��� ���������� NavMeshAgent");
        _agent.speed = _unit.parametres.speed;
        _agent.radius = _unit.parametres.modelRadius;
        _agent.stoppingDistance = _unit.parametres.stopAttackDistance;

    }
    public override void Init()
    {
        Vector3 unitPosition = _unit.transform.position;

        _nearestTower = MapInfo.Instance.GetNearestTower(in unitPosition, _targetIsEnemy);
        


        _agent = _unit.GetComponent<NavMeshAgent>();
        _agent.SetDestination(_nearestTower.transform.position);

    }

    private bool TryAttackTower()
    {
        float distanceToTarget = _nearestTower.GetDistance(_unit.transform.position);
        if (distanceToTarget <= _unit.parametres.startAttackDistance)
        {
            
            _unit.SetState(UnitStateType.Attack);
            return true;
        }
        return false;
    }

    public override void Run()
    {
        if (TryAttackTower()) return;
        if (TryAttackUnit()) return;
        
    }

    private bool TryAttackUnit()
    {
        if (!MapInfo.Instance.TryGetNearestUnit(_unit.transform.position, _targetIsEnemy, out Unit enemy, out float distance))
            return false;

        if (_unit.parametres.startChaseDistance >= distance)
        {
            _unit.SetState(UnitStateType.Chase);
            return true;
        }
        
        return false;
    }

    public override void Finish()
    {
        _agent.ResetPath();
    }
    
    
}
