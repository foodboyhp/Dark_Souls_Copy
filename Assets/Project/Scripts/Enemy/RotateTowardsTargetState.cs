using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHH
{
    public class RotateTowardsTargetState : State
    {
        public CombatStanceState combatStanceState;
        public override State Tick(EnemyManager enemy)
        {
            enemy.animator.SetFloat("Vertical", 0f);
            enemy.animator.SetFloat("Horizontal", 0f);

            Vector3 targetDirection = enemy.currentTarget.transform.position - enemy.transform.position;
            float viewableAngle = Vector3.SignedAngle(targetDirection, enemy.transform.forward, Vector3.up);

            if (enemy.isInteracting)
            {
                return this;
            }

            if (viewableAngle > 100f && viewableAngle < 180f && !enemy.isInteracting)
            {
                enemy.enemyAnimatorManager.PlayTargetAnimationWithRootRotation("Turn Behind", true);
                return combatStanceState;
            }
            else if (viewableAngle <= -101 && viewableAngle >= -180 && !enemy.isInteracting)
            {
                enemy.enemyAnimatorManager.PlayTargetAnimationWithRootRotation("Turn Behind", true);
                return combatStanceState;

            }
            else if (viewableAngle <= -45 && viewableAngle >= -100 && !enemy.isInteracting)
            {
                enemy.enemyAnimatorManager.PlayTargetAnimationWithRootRotation("Turn Right", true);
                return combatStanceState;

            }
            else if (viewableAngle >= 45 && viewableAngle <= 100 && !enemy.isInteracting)
            {
                enemy.enemyAnimatorManager.PlayTargetAnimationWithRootRotation("Turn Left", true);
                return combatStanceState;

            }
            return combatStanceState;
        }
    }
}
