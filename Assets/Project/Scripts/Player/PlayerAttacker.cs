using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PHH
{
    public class PlayerAttacker : MonoBehaviour
    {
        PlayerEquipmentManager playerEquipmentManager;
        PlayerAnimatorManager animatorHandler;
        PlayerManager playerManager;
        PlayerStats playerStats;
        PlayerInventory playerInventory;
        InputHandler inputHandler;
        WeaponSlotManager weaponSlotManager;
        public string lastAttack;

        public LayerMask backStabLayer;
        public LayerMask riposteLayer;

        private void Awake()
        {
            playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
            playerManager = GetComponentInParent<PlayerManager>();  
            playerStats = GetComponentInParent<PlayerStats>();
            playerInventory = GetComponentInParent<PlayerInventory>();  
            animatorHandler = GetComponent<PlayerAnimatorManager>();
            weaponSlotManager = GetComponent<WeaponSlotManager>();
            inputHandler = GetComponentInParent<InputHandler>();
        }

        public void HandleWeaponCombo(WeaponItem weapon)
        {
            if (playerStats.currentStamina <= 0)
            {
                return;
            }
            if (inputHandler.comboFlag)
            {
                animatorHandler.anim.SetBool("canDoCombo", false);
                if (lastAttack == weapon.OH_Light_Attack_1)
                {
                    animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_2, true);
                }
                else if(lastAttack == weapon.th_light_attack_1)
                {
                    animatorHandler.PlayTargetAnimation(weapon.th_light_attack_2 , true);
                }
            }
        }
        public void HandleLightAttack(WeaponItem weapon)
        {
            if (playerStats.currentStamina <= 0)
            {
                return;
            }
            weaponSlotManager.attackingWeapon = weapon;
            if (inputHandler.twoHandFlag)
            {
                animatorHandler.PlayTargetAnimation(weapon.th_light_attack_1, true);
                lastAttack = weapon.th_light_attack_1;
            }
            else
            {
                animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_1, true);
                lastAttack = weapon.OH_Light_Attack_1;
            }
        }

        public void HandleHeavyAttack(WeaponItem weapon)
        {
            if (playerStats.currentStamina <= 0)
            {
                return;
            }
            weaponSlotManager.attackingWeapon = weapon;
            if (inputHandler.twoHandFlag)
            {

            }
            else
            {
                animatorHandler.PlayTargetAnimation(weapon.OH_Heavy_Attack_1, true);
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

        public void HandleLBAction()
        {
            PerformLBBlockingAction();
        }
        public void HandleLTAction()
        {
            if(playerInventory.leftWeapon.isShieldWeapon)
            {
                PerformLTWeaponArt(inputHandler.twoHandFlag);
            }
            else if(playerInventory.leftWeapon.isMeleeWeapon)
            {

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
            if (playerManager.isInteracting)
            {
                return ;
            }
            if (weapon.isFaithCaster)
            {
                if(playerInventory.currentSpell != null && playerInventory.currentSpell.isFaithSpell)
                {
                    if(playerStats.currentFocusPoint >= playerInventory.currentSpell.focusPointCost)
                    {
                        playerInventory.currentSpell.AttemptToCastSpell(animatorHandler, playerStats);
                    }
                }
            }
        }

        private void PerformLTWeaponArt(bool isTwoHanding)
        {
            if (playerManager.isInteracting)
            {
                return ;
            }
            if(isTwoHanding)
            {
            }
            else
            {
                animatorHandler.PlayTargetAnimation(playerInventory.leftWeapon.weapon_art, true);

            }

        }

        private void SuccessfullyCastSpell()
        {
            playerInventory.currentSpell.SuccessfullyCastSpell(animatorHandler, playerStats);
        }
        #endregion
        #region Defense Actions
        private void PerformLBBlockingAction()
        {
            if (playerManager.isInteracting)
            {
                return ;
            }
            if (playerManager.isBlocking)
            {
                return;
            }
            animatorHandler.PlayTargetAnimation("Block_Start", false, true);
            playerEquipmentManager.OpenBlockingCollider();
            playerManager.isBlocking = true;

        }
        #endregion
        public void AttemptBackStabOrRiposte()
        {
            if (playerStats.currentStamina <= 0)
            {
                return;
            }
            RaycastHit hit;
            if (Physics.Raycast(inputHandler.criticalAttackRayCastStartPoint.position,
                transform.TransformDirection(Vector3.forward), out hit, 0.5f, backStabLayer))
            {
                CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
                DamageCollider rightWeapon = weaponSlotManager.rightHandDamageCollider;

                if (enemyCharacterManager != null)
                {
                    playerManager.transform.position = enemyCharacterManager.backStabCollider.criticalDamageStandPosition.position;

                    Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
                    rotationDirection = hit.transform.position - playerManager.transform.position;
                    rotationDirection.y = 0;
                    rotationDirection.Normalize();
                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
                    playerManager.transform.rotation = targetRotation;

                    int criticalDamage = playerInventory.rightWeapon.criticalDamageMultiplier * rightWeapon.currentWeaponDamage;
                    enemyCharacterManager.pendingCriticalDamage = criticalDamage;

                    animatorHandler.PlayTargetAnimation("Stab", true);
                    enemyCharacterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Damage_01", true);
                }
            }
            else if (Physics.Raycast(inputHandler.criticalAttackRayCastStartPoint.position,
                transform.TransformDirection(Vector3.forward), out hit, 0.5f, riposteLayer))
            {
                CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
                DamageCollider rightWeapon = weaponSlotManager.rightHandDamageCollider;
                playerManager.transform.position = enemyCharacterManager.riposteCollider.criticalDamageStandPosition.position;

                if(enemyCharacterManager != null && enemyCharacterManager.canBeRiposted)
                {
                    Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
                    rotationDirection = hit.transform.position - playerManager.transform.position;
                    rotationDirection.y = 0;
                    rotationDirection.Normalize();
                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
                    playerManager.transform.rotation = targetRotation;

                    int criticalDamage = playerInventory.rightWeapon.criticalDamageMultiplier * rightWeapon.currentWeaponDamage;
                    enemyCharacterManager.pendingCriticalDamage = criticalDamage;

                    animatorHandler.PlayTargetAnimation("Stab", true);
                    enemyCharacterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Damage_01", true);
                }
            }
        }
    }
}