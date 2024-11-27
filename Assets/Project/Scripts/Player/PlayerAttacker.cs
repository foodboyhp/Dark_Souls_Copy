using System.Collections;
using UnityEngine;

namespace PHH
{
    public class PlayerAttacker : MonoBehaviour
    {
        AnimatorHandler animatorHandler;
        PlayerManager playerManager;
        PlayerInventory playerInventory;
        InputHandler inputHandler;
        WeaponSlotManager weaponSlotManager;
        public string lastAttack;
        private void Awake()
        {
            playerManager = GetComponentInParent<PlayerManager>();  
            playerInventory = GetComponentInParent<PlayerInventory>();  
            animatorHandler = GetComponent<AnimatorHandler>();
            weaponSlotManager = GetComponent<WeaponSlotManager>();
            inputHandler = GetComponentInParent<InputHandler>();
        }

        public void HandleWeaponCombo(WeaponItem weapon)
        {
            if (inputHandler.comboFlag)
            {
                animatorHandler.anim.SetBool("canDoCombo", false);
                if (lastAttack == weapon.OH_Light_Attack_1)
                {
                    animatorHandler.PlayerTargetAnimation(weapon.OH_Light_Attack_2, true);
                }
                else if(lastAttack == weapon.th_light_attack_1)
                {
                    animatorHandler.PlayerTargetAnimation(weapon.th_light_attack_2 , true);
                }
            }
        }
        public void HandleLightAttack(WeaponItem weapon)
        {
            weaponSlotManager.attackingWeapon = weapon;
            if (inputHandler.twoHandFlag)
            {
                animatorHandler.PlayerTargetAnimation(weapon.th_light_attack_1, true);
                lastAttack = weapon.th_light_attack_1;
            }
            else
            {
                animatorHandler.PlayerTargetAnimation(weapon.OH_Light_Attack_1, true);
                lastAttack = weapon.OH_Light_Attack_1;
            }
        }

        public void HandleHeavyAttack(WeaponItem weapon)
        {
            weaponSlotManager.attackingWeapon = weapon;
            if (inputHandler.twoHandFlag)
            {

            }
            else
            {
                animatorHandler.PlayerTargetAnimation(weapon.OH_Heavy_Attack_1, true);
                lastAttack = weapon.OH_Heavy_Attack_1;
            }
        }
        #region Handle InputActions
        public void HandlerRBAction()
        {
            if (playerInventory.rightWeapon.isMeleeWeapon)
            {
                PerformRBMeleeAction();
            }
            else if(playerInventory.rightWeapon.isSpellCaster 
                || playerInventory.rightWeapon.isFaithCaster 
                || playerInventory.rightWeapon.isPyroCaster)
            {
                PerformRBMagicAction(playerInventory.rightWeapon);
            }
        }
        #endregion
        #region Attack Actions
        private void PerformRBMeleeAction()
        {
            if (playerManager.canDoCombo)
            {
                inputHandler.comboFlag = true;
                HandleWeaponCombo(playerInventory.rightWeapon);
                inputHandler.comboFlag = false;
            }
            else
            {
                if (playerManager.isInteracting)
                {
                    return;
                }
                if (playerManager.canDoCombo)
                {
                    return;
                }
                animatorHandler.anim.SetBool("IsUsingRightHand", true);
                HandleLightAttack(playerInventory.rightWeapon);
            }
        }
        private void PerformRBMagicAction(WeaponItem weapon)
        {
            if (weapon.isFaithCaster)
            {
                if(playerInventory.currentSpell != null && playerInventory.currentSpell.isFaithSpell)
                {

                }
            }
        }
        #endregion
    }
}