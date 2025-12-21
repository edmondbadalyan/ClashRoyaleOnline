using UnityEngine;
using static UnitState;

public abstract class UnitStateAttack : UnitState
{
    [SerializeField] protected float _damage = 1.5f;
    [SerializeField] protected float _attackDelay = 0.5f;
    [SerializeField] protected float _stopAttackDistance = 0;
    protected float _attackTimer = 0f;
    protected bool _targetIsEnemy;
    protected Health _targetHealth;

    public override void Constructor(Unit unit)
    {
        base.Constructor(unit);
        _targetIsEnemy = !_unit._isEnemy;
    }
    public override void Finish()
    {

    }

    public override void Init()
    {
        if (TryFindTarget(out _stopAttackDistance) == false)
        {
            _unit.SetState(UnitStateType.Default);
            return;
        }


        _unit.transform.LookAt(_targetHealth.transform.position);
        _attackTimer = 0f;


    }
    public override void Run()
    {
        _attackTimer += Time.deltaTime;
        if (_attackTimer < _attackDelay) return;
        _attackTimer -= _attackDelay;


        if (_targetHealth != null)
        {
            float distance = Vector3.Distance(_unit.transform.position, _targetHealth.transform.position);
            if (distance > _stopAttackDistance) _unit.SetState(UnitStateType.Chase);
            _targetHealth.ApplyDamage(_damage);

        }
        else
        {
            _unit.SetState(UnitStateType.Default);

        }
    }



    protected abstract bool TryFindTarget(out float stopAttackDistance);
}
