using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHH
{
    public class PlayerWeaponSlotManager : CharacterWeaponSlotManager
    {
        QuickSlotsUI quickSlotsUI;
        InputHandler inputHandler;
        PlayerManager player;
        PlayerInventoryManager playerInventoryManager;
        PlayerStatsManager playerStatsManager;
        PlayerEffectsManager playerEffectsManager;
        PlayerAnimatorManager playerAnimatorManager;
        CameraHandler cameraHandler;

        protected override void Awake()
        {
            base.Awake();
            cameraHandler = FindObjectOfType<CameraHandler>();
            quickSlotsUI = FindObjectOfType<QuickSlotsUI>();
            inputHandler = GetComponent<InputHandler>();
            player = GetComponent<PlayerManager>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerEffectsManager = GetComponent<PlayerEffectsManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        }

        public override void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
        {
            if (weaponItem != null)
            {
                if (isLeft)
                {
                    leftHandSlot.currentWeapon = weaponItem;
                    leftHandSlot.LoadWeaponModel(weaponItem);
                    LoadLeftWeaponDamageCollider();
                    quickSlotsUI.UpdateWeaponQuickSlotsUI(true, weaponItem);
                    playerAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);
                }
                else
                {
                    if (inputHandler.twoHandFlag)
                    {
                        backSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                        leftHandSlot.UnloadWeaponAndDestroy();
                        playerAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);

                    }
                    else
                    {
                        backSlot.UnloadWeaponAndDestroy();
                    }
                    rightHandSlot.currentWeapon = weaponItem;
                    rightHandSlot.LoadWeaponModel(weaponItem);
                    LoadRightWeaponDamageCollider();
                    quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);
                    player.animator.runtimeAnimatorController = weaponItem.weaponController;
                }
            }
            else
            {
                weaponItem = unarmedWeapon;
                if (isLeft)
                {
                    playerInventoryManager.leftWeapon = unarmedWeapon;
                    leftHandSlot.currentWeapon = weaponItem;
                    leftHandSlot.LoadWeaponModel(weaponItem);
                    LoadLeftWeaponDamageCollider();
                    quickSlotsUI.UpdateWeaponQuickSlotsUI(true, weaponItem);
                    playerAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);

                }
                else
                {
                    playerInventoryManager.rightWeapon = unarmedWeapon;
                    rightHandSlot.currentWeapon = weaponItem;
                    rightHandSlot.LoadWeaponModel(weaponItem);
                    LoadRightWeaponDamageCollider();
                    quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);
                    player.animator.runtimeAnimatorController = weaponItem.weaponController;

                }
            }
        }
        public void SuccessfullyThrowingBomb()
        {
            Destroy(playerEffectsManager.instantiatedFXModel);
            BombConsumableItem fireBombItem = playerInventoryManager.currentConsumable as BombConsumableItem;

            GameObject activeBombModel = Instantiate(fireBombItem.liveBombModel, rightHandSlot.transform.position,
                cameraHandler.cameraPivotTransform.rotation);
            activeBombModel.transform.rotation = Quaternion.Euler(cameraHandler.cameraPivotTransform.eulerAngles.x,
                player.lockOnTransform.eulerAngles.y, 0);
            BombDamageCollider damageCollider = activeBombModel.GetComponentInChildren<BombDamageCollider>();

            damageCollider.explosionDamage = fireBombItem.baseDamage;
            damageCollider.explosionSplashDamage = fireBombItem.explosiveDamage;
            damageCollider.bombRigidbody.AddForce(activeBombModel.transform.forward * fireBombItem.forwadVelocity);
            damageCollider.bombRigidbody.AddForce(activeBombModel.transform.up * fireBombItem.upwardVelocity);
            damageCollider.teamIDNumber = playerStatsManager.teamIDNumber;
            LoadWeaponOnSlot(playerInventoryManager.rightWeapon, false);
        }

    }
}
