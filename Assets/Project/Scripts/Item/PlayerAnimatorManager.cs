using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHH
{
    public class PlayerAnimatorManager : CharacterAnimatorManager
    {
        InputHandler inputHandler;
        PlayerLocomotionManager playerLocomotion;
        int vertical;
        int horizontal;
        protected override void Awake()
        {
            base.Awake();
            inputHandler = GetComponent<InputHandler>();
            playerLocomotion = GetComponent<PlayerLocomotionManager>();
            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
        }

        public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting)
        {
            #region Vertical
            float v = 0;
            if (verticalMovement > 0 && verticalMovement < 0.55f)
            {
                v = 0.5f;
            }
            else if (verticalMovement > 0.55f)
            {
                v = 1f;
            }
            else if (verticalMovement < 0f && verticalMovement > -0.55f)
            {
                v = -0.5f;
            }
            else if (verticalMovement < -0.55f)
            {
                v = -1;
            }
            else v = 0;
            #endregion

            #region Horizontal
            float h = 0;
            if (horizontalMovement > 0 && horizontalMovement < 0.55f)
            {
                h = 0.5f;
            }
            else if (horizontalMovement > 0.55f)
            {
                h = 1f;
            }
            else if (horizontalMovement < 0f && horizontalMovement > -0.55f)
            {
                h = -0.5f;
            }
            else if (horizontalMovement < -0.55f)
            {
                h = -1;
            }
            else h = 0;
            #endregion

            if (isSprinting)
            {
                v = 2;
                h = horizontalMovement;
            }

            animator.SetFloat(vertical, v, 0.1f, Time.deltaTime);
            animator.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
        }

        public void DisableCollision()
        {
            playerLocomotion.characterCollider.enabled = false;
            playerLocomotion.characterCollisionBlockerCollider.enabled = false;
        }

        public void EnableCollision()
        {
            playerLocomotion.characterCollider.enabled = true;
            playerLocomotion.characterCollisionBlockerCollider.enabled = true;
        }

        private void OnAnimatorMove()
        {
            if (characterManager.isInteracting == false) return;

            float delta = Time.deltaTime;
            playerLocomotion.rigidbody.drag = 0;
            Vector3 deltaPosition = animator.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            playerLocomotion.rigidbody.velocity = velocity;

        }
    }
}
