using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHH
{
    public class EnemyStats : CharacterStats
    {
        EnemyAnimatorManager enemyAnimatorManager;
        public UIEnemyHealthBar enemyHealthBar;
        public int soulsAwardedOnDeath = 50;
        private void Awake()
        {
            enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
        }
        void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
            enemyHealthBar.SetMaxHealth(maxHealth);
        }

        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public void TakeDamageNoAnimation(int damage)
        {
            if (isDead) return;
            currentHealth = currentHealth - damage;
            enemyHealthBar.SetCurrentHealth(currentHealth);
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
            }
        }

        public virtual void TakeDamage(int damage, string damageAnimation = "Damage_01")
        {
            if (isDead) return;
            currentHealth = currentHealth - damage;
            enemyHealthBar.SetCurrentHealth(currentHealth);

            enemyAnimatorManager.PlayTargetAnimation(damageAnimation, true);

            if (currentHealth <= 0)
            {
                HandleDeath();
            }
        }

        private void HandleDeath()
        {
            currentHealth = 0;
            enemyAnimatorManager.PlayTargetAnimation("Dead_01", true);
            isDead = true;
        }
    }
}
