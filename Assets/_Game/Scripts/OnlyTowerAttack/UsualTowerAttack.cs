using UnityEngine;
using static UnitState;

[CreateAssetMenu(fileName = "_UsualTowerAttack", menuName = "UnitState/UsualTowerAttack")]
public class UsualTowerAttack : UnitStateAttack
{
    protected override bool TryFindTarget(out float stopAttackDistance)
    {
       // stopAttackDistance = _stopAttackDistance;
        
            Tower tower = MapInfo.Instance.GetNearestTower(_unit.transform.position, _targetIsEnemy);
            if (tower.GetDistance(_unit.transform.position) <= _unit.parametres.startAttackDistance)
            {
                _targetHealth = tower.health;
                stopAttackDistance = _unit.parametres.stopAttackDistance - tower.radius;
                return true;
            }

        stopAttackDistance = 0;
        return false;
    }
}
