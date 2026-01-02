using System.Collections;
using UnityEngine;

public class SunLaser : MonoBehaviour
{
    public GameObject laserPrefab;
    public float delayBeforeFiring = 2.0f;
    public Transform[] leafTargets;

    void Start()
    {
        StartCoroutine(FireLasersAfterDelay());
    }

    IEnumerator FireLasersAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeFiring);

        foreach (Transform leaf in leafTargets)
        {
            FireLaserAtLeaf(leaf);
        }
    }

    void FireLaserAtLeaf(Transform leaf)
    {
        GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
        LaserBehavior laserBehavior = laser.GetComponent<LaserBehavior>();
        if (laserBehavior != null)
        {
            laserBehavior.SetTarget(leaf);
        }
    }
}