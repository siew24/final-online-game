using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeakPoint : MonoBehaviour
{

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
        GameObject currentObject = gameObject;
        while (currentObject.transform.parent.name != "Boss")
        {
            if (currentObject.transform.parent.name != "Boss")
                currentObject = currentObject.transform.parent.gameObject;
        }

        currentObject.transform.parent.GetComponent<Health>().TakeDamage(damage);
    }
}
