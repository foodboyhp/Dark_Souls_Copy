using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PHH
{
    public class PoisonAmountBar : MonoBehaviour
    {
        public Slider slider;

        private void Start()
        {
            slider = GetComponent<Slider>();
            slider.value = 100;
            slider.maxValue = 100;
            gameObject.SetActive(false);
        }

        public virtual void SetPoisonAmount(int poisonAmount)
        {
            slider.value = poisonAmount;
        }

    }
}
