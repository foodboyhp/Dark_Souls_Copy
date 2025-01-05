using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHH
{
    public class EnemyAnimatorManager : CharacterAnimatorManager
    {
        EnemyManager enemyManager;
        EnemyEffectsManager enemyEffectsManager;
        EnemyBossManager enemyBossManager;
        protected override void Awake()
        {
            base.Awake();
            enemyManager = GetComponent<EnemyManager>();
            enemyEffectsManager = GetComponent<EnemyEffectsManager>();
            enemyBossManager = GetComponent<EnemyBossManager>();
        }

        public void AwardSoulsOnDeath()
        {
            PlayerStatsManager playerStats = FindObjectOfType<PlayerStatsManager>();
            SoulsCountBar soulsCountBar = FindObjectOfType<SoulsCountBar>();
            if (playerStats != null)
            {
                playerStats.AddSouls(characterStatsManager.soulsAwardedOnDeath);
                if (soulsCountBar != null)
                {
                    soulsCountBar.SetSoulsCountText(playerStats.soulCount);
                }
            }
        }
        public void InstantiateBossParticleFX()
        {
            BossFXTransform bossFXTransform = GetComponentInChildren<BossFXTransform>();
            GameObject phaseFX = Instantiate(enemyBossManager.particleFX, bossFXTransform.transform);
        }
        public void PlayWeaponTrailFX()
        {
            enemyEffectsManager.PlayWeaponFX(false);
        }
        private void OnAnimatorMove()
        {
            float delta = Time.deltaTime;
            enemyManager.enemyRigidbody.drag = 0;
            Vector3 deltaPosition = animator.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            enemyManager.enemyRigidbody.velocity = velocity;

            if (enemyManager.isRotatingWithRootMotion)
            {
                enemyManager.transform.rotation *= animator.deltaRotation;
            }
        }
    }
}
