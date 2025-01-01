using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHH
{
    public class CharacterStatsManager : MonoBehaviour
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

        public int soulsAwardedOnDeath = 50;
        public int soulCount = 0;

        [Header("Poise")]
        public float totalPoiseDefense; // The total poise after damage calculation
        public float offensivePoiseBonus;//poise gain during an attack with a weapon
        public float armorPoiseBonus; //poise gain from wearing
        public float totalPoiseResetTimer = 15f;
        public float poiseResetTimer = 0;

        [Header("Armor absorbtion")]
        public float physicalDamageAbsorbtionHead;
        public float physicalDamageAbsorbtionTorso;
        public float physicalDamageAbsorbtionLeg;
        public float physcialDamageAbsorbtionHand;


        public bool isDead;

        protected virtual void Update()
        {
            HandlePoiseResetTimer();
        }

        private void Start()
        {
            totalPoiseDefense = armorPoiseBonus;
        }

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

        public virtual void TakeDamageNoAnimation(int damage)
        {
            if (isDead) return;
            currentHealth = currentHealth - damage;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
            }
        }

        public virtual void HandlePoiseResetTimer()
        {
            if (poiseResetTimer > 0)
            {
                poiseResetTimer = poiseResetTimer - Time.deltaTime;
            }
            else
            {
                totalPoiseDefense = armorPoiseBonus;
            }
        }
    }
}
