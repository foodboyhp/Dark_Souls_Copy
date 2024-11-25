using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PHH
{
    public class StaminaBar : MonoBehaviour
    {
        public Slider slider;

        public void SetCurrentStamina(int currentStamina)
        {
            slider.value = currentStamina;
        }

        public void SetMaxStamina(int maxStamina)
        {
            slider.maxValue = maxStamina;
            slider.value = maxStamina;
        }
    }
}
