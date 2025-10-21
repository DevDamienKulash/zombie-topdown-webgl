using UnityEngine;
using UnityEngine.Events;


public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] int maxHP = 100;
    public int Current { get; private set; }
    public UnityEvent onDeath;
    public UnityEvent<int, int> onHealthChanged; // current, max


    void Awake() => Current = maxHP;


    public void TakeDamage(int amount)
    {
        if (Current <= 0) return;
        Current = Mathf.Max(0, Current - amount);
        onHealthChanged?.Invoke(Current, maxHP);
        if (Current == 0) onDeath?.Invoke();
    }
}