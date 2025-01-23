using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHH
{
    public class PlayerLocomotionManager : MonoBehaviour
    {
        CameraHandler cameraHandler;
        PlayerManager player;
        PlayerStatsManager playerStatsManager;
        Transform cameraObject;
        InputHandler inputHandler;
        public Vector3 moveDirection;

        [HideInInspector]
        public Transform myTransform;
        [HideInInspector]
        public PlayerAnimatorManager playerAnimatorManager;

        public new Rigidbody rigidbody;
        public GameObject normalCamera;

        [Header("Ground and Air detection Stats")]
        [SerializeField] private float groundDetectionRayStartPoint = 0.5f;
        [SerializeField] float minimumDistanceNeededToBeginFall = 1f;
        [SerializeField] float groundDirectionRayDistance = 0.2f;
        public LayerMask groundLayer;
        public float inAirTimer;

        [Header("Movement Stats")]
        [SerializeField] float walkingSpeed = 1;
        [SerializeField] float movementSpeed = 5;
        [SerializeField] float sprintSpeed = 7;
        [SerializeField] float rotationSpeed = 10;
        [SerializeField] float fallingSpeed = 45;

        [Header("Stamina Costs")]
        [SerializeField] int rollStaminaCost = 15;
        [SerializeField] int backStepStaminaCost = 12;
        [SerializeField] int sprintStaminaCost = 1;

        public CapsuleCollider characterCollider;
        public CapsuleCollider characterCollisionBlockerCollider;
        private void Awake()
        {
            cameraHandler = FindObjectOfType<CameraHandler>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            player = GetComponent<PlayerManager>();
            rigidbody = GetComponent<Rigidbody>();
            inputHandler = GetComponent<InputHandler>();
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
        }

        void Start()
        {
            cameraObject = Camera.main.transform;
            myTransform = transform;

            player.isGrounded = true;
            Physics.IgnoreCollision(characterCollider, characterCollisionBlockerCollider, true);
        }

        #region Movement
        Vector3 normalVector;
        Vector3 targetPosition;

        public void HandleRotation()
        {
            if (player.canRotate)
            {
                if (player.isAiming)
                {
                    Quaternion targetRotation = Quaternion.Euler(0, cameraHandler.cameraTransform.eulerAngles.y, 0);
                    Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                    transform.rotation = playerRotation;
                }
                else
                {
                    if (inputHandler.lockOnFlag)
                    {
                        if (inputHandler.rollFlag || inputHandler.sprintFlag)
                        {
                            Vector3 targetDirection = Vector3.zero;
                            targetDirection = cameraHandler.cameraTransform.forward * inputHandler.vertical;
                            targetDirection += cameraHandler.cameraTransform.right * inputHandler.horizontal;

                            if (targetDirection == Vector3.zero)
                            {
                                targetDirection = transform.forward;
                            }

                            Quaternion tr = Quaternion.LookRotation(targetDirection);
                            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);

                            transform.rotation = targetRotation;
                        }
                        else
                        {
                            Vector3 rotationDirection = moveDirection;
                            rotationDirection = cameraHandler.currentLockOnTarget.transform.position - transform.position;
                            rotationDirection.y = 0;
                            rotationDirection.Normalize();
                            Quaternion tr = Quaternion.LookRotation(rotationDirection);
                            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);
                            transform.rotation = targetRotation;
                        }
                    }
                    else
                    {
                        Vector3 targetDir = Vector3.zero;
                        float moveOverride = inputHandler.moveAmount;

                        targetDir = cameraObject.forward * inputHandler.vertical;
                        targetDir += cameraObject.right * inputHandler.horizontal;

                        targetDir.Normalize();
                        targetDir.y = 0;

                        if (targetDir == Vector3.zero)
                        {
                            targetDir = myTransform.forward;
                        }

                        float rs = rotationSpeed;

                        Quaternion tr = Quaternion.LookRotation(targetDir);
                        Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * Time.deltaTime);

                        myTransform.rotation = targetRotation;
                    }
                }
            }
        }

        public void HandleMovement()
        {
            if (inputHandler.rollFlag)
            {
                return;
            }
            if (player.isInteracting)
            {
                return;
            }

            moveDirection = cameraObject.forward * inputHandler.vertical;
            moveDirection += cameraObject.right * inputHandler.horizontal;
            moveDirection.Normalize();
            moveDirection.y = 0;

            float speed = movementSpeed;

            if (inputHandler.sprintFlag && inputHandler.moveAmount > 0.5f)
            {
                speed = sprintSpeed;
                player.isSprinting = true;
                moveDirection *= speed;
                playerStatsManager.TakeDamage(sprintStaminaCost, 0, "Damage_01");
            }
            else
            {
                if (inputHandler.moveAmount <= 0.5f)
                {
                    moveDirection *= walkingSpeed;
                    player.isSprinting = false;
                }
                else
                {
                    moveDirection *= speed;
                    player.isSprinting = false;
                }
            }

            Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
            rigidbody.velocity = projectedVelocity;

            if (inputHandler.lockOnFlag && inputHandler.sprintFlag == false)
            {
                playerAnimatorManager.UpdateAnimatorValues(inputHandler.vertical, inputHandler.horizontal, player.isSprinting);
            }
            else
            {
                playerAnimatorManager.UpdateAnimatorValues(inputHandler.moveAmount, 0, player.isSprinting);
            }
        }

        public void HandleRollingAndSprinting()
        {
            if (player.animator.GetBool("isInteracting"))
            {
                return;
            }

            if (playerStatsManager.currentStamina <= 0)
            {
                return;
            }

            if (inputHandler.rollFlag)
            {
                inputHandler.rollFlag = false;
                moveDirection = cameraObject.forward * inputHandler.vertical;
                moveDirection += cameraObject.right * inputHandler.horizontal;

                if (inputHandler.moveAmount > 0)
                {
                    playerAnimatorManager.PlayTargetAnimation("Rolling", true);
                    playerAnimatorManager.EraseHandIKForWeapon();
                    moveDirection.y = 0;
                    Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                    myTransform.rotation = rollRotation;
                    playerStatsManager.TakeStaminaDamage(rollStaminaCost);
                }
                else
                {
                    playerAnimatorManager.PlayTargetAnimation("Backstep", true);
                    playerAnimatorManager.EraseHandIKForWeapon();

                    playerStatsManager.TakeStaminaDamage(backStepStaminaCost);

                }
            }
        }

        public void HandleFalling(Vector3 direction)
        {
            player.isGrounded = false;
            RaycastHit hit;
            Vector3 origin = myTransform.position;
            origin.y += groundDetectionRayStartPoint;

            if (Physics.Raycast(origin, myTransform.forward, out hit, 0.4f))
            {
                moveDirection = Vector3.zero;
            }

            if (player.isInAir)
            {
                rigidbody.AddForce(-Vector3.up * fallingSpeed);
                rigidbody.AddForce(moveDirection * fallingSpeed / 5f);
            }

            Vector3 dir = moveDirection;
            dir.Normalize();
            origin += dir * groundDirectionRayDistance;

            targetPosition = myTransform.position;

            Debug.DrawRay(origin, -Vector3.up * minimumDistanceNeededToBeginFall, Color.red, 0.1f, false);
            if (Physics.Raycast(origin, -Vector3.up, out hit, minimumDistanceNeededToBeginFall, groundLayer))
            {
                normalVector = hit.normal;
                Vector3 tp = hit.point;
                player.isGrounded = true;
                targetPosition.y = tp.y;

                if (player.isInAir)
                {
                    if (inAirTimer > 0.5f)
                    {
                        playerAnimatorManager.PlayTargetAnimation("Landing", true);
                        inAirTimer = 0f;
                    }
                    else
                    {
                        playerAnimatorManager.PlayTargetAnimation("Empty", false);
                        inAirTimer = 0f;
                    }

                    player.isInAir = false;
                }
            }
            else
            {
                if (player.isGrounded)
                {
                    player.isGrounded = false;
                }

                if (player.isInAir == false)
                {
                    if (player.isInteracting == false && !inputHandler.rollFlag)
                    {
                        playerAnimatorManager.PlayTargetAnimation("Falling", true);
                    }

                    Vector3 vel = rigidbody.velocity;
                    vel.Normalize();
                    rigidbody.velocity = vel * (movementSpeed / 2);
                    player.isInAir = true;
                }
            }

            if (player.isGrounded)
            {
                if (player.isInteracting || inputHandler.moveAmount > 0)
                {
                    myTransform.position = Vector3.Lerp(myTransform.position, targetPosition, Time.deltaTime);
                }
                else
                {
                    myTransform.position = targetPosition;
                }
            }
        }

        public void HandleJumping()
        {
            if (player.isInteracting)
            {
                return;
            }
            if (playerStatsManager.currentStamina <= 0)
            {
                return;
            }
            if (inputHandler.jump_Input)
            {
                if (inputHandler.moveAmount > 0)
                {
                    moveDirection = cameraObject.forward * inputHandler.vertical;
                    moveDirection += cameraObject.right * inputHandler.horizontal;
                    moveDirection.Normalize();
                    //moveDirection.y = 0;

                    playerAnimatorManager.PlayTargetAnimation("Jump", true);
                    playerAnimatorManager.EraseHandIKForWeapon();

                    Quaternion jumpRotation = Quaternion.LookRotation(moveDirection);
                    myTransform.rotation = jumpRotation;
                }
            }
        }
        #endregion
    }
}
