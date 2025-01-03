using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHH
{
    public class BlockingCollider : MonoBehaviour
    {
        public BoxCollider blockingCollider;
        public float blockingPhysicalDamageAbsorbtion;
        public float blockingFireDamageAbsorbtion;

        private void Awake()
        {
            blockingCollider = GetComponent<BoxCollider>();
        }

        public void SetColliderDamageAbsorbtion(WeaponItem weapon)
        {
            if (weapon != null)
            {
                blockingPhysicalDamageAbsorbtion = weapon.physicalDamageAbsorbtion;
            }
        }

        public void EnableBlockingCollider()
        {
            blockingCollider.enabled = true;
        }

        public void DisableBlockingCollider()
        {
            blockingCollider.enabled = false;
        }
    }

}
