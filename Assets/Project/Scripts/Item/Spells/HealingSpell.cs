using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHH
{
    [CreateAssetMenu(menuName = "Spells/Healing Spell")]
    public class HealingSpell : SpellItem
    {
        public int healAmount;

        public override void AttemptToCastSpell(PlayerAnimatorManager animatorHandler,
            PlayerStatsManager playerStats, PlayerWeaponSlotManager weaponSlotManager, bool isLeftHanded)
        {
            base.AttemptToCastSpell(animatorHandler, playerStats, weaponSlotManager, isLeftHanded);
            GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, animatorHandler.transform);
            animatorHandler.PlayTargetAnimation(spellAnimation, true, false, isLeftHanded);
        }

        public override void SuccessfullyCastSpell(PlayerAnimatorManager animatorHandler, PlayerStatsManager playerStats,
            CameraHandler cameraHandler, PlayerWeaponSlotManager weaponSlotManager, bool isLeftHanded)
        {
            base.SuccessfullyCastSpell(animatorHandler, playerStats, cameraHandler, weaponSlotManager, isLeftHanded);
            GameObject instantiatedSpellFX = Instantiate(spellCastFX, animatorHandler.transform);
            playerStats.HealPlayer(healAmount);
            playerStats.currentHealth = playerStats.currentHealth + healAmount;
        }
    }
}
