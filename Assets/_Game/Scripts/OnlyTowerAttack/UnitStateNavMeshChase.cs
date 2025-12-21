using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using static UnitState;

public abstract class UnitStateNavMeshChase : UnitState
{
    private NavMeshAgent _agent;
    protected bool _targetIsEnemy;
    protected Unit _targetUnit;
    protected float _startAttackDistance = 0;

    public override void Constructor(Unit unit)
    {
        base.Constructor(unit);

        _targetIsEnemy = !_unit._isEnemy;
        _agent = _unit.GetComponent<NavMeshAgent>();
        if (_agent == null) Debug.LogWarning($"�� ��������� {unit.name}, ��� ���������� NavMeshAgent");
    }
    public override void Init()
    {
        FindTarget(out _targetUnit);
        if (_targetUnit == null)
        {
            _unit.SetState(UnitStateType.Default);
           
        }
    }

    protected abstract void FindTarget(out Unit targetUnit);
    public override void Run()
    {
        if (_targetUnit == null)
        {
            _unit.SetState(UnitStateType.Default);
            return;
        }

        float distance = Vector3.Distance(_unit.transform.position, _targetUnit.transform.position);

        // Improved state transitions: use named variables, clarify intention, avoid duplication.
        float stopChaseThreshold = _unit.parametres.stoptChaseDistance + _targetUnit.parametres.modelRadius;
        float attackThreshold = _unit.parametres.startAttackDistance + _targetUnit.parametres.modelRadius;

        if (distance > stopChaseThreshold)
        {
            // Target is too far; revert to default state (e.g., idle/patrol)
            _unit.SetState(UnitStateType.Default);
            return;
        }

        if (distance <= attackThreshold)
        {
            // Close enough to attack
            _unit.SetState(UnitStateType.Attack);
            return;
        }

        _agent.SetDestination(_targetUnit.transform.position);


    }

    public override void Finish()
    {
        _agent.ResetPath();
    }

#if UNITY_EDITOR
    public override void DebugDrawDistance(Unit unit)
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(unit.transform.position, Vector3.up, unit.parametres.startChaseDistance);
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(unit.transform.position, Vector3.up, unit.parametres.stoptChaseDistance);
    }
#endif
}
