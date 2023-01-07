using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeakPoint : MonoBehaviour
{
    /*
        We utilize that this boss's gameobject is this gameobject's grandparent
    */

    [SerializeField] float damage;

    public float DamagePoints { get { return damage; } }

    // Start is called before the first frame update
    void Start()
    {
        // Set this gameobject to be "Boss Weak Point" tag
        tag = "Boss Weak Point";
        gameObject.layer = LayerMask.NameToLayer("Shootable");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Damage()
    {
        transform.parent.parent.GetComponent<Health>().TakeDamage(damage);
    }
}
