using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHH
{
    public class CharacterManager : MonoBehaviour
    {
        [Header("Lock On Transform")]
        public Transform lockOnTransform;

        [Header("Combat Colliders")]
        public CriticalDamageCollider backStabCollider;
        public CriticalDamageCollider riposteCollider;

        [Header("Interaction")]
        public bool isInteracting;

        [Header("Combat Flags")]
        public bool canBeRiposted;
        public bool canBeParried;
        public bool canDoCombo;
        public bool isParrying;
        public bool isBlocking;
        public bool isInvulnerable;
        public bool isUsingRightHand;
        public bool isUsingLeftHand;
        [Header("Movement")]
        public bool isRotatingWithRootMotion;
        public bool canRotate;
        public bool isSprinting;
        public bool isInAir;
        public bool isGrounded;


        [Header("Spell Flags")]
        public bool isFiringSpell;

        public int pendingCriticalDamage;
    }
}

