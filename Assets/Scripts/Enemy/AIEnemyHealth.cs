using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;

public class AIEnemyHealth : MonoBehaviour//Pun
{
    //new PhotonView photonView;

    public Slider healthSlider;
    public float maxHealth = 100f;
    public float currentHealth;
    public float damage = 20f;

    void Awake()
    {
        //photonView = GetComponent<PhotonView>();
    }

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthSlider();
    }

    public void TakeDamage(float damage)
    {
        //photonView.RPC(nameof(NetworkedTakeDamage), RpcTarget.AllBuffered, damage);
        NetworkedTakeDamage(damage, new());
    }

    public void Die()
    {
        // Trigger death animation or effect
        //photonView.RPC(nameof(NetworkedDie), RpcTarget.AllBuffered);
        NetworkedDie(new());
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

    [PunRPC]
    void NetworkedTakeDamage(float damage, PhotonMessageInfo photonMessageInfo)
    {
        Debug.Log($"{photonMessageInfo.SentServerTime}: Hit {gameObject.name} with {damage}.");

        currentHealth -= damage;
        UpdateHealthSlider();
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    [PunRPC]
    void NetworkedDie(PhotonMessageInfo photonMessageInfo)
    {
        Debug.Log($"{photonMessageInfo.SentServerTime}: Destroyed {gameObject.name}.");
        Destroy(gameObject);
    }
}
