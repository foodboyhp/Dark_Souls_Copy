using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHH
{
    public class EnemyAnimatorManager : CharacterAnimatorManager
    {
        EnemyManager enemy;
        protected override void Awake()
        {
            base.Awake();
            enemy = GetComponent<EnemyManager>();
        }

        public void AwardSoulsOnDeath()
        {
            PlayerStatsManager playerStats = FindObjectOfType<PlayerStatsManager>();
            SoulsCountBar soulsCountBar = FindObjectOfType<SoulsCountBar>();
            if (playerStats != null)
            {
                playerStats.AddSouls(enemy.enemyStatsManager.soulsAwardedOnDeath);
                if (soulsCountBar != null)
                {
                    soulsCountBar.SetSoulsCountText(playerStats.currentSoulCount);
                }
            }
        }
        public void InstantiateBossParticleFX()
        {
            BossFXTransform bossFXTransform = GetComponentInChildren<BossFXTransform>();
            GameObject phaseFX = Instantiate(enemy.enemyBossManager.particleFX, bossFXTransform.transform);
        }
        public void PlayWeaponTrailFX()
        {
            enemy.enemyEffectsManager.PlayWeaponFX(false);
        }
        private void OnAnimatorMove()
        {
            float delta = Time.deltaTime;
            enemy.enemyRigidbody.drag = 0;
            Vector3 deltaPosition = enemy.animator.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            enemy.enemyRigidbody.velocity = velocity;

            if (enemy.isRotatingWithRootMotion)
            {
                enemy.transform.rotation *= enemy.animator.deltaRotation;
            }
        }
    }
}
