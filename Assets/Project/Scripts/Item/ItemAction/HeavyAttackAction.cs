using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHH
{
    [CreateAssetMenu(menuName = "Item Actions/Heavy Attack Action")]
    public class HeavyAttackAction : ItemAction
    {
        public override void PerformAction(PlayerManager player)
        {
            if (player.playerStatsManager.currentStamina <= 0)
            {
                return;
            }
            player.playerAnimatorManager.EraseHandIKForWeapon();
            player.playerEffectsManager.PlayWeaponFX(false);

            if (player.isSprinting)
            {
                HandleJumpingAttack(player);
                return;
            }

            if (player.canDoCombo)
            {
                player.inputHandler.comboFlag = true;
                HandleHeavyWeaponCombo(player);
                player.inputHandler.comboFlag = false;
            }
            else
            {
                if (player.isInteracting)
                {
                    return;
                }
                if (player.canDoCombo)
                {
                    return;
                }
                HandleHeavyAttack(player);
            }

        }
        private void HandleHeavyAttack(PlayerManager player)
        {
            if (player.isUsingLeftHand)
            {
                player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.OH_Heavy_Attack_1, true, false, true);
                player.playerCombatManager.lastAttack = player.playerCombatManager.OH_Heavy_Attack_1;
            }
            else if (player.isUsingRightHand)
            {
                if (player.inputHandler.twoHandFlag)
                {
                    player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.TH_Heavy_Attack_1, true);
                    player.playerCombatManager.lastAttack = player.playerCombatManager.TH_Heavy_Attack_1;
                }
                else
                {
                    player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.OH_Heavy_Attack_1, true);
                    player.playerCombatManager.lastAttack = player.playerCombatManager.OH_Heavy_Attack_1;
                }
            }
        }

        private void HandleJumpingAttack(PlayerManager player)
        {
            if (player.isUsingLeftHand)
            {
                player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.OH_Jumping_Attack_01, true, false, true);
                player.playerCombatManager.lastAttack = player.playerCombatManager.OH_Jumping_Attack_01;
            }
            else if (player.isUsingRightHand)
            {
                if (player.inputHandler.twoHandFlag)
                {
                    player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.TH_Jumping_Attack_01, true);
                    player.playerCombatManager.lastAttack = player.playerCombatManager.TH_Jumping_Attack_01;
                }
                else
                {
                    player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.OH_Jumping_Attack_01, true);
                    player.playerCombatManager.lastAttack = player.playerCombatManager.OH_Jumping_Attack_01;
                }
            }

        }

        private void HandleHeavyWeaponCombo(PlayerManager player)
        {
            if (player.inputHandler.comboFlag)
            {
                player.animator.SetBool("canDoCombo", false);
                if (player.isUsingLeftHand)
                {
                    if (player.playerCombatManager.lastAttack == player.playerCombatManager.OH_Heavy_Attack_1)
                    {
                        player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.OH_Heavy_Attack_2, true, false, true);
                        player.playerCombatManager.lastAttack = player.playerCombatManager.OH_Heavy_Attack_2;

                    }
                    else
                    {
                        player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.OH_Heavy_Attack_1, true, false, true);
                        player.playerCombatManager.lastAttack = player.playerCombatManager.OH_Heavy_Attack_1;

                    }
                }
                else if (player.isUsingRightHand)
                {
                    if (player.isTwoHandingWeapon)
                    {
                        if (player.playerCombatManager.lastAttack == player.playerCombatManager.TH_Heavy_Attack_1)
                        {
                            player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.TH_Heavy_Attack_2, true);
                            player.playerCombatManager.lastAttack = player.playerCombatManager.TH_Heavy_Attack_2;
                        }
                        else
                        {
                            player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.TH_Heavy_Attack_1, true);
                            player.playerCombatManager.lastAttack = player.playerCombatManager.TH_Heavy_Attack_1;

                        }
                    }
                    else
                    {
                        if (player.playerCombatManager.lastAttack == player.playerCombatManager.OH_Heavy_Attack_1)
                        {
                            player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.OH_Heavy_Attack_2, true);
                            player.playerCombatManager.lastAttack = player.playerCombatManager.OH_Heavy_Attack_2;

                        }
                        else
                        {
                            player.playerAnimatorManager.PlayTargetAnimation(player.playerCombatManager.OH_Heavy_Attack_1, true);
                            player.playerCombatManager.lastAttack = player.playerCombatManager.OH_Heavy_Attack_1;

                        }
                    }
                }

            }
        }
    }
}

