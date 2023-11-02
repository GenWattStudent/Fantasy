using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;

    public void UpdateHealthBar(float currentHealth, float maxHealth) {
        slider.value = currentHealth / maxHealth;
    }

    void Update()
    {
        
    }
}
