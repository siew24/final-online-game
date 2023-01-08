using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BossDiedListener))]
public class BossLaserGunController : MonoBehaviour
{
    [SerializeField] GameObject[] laserPoints;
    [SerializeField] Material lineMaterial;
    [SerializeField] float turnSpeed;
    [SerializeField] float turnIncrement;
    [SerializeField] float damage;

    BossDiedListener bossDiedListener;
    Animator animator;

    bool _canDrawLaser;

    // Start is called before the first frame update
    void Start()
    {
        _canDrawLaser = false;

        Utils.GetListener(this, out bossDiedListener);
        bossDiedListener.Register(StopLaser);
    }

    // Update is called once per frame
    void Update()
    {
        if (_canDrawLaser)
        {
            transform.rotation *= Quaternion.Euler(0f, turnSpeed, 0f);

            foreach (GameObject laserPoint in laserPoints)
            {
                if (!laserPoint.TryGetComponent(out LineRenderer lineRenderer))
                {
                    lineRenderer = laserPoint.AddComponent<LineRenderer>();
                    lineRenderer.widthCurve = new(new Keyframe(0f, .01f));
                    lineRenderer.material = lineMaterial;
                }

                lineRenderer.SetPosition(0, laserPoint.transform.position);

                if (Physics.Raycast(laserPoint.transform.position, laserPoint.transform.forward, out RaycastHit hit))
                {
                    lineRenderer.SetPosition(1, hit.point);

                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Players"))
                    {
                        hit.collider.GetComponent<Health>().TakeDamage(damage);
                    }
                }
            }

        }
    }

    void StopLaser()
    {
        animator.Play("Laser Gun Disassemble");

        foreach (GameObject laserPoint in laserPoints)
            if (laserPoint.TryGetComponent(out LineRenderer lineRenderer))
                Destroy(lineRenderer);

        return;
    }

    public void ShootLaser()
    {
        _canDrawLaser = true;
    }

    public void SpeedUpLaser()
    {
        turnSpeed += turnIncrement;
    }
}
