using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHH
{
    public class EnemyWeaponSlotManager : CharacterWeaponSlotManager
    {

        public override void GrantWeaponAttackingPoiseBonus()
        {
            characterStatsManager.totalPoiseDefense = characterStatsManager.totalPoiseDefense + attackingWeapon.offensivePoiseBonus;
        }

        public override void ResetWeaponAttackingPoise()
        {
            characterStatsManager.totalPoiseDefense = characterStatsManager.armorPoiseBonus;
        }
    }
}
