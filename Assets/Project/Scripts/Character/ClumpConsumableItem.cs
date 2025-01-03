using UnityEngine;

namespace PHH
{
    [CreateAssetMenu(menuName = "Items/Consumables/Cure Effect Clump")]
    public class ClumpConsumableItem : ConsumableItem
    {

        [Header("Recovery VFX")]
        public GameObject clumpConsumeFX;

        [Header("Cure FX")]
        public bool curePoison;
        //Cure Bleed
        //Cure Curse    
        public override void AttemptToConsumeItem(PlayerAnimatorManager playerAnimatorManager,
            PlayerWeaponSlotManager weaponSlotManager, PlayerEffectsManager playerEffectsManager)
        {
            base.AttemptToConsumeItem(playerAnimatorManager, weaponSlotManager, playerEffectsManager);
            GameObject clump = Instantiate(itemModel, weaponSlotManager.leftHandSlot.transform);
            playerEffectsManager.currentParticleFX = clumpConsumeFX;
            playerEffectsManager.instantiatedFXModel = clump;
            if (curePoison)
            {
                playerEffectsManager.poisonBuildUp = 0;
                playerEffectsManager.poisonAmount = playerEffectsManager.defaultPoisonAmount;
                playerEffectsManager.isPoisoned = false;

                if (playerEffectsManager.currentPoisonParticleFX != null)
                {
                    Destroy(playerEffectsManager.currentPoisonParticleFX);
                }
            }
            weaponSlotManager.rightHandSlot.UnloadWeapon();
        }
    }
}
