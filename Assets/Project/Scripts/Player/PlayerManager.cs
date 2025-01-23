using PHH;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PHH
{
    public class PlayerManager : CharacterManager
    {
        [Header("Camera")]
        public CameraHandler cameraHandler;

        [Header("Input")]
        public InputHandler inputHandler;

        [Header("UI")]
        public UIManager uiManager;

        [Header("Player")]
        public PlayerLocomotionManager playerLocomotion;
        public PlayerInventoryManager playerInventoryManager;
        public PlayerAnimatorManager playerAnimatorManager;
        public PlayerStatsManager playerStatsManager;
        public PlayerWeaponSlotManager playerWeaponSlotManager;
        public PlayerCombatManager playerCombatManager;
        public PlayerEffectsManager playerEffectsManager;
        public PlayerEquipmentManager PlayerEquipmentManager;

        [Header("Colliders")]
        public BlockingCollider blockingCollider;

        [Header("Interactable")]
        public InteractableUI interactableUI;
        public GameObject interactableUIGameObject;
        public GameObject itemInteractableGameObject;


        protected override void Awake()
        {
            base.Awake();
            inputHandler = GetComponent<InputHandler>();
            animator = GetComponent<Animator>();
            uiManager = FindObjectOfType<UIManager>();
            playerLocomotion = GetComponent<PlayerLocomotionManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            interactableUI = FindObjectOfType<InteractableUI>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
            playerCombatManager = GetComponent<PlayerCombatManager>();
            playerEffectsManager = GetComponent<PlayerEffectsManager>();
            PlayerEquipmentManager = GetComponent<PlayerEquipmentManager>();
        }


        void Start()
        {
            cameraHandler = CameraHandler.singleton;
        }

        // Update is called once per frame
        void Update()
        {
            float delta = Time.deltaTime;
            isInteracting = animator.GetBool("isInteracting");
            canDoCombo = animator.GetBool("canDoCombo");
            canRotate = animator.GetBool("canRotate");
            isInvulnerable = animator.GetBool("isInvulnerable");
            isFiringSpell = animator.GetBool("isFiringSpell");
            isHoldingArrow = animator.GetBool("isHoldingArrow");
            animator.SetBool("isTwoHandingWeapon", isTwoHandingWeapon);
            animator.SetBool("isInAir", isInAir);
            animator.SetBool("isDead", isDead);
            animator.SetBool("isBlocking", isBlocking);


            inputHandler.TickInput();
            playerLocomotion.HandleFalling(playerLocomotion.moveDirection);
            playerLocomotion.HandleRollingAndSprinting();
            playerLocomotion.HandleJumping();
            playerStatsManager.RegenerateStamina();

            CheckForInteractableObject();

        }
        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            playerLocomotion.HandleFalling(playerLocomotion.moveDirection);
            playerLocomotion.HandleMovement();
            playerLocomotion.HandleRotation();

            playerEffectsManager.HandleBuildUpEffects();
        }
        private void LateUpdate()
        {
            inputHandler.d_Pad_Up = false;
            inputHandler.d_Pad_Down = false;
            inputHandler.d_Pad_Left = false;
            inputHandler.d_Pad_Right = false;
            inputHandler.a_Input = false;
            inputHandler.inventory_Input = false;

            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget();
                cameraHandler.HandleCameraRotation();
            }

            if (isInAir)
            {
                playerLocomotion.inAirTimer += Time.deltaTime;
            }
        }
        #region Player Interaction
        public void CheckForInteractableObject()
        {
            RaycastHit hit;
            if (Physics.SphereCast(transform.position, 0.3f, transform.forward, out hit, 1f, cameraHandler.ignoreLayers))
            {
                if (hit.collider.tag == "Interactable")
                {
                    Interactable interactableObject = hit.collider.GetComponent<Interactable>();
                    if (interactableObject != null)
                    {
                        string interactableText = interactableObject.interactableText;
                        interactableUI.interactableText.text = interactableText;
                        interactableUIGameObject.SetActive(true);
                        if (inputHandler.a_Input)
                        {
                            hit.collider.GetComponent<Interactable>().Interact(this);
                        }
                    }
                }
            }
            else
            {
                if (interactableUIGameObject != null)
                {
                    interactableUIGameObject.SetActive(false);
                }
                if (itemInteractableGameObject != null && inputHandler.a_Input)
                {
                    itemInteractableGameObject.SetActive(false);
                }
            }
        }
        public void OpenChestInteraction(Transform playerOpenChestStandingPoint)
        {
            playerLocomotion.rigidbody.velocity = Vector3.zero;
            transform.position = playerOpenChestStandingPoint.transform.position;
            playerAnimatorManager.PlayTargetAnimation("Pick Up Item", true);

        }

        public void PassThroughFogWallInteraction(Transform fogWallEntrance)
        {
            playerLocomotion.rigidbody.velocity = Vector3.zero;

            Vector3 rotationDirection = fogWallEntrance.transform.forward;
            Quaternion turnRotation = Quaternion.LookRotation(rotationDirection);
            transform.rotation = turnRotation;

            playerAnimatorManager.PlayTargetAnimation("Pass Through Fog", true);
        }
        #endregion
    }
}
