using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PHH
{
    public class BombDamageCollider : DamageCollider
    {
        [Header("Explosive Damage and Radius")]
        public int explosiveRadius = 1;
        public int explosionDamage = 1;
        public int explosionSplashDamage = 1;
        //magic explosiv
        //lightning explosive

        public Rigidbody bombRigidbody;
        private bool hasCollided = false;
        public GameObject impactParticles;

        protected override void Awake()
        {
            damageCollider = GetComponent<Collider>();
            bombRigidbody = GetComponent<Rigidbody>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!hasCollided)
            {
                hasCollided = true;
                impactParticles = Instantiate(impactParticles, transform.position, Quaternion.identity);
                Explode();

                CharacterStatsManager character = collision.transform.GetComponent<CharacterStatsManager>();

                if (character != null)
                {
                    if (character.teamIDNumber != teamIDNumber)
                    {
                        character.TakeDamage(0, explosionDamage, currentDamageAnimation);
                    }
                    //character take damage
                }
                Destroy(impactParticles, 5f);
                Destroy(transform.parent.parent.gameObject);
            }
        }
        private void Explode()
        {
            Collider[] characters = Physics.OverlapSphere(transform.position, explosiveRadius);
            foreach (Collider objectsInExplosion in characters)
            {
                CharacterStatsManager character = objectsInExplosion.GetComponent<CharacterStatsManager>();
                if (character != null)
                {
                    if (character.teamIDNumber != teamIDNumber)
                    {
                        character.TakeDamage(0, explosionSplashDamage, currentDamageAnimation);
                    }
                    // Deal fire damage
                }
            }
        }
    }
}
