using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PHH
{
    public class HealthBar : MonoBehaviour
    {
        public Slider slider;

        public void SetCurrentHealth(int currentHealth)
        {
            slider.value = currentHealth;
        }

        public void SetMaxHealth(int maxHealth)
        {
            slider.maxValue = maxHealth;
            slider.value = maxHealth;
        }
    }
}
