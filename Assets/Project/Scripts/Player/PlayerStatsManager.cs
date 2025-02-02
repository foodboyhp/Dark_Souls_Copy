using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHH
{
    public class PlayerStatsManager : CharacterStatsManager
    {
        public HealthBar healthBar;
        StaminaBar staminaBar;
        FocusPointBar focusPointBar;

        PlayerAnimatorManager playerAnimatorManager;
        PlayerManager player;

        public float staminaRegenerateAmount = 10f;
        public float staminaRegenTimer = 0f;

        protected override void Awake()
        {
            base.Awake();
            staminaBar = FindObjectOfType<StaminaBar>();
            focusPointBar = FindObjectOfType<FocusPointBar>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            player = GetComponent<PlayerManager>();
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
            else if (poiseResetTimer <= 0 && !player.isInteracting)
            {
                totalPoiseDefense = armorPoiseBonus;
            }
        }

        public override void TakeDamageNoAnimation(int physicalDamage, int fireDamage)
        {
            base.TakeDamageNoAnimation(physicalDamage, fireDamage);
            healthBar.SetCurrentHealth(currentHealth);

        }

        public override void TakePoisonDamage(int damage)
        {
            if (player.isDead) return;
            base.TakePoisonDamage(damage);
            healthBar.SetCurrentHealth(currentHealth);
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                playerAnimatorManager.PlayTargetAnimation("Dead_01", true);
                player.isDead = true;
            }
        }

        public override void TakeDamage(int physicalDamage, int fireDamage, string damageAnimation)
        {
            if (player.isInvulnerable)
            {
                return;
            }
            base.TakeDamage(physicalDamage, fireDamage, damageAnimation);
            healthBar.SetCurrentHealth(currentHealth);
            playerAnimatorManager.PlayTargetAnimation(damageAnimation, true);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                playerAnimatorManager.PlayTargetAnimation("Dead_01", true);
                player.isDead = true;
            }
        }

        public void TakeStaminaDamage(int damage)
        {
            currentStamina -= damage;
            staminaBar.SetCurrentStamina(currentStamina);
        }

        public void RegenerateStamina()
        {
            if (player.isInteracting)
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
            currentSoulCount += souls;
        }
    }
}
