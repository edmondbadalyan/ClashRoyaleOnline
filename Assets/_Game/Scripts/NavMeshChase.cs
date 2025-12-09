using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "_NavMeshChase", menuName = "UnitState/NavMeshChase")]
public class NavMeshChase : UnitState
{
    private NavMeshAgent _agent;
    private bool _targetIsEnemy;
    private Unit _targetUnit;


    public override void Constructor(Unit unit)
    {
        base.Constructor(unit);

        _targetIsEnemy = _unit._isEnemy;
        _agent = _unit.GetComponent<NavMeshAgent>();
        if (_agent == null) Debug.LogWarning($"�� ��������� {unit.name}, ��� ���������� NavMeshAgent");
        

    }
    public override void Init()
    {
        MapInfo.Instance.TryGetNearestUnit(_unit.transform.position, _targetIsEnemy, out _targetUnit, out float distance);
    }
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
