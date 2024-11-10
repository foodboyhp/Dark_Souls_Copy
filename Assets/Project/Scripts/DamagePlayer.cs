using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHH
{
    public class DamagePlayer : MonoBehaviour
    {
        private int damage = 100;
        private void OnTriggerEnter(Collider other)
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>(); 
            if(playerStats != null)
            {
                playerStats.TakeDamage(damage);
            }
        }
    }
}