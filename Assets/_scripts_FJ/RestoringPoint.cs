using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JAM3.Health
{
    public class RestoringPoint : MonoBehaviour
    {
        [SerializeField] float distToPoint;
        [SerializeField] float gainingLifeSpeed;
        [SerializeField] Health health;
        GameObject player;
        private void Start()
        {
            player = GameObject.FindWithTag("Player");
        }
        private void Update()
        {
            float dist = Vector3.Distance(player.transform.position, transform.position);
            Debug.Log(dist);
            if(dist <= distToPoint && health.currentHealth < health.maxHealth)
            {
                RestoringHealth();
            }
        }

        private void RestoringHealth()
        {
            health.currentHealth += gainingLifeSpeed * Time.deltaTime;
        }
       
    }
}
