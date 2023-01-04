using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIEnemyHealth : MonoBehaviour
{
    /*public int health = 100;
    public EnemyDamagedListener enemyDamagedListener;
    public EnemyDamaged enemyDamaged;
    [SerializeField] int damage = 10;

    private void Start()
    {
        // Create an instance of the OnPlayerDetectedListener event listener
        enemyDamagedListener = gameObject.AddComponent<EnemyDamagedListener>();
        // Register the AttackPlayer method as the UnityAction for the event listener
        enemyDamagedListener.Register(Damage);

    }

    void Damage()
    {
        // Reduce the enemy's health by 10 when the event is triggered
        health -= damage;
        // Raise the EnemyDamaged event, passing the amount of damage as an argument
        enemyDamaged.Raise(damage);
    }*/

    public Slider healthSlider;
    public float maxHealth = 100f;
    public float currentHealth;
    public float damage = 20f;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthSlider();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        UpdateHealthSlider();
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        // Trigger death animation or effect
        Destroy(gameObject);
    }

    public void Attack(GameObject target)
    {
        // Trigger attack animation or effect
        //target.GetComponent<HealthSystem>().TakeDamage(damage);
    }

    void UpdateHealthSlider()
    {
        float healthPercent = currentHealth / maxHealth;
        healthSlider.value = healthPercent;
    }
}
