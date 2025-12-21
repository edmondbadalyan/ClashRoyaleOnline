using UnityEngine;
using UnityEngine.AI;
using static UnitState;

public abstract class UnitStateNavMeshMove : UnitState
{
    private NavMeshAgent _agent;
    protected Tower _nearestTower;
    protected bool _targetIsEnemy;

    public override void Constructor(Unit unit)
    {
        base.Constructor(unit);

        _targetIsEnemy = !_unit._isEnemy;
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
        _agent.SetDestination(_nearestTower.transform.position);
    }
    public override void Run()
    {
        if (TryFindTarget(out UnitStateType changeType))
        {
            _unit.SetState(changeType);
        }
        
    }

    protected abstract bool TryFindTarget(out UnitStateType changeType);

    public override void Finish()
    {
        _agent.ResetPath();
    }
}
