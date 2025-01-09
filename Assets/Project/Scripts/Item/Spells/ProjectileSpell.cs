using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHH
{
    [CreateAssetMenu(menuName = "Spells/Projectile Spell")]
    public class ProjectileSpell : SpellItem
    {
        [Header("Projectile Damage")]
        public float baseDamage;

        [Header("Projectile Physics")]
        public float projectileForwardVelocity;
        public float projectileUpwardVelocity;
        public float projectileMass;
        public bool isEffectedByGravity;

        Rigidbody rigidbody;

        public override void AttemptToCastSpell(PlayerAnimatorManager animatorHandler,
            PlayerStatsManager playerStats, PlayerWeaponSlotManager weaponSlotManager, bool isLeftHanded)
        {
            base.AttemptToCastSpell(animatorHandler, playerStats, weaponSlotManager, isLeftHanded);
            if (isLeftHanded)
            {
                GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, weaponSlotManager.leftHandSlot.transform);
                instantiatedWarmUpSpellFX.gameObject.transform.localScale = new Vector3(100, 100, 100);
                animatorHandler.PlayTargetAnimation(spellAnimation, true, false, isLeftHanded);
            }
            else
            {
                GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, weaponSlotManager.rightHandSlot.transform);
                instantiatedWarmUpSpellFX.gameObject.transform.localScale = new Vector3(100, 100, 100);
                animatorHandler.PlayTargetAnimation(spellAnimation, true, false, isLeftHanded);
            }
        }

        public override void SuccessfullyCastSpell(PlayerAnimatorManager animatorHandler,
            PlayerStatsManager playerStats, CameraHandler cameraHandler
            , PlayerWeaponSlotManager weaponSlotManager, bool isLeftHanded)
        {
            base.SuccessfullyCastSpell(animatorHandler, playerStats, cameraHandler, weaponSlotManager, isLeftHanded);
            if (isLeftHanded)
            {
                GameObject instantiatedSpellFX = Instantiate(spellCastFX, weaponSlotManager.leftHandSlot.transform.position,
                cameraHandler.cameraPivotTransform.rotation);
                SpellDamageCollider spellDamageCollider = instantiatedSpellFX.GetComponent<SpellDamageCollider>();
                spellDamageCollider.teamIDNumber = playerStats.teamIDNumber;
                rigidbody = instantiatedSpellFX.GetComponent<Rigidbody>();

                if (cameraHandler.currentLockOnTarget != null)
                {
                    instantiatedSpellFX.transform.LookAt(cameraHandler.currentLockOnTarget.transform);
                }
                else
                {
                    instantiatedSpellFX.transform.rotation = Quaternion.Euler(cameraHandler.cameraPivotTransform.eulerAngles.x, playerStats.transform.eulerAngles.y, 0);
                }

                rigidbody.AddForce(instantiatedSpellFX.transform.forward * projectileForwardVelocity);
                rigidbody.AddForce(instantiatedSpellFX.transform.up * projectileUpwardVelocity);
                rigidbody.useGravity = isEffectedByGravity;
                rigidbody.mass = projectileMass;
                instantiatedSpellFX.transform.parent = null;
            }
            else
            {
                GameObject instantiatedSpellFX = Instantiate(spellCastFX, weaponSlotManager.rightHandSlot.transform.position,
                cameraHandler.cameraPivotTransform.rotation);
                SpellDamageCollider spellDamageCollider = instantiatedSpellFX.GetComponent<SpellDamageCollider>();
                spellDamageCollider.teamIDNumber = playerStats.teamIDNumber;
                rigidbody = instantiatedSpellFX.GetComponent<Rigidbody>();

                if (cameraHandler.currentLockOnTarget != null)
                {
                    instantiatedSpellFX.transform.LookAt(cameraHandler.currentLockOnTarget.transform);
                }
                else
                {
                    instantiatedSpellFX.transform.rotation = Quaternion.Euler(cameraHandler.cameraPivotTransform.eulerAngles.x, playerStats.transform.eulerAngles.y, 0);
                }

                rigidbody.AddForce(instantiatedSpellFX.transform.forward * projectileForwardVelocity);
                rigidbody.AddForce(instantiatedSpellFX.transform.up * projectileUpwardVelocity);
                rigidbody.useGravity = isEffectedByGravity;
                rigidbody.mass = projectileMass;
                instantiatedSpellFX.transform.parent = null;
            }
        }
    }
}
