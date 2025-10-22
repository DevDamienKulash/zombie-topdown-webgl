using UnityEngine;

// Add this to player GameObject
public class PlayerDeathRelay : MonoBehaviour
{
Health h;
void Awake() { h = GetComponent<Health>(); }
void OnEnable() { if (h) h.onDeath.AddListener(Notify); }
void OnDisable() { if (h) h.onDeath.RemoveListener(Notify); }
void Notify() { GameManager.Instance?.OnPlayerDied(); }
}
