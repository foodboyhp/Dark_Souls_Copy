using PHH;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PHH
{
    public class PlayerManager : CharacterManager
    {
        InputHandler inputHandler;
        Animator animator;
        CameraHandler cameraHandler;
        PlayerLocomotionManager playerLocomotion;
        PlayerAnimatorManager playerAnimatorManager;
        PlayerStatsManager playerStatsManager;
        PlayerEffectsManager playerEffectsManager;

        InteractableUI interactableUI;
        public GameObject interactableUIGameObject;
        public GameObject itemInteractableGameObject;

        private void Awake()
        {
            inputHandler = GetComponent<InputHandler>();
            animator = GetComponent<Animator>();
            playerLocomotion = GetComponent<PlayerLocomotionManager>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            interactableUI = FindObjectOfType<InteractableUI>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerEffectsManager = GetComponent<PlayerEffectsManager>();
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
            isUsingLeftHand = animator.GetBool("IsUsingLeftHand");
            isUsingRightHand = animator.GetBool("IsUsingRightHand");
            isInvulnerable = animator.GetBool("isInvulnerable");
            isFiringSpell = animator.GetBool("isFiringSpell");
            animator.SetBool("isTwoHandingWeapon", isTwoHandingWeapon);
            animator.SetBool("isInAir", isInAir);
            animator.SetBool("isDead", playerStatsManager.isDead);
            animator.SetBool("isBlocking", isBlocking);


            inputHandler.TickInput(delta);
            playerAnimatorManager.canRotate = animator.GetBool("canRotate");
            playerLocomotion.HandleRollingAndSprinting(delta);
            playerLocomotion.HandleJumping();
            playerStatsManager.RegenerateStamina();

            CheckForInteractableObject();

        }
        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;
            playerLocomotion.HandleMovement(delta);
            playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);
            playerLocomotion.HandleRotation(delta);

            playerEffectsManager.HandleBuildUpEffects();
        }
        private void LateUpdate()
        {
            inputHandler.rollFlag = false;
            inputHandler.rb_Input = false;
            inputHandler.rt_Input = false;
            inputHandler.lt_Input = false;
            inputHandler.d_Pad_Up = false;
            inputHandler.d_Pad_Down = false;
            inputHandler.d_Pad_Left = false;
            inputHandler.d_Pad_Right = false;
            inputHandler.a_Input = false;
            inputHandler.jump_Input = false;
            inputHandler.inventory_Input = false;

            float delta = Time.deltaTime;

            if (cameraHandler != null)
            {
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandlerCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
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
