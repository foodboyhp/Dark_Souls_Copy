using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHH
{
    public class CharacterEffectsManager : MonoBehaviour
    {
        CharacterManager character;
        [Header("Current Range FX")]
        public GameObject currentRangeFX;
        [Header("Damage FX")]
        public GameObject bloodSplatterFx;
        [Header("Weapon FX")]
        public WeaponFX rightWeaponFX;
        public WeaponFX leftWeaponFX;
        [Header("Poison FX")]
        public GameObject defaultPoisonParticleFX;
        public GameObject currentPoisonParticleFX;
        public Transform buildUpTransform;
        public bool isPoisoned;
        public float poisonBuildUp = 0;
        public float poisonAmount = 100;
        public float defaultPoisonAmount = 0;
        public float poisonTimer = 2f;
        public int poisonDamage = 1;
        float timer;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        public virtual void PlayWeaponFX(bool isLeft)
        {
            if (!isLeft)
            {
                if (rightWeaponFX != null)
                {
                    rightWeaponFX.PlayWeaponFX();
                }
            }
            else
            {
                if (leftWeaponFX != null)
                {
                    leftWeaponFX.PlayWeaponFX();
                }
            }
        }
        public virtual void PlayBloodSplatterFX(Vector3 bloodSplatterLocation)
        {
            GameObject blood = Instantiate(bloodSplatterFx, bloodSplatterLocation, Quaternion.identity);
        }

        public virtual void HandleBuildUpEffects()
        {
            if (character.isDead)
            {
                return;
            }
            HandlePoisonBuildUp();
            HandleIsPoisonedEffect();
        }

        protected virtual void HandlePoisonBuildUp()
        {
            if (isPoisoned)
                return;

            if (poisonBuildUp > 0 && poisonBuildUp < 100)
            {
                poisonBuildUp = poisonBuildUp - 1 * Time.deltaTime;
            }
            else if (poisonBuildUp >= 100)
            {
                isPoisoned = true;
                poisonBuildUp = 0;

                if (buildUpTransform != null)
                {
                    currentPoisonParticleFX = Instantiate(defaultPoisonParticleFX, buildUpTransform.transform);
                }
                else
                {
                    currentPoisonParticleFX = Instantiate(defaultPoisonParticleFX, character.transform);
                }
            }
        }

        protected virtual void HandleIsPoisonedEffect()
        {
            if (isPoisoned)
            {
                if (poisonAmount > 0)
                {
                    timer += Time.deltaTime;
                    if (timer >= poisonTimer)
                    {
                        character.characterStatsManager.TakePoisonDamage(poisonDamage);
                        timer = 0;
                    }
                    poisonAmount = poisonAmount - 1 * Time.deltaTime;
                }
                else
                {
                    isPoisoned = false;
                    poisonAmount = defaultPoisonAmount;
                    Destroy(currentPoisonParticleFX);
                }
            }
        }
    }
}
