using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHH
{
    public class CharacterWeaponSlotManager : MonoBehaviour
    {
        protected CharacterManager character;

        [Header("Unarmed Weapon")]
        public WeaponItem unarmedWeapon;

        [Header("Weapon Slots")]
        public WeaponHolderSlot leftHandSlot;
        public WeaponHolderSlot rightHandSlot;
        public WeaponHolderSlot backSlot;

        [Header("Damage Colliders")]
        public DamageCollider leftHandDamageCollider;
        public DamageCollider rightHandDamageCollider;

        [Header("Hand IK Targets")]
        public RightHandIKTarget rightHandIKTarget;
        public LeftHandIKTarget leftHandIKTarget;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
            LoadWeaponHolderSlots();

        }

        protected virtual void LoadWeaponHolderSlots()
        {
            WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
            foreach (WeaponHolderSlot weaponHolderSlot in weaponHolderSlots)
            {
                if (weaponHolderSlot.isLeftHandSlot)
                {
                    leftHandSlot = weaponHolderSlot;
                }
                else if (weaponHolderSlot.isRightHandSlot)
                {
                    rightHandSlot = weaponHolderSlot;
                }
                else if (weaponHolderSlot.isBackSlot)
                {
                    backSlot = weaponHolderSlot;
                }
            }
        }

        public virtual void LoadBothWeaponOnSlots()
        {
            LoadWeaponOnSlot(character.characterInventoryManager.rightWeapon, false);
            LoadWeaponOnSlot(character.characterInventoryManager.leftWeapon, true);
        }

        public virtual void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
        {
            if (weaponItem != null)
            {
                if (isLeft)
                {
                    leftHandSlot.currentWeapon = weaponItem;
                    leftHandSlot.LoadWeaponModel(weaponItem);
                    LoadLeftWeaponDamageCollider();
                }
                else
                {
                    if (character.isTwoHandingWeapon)
                    {
                        backSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                        leftHandSlot.UnloadWeaponAndDestroy();
                        character.characterAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);

                    }
                    else
                    {
                        backSlot.UnloadWeaponAndDestroy();
                    }
                    rightHandSlot.currentWeapon = weaponItem;
                    rightHandSlot.LoadWeaponModel(weaponItem);
                    LoadRightWeaponDamageCollider();
                    LoadTwoHandIKTarget(character.isTwoHandingWeapon);
                    character.animator.runtimeAnimatorController = weaponItem.weaponController;
                }
            }
            else
            {
                weaponItem = unarmedWeapon;
                if (isLeft)
                {
                    character.characterInventoryManager.leftWeapon = unarmedWeapon;
                    leftHandSlot.currentWeapon = weaponItem;
                    leftHandSlot.LoadWeaponModel(weaponItem);
                    LoadLeftWeaponDamageCollider();
                    character.characterAnimatorManager.PlayTargetAnimation(weaponItem.offHandIdleAnimation, false, true);

                }
                else
                {
                    character.characterInventoryManager.rightWeapon = unarmedWeapon;
                    rightHandSlot.currentWeapon = weaponItem;
                    rightHandSlot.LoadWeaponModel(weaponItem);
                    LoadRightWeaponDamageCollider();
                    character.animator.runtimeAnimatorController = weaponItem.weaponController;

                }
            }
        }
        protected virtual void LoadLeftWeaponDamageCollider()
        {
            leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();


            leftHandDamageCollider.physicalDamage = character.characterInventoryManager.leftWeapon.physicalDamage;
            leftHandDamageCollider.fireDamage = character.characterInventoryManager.leftWeapon.fireDamage;

            leftHandDamageCollider.characterManager = character;
            leftHandDamageCollider.teamIDNumber = character.characterStatsManager.teamIDNumber;

            leftHandDamageCollider.poiseBreak = character.characterInventoryManager.leftWeapon.poiseBreak;
            character.characterEffectsManager.leftWeaponFX = leftHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
        }

        protected virtual void LoadRightWeaponDamageCollider()
        {
            rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();


            rightHandDamageCollider.physicalDamage = character.characterInventoryManager.rightWeapon.physicalDamage;
            rightHandDamageCollider.fireDamage = character.characterInventoryManager.rightWeapon.fireDamage;

            rightHandDamageCollider.characterManager = character;
            rightHandDamageCollider.teamIDNumber = character.characterStatsManager.teamIDNumber;

            rightHandDamageCollider.poiseBreak = character.characterInventoryManager.rightWeapon.poiseBreak;
            character.characterEffectsManager.rightWeaponFX = rightHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();

        }
        public virtual void LoadTwoHandIKTarget(bool isTwoHandingWeapon)
        {
            leftHandIKTarget = rightHandSlot.currentWeaponModel.GetComponentInChildren<LeftHandIKTarget>();
            rightHandIKTarget = rightHandSlot.currentWeaponModel.GetComponentInChildren<RightHandIKTarget>();

            character.characterAnimatorManager.SetHandIKForWeapon(rightHandIKTarget, leftHandIKTarget, isTwoHandingWeapon);
        }
        public virtual void OpenDamageCollider()
        {
            if (character.isUsingRightHand)
            {
                rightHandDamageCollider.EnableDamageCollider();
            }
            if (character.isUsingLeftHand)
            {
                leftHandDamageCollider.EnableDamageCollider();
            }
        }

        public virtual void CloseDamageCollider()
        {
            if (rightHandDamageCollider != null)
            {
                rightHandDamageCollider.DisableDamageCollider();
            }
            if (leftHandDamageCollider != null)
            {
                leftHandDamageCollider.DisableDamageCollider();
            }
        }

        public virtual void GrantWeaponAttackingPoiseBonus()
        {
            WeaponItem currentWeaponBeingUsed = character.characterInventoryManager.currentItemBeingUsed as WeaponItem;
            character.characterStatsManager.totalPoiseDefense = character.characterStatsManager.totalPoiseDefense + currentWeaponBeingUsed.offensivePoiseBonus;
        }

        public virtual void ResetWeaponAttackingPoise()
        {
            character.characterStatsManager.totalPoiseDefense = character.characterStatsManager.armorPoiseBonus;
        }
    }
}
