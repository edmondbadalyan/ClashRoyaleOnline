using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "_NavMeshRangeChase", menuName = "UnitState/NavMeshRangeChase")]
public class NavMeshRangeChase : UnitStateNavMeshChase
{
    protected override void FindTarget(out Unit targetUnit)
    {
        MapInfo.Instance.TryGetNearestUnit(_unit.transform.position, _targetIsEnemy, out targetUnit, out float distance);
    }
}
