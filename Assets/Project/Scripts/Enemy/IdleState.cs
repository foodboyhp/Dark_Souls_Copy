using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHH
{
    public class IdleState : State
    {
        public PursueTargetState pursueTargetState;
        public LayerMask detectionLayer;

        public override State Tick(EnemyManager enemy)
        {
            if (enemy.isInteracting)
            {
                return this;
            }
            #region Handle EnemyTargetDetection
            Collider[] colliders = Physics.OverlapSphere(enemy.transform.position, enemy.detectionRadius, detectionLayer);

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterStatsManager characterStats = colliders[i].transform.GetComponent<CharacterStatsManager>();
                if (characterStats != null)
                {
                    if (characterStats.teamIDNumber != enemy.enemyStatsManager.teamIDNumber)
                    {
                        Vector3 targetDirection = characterStats.transform.position - enemy.transform.position;
                        float viewableAngle = Vector3.Angle(targetDirection, enemy.transform.forward);

                        if (viewableAngle > enemy.minimumDetectionAngle && viewableAngle < enemy.maximumDetectionAngle)
                        {
                            enemy.currentTarget = characterStats;
                        }
                    }
                }
            }
            #endregion
            #region Handle SwitchToNextState
            if (enemy.currentTarget != null)
            {
                return pursueTargetState;
            }
            else
            {
                return this;
            }
            #endregion
        }
    }
}
