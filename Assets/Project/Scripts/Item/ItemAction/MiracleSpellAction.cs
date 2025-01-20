using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHH
{
    [CreateAssetMenu(menuName = "Item Actions/Miracle Spell Action")]
    public class MiracleSpellAction : ItemAction
    {
        public override void PerformAction(PlayerManager player)
        {
            if (player.isInteracting)
            {
                return;
            }
            if (player.playerInventoryManager.currentSpell != null && player.playerInventoryManager.currentSpell.isFaithSpell)
            {
                if (player.playerStatsManager.currentFocusPoint >= player.playerInventoryManager.currentSpell.focusPointCost)
                {
                    player.playerInventoryManager.currentSpell.AttemptToCastSpell(player.playerAnimatorManager,
                        player.playerStatsManager, player.playerWeaponSlotManager, player.isUsingLeftHand);
                }
                else
                {
                    player.playerAnimatorManager.PlayTargetAnimation("Shrug", true);
                }
            }
        }
    }
}