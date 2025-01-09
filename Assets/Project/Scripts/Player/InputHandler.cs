using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHH
{
    public class InputHandler : MonoBehaviour
    {
        public float horizontal;
        public float vertical;
        public float moveAmount;
        public float mouseX;
        public float mouseY;

        public bool b_Input;
        public bool a_Input;
        public bool x_Input;
        public bool y_Input;


        public bool rb_Input;
        public bool hold_Rb_Input;
        public bool rt_Input;
        public bool lb_Input;
        public bool lt_Input;


        public bool jump_Input;
        public bool inventory_Input;
        public bool lockOn_Input;
        public bool right_Stick_Right_Input;
        public bool right_Stick_Left_Input;

        public bool d_Pad_Up;
        public bool d_Pad_Down;
        public bool d_Pad_Left;
        public bool d_Pad_Right;


        public bool rollFlag;
        public bool twoHandFlag;
        public bool sprintFlag;
        public bool comboFlag;
        public bool lockOnFlag;
        public bool fireFlag;
        public bool inventoryFlag;
        public float rollInputTimer;

        public Transform criticalAttackRayCastStartPoint;

        PlayerControls inputActions;
        PlayerCombatManager playerCombatManager;
        PlayerInventoryManager playerInventoryManager;
        PlayerManager playerManager;
        PlayerAnimatorManager playerAnimatorManager;
        PlayerEffectsManager playerEffectsManager;
        PlayerStatsManager playerStatsManager;
        BlockingCollider blockingCollider;
        UIManager uiManager;
        CameraHandler cameraHandler;
        PlayerWeaponSlotManager weaponSlotManager;

        Vector2 movementInput;
        Vector2 cameraInput;

        private void Awake()
        {
            playerCombatManager = GetComponent<PlayerCombatManager>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerManager = GetComponent<PlayerManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerEffectsManager = GetComponent<PlayerEffectsManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            weaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
            blockingCollider = GetComponentInChildren<BlockingCollider>();
            uiManager = FindObjectOfType<UIManager>();
            cameraHandler = FindObjectOfType<CameraHandler>();
        }

        public void OnEnable()
        {
            if (inputActions == null)
            {
                inputActions = new PlayerControls();
                inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
                inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
                inputActions.PlayerActions.RB.performed += i => rb_Input = true;
                inputActions.PlayerActions.HoldRb.performed += i => hold_Rb_Input = true;
                inputActions.PlayerActions.HoldRb.canceled += i => hold_Rb_Input = false;
                inputActions.PlayerActions.HoldRb.canceled += i => fireFlag = true;


                inputActions.PlayerActions.RT.performed += i => rt_Input = true;
                inputActions.PlayerActions.LB.performed += i => lb_Input = true;
                inputActions.PlayerActions.LB.canceled += i => lb_Input = false;
                inputActions.PlayerActions.LT.performed += i => lt_Input = true;
                inputActions.PlayerQuickSlots.DPadRight.performed += i => d_Pad_Right = true;
                inputActions.PlayerQuickSlots.DPadLeft.performed += i => d_Pad_Left = true;
                inputActions.PlayerActions.A.performed += i => a_Input = true;
                inputActions.PlayerActions.Roll.performed += i => b_Input = true;
                inputActions.PlayerActions.X.performed += i => x_Input = true;

                inputActions.PlayerActions.Roll.canceled += i => b_Input = false;
                inputActions.PlayerActions.Jump.performed += i => jump_Input = true;
                inputActions.PlayerActions.Inventory.performed += i => inventory_Input = true;
                inputActions.PlayerActions.LockOn.performed += i => lockOn_Input = true;
                inputActions.PlayerMovement.LockOnTargetRight.performed += i => right_Stick_Right_Input = true;
                inputActions.PlayerMovement.LockOnTargetLeft.performed += i => right_Stick_Left_Input = true;
                inputActions.PlayerActions.Y.performed += i => y_Input = true;
            }

            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        public void TickInput(float delta)
        {
            if (playerStatsManager.isDead) return;
            HandleMovementInput(delta);
            HandleRollInput(delta);
            HandleLBInput();
            HandleCombatInputs(delta);
            HandleQuickSlotInput();
            HandleInventoryInput();
            HandleLockOnInput();
            HandleTwoHandInput();
            HandleUseConsumableInput();
            HandleHoldRBInput();
            HandleFireBowInput();
        }

        private void HandleMovementInput(float delta)
        {
            if (playerManager.isHoldingArrow)
            {
                horizontal = movementInput.x;
                vertical = movementInput.y;
                moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical) / 2);
                mouseX = cameraInput.x;
                mouseY = cameraInput.y;
            }
            else
            {
                horizontal = movementInput.x;
                vertical = movementInput.y;
                moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
                mouseX = cameraInput.x;
                mouseY = cameraInput.y;
            }
        }

        private void HandleRollInput(float delta)
        {
            if (b_Input)
            {
                rollInputTimer += delta;
                if (playerStatsManager.currentStamina <= 0)
                {
                    b_Input = false;
                    sprintFlag = false;
                }

                if (moveAmount > 0.5f && playerStatsManager.currentStamina > 0)
                {
                    sprintFlag = true;
                }
            }
            else
            {
                sprintFlag = false;
                if (rollInputTimer > 0 && rollInputTimer < 0.5f)
                {
                    rollFlag = true;
                }

                rollInputTimer = 0;
            }
        }

        private void HandleCombatInputs(float delta)
        {
            //RB for the right hand
            if (rb_Input)
            {
                playerCombatManager.HandleRBAction();
            }

            if (rt_Input)
            {
                playerCombatManager.HandleRTAction();
            }

            if (lt_Input)
            {
                if (twoHandFlag)
                {

                }
                else
                {
                    playerCombatManager.HandleLTAction();
                }
            }
        }

        private void HandleLBInput()
        {
            if (playerManager.isInAir || playerManager.isSprinting || playerManager.isFiringSpell)
            {
                lb_Input = false;
                return;
            }
            if (lb_Input)
            {
                playerCombatManager.HandleLBAction();
            }
            else if (lb_Input == false)
            {
                playerManager.isBlocking = false;
                if (blockingCollider.blockingCollider.enabled)
                {
                    blockingCollider.DisableBlockingCollider();
                }
                if (playerManager.isHoldingArrow)
                {
                    //playerAnimatorManager.animator.SetBool("isAiming", false);
                }
            }
        }

        private void HandleQuickSlotInput()
        {
            if (d_Pad_Right)
            {
                playerInventoryManager.ChangeRightWeapon();
            }
            else if (d_Pad_Left)
            {
                playerInventoryManager.ChangeLeftWeapon();
            }
        }

        private void HandleInventoryInput()
        {
            if (inventory_Input)
            {
                inventoryFlag = !inventoryFlag;
                uiManager.ToggleSelectWindow(inventoryFlag);
                uiManager.hudWindow.SetActive(!inventoryFlag);
                if (inventoryFlag)
                {
                    uiManager.UpdateUI();
                }
                if (!inventoryFlag)
                {
                    uiManager.CloseAllInventoryWindow();
                }
            }
        }
        private void HandleLockOnInput()
        {
            if (lockOn_Input && lockOnFlag == false)
            {
                lockOn_Input = false;
                cameraHandler.HandleLockOn();
                if (cameraHandler.nearestLockOnTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.nearestLockOnTarget;
                    lockOnFlag = true;
                }
            }
            else if (lockOn_Input && lockOnFlag)
            {
                lockOn_Input = false;
                lockOnFlag = false;
                cameraHandler.ClearLockOnTargets();
            }

            if (lockOnFlag && right_Stick_Left_Input)
            {
                right_Stick_Left_Input = false;
                cameraHandler.HandleLockOn();
                if (cameraHandler.leftLockTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.leftLockTarget;
                }
            }

            if (lockOnFlag && right_Stick_Right_Input)
            {
                right_Stick_Right_Input = false;
                cameraHandler.HandleLockOn();
                if (cameraHandler.rightLockTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.rightLockTarget;
                }
            }

            cameraHandler.SetCameraHeight();
        }
        private void HandleTwoHandInput()
        {
            if (y_Input)
            {
                y_Input = false;
                twoHandFlag = !twoHandFlag;
                if (twoHandFlag)
                {
                    playerManager.isTwoHandingWeapon = true;
                    weaponSlotManager.LoadWeaponOnSlot(playerInventoryManager.rightWeapon, false);
                    weaponSlotManager.LoadTwoHandIKTarget(true);
                }
                else
                {
                    playerManager.isTwoHandingWeapon = false;
                    weaponSlotManager.LoadWeaponOnSlot(playerInventoryManager.rightWeapon, false);
                    weaponSlotManager.LoadWeaponOnSlot(playerInventoryManager.leftWeapon, true);
                    weaponSlotManager.LoadTwoHandIKTarget(false);

                }
            }
        }

        private void HandleHoldRBInput()
        {
            if (hold_Rb_Input)
            {
                if (playerInventoryManager.rightWeapon.weaponType.Equals(WeaponType.Bow))
                {
                    //Action
                    playerCombatManager.HandleHoldRBAction();
                }
                else
                {
                    hold_Rb_Input = false;
                    playerCombatManager.AttemptBackStabOrRiposte();
                }
            }
        }
        private void HandleFireBowInput()
        {
            if (fireFlag)
            {
                if (playerManager.isHoldingArrow)
                {
                    fireFlag = false;
                    playerCombatManager.FireArrowAction();
                }
            }
        }

        private void HandleUseConsumableInput()
        {
            if (x_Input)
            {
                x_Input = false;
                playerInventoryManager.currentConsumable.AttemptToConsumeItem(playerAnimatorManager, weaponSlotManager, playerEffectsManager);
            }
        }
    }
}
