using UnityEngine;

public class Health : MonoBehaviour
{
    [field: SerializeField] public float maxHealth { get; private set; } = 10f;
    private float _currentHealth;

    private void Start(){
        _currentHealth = maxHealth;
    }
    public void ApplyDamage(float damage){
        _currentHealth -= damage;
        if (_currentHealth <= 0){
           _currentHealth = 0;
        }
        Debug.Log($"Health: {_currentHealth} - {damage}" );
    }
}

public interface IHealth
{
    Health health { get; }
}