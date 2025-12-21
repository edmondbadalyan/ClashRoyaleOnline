using UnityEngine;

public class UnitParametres : MonoBehaviour
{
    public float startAttackDistance => modelRadius + _startAttackDistance;
    public float stopAttackDistance => modelRadius + _stopAttackDistance;
    [field: SerializeField] public float modelRadius { get; private set; } = 1f;
    [field: SerializeField] public bool isFlying { get; private set; } = false;
    [field: SerializeField] public float startChaseDistance { get; private set; } = 5f;
    [field: SerializeField] public float stoptChaseDistance { get; private set; } = 7f;
    [field: SerializeField] public float speed { get; private set; } = 3.5f;


    [SerializeField] public float _startAttackDistance = 1f;
    [SerializeField] public float _stopAttackDistance = 1.5f;
}
