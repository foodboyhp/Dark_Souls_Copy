using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PHH 
{
    public class WeaponPickup : Interactable
    {
        public WeaponItem weapon;

        public override void Interact(PlayerManager playerManager)
        {
            base.Interact(playerManager);
            PickupItem(playerManager);  
        }

        private void PickupItem(PlayerManager playerManager)
        {
            PlayerInventory playerInventory = playerManager.GetComponent<PlayerInventory>();
            PlayerLocomotion playerLocomotion = playerManager.GetComponent<PlayerLocomotion>();
            AnimatorHandler animatorHandler = playerLocomotion.GetComponentInChildren<AnimatorHandler>();   

            playerLocomotion.rigidbody.velocity = Vector3.zero;
            animatorHandler.PlayerTargetAnimation("Pick Up Item", true);
            playerInventory.weaponsInventory.Add(weapon);
            playerManager.itemInteractableGameObject.GetComponentInChildren<TextMeshProUGUI>().text = weapon.itemName;
            //playerManager.itemInteractableGameObject.GetComponentInChildren<RawImage>().texture = weapon.itemIcon.texture;
            playerManager.itemInteractableGameObject.SetActive(true);
            Destroy(gameObject);
        }
    }
}

