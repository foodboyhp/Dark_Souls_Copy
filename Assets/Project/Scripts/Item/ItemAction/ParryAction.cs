using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHH
{
    public class ParryAction : ItemAction
    {
        public override void PerformAction(PlayerManager player)
        {
            if (player.isInteracting)
            {
                return;
            }
            player.playerAnimatorManager.EraseHandIKForWeapon();
            WeaponItem parryWeapon = player.playerInventoryManager.currentItemBeingUsed as WeaponItem;

            if (parryWeapon.weaponType == WeaponType.SmallShield)
            {
                player.playerAnimatorManager.PlayTargetAnimation("Parry_01", true);
            }
            else if (parryWeapon.weaponType == WeaponType.Shield)
            {
                player.playerAnimatorManager.PlayTargetAnimation("Parry_01", true);
            }
        }
    }
}
