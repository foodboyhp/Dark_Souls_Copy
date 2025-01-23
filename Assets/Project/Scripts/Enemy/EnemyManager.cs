using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace PHH
{
    public class EnemyManager : CharacterManager
    {
        public EnemyBossManager enemyBossManager;
        public EnemyLocomotionManager enemyLocomotionManager;
        public EnemyAnimatorManager enemyAnimatorManager;
        public EnemyStatsManager enemyStatsManager;
        public EnemyEffectsManager enemyEffectsManager;

        public NavMeshAgent navMeshAgent;
        public State currentState;
        public CharacterStatsManager currentTarget;
        public Rigidbody enemyRigidbody;

        public bool isPerformingAction;
        public float rotationSpeed = 15f;
        public float maximumAggroRadius = 1.5f;

        [Header("AI Settings")]
        public float detectionRadius = 20;
        public float maximumDetectionAngle = 50;
        public float minimumDetectionAngle = -50;
        public float viewableAngle;
        public float currentRecoveryTime = 0;
        [Header("AI Combat Settings")]
        public bool isPhaseShifting;
        public bool allowAIToperformCombo;
        public float comboLikelyhood;

        protected override void Awake()
        {
            base.Awake();
            enemyBossManager = GetComponent<EnemyBossManager>();
            enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
            enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();
            enemyStatsManager = GetComponent<EnemyStatsManager>();
            enemyEffectsManager = GetComponent<EnemyEffectsManager>();
            enemyRigidbody = GetComponent<Rigidbody>();
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
            navMeshAgent.enabled = false;
        }
        private void Start()
        {
            enemyRigidbody.isKinematic = false;
        }
        private void Update()
        {
            HandleRecoveryTime();
            HandleStateMachine();

            isRotatingWithRootMotion = animator.GetBool("isRotatingWithRootMotion");
            isInteracting = animator.GetBool("isInteracting");
            isPhaseShifting = animator.GetBool("isPhaseShifting");
            isInvulnerable = animator.GetBool("isInvulnerable");
            canDoCombo = animator.GetBool("canDoCombo");
            canRotate = animator.GetBool("canRotate");
            animator.SetBool("isDead", isDead);
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            enemyEffectsManager.HandleBuildUpEffects();
        }

        private void LateUpdate()
        {
            navMeshAgent.transform.localPosition = Vector3.zero;
            navMeshAgent.transform.localRotation = Quaternion.identity;
        }

        private void HandleStateMachine()
        {
            if (currentState != null)
            {
                State nextState = currentState.Tick(this);

                if (nextState != null)
                {
                    SwitchToNextState(nextState);
                }
            }
        }

        private void SwitchToNextState(State nextState)
        {
            currentState = nextState;
        }

        private void HandleRecoveryTime()
        {
            if (currentRecoveryTime > 0)
            {
                currentRecoveryTime -= Time.deltaTime;
            }

            if (isPerformingAction)
            {
                if (currentRecoveryTime <= 0)
                {
                    isPerformingAction = false;
                }
            }
        }
    }
}
