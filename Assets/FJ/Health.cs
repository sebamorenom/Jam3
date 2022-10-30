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
        bool isPoisoned;
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
            if(isPoisoned)
            {
                
            }
        }

        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
        }

       

        public void Die()
        {

        }

        public IEnumerator TakeDamageOverTime(float damage, float timebetweenticks, float duration)
        {
            while(duration > 0)
            {
                currentHealth -= damage;
                poisonEffect = true;
                yield return new WaitForSeconds(timebetweenticks);
                poisonEffect = true;
                duration--;
            }
            yield return null;
        }
    }
}

