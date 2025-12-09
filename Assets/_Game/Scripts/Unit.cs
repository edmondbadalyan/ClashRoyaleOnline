using UnityEngine;
using static UnitState;

[RequireComponent(typeof(UnitParametres)),RequireComponent(typeof(Health))]
public class Unit : MonoBehaviour, IHealth
{
    [field: SerializeField] public Health health { get; private set; }
    [field: SerializeField] public bool _isEnemy { get; private set; } = false;
    [field: SerializeField] public UnitParametres parametres;
    [SerializeField] private UnitState _defaultStateSO;
    [SerializeField] private UnitState _chasetStateSO;
    [SerializeField] private UnitState _attacktStateSO;

    private UnitState _defaultState;
    private UnitState _chasetState;
    private UnitState _attacktState;


    private UnitState _currentState;


    private void Start()
    {
        _defaultState = Instantiate(_defaultStateSO);
        _defaultState.Constructor(this);
        _chasetState = Instantiate(_chasetStateSO);
        _chasetState.Constructor(this);
        _attacktState = Instantiate(_attacktStateSO);
        _attacktState.Constructor(this);

        _currentState = _defaultState;
        _currentState.Init();
        
    }
    private void Update()
    {
        _currentState.Run();
    }
    public void SetState(UnitStateType type)
    {
        _currentState.Finish();
        switch (type)
        {
            case UnitStateType.Default:
                _currentState = _defaultState;
                break;
            case UnitStateType.Chase:
                _currentState = _chasetState;
                break;
            case UnitStateType.Attack:
                _currentState = _attacktState;
                break;
            default:
                Debug.LogError("�� �������������� ���������" + type);
                break;
        }
        _currentState.Init();
    }


    #if UNITY_EDITOR
    [Space(20)]
    [SerializeField] private bool _drawGizmos = false;
    private void OnDrawGizmos()
    {
        if (!_drawGizmos) return;
        if (_chasetStateSO != null){ _chasetStateSO.DebugDrawDistance(this); }
    }
    #endif
}
