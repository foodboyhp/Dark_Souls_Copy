using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHH
{
    public class EnemyWeaponSlotManager : CharacterWeaponSlotManager
    {

        public override void GrantWeaponAttackingPoiseBonus()
        {
            character.characterStatsManager.totalPoiseDefense = character.characterStatsManager.totalPoiseDefense + character.characterStatsManager.offensivePoiseBonus;
        }

        public override void ResetWeaponAttackingPoise()
        {
            character.characterStatsManager.totalPoiseDefense = character.characterStatsManager.armorPoiseBonus;
        }
    }
}
