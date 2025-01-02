using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHH
{
    public class WeaponFX : MonoBehaviour
    {
        [Header("Weapon FX")]
        public ParticleSystem normalWeaponTrail;
        //fire
        //dark
        //lightning

        public void PlayWeaponFX()
        {
            normalWeaponTrail.Stop();
            if (normalWeaponTrail.isStopped)
            {
                normalWeaponTrail.Play();
            }
        }
    }
}
