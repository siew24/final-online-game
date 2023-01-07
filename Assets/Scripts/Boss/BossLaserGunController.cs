using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLaserGunController : MonoBehaviour
{
    [SerializeField] GameObject[] laserPoints;
    [SerializeField] Material lineMaterial;
    [SerializeField] float turnSpeed;

    bool _canDrawLaser;

    // Start is called before the first frame update
    void Start()
    {
        _canDrawLaser = false;
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

                if (Physics.Raycast(laserPoint.transform.position, laserPoint.transform.forward, out RaycastHit hit, 100f))
                    lineRenderer.SetPosition(1, hit.point);
                else
                    lineRenderer.SetPosition(1, laserPoint.transform.position + (laserPoint.transform.forward * 100f));
            }

        }
    }

    public void ShootLaser()
    {
        _canDrawLaser = true;

        foreach (GameObject laserPoint in laserPoints)
        {
            if (Physics.Raycast(laserPoint.transform.position, laserPoint.transform.forward, out RaycastHit hit, LayerMask.NameToLayer("Players")))
            {
                // TODO: Deal damage to player
            }
        }
    }
}
