using PHH;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHH
{
    [CreateAssetMenu(menuName = "Item Actions/Block Action")]
    public class BlockingAction : ItemAction
    {
        public override void PerformAction(PlayerManager player)
        {
            if (player.isInteracting)
            {
                return;
            }
            if (player.isBlocking)
            {
                return;
            }
            player.playerAnimatorManager.PlayTargetAnimation("Block_Start", false, true);
            player.PlayerEquipmentManager.OpenBlockingCollider();
            player.isBlocking = true;
        }
    }
}
