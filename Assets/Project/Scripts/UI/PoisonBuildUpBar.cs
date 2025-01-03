using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PHH
{
    public class PoisonBuildUpBar : MonoBehaviour
    {
        public Slider slider;

        private void Start()
        {
            slider = GetComponent<Slider>();
            slider.value = 0;
            slider.maxValue = 100;
            gameObject.SetActive(false);
        }

        public virtual void SetCurrentPoisonUpBar(int currentPoisonBuildUp)
        {
            slider.value = currentPoisonBuildUp;
        }

    }
}
