using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHH
{
    public class PlayerEffectsManager : CharacterEffectsManager
    {
        PlayerStatsManager playerStatsManager;
        PlayerWeaponSlotManager playerWeaponSlotManager;

        PoisonBuildUpBar poisonBuildUpBar;
        PoisonAmountBar poisonAmountBar;

        public GameObject currentParticleFX;
        public GameObject instantiatedFXModel;
        public int amountToBeHealed;

        protected override void Awake()
        {
            base.Awake();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();

            poisonBuildUpBar = FindObjectOfType<PoisonBuildUpBar>();
            poisonAmountBar = FindObjectOfType<PoisonAmountBar>();
        }

        public void HealPlayerFromEffect()
        {
            playerStatsManager.HealPlayer(amountToBeHealed);
            GameObject healParticles = Instantiate(currentParticleFX, playerStatsManager.transform);
            Destroy(instantiatedFXModel.gameObject, 2);
            playerWeaponSlotManager.LoadBothWeaponOnSlots();
        }

        protected override void HandlePoisonBuildUp()
        {
            if (poisonBuildUp <= 0)
            {
                poisonBuildUpBar.gameObject.SetActive(false);
            }
            else
            {
                poisonBuildUpBar.gameObject.SetActive(true);
            }
            base.HandlePoisonBuildUp();
            poisonBuildUpBar.SetCurrentPoisonUpBar(Mathf.RoundToInt(poisonBuildUp));
        }

        protected override void HandleIsPoisonedEffect()
        {
            if (!isPoisoned)
            {
                poisonAmountBar.gameObject.SetActive(false);
            }
            else
            {
                poisonAmountBar.gameObject.SetActive(true);
            }
            base.HandleIsPoisonedEffect();
            poisonAmountBar.SetPoisonAmount(Mathf.RoundToInt(poisonAmount));
        }


    }
}
