using PixelCrushers.DialogueSystem.Demo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField]
    public float maxHealth;
    [SerializeField]
    public float health;
    [SerializeField]
    public float strength;
    [SerializeField]
    public float protection;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(float damage)
    {
        Debug.Log("Ouch");
        health -= damage - damage * (protection / 100);
        Debug.Log("Current Health: " + health);
        if (health <= 0)
        {
            Die();
        }
    }

    public IEnumerator TakeDamageOverTime(float totalDamage, float duration, float tickRate)
    {
        float tickDamage = (totalDamage * tickRate) / duration;
        float endTime = Time.fixedTime + duration;
        for (; ; )
        {
            if (Time.fixedTime >= endTime)
                yield break;
            health -= tickDamage;
            yield return tickRate;
        }
    }

    public IEnumerator BuffDebuff(string[] statsToBuff, float[] buffModifiers, float buffDuration)
    {
        float endTime = Time.fixedTime + buffDuration;
        for (int i = 0; i < statsToBuff.Length; i++)
        {

            this.GetType().GetField(statsToBuff[i]).SetValue(this, (float)this.GetType().GetField(statsToBuff[i]).GetValue(this) + buffModifiers[i]);
        }
        if (protection > 100)
        {
            protection = 100;
        }
        for (; ; )
        {
            if (Time.fixedTime >= endTime)
            {
                //Debug.Log("buffos quitados");   
                for (int i = 0; i < statsToBuff.Length; i++)
                {
                    if (statsToBuff[i] == "health" && health - buffModifiers[i] <= 0)
                    {

                    }
                    else
                    {
                        this.GetType().GetField(statsToBuff[i]).SetValue(this, (float)this.GetType().GetField(statsToBuff[i]).GetValue(this) - buffModifiers[i]);
                    }


                }

                yield break;
            }

            yield return null;
        }
    }

    public void Die() { }
}
