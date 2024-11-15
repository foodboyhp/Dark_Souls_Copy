using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PHH
{
    [CreateAssetMenu(menuName = "Items/ Weapon Item")]
    public class WeaponItem : Item
    {
        public GameObject modelPrefab;
        public bool isUnarmed;

        [Header("Idle Animations")]
        public string left_hand_idle;
        public string right_hand_idle;

        [Header("Attack Animations")]
        public string OH_Light_Attack_1;
        public string OH_Light_Attack_2;
        public string OH_Heavy_Attack_1;
    }
}
