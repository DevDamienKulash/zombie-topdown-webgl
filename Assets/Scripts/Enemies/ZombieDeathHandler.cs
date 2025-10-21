using UnityEngine;


public class ZombieDeathHandler : MonoBehaviour
{
Health h;


void Awake()
{
h = GetComponent<Health>();
}


void OnEnable()
{
if (h) h.onDeath.AddListener(OnDeath);
}


void OnDisable()
{
if (h) h.onDeath.RemoveListener(OnDeath);
}


void OnDeath()
{
    Debug.Log($"[ZombieDeathHandler] {name} died → +1 score");
    ScoreManager.Add(1);
    gameObject.SetActive(false);
}

}