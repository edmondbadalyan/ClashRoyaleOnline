using UnityEngine;

[CreateAssetMenu(fileName = "_UsualRangeAttack", menuName = "UnitState/UsualRangeAttack")]
public class UsualRangeAttack : UnitStateAttack
{
   
    protected override bool TryFindTarget(out float stopAttackDistance)
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
