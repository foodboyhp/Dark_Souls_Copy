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
        string OH_Running_Attack_01 = "OH_Running_Attack_01";
        string OH_Jumping_Attack_01 = "OH_Jumping_Attack_01";

        string TH_Light_Attack_1 = "TH_Light_Attack_1";
        string TH_Light_Attack_2 = "TH_Light_Attack_2";
        string TH_Heavy_Attack_1 = "TH_Heavy_Attack_1";
        string TH_Heavy_Attack_2 = "TH_Heavy_Attack_2";
        string TH_Running_Attack_01 = "TH_Running_Attack_01";
        string TH_Jumping_Attack_01 = "TH_Jumping_Attack_01";
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
        public void HandleHoldRBAction()
        {
            if (playerManager.isTwoHandingWeapon)
            {
                PerformRBRangeAction();
            }
            else
            {
            }
        }
        public void HandleRBAction()
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
                PerformMagicAction(playerInventoryManager.rightWeapon, true);
                playerAnimatorManager.animator.SetBool("isUsingLeftHand", true);
            }
        }
        public void HandleRTAction()
        {
            if (playerInventoryManager.rightWeapon.weaponType.Equals(WeaponType.StraighSword)
                || playerInventoryManager.rightWeapon.weaponType.Equals(WeaponType.Unarmed))
            {
                PerformRTMeleeAction();
            }
            else if (playerInventoryManager.rightWeapon.weaponType.Equals(WeaponType.SpellCaster)
                || playerInventoryManager.rightWeapon.weaponType.Equals(WeaponType.FaithCaster)
                || playerInventoryManager.rightWeapon.weaponType.Equals(WeaponType.PyromancyCaster))
            {
                PerformMagicAction(playerInventoryManager.rightWeapon, true);
                playerAnimatorManager.animator.SetBool("isUsingLeftHand", true);
            }
        }
        public void HandleLBAction()
        {
            PerformLBBlockingAction();
            if (playerManager.isTwoHandingWeapon)
            {
                if (playerInventoryManager.rightWeapon.weaponType == WeaponType.Bow)
                {
                    PerformLBAimingAction();
                }
            }
            else
            {
                if (playerInventoryManager.rightWeapon.weaponType == WeaponType.Shield ||
                    playerInventoryManager.rightWeapon.weaponType == WeaponType.StraighSword)
                {
                    PerformLBBlockingAction();
                }
                else if (playerInventoryManager.rightWeapon.weaponType == WeaponType.FaithCaster ||
                    playerInventoryManager.rightWeapon.weaponType == WeaponType.PyromancyCaster)
                {
                }
            }
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
        private void HandleLightWeaponCombo(WeaponItem weapon)
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

        private void HandleHeavyWeaponCombo(WeaponItem weapon)
        {
            if (playerStatsManager.currentStamina <= 0)
            {
                return;
            }
            if (inputHandler.comboFlag)
            {
                playerAnimatorManager.animator.SetBool("canDoCombo", false);
                if (lastAttack == OH_Heavy_Attack_1)
                {
                    playerAnimatorManager.PlayTargetAnimation(OH_Heavy_Attack_2, true);
                }
                else if (lastAttack == TH_Heavy_Attack_1)
                {
                    playerAnimatorManager.PlayTargetAnimation(TH_Heavy_Attack_2, true);
                }
            }
        }
        private void HandleLightAttack(WeaponItem weapon)
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
        private void HandleJumpingAttack(WeaponItem weapon)
        {
            if (playerStatsManager.currentStamina <= 0)
            {
                return;
            }
            playerWeaponSlotManager.attackingWeapon = weapon;
            if (inputHandler.twoHandFlag)
            {
                playerAnimatorManager.PlayTargetAnimation(TH_Jumping_Attack_01, true);
                lastAttack = TH_Jumping_Attack_01;
            }
            else
            {
                playerAnimatorManager.PlayTargetAnimation(OH_Jumping_Attack_01, true);
                lastAttack = OH_Jumping_Attack_01;
            }
        }
        private void HandleHeavyAttack(WeaponItem weapon)
        {
            if (playerStatsManager.currentStamina <= 0)
            {
                return;
            }
            playerWeaponSlotManager.attackingWeapon = weapon;
            if (inputHandler.twoHandFlag)
            {
                playerAnimatorManager.PlayTargetAnimation(TH_Heavy_Attack_1, true);
                lastAttack = TH_Heavy_Attack_1;
            }
            else
            {
                playerAnimatorManager.PlayTargetAnimation(OH_Heavy_Attack_1, true);
                lastAttack = OH_Heavy_Attack_1;
            }
        }
        private void HandleRunningAttack(WeaponItem weapon)
        {
            if (playerStatsManager.currentStamina <= 0)
            {
                return;
            }
            playerWeaponSlotManager.attackingWeapon = weapon;
            if (inputHandler.twoHandFlag)
            {
                playerAnimatorManager.PlayTargetAnimation(TH_Running_Attack_01, true);
                lastAttack = TH_Running_Attack_01;
            }
            else
            {
                playerAnimatorManager.PlayTargetAnimation(OH_Running_Attack_01, true);
                lastAttack = OH_Running_Attack_01;
            }
        }
        private void DrawArrowAction()
        {
            playerAnimatorManager.animator.SetBool("isHoldingArrow", true);
            playerAnimatorManager.PlayTargetAnimation("Bow_TH_Draw_01", true);
            GameObject loadedArrow = Instantiate(playerInventoryManager.currentAmmo.loadedItemModel, playerWeaponSlotManager.leftHandSlot.transform);
            Animator bowAnimator = playerWeaponSlotManager.rightHandSlot.GetComponentInChildren<Animator>();
            bowAnimator.SetBool("isDrawn", true);
            bowAnimator.Play("Bow_TH_Draw_01");
            playerEffectsManager.currentRangeFX = loadedArrow;
        }

        public void FireArrowAction()
        {
            ArrowInstantiationLocation arrowInstantiationLocation;
            arrowInstantiationLocation = playerWeaponSlotManager.rightHandSlot.GetComponentInChildren<ArrowInstantiationLocation>();

            Animator bowAnimator = playerWeaponSlotManager.rightHandSlot.GetComponentInChildren<Animator>();
            bowAnimator.SetBool("isDrawn", true);
            bowAnimator.Play("Bow_TH_Draw_01");
            Destroy(playerEffectsManager.currentRangeFX);

            playerAnimatorManager.PlayTargetAnimation("Bow_TH_Fire_01", true);
            playerAnimatorManager.animator.SetBool("isHoldingArrow", false);

            GameObject liveArrow = Instantiate(playerInventoryManager.currentAmmo.liveAmmoModel, arrowInstantiationLocation.transform.position,
                cameraHandler.cameraPivotTransform.rotation);
            Rigidbody rigidbody = liveArrow.GetComponentInChildren<Rigidbody>();
            RangedProjectileDamageCollider damageCollider = liveArrow.GetComponentInChildren<RangedProjectileDamageCollider>();

            if (cameraHandler.currentLockOnTarget != null)
            {
                Quaternion arrowRotation = Quaternion.LookRotation(transform.forward);
                liveArrow.transform.rotation = arrowRotation;
            }
            else
            {
                liveArrow.transform.rotation = Quaternion.Euler(cameraHandler.cameraPivotTransform.eulerAngles.x, playerManager.lockOnTransform.eulerAngles.y, 0);
            }
            rigidbody.AddForce(liveArrow.transform.forward * playerInventoryManager.currentAmmo.forwardVelocity);
            rigidbody.AddForce(liveArrow.transform.up * playerInventoryManager.currentAmmo.upwardVelocity);
            rigidbody.useGravity = playerInventoryManager.currentAmmo.useGravity;
            rigidbody.mass = playerInventoryManager.currentAmmo.ammoMass;
            liveArrow.transform.parent = null;

            damageCollider.characterManager = playerManager;
            damageCollider.ammoItem = playerInventoryManager.currentAmmo;
            damageCollider.physicalDamage = playerInventoryManager.currentAmmo.physicalDamage;




        }
        private void PerformRBMeleeAction()
        {
            playerAnimatorManager.animator.SetBool("IsUsingRightHand", true);

            if (playerManager.isSprinting)
            {
                HandleRunningAttack(playerInventoryManager.rightWeapon);
                return;
            }

            if (playerManager.canDoCombo)
            {
                inputHandler.comboFlag = true;
                HandleLightWeaponCombo(playerInventoryManager.rightWeapon);
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
                HandleLightAttack(playerInventoryManager.rightWeapon);
            }

            playerEffectsManager.PlayWeaponFX(false);
        }
        private void PerformRTMeleeAction()
        {
            playerAnimatorManager.animator.SetBool("IsUsingRightHand", true);

            if (playerManager.isSprinting)
            {
                HandleJumpingAttack(playerInventoryManager.rightWeapon);
                return;
            }

            if (playerManager.canDoCombo)
            {
                inputHandler.comboFlag = true;
                HandleHeavyWeaponCombo(playerInventoryManager.rightWeapon);
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
                HandleHeavyAttack(playerInventoryManager.rightWeapon);
            }

            playerEffectsManager.PlayWeaponFX(false);
        }
        private void PerformRBRangeAction()
        {
            if (playerStatsManager.currentStamina <= 0)
                return;
            playerAnimatorManager.EraseHandIKForWeapon();
            playerAnimatorManager.animator.SetBool("isUsingRightHand", true);
            if (!playerManager.isHoldingArrow)
            {
                if (playerInventoryManager.currentAmmo != null)
                {
                    DrawArrowAction();
                }
                else
                {
                    playerAnimatorManager.PlayTargetAnimation("Shrug_01", true);
                }
            }
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
        private void PerformLBAimingAction()
        {
            //playerAnimatorManager.animator.SetBool("isAiming", true);
        }
        private void PerformMagicAction(WeaponItem weapon, bool isLeftHanded)
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
                        playerInventoryManager.currentSpell.AttemptToCastSpell(playerAnimatorManager, playerStatsManager, playerWeaponSlotManager, isLeftHanded);
                    }
                }
            }
            else if (weapon.weaponType.Equals(WeaponType.PyromancyCaster))
            {
                if (playerInventoryManager.currentSpell != null && playerInventoryManager.currentSpell.isPyroSpell)
                {
                    if (playerStatsManager.currentFocusPoint >= playerInventoryManager.currentSpell.focusPointCost)
                    {
                        playerInventoryManager.currentSpell.AttemptToCastSpell(playerAnimatorManager, playerStatsManager, playerWeaponSlotManager, isLeftHanded);
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
            playerInventoryManager.currentSpell.SuccessfullyCastSpell(playerAnimatorManager, playerStatsManager, cameraHandler, playerWeaponSlotManager, playerManager.isUsingLeftHand);
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