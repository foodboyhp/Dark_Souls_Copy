using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHH
{
    public class RangedProjectileDamageCollider : DamageCollider
    {
        public RangedAmmoItem ammoItem;
        protected bool hasAlreadyPenetratedASurface;
        protected GameObject penetratedProjectile;
        protected override void OnTriggerEnter(Collider collision)
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

    }

}
