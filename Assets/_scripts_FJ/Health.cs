using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JAM3.Health
{
    public class Health : MonoBehaviour
    {
        public float maxHealth;
        public float currentHealth;
        void Start()
        {
            currentHealth = maxHealth;
        }


        void Update()
        {
            if(currentHealth <= 0)
            {
                Die();
            }
        }

        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
        }

        public void Die()
        {

        }
    }
}

