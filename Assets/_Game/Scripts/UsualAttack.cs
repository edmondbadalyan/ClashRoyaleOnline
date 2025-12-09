using UnityEngine;

[CreateAssetMenu(fileName = "_UsualAttack", menuName = "UnitState/UsualAttack")]
public class UsualAttack : UnitState
{
   [SerializeField] private float _damage = 1.5f;
   [SerializeField] private float _attackDelay = 0.5f;
   [SerializeField] private float _stopAttackDistance = 0;
    private float _attackTimer = 0f;
    private bool _targetIsEnemy;
    private Health _targetHealth;   

    public override void Constructor(Unit unit)
    {
        base.Constructor(unit);
        _targetIsEnemy = _unit._isEnemy;
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
        if(_attackTimer < _attackDelay) return;
        _attackTimer -= _attackDelay;


        if(_targetHealth != null){
            float distance = Vector3.Distance(_unit.transform.position, _targetHealth.transform.position);
            if(distance > _stopAttackDistance)  _unit.SetState(UnitStateType.Chase);
            _targetHealth.ApplyDamage(_damage);
            
        }
        else
        {
            _unit.SetState(UnitStateType.Default);
            
        }
    }
    


    private bool TryFindTarget(out float stopAttackDistance)
    {
        stopAttackDistance = _stopAttackDistance;
        bool isFoundEnemy = MapInfo.Instance.TryGetNearestUnit(_unit.transform.position, _targetIsEnemy, out Unit enemyUnit, out float distance);
        
        
        if(isFoundEnemy && distance - enemyUnit.parametres.modelRadius <= _unit.parametres.startAttackDistance)
        {
            _targetHealth = enemyUnit.health;
            stopAttackDistance = _unit.parametres.stopAttackDistance - enemyUnit.parametres.modelRadius;
            return true;
        }
        else
        {
            Tower tower = MapInfo.Instance.GetNearestTower(_unit.transform.position, _targetIsEnemy);
            if(tower.GetDistance(_unit.transform.position) <= _unit.parametres.startAttackDistance)
            {
                _targetHealth = tower.health;
                stopAttackDistance = _unit.parametres.stopAttackDistance - tower.radius;
                return true;
            }
        }
        return false;
    }
}
