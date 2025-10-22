using UnityEngine;

public class DamagePing : MonoBehaviour
{
    Health h;
    HitFlash[] flashes;

    void Awake()
    {
        h = GetComponent<Health>();
        flashes = GetComponentsInChildren<HitFlash>(true);
    }

    void OnEnable()
    {
        if (h != null) h.onHealthChanged.AddListener(OnHealthChanged);
    }

    void OnDisable()
    {
        if (h != null) h.onHealthChanged.RemoveListener(OnHealthChanged);
    }

    void OnHealthChanged(int current, int max)
    {
        foreach (var f in flashes) f.Ping();
        if (current <= 0) gameObject.SetActive(false); // temp death behavior
    }
}
