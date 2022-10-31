using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JAM3.Health
{
    public class Health : MonoBehaviour
    {
        public float maxHealth;
        public float currentHealth;
        bool poisonEffect;
        public bool isPoisoned;
        void Start()
        {
            currentHealth = maxHealth;
            poisonEffect = false;
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

        public void TakeDamageOverTime(float damage, float timebetweenticks, float duration)
        {
            while(duration > 0)
            {
                float aux = timebetweenticks;
                currentHealth -= damage;

                poisonEffect = false;
                while(timebetweenticks > 0)
                {
                    timebetweenticks--;
                }
                timebetweenticks = aux;
                poisonEffect = true;
                duration--;
            }
            
        }

        public void StartChildCoroutine(IEnumerator coroutine)
        {
            StartCoroutine(coroutine);
        }
    }
}

