using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHH
{
    public class EnemyWeaponSlotManager : MonoBehaviour
    {
        public WeaponItem rightHandWeapon;
        public WeaponItem leftHandWeapon;
        WeaponHolderSlot rightHandSlot;
        WeaponHolderSlot leftHandSlot;

        DamageCollider leftHandDamageCollider;
        DamageCollider rightHandDamageCollider;

        private void Awake()
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
            }
        }

        private void Start()
        {
            LoadWeaponOnBothHands();
        }

        public void LoadWeaponOnSlot(WeaponItem weapon, bool isLeft)
        {
            if (isLeft)
            {
                leftHandSlot.currentWeapon = weapon;
                leftHandSlot.LoadWeaponModel(weapon);
                LoadWeaponDamageCollider(true);
            }
            else
            {
                rightHandSlot.currentWeapon = weapon;
                rightHandSlot.LoadWeaponModel(weapon);
                LoadWeaponDamageCollider(false);
            }
        }
        public void LoadWeaponOnBothHands()
        {
            if (rightHandWeapon != null)
            {
                LoadWeaponOnSlot(rightHandWeapon, false);
            }
            if (leftHandWeapon != null)
            {
                LoadWeaponOnSlot(leftHandWeapon, true);
            }
        }

        public void LoadWeaponDamageCollider(bool isLeft)
        {
            if (isLeft)
            {
                leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            }
            else
            {
                rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            }
        }

        public void OpenDamageCollider()
        {
            rightHandDamageCollider.EnableDamageCollider();
        }

        public void CloseDamageCollider()
        {
            rightHandDamageCollider.DisableDamageCollider();
        }
        public void DrainStaminaLightAttack()
        {
        }
        public void DrainStaminaHeavyAttack()
        {
        }
        public void EnableCombo()
        {
            //anim.SetBool("canDoCombo", true);
        }

        public void DisableCombo()
        {
            //anim.SetBool("canDoCombo", false);
        }
    }
}
