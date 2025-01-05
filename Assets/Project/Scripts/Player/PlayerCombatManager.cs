using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PHH
{
    public class PlayerCombatManager : MonoBehaviour
    {
        InputHandler inputHandler;
        CameraHandler cameraHandler;
        PlayerManager playerManager;
        PlayerStatsManager playerStatsManager;
        PlayerAnimatorManager playerAnimatorManager;
        PlayerEquipmentManager playerEquipmentManager;
        PlayerInventoryManager playerInventoryManager;
        PlayerWeaponSlotManager playerWeaponSlotManager;
        PlayerEffectsManager playerEffectsManager;

        [Header("Attack Animations")]
        string OH_Light_Attack_1 = "OH_Light_Attack_1";
        string OH_Light_Attack_2 = "OH_Light_Attack_2";
        string OH_Heavy_Attack_1 = "OH_Heavy_Attack_1";
        string OH_Heavy_Attack_2 = "OH_Heavy_Attack_2";

        string TH_Light_Attack_1 = "TH_Light_Attack_1";
        string TH_Light_Attack_2 = "TH_Light_Attack_2";
        string TH_Heavy_Attack_1 = "TH_Heavy_Attack_1";
        string TH_Heavy_Attack_2 = "TH_Heavy_Attack_2";
        string Weapon_Art = "Weapon_Art";

        public string lastAttack;

        public LayerMask backStabLayer;
        public LayerMask riposteLayer;

        private void Awake()
        {
            inputHandler = GetComponent<InputHandler>();
            cameraHandler = FindObjectOfType<CameraHandler>();
            playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
            playerManager = GetComponent<PlayerManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
            playerEffectsManager = GetComponent<PlayerEffectsManager>();
        }
        public void HandlerRBAction()
        {
            if (playerInventoryManager.rightWeapon.weaponType.Equals(WeaponType.StraighSword)
                || playerInventoryManager.rightWeapon.weaponType.Equals(WeaponType.Unarmed))
            {
                PerformRBMeleeAction();
            }
            else if (playerInventoryManager.rightWeapon.weaponType.Equals(WeaponType.SpellCaster)
                || playerInventoryManager.rightWeapon.weaponType.Equals(WeaponType.FaithCaster)
                || playerInventoryManager.rightWeapon.weaponType.Equals(WeaponType.PyromancyCaster))
            {
                PerformRBMagicAction(playerInventoryManager.rightWeapon);
            }
        }
        public void HandleLBAction()
        {
            PerformLBBlockingAction();
        }
        public void HandleLTAction()
        {
            if (playerInventoryManager.leftWeapon.weaponType.Equals(WeaponType.Shield)
                || playerInventoryManager.leftWeapon.weaponType.Equals(WeaponType.Unarmed))
            {
                PerformLTWeaponArt(inputHandler.twoHandFlag);
            }
            else if (playerInventoryManager.leftWeapon.weaponType.Equals(WeaponType.StraighSword))
            {

            }
        }
        public void HandleWeaponCombo(WeaponItem weapon)
        {
            if (playerStatsManager.currentStamina <= 0)
            {
                return;
            }
            if (inputHandler.comboFlag)
            {
                playerAnimatorManager.animator.SetBool("canDoCombo", false);
                if (lastAttack == OH_Light_Attack_1)
                {
                    playerAnimatorManager.PlayTargetAnimation(OH_Light_Attack_2, true);
                }
                else if (lastAttack == TH_Light_Attack_1)
                {
                    playerAnimatorManager.PlayTargetAnimation(TH_Light_Attack_2, true);
                }
            }
        }
        public void HandleLightAttack(WeaponItem weapon)
        {
            if (playerStatsManager.currentStamina <= 0)
            {
                return;
            }
            playerWeaponSlotManager.attackingWeapon = weapon;
            if (inputHandler.twoHandFlag)
            {
                playerAnimatorManager.PlayTargetAnimation(TH_Light_Attack_1, true);
                lastAttack = TH_Light_Attack_1;
            }
            else
            {
                playerAnimatorManager.PlayTargetAnimation(OH_Light_Attack_1, true);
                lastAttack = OH_Light_Attack_1;
            }
        }
        public void HandleHeavyAttack(WeaponItem weapon)
        {
            if (playerStatsManager.currentStamina <= 0)
            {
                return;
            }
            playerWeaponSlotManager.attackingWeapon = weapon;
            if (inputHandler.twoHandFlag)
            {

            }
            else
            {
                playerAnimatorManager.PlayTargetAnimation(OH_Heavy_Attack_1, true);
                lastAttack = OH_Heavy_Attack_1;
            }
        }
        private void PerformRBMeleeAction()
        {
            if (playerManager.canDoCombo)
            {
                inputHandler.comboFlag = true;
                HandleWeaponCombo(playerInventoryManager.rightWeapon);
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
                playerAnimatorManager.animator.SetBool("IsUsingRightHand", true);
                HandleLightAttack(playerInventoryManager.rightWeapon);
            }

            playerEffectsManager.PlayWeaponFX(false);
        }
        private void PerformLBBlockingAction()
        {
            if (playerManager.isInteracting)
            {
                return;
            }
            if (playerManager.isBlocking)
            {
                return;
            }
            playerAnimatorManager.PlayTargetAnimation("Block_Start", false, true);
            playerEquipmentManager.OpenBlockingCollider();
            playerManager.isBlocking = true;

        }
        private void PerformRBMagicAction(WeaponItem weapon)
        {
            if (playerManager.isInteracting)
            {
                return;
            }
            if (weapon.weaponType.Equals(WeaponType.FaithCaster))
            {
                if (playerInventoryManager.currentSpell != null && playerInventoryManager.currentSpell.isFaithSpell)
                {
                    if (playerStatsManager.currentFocusPoint >= playerInventoryManager.currentSpell.focusPointCost)
                    {
                        playerInventoryManager.currentSpell.AttemptToCastSpell(playerAnimatorManager, playerStatsManager, playerWeaponSlotManager);
                    }
                }
            }
            else if (weapon.weaponType.Equals(WeaponType.PyromancyCaster))
            {
                if (playerInventoryManager.currentSpell != null && playerInventoryManager.currentSpell.isPyroSpell)
                {
                    if (playerStatsManager.currentFocusPoint >= playerInventoryManager.currentSpell.focusPointCost)
                    {
                        playerInventoryManager.currentSpell.AttemptToCastSpell(playerAnimatorManager, playerStatsManager, playerWeaponSlotManager);
                    }
                }
            }
        }
        private void PerformLTWeaponArt(bool isTwoHanding)
        {
            if (playerManager.isInteracting)
            {
                return;
            }
            if (isTwoHanding)
            {
            }
            else
            {
                playerAnimatorManager.PlayTargetAnimation(Weapon_Art, true);

            }

        }
        private void SuccessfullyCastSpell()
        {
            playerInventoryManager.currentSpell.SuccessfullyCastSpell(playerAnimatorManager, playerStatsManager, cameraHandler, playerWeaponSlotManager);
            playerAnimatorManager.animator.SetBool("isFiringSpell", true);
        }
        public void AttemptBackStabOrRiposte()
        {
            if (playerStatsManager.currentStamina <= 0)
            {
                return;
            }
            RaycastHit hit;
            if (Physics.Raycast(inputHandler.criticalAttackRayCastStartPoint.position,
                transform.TransformDirection(Vector3.forward), out hit, 0.5f, backStabLayer))
            {
                CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
                DamageCollider rightWeapon = playerWeaponSlotManager.rightHandDamageCollider;

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

                    int criticalDamage = playerInventoryManager.rightWeapon.criticalDamageMultiplier * rightWeapon.physicalDamage;
                    enemyCharacterManager.pendingCriticalDamage = criticalDamage;

                    playerAnimatorManager.PlayTargetAnimation("Stab", true);
                    enemyCharacterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimation("Damage_01", true);
                }
            }
            else if (Physics.Raycast(inputHandler.criticalAttackRayCastStartPoint.position,
                transform.TransformDirection(Vector3.forward), out hit, 0.5f, riposteLayer))
            {
                CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
                DamageCollider rightWeapon = playerWeaponSlotManager.rightHandDamageCollider;
                playerManager.transform.position = enemyCharacterManager.riposteCollider.criticalDamageStandPosition.position;

                if (enemyCharacterManager != null && enemyCharacterManager.canBeRiposted)
                {
                    Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
                    rotationDirection = hit.transform.position - playerManager.transform.position;
                    rotationDirection.y = 0;
                    rotationDirection.Normalize();
                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
                    playerManager.transform.rotation = targetRotation;

                    int criticalDamage = playerInventoryManager.rightWeapon.criticalDamageMultiplier * rightWeapon.physicalDamage;
                    enemyCharacterManager.pendingCriticalDamage = criticalDamage;

                    playerAnimatorManager.PlayTargetAnimation("Stab", true);
                    enemyCharacterManager.GetComponentInChildren<CharacterAnimatorManager>().PlayTargetAnimation("Damage_01", true);
                }
            }
        }
    }
}