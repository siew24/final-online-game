using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour//Pun
{
    // new PhotonView photonView;

    public Slider healthSlider;
    public float maxHealth = 100f;
    public float currentHealth;

    [Header("Boss")]
    public bool isBoss = false;
    public OnBossDied onBossDied;

    [Header("Player")]
    public bool isPlayer = false;
    public OnPlayerHealthChange onPlayerHealthChange;

    Animator animator;

    void Awake()
    {
        // photonView = GetComponent<PhotonView>();
    }

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthSlider();
        onPlayerHealthChange.Raise((int)currentHealth);
    }

    public void TakeDamage(float damage)
    {
        if (currentHealth <= 0)
            return;

        currentHealth -= damage;
        UpdateHealthSlider();
        onPlayerHealthChange.Raise((int)currentHealth);

        if (currentHealth <= 0)
        {
            if (isBoss)
            {
                Debug.Log("raising");
                onBossDied.Raise();
                return;
            }

            Die();
        }
    }

    public void Die()
    {
        //photonView.RPC(nameof(NetworkedDie), RpcTarget.AllBuffered);
        NetworkedDie();
    }

    public void Attack(GameObject target)
    {
    }

    void UpdateHealthSlider()
    {
        float healthPercent = currentHealth / maxHealth;
        healthSlider.value = healthPercent;
    }

    [PunRPC]
    void NetworkedDie()
    {
        Destroy(gameObject);
    }
}
