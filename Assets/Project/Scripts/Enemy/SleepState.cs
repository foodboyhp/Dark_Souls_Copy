using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHH
{
    public class SleepState : State
    {
        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            return this;
        }
    }
}
