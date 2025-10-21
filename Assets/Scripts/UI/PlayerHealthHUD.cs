using UnityEngine;
using TMPro;

public class PlayerHealthHUD_TMP : MonoBehaviour
{
    [SerializeField] TMP_Text hpText;
    [SerializeField] Health playerHealth;

    void Awake()
    {
        if (!playerHealth)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p) playerHealth = p.GetComponent<Health>();
        }
    }

    void OnEnable()
    {
        if (playerHealth)
        {
            playerHealth.onHealthChanged.AddListener(UpdateHP);
            UpdateHP(playerHealth.Current, 100); // replace 100 with your max if needed
        }
    }

    void OnDisable()
    {
        if (playerHealth) playerHealth.onHealthChanged.RemoveListener(UpdateHP);
    }

    void UpdateHP(int current, int max)
    {
        if (hpText) hpText.text = $"HP: {current}";
    }
}
