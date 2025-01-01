using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHH
{
    public class DamageCollider : MonoBehaviour
    {
        CharacterManager characterManager;
        Collider damageCollider;
        public bool enabledDamageColliderOnStartUp = false;

        [Header("Poise")]
        public float poiseBreak;
        public float offensivePoiseBonus;

        [Header("Damage")]
        public int currentWeaponDamage;

        private void Awake()
        {
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            damageCollider.enabled = enabledDamageColliderOnStartUp;
        }

        public void EnableDamageCollider()
        {
            damageCollider.enabled = true;
        }

        public void DisableDamageCollider()
        {
            damageCollider.enabled = false;
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.tag == "Player")
            {
                PlayerStatsManager playerStats = collision.GetComponent<PlayerStatsManager>();
                CharacterManager enemyCharacterManager = collision.GetComponent<CharacterManager>();
                BlockingCollider shield = collision.GetComponentInChildren<BlockingCollider>();

                if (enemyCharacterManager != null)
                {
                    if (enemyCharacterManager.isParrying)
                    {
                        characterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Damage_01", true);
                        return;
                    }
                    else if (shield != null && enemyCharacterManager.isBlocking)
                    {
                        float physicalDamageAfterBlock =
                            currentWeaponDamage - (currentWeaponDamage * shield.blockingPhysicalDamageAbsorbtion) / 100;
                        if (playerStats != null)
                        {
                            playerStats.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock), "Block_Idle");
                            return;
                        }
                    }
                }
                if (playerStats != null)
                {
                    playerStats.poiseResetTimer = playerStats.totalPoiseResetTimer;
                    playerStats.totalPoiseDefense = playerStats.totalPoiseDefense - poiseBreak;
                    if (playerStats.totalPoiseDefense > poiseBreak)
                    {
                        playerStats.TakeDamageNoAnimation(currentWeaponDamage);
                    }
                    else
                    {
                        playerStats.TakeDamage(currentWeaponDamage);
                    }
                }
            }
            if (collision.tag == "Enemy")
            {
                EnemyStatsManager enemyStats = collision.GetComponent<EnemyStatsManager>();
                CharacterManager enemyCharacterManager = collision.GetComponent<CharacterManager>();
                BlockingCollider shield = collision.GetComponentInChildren<BlockingCollider>();

                if (enemyCharacterManager != null)
                {
                    if (enemyCharacterManager.isParrying)
                    {
                        characterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Damage_01", true);
                        return;
                    }
                    else if (shield != null && enemyCharacterManager.isBlocking)
                    {
                        float physicalDamageAfterBlock =
                            currentWeaponDamage - (currentWeaponDamage * shield.blockingPhysicalDamageAbsorbtion) / 100;
                        if (enemyStats != null)
                        {
                            enemyStats.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock), "Block_Idle");
                            return;
                        }
                    }
                }
                if (enemyStats != null)
                {
                    enemyStats.poiseResetTimer = enemyStats.totalPoiseResetTimer;
                    enemyStats.totalPoiseDefense = enemyStats.totalPoiseDefense - poiseBreak;

                    if (enemyStats.isBoss)
                    {
                        if (enemyStats.totalPoiseDefense > poiseBreak)
                        {
                            enemyStats.TakeDamageNoAnimation(currentWeaponDamage);
                        }
                        else
                        {
                            enemyStats.TakeDamageNoAnimation(currentWeaponDamage);
                            enemyStats.BreakGuard();
                        }
                    }
                    else
                    {
                        if (enemyStats.totalPoiseDefense > poiseBreak)
                        {
                            enemyStats.TakeDamageNoAnimation(currentWeaponDamage);
                        }
                        else
                        {
                            enemyStats.TakeDamage(currentWeaponDamage);
                        }
                    }
                }
            }
            if (collision.tag == "IllusionaryWall")
            {
                IllusionaryWall illusionaryWall = collision.GetComponent<IllusionaryWall>();
                illusionaryWall.wallHasBeenHit = true;
            }
        }
    }
}
