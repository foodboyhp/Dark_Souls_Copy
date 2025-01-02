using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHH
{
    public class PlayerEffectsManager : CharacterEffectsManager
    {
        PlayerStatsManager playerStatsManager;
        PlayerWeaponSlotManager playerWeaponSlotManager;

        public GameObject currentParticleFX;
        public GameObject instantiatedFXModel;
        public int amountToBeHealed;

        private void Awake()
        {
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
        }

        public void HealPlayerFromEffect()
        {
            playerStatsManager.HealPlayer(amountToBeHealed);
            GameObject healParticles = Instantiate(currentParticleFX, playerStatsManager.transform);
            Destroy(instantiatedFXModel.gameObject, 2);
            playerWeaponSlotManager.LoadBothWeaponOnSlots();
        }
    }
}
