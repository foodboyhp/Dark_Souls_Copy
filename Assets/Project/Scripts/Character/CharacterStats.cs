using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHH
{
    public class CharacterStats : MonoBehaviour
    {
        public int healthLevel = 10;
        public int maxHealth;
        public int currentHealth;

        public int staminaLevel = 10;
        public float maxStamina;
        public float currentStamina;

        public int focusPointLevel = 10;
        public float maxFocusPoint;
        public float currentFocusPoint;

        public int soulCount = 0;

        [Header("Armor absorbtion")]
        public float physicalDamageAbsorbtionHead;
        public float physicalDamageAbsorbtionTorso;
        public float physicalDamageAbsorbtionLeg;
        public float physcialDamageAbsorbtionHand;


        public bool isDead;

        public virtual void TakeDamage(int physicalDamage, string damageAnimation = "Damage_01")
        {
            if (isDead) return;
            float totalPhysicalDamageAbsorbtion = 1 -
                (1 - physcialDamageAbsorbtionHand / 100) *
                (1 - physicalDamageAbsorbtionHead / 100) *
                (1 - physicalDamageAbsorbtionLeg / 100) *
                (1 - physicalDamageAbsorbtionTorso / 100);
            physicalDamage = Mathf.RoundToInt((float)physicalDamage * (1.0f - totalPhysicalDamageAbsorbtion));
            float finalDamage = physicalDamage;
            currentHealth = Mathf.RoundToInt(currentHealth - finalDamage);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;

            }
        }
    }
}
