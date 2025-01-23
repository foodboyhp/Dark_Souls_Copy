using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHH
{
    public class EnemyStatsManager : CharacterStatsManager
    {
        EnemyManager enemy;
        public UIEnemyHealthBar enemyHealthBar;

        public bool isBoss = false;
        protected override void Awake()
        {
            base.Awake();
            enemy = GetComponent<EnemyManager>();
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
        }
        void Start()
        {
            if (!isBoss)
            {
                enemyHealthBar.SetMaxHealth(maxHealth);
            }
        }

        public override void HandlePoiseResetTimer()
        {
            if (poiseResetTimer > 0)
            {
                poiseResetTimer = poiseResetTimer - Time.deltaTime;
            }
            else if (poiseResetTimer <= 0 && !enemy.isInteracting)
            {
                totalPoiseDefense = armorPoiseBonus;
            }
        }
        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public override void TakeDamageNoAnimation(int physicalDamage, int fireDamage)
        {
            base.TakeDamageNoAnimation(physicalDamage, fireDamage);

            if (!isBoss)
            {
                enemyHealthBar.SetCurrentHealth(currentHealth);
            }
            else if (isBoss && enemy.enemyBossManager != null)
            {
                enemy.enemyBossManager.UpdateBossHealthBar(currentHealth, maxHealth);
            }
        }

        public override void TakePoisonDamage(int damage)
        {
            if (enemy.isDead) return;
            base.TakePoisonDamage(damage);
            if (!isBoss)
            {
                enemyHealthBar.SetCurrentHealth(currentHealth);
            }
            else if (isBoss && enemy.enemyBossManager != null)
            {
                enemy.enemyBossManager.UpdateBossHealthBar(currentHealth, maxHealth);
            }
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                enemy.enemyAnimatorManager.PlayTargetAnimation("Dead_01", true);
                enemy.isDead = true;
            }
        }

        public void BreakGuard()
        {
            enemy.enemyAnimatorManager.PlayTargetAnimation("Break_Guard", true);
        }

        public override void TakeDamage(int physicalDamage, int fireDamage, string damageAnimation)
        {
            base.TakeDamage(physicalDamage, fireDamage, damageAnimation);
            if (!isBoss)
            {
                enemyHealthBar.SetCurrentHealth(currentHealth);
            }
            else if (isBoss && enemy.enemyBossManager != null)
            {
                enemy.enemyBossManager.UpdateBossHealthBar(currentHealth, maxHealth);
            }

            enemy.enemyAnimatorManager.PlayTargetAnimation(damageAnimation, true);

            if (currentHealth <= 0)
            {
                HandleDeath();
            }
        }

        private void HandleDeath()
        {
            currentHealth = 0;
            enemy.enemyAnimatorManager.PlayTargetAnimation("Dead_01", true);
            enemy.isDead = true;
        }
    }
}
