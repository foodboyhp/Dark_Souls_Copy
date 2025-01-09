using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHH
{
    public class DamageCollider : MonoBehaviour
    {
        public CharacterManager characterManager;
        protected Collider damageCollider;
        public bool enabledDamageColliderOnStartUp = false;

        [Header("Team I.D")]
        public int teamIDNumber = 0;

        [Header("Poise")]
        public float poiseBreak;
        public float offensivePoiseBonus;

        [Header("Damage")]
        public int physicalDamage;
        public int fireDamage;
        public int magicDamage;
        public int lightningDamage;
        public int darkDamage;

        protected bool shieldHasBeenHit;
        protected bool hasBeenParried;
        protected string currentDamageAnimation;
        protected virtual void Awake()
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

        protected virtual void OnTriggerEnter(Collider collision)
        {
            if (collision.tag == "Character")
            {
                shieldHasBeenHit = false;
                hasBeenParried = false;
                CharacterStatsManager enemyStats = collision.GetComponent<CharacterStatsManager>();
                CharacterManager enemyManager = collision.GetComponent<CharacterManager>();
                CharacterEffectsManager enemyEffects = collision.GetComponent<CharacterEffectsManager>();
                BlockingCollider shield = collision.GetComponentInChildren<BlockingCollider>();

                if (enemyManager != null)
                {
                    if (enemyStats.teamIDNumber == teamIDNumber)
                        return;
                    CheckForParry(enemyManager);
                    CheckForBlock(enemyManager, enemyStats, shield);


                }
                if (enemyStats != null)
                {
                    if (enemyStats.teamIDNumber == teamIDNumber)
                        return;
                    if (hasBeenParried)
                        return;
                    if (shieldHasBeenHit)
                        return;
                    enemyStats.poiseResetTimer = enemyStats.totalPoiseResetTimer;
                    enemyStats.totalPoiseDefense = enemyStats.totalPoiseDefense - poiseBreak;

                    //Detect first contact point
                    Vector3 contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                    float directionHitFrom = (Vector3.SignedAngle(characterManager.transform.forward, enemyManager.transform.forward, Vector3.up));
                    ChooseWhichDirectionDamageComeFrom(directionHitFrom);
                    enemyEffects.PlayBloodSplatterFX(contactPoint);
                    if (enemyStats.totalPoiseDefense > poiseBreak)
                    {
                        enemyStats.TakeDamageNoAnimation(physicalDamage, 0);
                    }
                    else
                    {
                        enemyStats.TakeDamage(physicalDamage, 0, currentDamageAnimation);
                    }
                }
            }

            if (collision.tag == "IllusionaryWall")
            {
                IllusionaryWall illusionaryWall = collision.GetComponent<IllusionaryWall>();
                illusionaryWall.wallHasBeenHit = true;
            }
        }

        protected virtual void CheckForParry(CharacterManager enemyManager)
        {
            if (enemyManager.isParrying)
            {
                characterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimation("Damage_01", true);
                hasBeenParried = true;
            }
        }

        protected virtual void CheckForBlock(CharacterManager enemyManager, CharacterStatsManager enemyStats, BlockingCollider shield)
        {
            if (shield != null && enemyManager.isBlocking)
            {
                float physicalDamageAfterBlock =
                    physicalDamage - (physicalDamage * shield.blockingPhysicalDamageAbsorbtion) / 100;
                float fireDamageAfterBlock = fireDamage - (fireDamage * shield.blockingFireDamageAbsorbtion) / 100;
                if (enemyStats != null)
                {
                    enemyStats.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock), 0, "Block_Idle");
                    shieldHasBeenHit = true;
                }
            }
        }
        protected virtual void ChooseWhichDirectionDamageComeFrom(float direction)
        {
            if (direction >= 145 && direction <= 180)
            {
                currentDamageAnimation = "Damage_Forward_01";
            }
            if (direction >= -180 && direction < -145)
            {
                currentDamageAnimation = "Damage_Forward_01";
            }
            if (direction >= -45 && direction < 45)
            {
                currentDamageAnimation = "Damage_Back_01";
            }
            if (direction >= -145 && direction < -45)
            {
                currentDamageAnimation = "Damage_Left_01";
            }
            if (direction >= 45 && direction < 145)
            {
                currentDamageAnimation = "Damage_Right_01";
            }
        }
    }
}
