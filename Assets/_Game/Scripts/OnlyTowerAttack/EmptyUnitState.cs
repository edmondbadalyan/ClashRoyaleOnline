using UnityEngine;
[CreateAssetMenu(fileName = "_EmptyUnitState", menuName = "UnitState/EmptyUnitState")]
public class EmptyUnitState : UnitState
{
    public override void Finish()
    {
        
    }

    public override void Init()
    {
        _unit.SetState(UnitStateType.Default);
    }

    public override void Run()
    {
        
    }
}
