using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PHH
{
    public class StaminaBar : MonoBehaviour
    {
        public Slider slider;

        public void SetCurrentStamina(float currentStamina)
        {
            slider.value = currentStamina;
        }

        public void SetMaxStamina(float maxStamina)
        {
            slider.maxValue = maxStamina;
            slider.value = maxStamina;
        }
    }
}
