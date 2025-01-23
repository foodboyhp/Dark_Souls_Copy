using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHH
{
    public class CharacterStatsManager : MonoBehaviour
    {
        CharacterManager characterManager;

        [Header("Team I.D")]
        public int teamIDNumber = 0;

        public int maxHealth;
        public int currentHealth;
        public float maxStamina;
        public float currentStamina;
        public float maxFocusPoint;
        public float currentFocusPoint;

        public int currentSoulCount = 0;
        public int soulsAwardedOnDeath = 50;
        [Header("Character Level")]
        public int playerLevel = 1;
        [Header("Levels")]
        public int healthLevel = 10;
        public int staminaLevel = 10;
        public int focusLevel = 10;
        public int poiseLevel = 10;
        public int strengthLevel = 10;
        public int dexterityLevel = 10;
        public int intelligenceLevel = 10;
        public int faithLevel = 10;

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
        public float physicalDamageAbsorbtionHand;

        public float fireDamageAbsorbtionHead;
        public float fireDamageAbsorbtionTorso;
        public float fireDamageAbsorbtionLeg;
        public float fireDamageAbsorbtionHand;


        protected virtual void Awake()
        {
            characterManager = GetComponent<CharacterManager>();
        }

        protected virtual void Update()
        {
            HandlePoiseResetTimer();
        }

        private void Start()
        {
            totalPoiseDefense = armorPoiseBonus;
        }

        public virtual void TakeDamage(int physicalDamage, int fireDamage, string damageAnimation)
        {
            if (characterManager.isDead) return;
            float totalPhysicalDamageAbsorbtion = 1 -
                (1 - physicalDamageAbsorbtionHand / 100) *
                (1 - physicalDamageAbsorbtionHead / 100) *
                (1 - physicalDamageAbsorbtionLeg / 100) *
                (1 - physicalDamageAbsorbtionTorso / 100);
            physicalDamage = Mathf.RoundToInt((float)physicalDamage * (1.0f - totalPhysicalDamageAbsorbtion));

            float totalFireDamageAbsorbtion = 1 -
                (1 - fireDamageAbsorbtionHand / 100) *
                (1 - fireDamageAbsorbtionHead / 100) *
                (1 - fireDamageAbsorbtionLeg / 100) *
                (1 - fireDamageAbsorbtionTorso / 100);
            fireDamage = Mathf.RoundToInt((float)fireDamage * (1.0f - totalFireDamageAbsorbtion));

            float finalDamage = physicalDamage + fireDamage;
            currentHealth = Mathf.RoundToInt(currentHealth - finalDamage);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                characterManager.isDead = true;

            }
        }

        public virtual void TakeDamageNoAnimation(int physicalDamage, int fireDamage)
        {
            if (characterManager.isDead) return;

            characterManager.characterAnimatorManager.EraseHandIKForWeapon();
            float totalPhysicalDamageAbsorbtion = 1 -
                (1 - physicalDamageAbsorbtionHand / 100) *
                (1 - physicalDamageAbsorbtionHead / 100) *
                (1 - physicalDamageAbsorbtionLeg / 100) *
                (1 - physicalDamageAbsorbtionTorso / 100);
            physicalDamage = Mathf.RoundToInt((float)physicalDamage * (1.0f - totalPhysicalDamageAbsorbtion));

            float totalFireDamageAbsorbtion = 1 -
                (1 - fireDamageAbsorbtionHand / 100) *
                (1 - fireDamageAbsorbtionHead / 100) *
                (1 - fireDamageAbsorbtionLeg / 100) *
                (1 - fireDamageAbsorbtionTorso / 100);
            fireDamage = Mathf.RoundToInt((float)fireDamage * (1.0f - totalFireDamageAbsorbtion));

            float finalDamage = physicalDamage + fireDamage;
            currentHealth = Mathf.RoundToInt(currentHealth - finalDamage);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                characterManager.isDead = true;
            }
        }

        public virtual void TakePoisonDamage(int damage)
        {
            if (characterManager.isDead) return;
            currentHealth = currentHealth - damage;

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                characterManager.isDead = true;
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

        public virtual void DrainStaminaBasedOnAttackType()
        {

        }


        public int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }
        public float SetMaxStaminaFromStaminaLevel()
        {
            maxStamina = staminaLevel * 10;
            return maxStamina;
        }
        public float SetMaxFocusPointFromFocusPointLevel()
        {
            maxFocusPoint = focusLevel * 10;
            return maxFocusPoint;
        }
    }
}
