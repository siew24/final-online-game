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
    public float timeBetweenShooting,  range, reloadTime;
    public int magazineSize;
    private int maxammoCharge = 5;
    public int currentAmmoCharge;

    public ParticleSystem muzzleFlash;

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

        if(bulletsLeft <= 0 && currentAmmoCharge > 0 || assetInputs.reload && currentAmmoCharge > 0)
        {
            Reload();
        }

    }
    public void Shoot()
    {
        screenCentrePoint = new Vector2(Screen.width / 2f, Screen.height / 2f);

        Ray ray = Camera.main.ScreenPointToRay(screenCentrePoint);

        RaycastHit hit;

        if (assetInputs.shoot)
        {
            Debug.Log("Shot");
            readyToShoot = false;
            if (Physics.Raycast(ray, out hit, range, whatIsEnemy))
            {

                if (hit.collider.CompareTag("Enemy"))
                {
                    Debug.Log(hit.collider.name);
                    hit.collider.GetComponent<EnemyHealth>().TakeDamage(damage);
                }

            }
            muzzleFlash.Play();
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

