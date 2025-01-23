using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHH
{
    public class InputHandler : MonoBehaviour
    {
        PlayerControls inputActions;
        PlayerManager player;

        public float horizontal;
        public float vertical;
        public float moveAmount;
        public float mouseX;
        public float mouseY;

        public bool b_Input;
        public bool a_Input;
        public bool x_Input;
        public bool y_Input;


        public bool tap_rb_Input;
        public bool hold_Rb_Input;
        public bool tap_rt_Input;

        public bool lb_Input;
        public bool tap_Lb_Input;
        public bool tap_lt_Input;


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

        Vector2 movementInput;
        Vector2 cameraInput;

        private void Awake()
        {
            player = GetComponent<PlayerManager>();
        }

        public void OnEnable()
        {
            if (inputActions == null)
            {
                inputActions = new PlayerControls();
                inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
                inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
                inputActions.PlayerActions.RB.performed += i => tap_rb_Input = true;
                inputActions.PlayerActions.HoldRb.performed += i => hold_Rb_Input = true;
                inputActions.PlayerActions.HoldRb.canceled += i => hold_Rb_Input = false;


                inputActions.PlayerActions.RT.performed += i => tap_rt_Input = true;

                inputActions.PlayerActions.TapLB.performed += i => tap_Lb_Input = true;
                inputActions.PlayerActions.LB.performed += i => lb_Input = true;
                inputActions.PlayerActions.LB.canceled += i => lb_Input = false;
                inputActions.PlayerActions.LT.performed += i => tap_lt_Input = true;
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

        public void TickInput()
        {
            if (player.isDead) return;
            HandleMovementInput();
            HandleRollInput();

            HandleHoldLBInput();
            HandleHoldRBInput();

            HandleTapLBInput();
            HandleTapRBInput();
            HandleTapRTInput();
            HandleTapLTInput();

            HandleQuickSlotInput();
            HandleInventoryInput();
            HandleLockOnInput();
            HandleTwoHandInput();
            HandleUseConsumableInput();
        }

        private void HandleMovementInput()
        {
            if (player.isHoldingArrow)
            {
                horizontal = movementInput.x;
                vertical = movementInput.y;
                moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));

                if (moveAmount > 0.5f)
                {
                    moveAmount = 0.5f;
                }
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
        private void HandleRollInput()
        {
            if (b_Input)
            {
                rollInputTimer += Time.deltaTime;
                if (player.playerStatsManager.currentStamina <= 0)
                {
                    b_Input = false;
                    sprintFlag = false;
                }

                if (moveAmount > 0.5f && player.playerStatsManager.currentStamina > 0)
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
        private void HandleTapRBInput()
        {
            //RB for the right hand
            if (tap_rb_Input)
            {
                tap_rb_Input = false;

                if (player.playerInventoryManager.rightWeapon.tap_RB_Action != null)
                {
                    player.UpdateWhichHandCharacterIsUsing(true);
                    player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                    player.playerInventoryManager.rightWeapon.tap_RB_Action.PerformAction(player);
                }
            }
        }
        private void HandleHoldRBInput()
        {
            //RB for the right hand
            if (hold_Rb_Input)
            {
                if (player.playerInventoryManager.rightWeapon.hold_RB_Action != null)
                {
                    player.UpdateWhichHandCharacterIsUsing(true);
                    player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                    player.playerInventoryManager.rightWeapon.hold_RB_Action.PerformAction(player);
                }
            }
        }
        private void HandleTapRTInput()
        {
            if (tap_rt_Input)
            {
                tap_rt_Input = false;
                if (player.playerInventoryManager.rightWeapon.tap_RT_Action != null)
                {
                    player.UpdateWhichHandCharacterIsUsing(true);
                    player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                    player.playerInventoryManager.rightWeapon.tap_RT_Action.PerformAction(player);
                }
            }
        }
        private void HandleTapLTInput()
        {
            if (tap_lt_Input)
            {
                tap_lt_Input = false;
                if (player.isTwoHandingWeapon)
                {
                    if (player.playerInventoryManager.rightWeapon.tap_LT_Action != null)
                    {
                        player.UpdateWhichHandCharacterIsUsing(true);
                        player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;

                        player.playerInventoryManager.rightWeapon.tap_LT_Action.PerformAction(player);
                    }
                }
                else
                {
                    if (player.playerInventoryManager.rightWeapon.tap_LT_Action != null)
                    {
                        player.UpdateWhichHandCharacterIsUsing(false);
                        player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.leftWeapon;
                        player.playerInventoryManager.leftWeapon.tap_LT_Action.PerformAction(player);
                    }
                }
            }
        }
        private void HandleTapLBInput()
        {
            if (tap_Lb_Input)
            {
                tap_Lb_Input = false;
                if (player.isTwoHandingWeapon)
                {
                    if (player.playerInventoryManager.rightWeapon.tap_LB_Action != null)
                    {
                        player.UpdateWhichHandCharacterIsUsing(true);
                        player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                        player.playerInventoryManager.rightWeapon.tap_LB_Action.PerformAction(player);
                    }
                }
                else
                {
                    if (player.playerInventoryManager.rightWeapon.tap_LB_Action != null)
                    {
                        player.UpdateWhichHandCharacterIsUsing(false);
                        player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.leftWeapon;
                        player.playerInventoryManager.leftWeapon.tap_LB_Action.PerformAction(player);
                    }
                }
            }
        }
        private void HandleHoldLBInput()
        {
            if (player.isInAir || player.isSprinting || player.isFiringSpell)
            {
                lb_Input = false;
                return;
            }
            if (lb_Input)
            {
                if (player.isTwoHandingWeapon)
                {
                    player.UpdateWhichHandCharacterIsUsing(true);
                    player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                    player.playerInventoryManager.rightWeapon.hold_LB_Action.PerformAction(player);
                }
                else
                {
                    player.UpdateWhichHandCharacterIsUsing(false);
                    player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.leftWeapon;
                    player.playerInventoryManager.leftWeapon.hold_LB_Action.PerformAction(player);
                }
            }
            else if (lb_Input == false)
            {
                if (player.isAiming)
                {
                    player.isAiming = false;
                    player.uiManager.crossHair.SetActive(false);
                    player.cameraHandler.ResetAimCameraRotation();
                }
                if (player.blockingCollider.blockingCollider.enabled)
                {
                    player.isBlocking = false;
                    player.blockingCollider.DisableBlockingCollider();
                }
            }
        }
        private void HandleQuickSlotInput()
        {
            if (d_Pad_Right)
            {
                player.playerInventoryManager.ChangeRightWeapon();
            }
            else if (d_Pad_Left)
            {
                player.playerInventoryManager.ChangeLeftWeapon();
            }
        }
        private void HandleInventoryInput()
        {
            if (inventory_Input)
            {
                inventoryFlag = !inventoryFlag;
                player.uiManager.ToggleSelectWindow(inventoryFlag);
                player.uiManager.hudWindow.SetActive(!inventoryFlag);
                if (inventoryFlag)
                {
                    player.uiManager.UpdateUI();
                }
                if (!inventoryFlag)
                {
                    player.uiManager.CloseAllInventoryWindow();
                }
            }
        }
        private void HandleLockOnInput()
        {
            if (lockOn_Input && lockOnFlag == false)
            {
                lockOn_Input = false;
                player.cameraHandler.HandleLockOn();
                if (player.cameraHandler.nearestLockOnTarget != null)
                {
                    player.cameraHandler.currentLockOnTarget = player.cameraHandler.nearestLockOnTarget;
                    lockOnFlag = true;
                }
            }
            else if (lockOn_Input && lockOnFlag)
            {
                lockOn_Input = false;
                lockOnFlag = false;
                player.cameraHandler.ClearLockOnTargets();
            }

            if (lockOnFlag && right_Stick_Left_Input)
            {
                right_Stick_Left_Input = false;
                player.cameraHandler.HandleLockOn();
                if (player.cameraHandler.leftLockTarget != null)
                {
                    player.cameraHandler.currentLockOnTarget = player.cameraHandler.leftLockTarget;
                }
            }

            if (lockOnFlag && right_Stick_Right_Input)
            {
                right_Stick_Right_Input = false;
                player.cameraHandler.HandleLockOn();
                if (player.cameraHandler.rightLockTarget != null)
                {
                    player.cameraHandler.currentLockOnTarget = player.cameraHandler.rightLockTarget;
                }
            }

            player.cameraHandler.SetCameraHeight();
        }
        private void HandleTwoHandInput()
        {
            if (y_Input)
            {
                y_Input = false;
                twoHandFlag = !twoHandFlag;
                if (twoHandFlag)
                {
                    player.isTwoHandingWeapon = true;
                    player.playerWeaponSlotManager.LoadWeaponOnSlot(player.playerInventoryManager.rightWeapon, false);
                    player.playerWeaponSlotManager.LoadTwoHandIKTarget(true);
                }
                else
                {
                    player.isTwoHandingWeapon = false;
                    player.playerWeaponSlotManager.LoadWeaponOnSlot(player.playerInventoryManager.rightWeapon, false);
                    player.playerWeaponSlotManager.LoadWeaponOnSlot(player.playerInventoryManager.leftWeapon, true);
                    player.playerWeaponSlotManager.LoadTwoHandIKTarget(false);

                }
            }
        }
        private void HandleUseConsumableInput()
        {
            if (x_Input)
            {
                x_Input = false;
                player.playerInventoryManager.currentConsumable.AttemptToConsumeItem(player.playerAnimatorManager, player.playerWeaponSlotManager, player.playerEffectsManager);
            }
        }
    }
}
