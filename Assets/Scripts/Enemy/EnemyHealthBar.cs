using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public Slider healthBar;
    public AIEnemyHealth aiEnemyHealth;

    void Start()
    {
        // Get a reference to the enemy's health script
        aiEnemyHealth = GetComponentInParent<AIEnemyHealth>();
        // Register the UpdateHealthBar method with the enemy's damaged event
        //aiEnemyHealth.enemyDamaged.RegisterListener(UpdateHealthBar());
    }

    void UpdateHealthBar()
    {
        // Update the value of the health bar to reflect the current health of the enemy
        //healthBar.value = aiEnemyHealth.health;
    }
}
