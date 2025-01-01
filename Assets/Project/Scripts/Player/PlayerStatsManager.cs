using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHH
{
    public class PlayerStatsManager : CharacterStatsManager
    {
        HealthBar healthBar;
        StaminaBar staminaBar;
        FocusPointBar focusPointBar;

        PlayerAnimatorManager playerAnimatorManager;
        PlayerManager playerManager;

        public float staminaRegenerateAmount = 10f;
        public float staminaRegenTimer = 0f;

        private void Awake()
        {
            healthBar = FindObjectOfType<HealthBar>();
            staminaBar = FindObjectOfType<StaminaBar>();
            focusPointBar = FindObjectOfType<FocusPointBar>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerManager = GetComponent<PlayerManager>();
        }

        void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);
            healthBar.SetCurrentHealth(currentHealth);

            maxStamina = SetMaxStaminaFromStaminaLevel();
            currentStamina = maxStamina;
            staminaBar.SetMaxStamina(maxStamina);
            staminaBar.SetCurrentStamina(currentStamina);

            maxFocusPoint = SetMaxFocusPointFromFocusPointLevel();
            currentFocusPoint = maxFocusPoint;
            focusPointBar.SetMaxFocusPoint(maxFocusPoint);
            focusPointBar.SetCurrentFocusPoint(currentFocusPoint);
        }

        public override void HandlePoiseResetTimer()
        {
            if (poiseResetTimer > 0)
            {
                poiseResetTimer = poiseResetTimer - Time.deltaTime;
            }
            else if (poiseResetTimer <= 0 && !playerManager.isInteracting)
            {
                totalPoiseDefense = armorPoiseBonus;
            }
        }
        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }
        private float SetMaxStaminaFromStaminaLevel()
        {
            maxStamina = staminaLevel * 10;
            return maxStamina;
        }
        private float SetMaxFocusPointFromFocusPointLevel()
        {
            maxFocusPoint = focusPointLevel * 10;
            return maxFocusPoint;
        }

        public override void TakeDamageNoAnimation(int damage)
        {
            base.TakeDamageNoAnimation(damage);
            healthBar.SetCurrentHealth(currentHealth);

        }

        public override void TakeDamage(int damage, string damageAnimation = "Damage_01")
        {
            if (playerManager.isInvulnerable)
            {
                return;
            }
            base.TakeDamage(damage, damageAnimation);
            currentHealth = currentHealth - damage;
            healthBar.SetCurrentHealth(currentHealth);
            playerAnimatorManager.PlayTargetAnimation(damageAnimation, true);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                playerAnimatorManager.PlayTargetAnimation("Dead_01", true);
                isDead = true;
            }
        }

        public void TakeStaminaDamage(int damage)
        {
            currentStamina -= damage;
            staminaBar.SetCurrentStamina(currentStamina);
        }

        public void RegenerateStamina()
        {
            if (playerManager.isInteracting)
            {
                staminaRegenTimer = 0;
            }
            else
            {
                staminaRegenTimer += Time.deltaTime;
                if (currentStamina < maxStamina && staminaRegenTimer > 1f)
                {
                    currentStamina += staminaRegenerateAmount * Time.deltaTime;
                    staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
                }
            }
        }

        public void HealPlayer(int amount)
        {
            currentHealth = currentHealth + amount;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
            healthBar.SetCurrentHealth(currentHealth);
        }

        public void DeductFocusPoint(int focusPoint)
        {
            currentFocusPoint -= focusPoint;
            if (currentFocusPoint < 0)
            {
                currentFocusPoint = 0;
            }
            focusPointBar.SetCurrentFocusPoint(currentFocusPoint);
        }

        public void AddSouls(int souls)
        {
            soulCount += souls;
        }
    }
}
