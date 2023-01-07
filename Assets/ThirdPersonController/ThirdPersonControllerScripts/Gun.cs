using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;

public class Gun : MonoBehaviour
{
    public static Gun instance;
    private StarterAssetsInputs assetInputs;

    bool readyToShoot, reloading;
    int bulletsLeft, bulletsShot;
    public LayerMask whatIsEnemy;
    public int damage;
    public float timeBetweenShooting, range, reloadTime;
    public int magazineSize;
    private int maxammoCharge = 5;
    public int currentAmmoCharge;

    [SerializeField] public Vector2 screenCentrePoint;

    private void Awake()
    {
        instance = this;
        assetInputs = GetComponentInParent<StarterAssetsInputs>();

        bulletsLeft = magazineSize;
        readyToShoot = true;
        reloading = false;
    }

    public void Input()
    {
        if (!reloading && readyToShoot && bulletsLeft > 0)
        {
            Shoot();
        }

        if (bulletsLeft <= 0 && currentAmmoCharge > 0 || assetInputs.reload && currentAmmoCharge > 0)
        {
            Reload();
        }
    }

    public void Shoot()
    {
        screenCentrePoint = new Vector2(Screen.width / 2f, Screen.height / 2f);

        RaycastHit hit;

        if (assetInputs.shoot)
        {
            Ray ray = Camera.main.ScreenPointToRay(screenCentrePoint);
            Debug.DrawRay(ray.origin, Camera.main.ScreenPointToRay(screenCentrePoint).direction * 5000, Color.red);

            Debug.Log("Shot");
            readyToShoot = false;
            if (Physics.Raycast(ray, out hit, range, whatIsEnemy))
            {

                if (hit.collider.CompareTag("Enemy"))
                {
                    Debug.Log(hit.collider.name);
                    hit.collider.GetComponent<AIEnemyHealth>().TakeDamage(damage);
                }

                if (hit.collider.CompareTag("Boss Weak Point"))
                {
                    Debug.Log($"Hit Boss Weak Point with {hit.collider.GetComponent<BossWeakPoint>().DamagePoints}");
                    hit.collider.GetComponent<BossWeakPoint>().Damage();
                }

            }
            //muzzleFlash?.Play();
            bulletsLeft--;
            Invoke("ResetShot", timeBetweenShooting);
        }
    }

    void ResetShot()
    {
        readyToShoot = true;
    }

    void Reload()
    {
        Debug.Log("Reloading");
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    void ReloadFinished()
    {
        Debug.Log("Reloaded");
        reloading = false;
        currentAmmoCharge -= 1;
        bulletsLeft = magazineSize;
    }
}

