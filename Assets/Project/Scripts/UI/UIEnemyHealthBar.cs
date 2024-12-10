using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PHH
{
    public class UIEnemyHealthBar : HealthBar
    {
        float timeUntilBarIsHidden = 0;

        private void Awake()
        {
            slider = GetComponentInChildren<Slider>();
        }

        public override void SetCurrentHealth(int currentHealth)
        {
            base.SetCurrentHealth(currentHealth);
            timeUntilBarIsHidden = 3f;
        }

        private void Update()
        {
            timeUntilBarIsHidden = timeUntilBarIsHidden - Time.deltaTime;
            if(slider != null)
            {
                if(timeUntilBarIsHidden <= 0)
                {
                    timeUntilBarIsHidden = 0;
                    slider.gameObject.SetActive(false);
                }
                else
                {
                    if(!slider.gameObject.activeInHierarchy)
                    {
                        slider.gameObject.SetActive(true);  
                    }
                }

                if(slider.value <= 0)
                {
                    Destroy(slider.gameObject);
                }
            }
        }
    }
}

