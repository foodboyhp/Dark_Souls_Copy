using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHH
{
    [CreateAssetMenu(menuName = "Spells/Healing Spell")]
    public class HealingSpell : SpellItem
    {
        public int healAmount;

        public override void AttemptToCastSpell(PlayerAnimatorManager animatorHandler, PlayerStats playerStats)
        {
            base.AttemptToCastSpell(animatorHandler, playerStats);  
            GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, animatorHandler.transform);
            animatorHandler.PlayTargetAnimation(spellAnimation, true);
        }

        public override void SuccessfullyCastSpell(PlayerAnimatorManager animatorHandler, PlayerStats playerStats)
        {
            base.SuccessfullyCastSpell(animatorHandler, playerStats);
            GameObject instantiatedSpellFX = Instantiate(spellCastFX, animatorHandler.transform);
            playerStats.HealPlayer(healAmount);
            playerStats.currentHealth = playerStats.currentHealth + healAmount;
        }
    }
}