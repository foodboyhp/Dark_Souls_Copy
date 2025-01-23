using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHH
{
    public class SleepState : State
    {
        public override State Tick(EnemyManager enemy)
        {
            return this;
        }
    }
}
