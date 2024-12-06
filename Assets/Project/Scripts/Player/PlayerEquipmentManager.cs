using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHH
{

    public class PlayerEquipmentManager : MonoBehaviour
    {
        InputHandler inputHandler;
        PlayerInventory playerInventory;
        public BlockingCollider blockingCollider;

        private void Awake()
        {
            inputHandler = GetComponentInParent<InputHandler>();
            playerInventory = GetComponentInParent<PlayerInventory>();
        }

        public void OpenBlockingCollider()
        {
            if(inputHandler.twoHandFlag)
            {
                blockingCollider.SetColliderDamageAbsorbtion(playerInventory.rightWeapon);
            }
            else
            {
                blockingCollider.SetColliderDamageAbsorbtion(playerInventory.leftWeapon);

            }
            blockingCollider.EnableBlockingCollider();
        }

        public void CloseBlockingCollider()
        {
            blockingCollider.DisableBlockingCollider();
        }
    }
}
