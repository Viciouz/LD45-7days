using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
    // ========== Laser Firing Settings ==========
    [Header("Laser Settings")]
    [Tooltip("Time delay (in seconds) before the sun fires lasers at leaves")]
    public float delay = 5f;
    
    [Tooltip("Prefab used to instantiate the laser beam")]
    public GameObject laserPrefab;
    
    [Tooltip("Transform from which lasers are fired (optional, uses sun position if null)")]
    public Transform firePoint;

    private bool hasFiredLaser = false;

    // ========== Unity Lifecycle Methods ==========
    private void Start()
    {
        // Start the delayed laser firing coroutine
        StartCoroutine(FireLaserAfterDelay());
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        GameManager.Instance.CompleteLevel();
    }

    // ========== Laser Firing Logic ==========
    
    /// <summary>
    /// Coroutine that waits for the specified delay, then fires lasers at all leaf objects
    /// </summary>
    private IEnumerator FireLaserAfterDelay()
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Fire lasers at all leaves
        FireLasersAtLeaves();
    }

    /// <summary>
    /// Finds all leaf objects tagged as "Leaf" and fires a laser at each one
    /// </summary>
    private void FireLasersAtLeaves()
    {
        // Prevent multiple laser firings
        if (hasFiredLaser)
            return;

        hasFiredLaser = true;

        // Find all game objects tagged as "Leaf"
        GameObject[] leaves = GameObject.FindGameObjectsWithTag("Leaf");

        // Get the position from which lasers should be fired
        Vector3 startPosition = firePoint != null ? firePoint.position : transform.position;

        // Fire a laser at each leaf
        foreach (GameObject leaf in leaves)
        {
            if (leaf != null && leaf.activeInHierarchy)
            {
                // Calculate direction to the leaf
                Vector3 targetPosition = leaf.transform.position;
                
                // Instantiate laser if prefab is assigned
                if (laserPrefab != null)
                {
                    GameObject laserObj = Instantiate(laserPrefab, startPosition, Quaternion.identity);
                    
                    // Get LaserBeam component and set positions
                    LaserBeam laser = laserObj.GetComponent<LaserBeam>();
                    if (laser != null)
                    {
                        laser.SetPositions(startPosition, targetPosition);
                    }
                }
            }
        }
    }

    // ========== Legacy Code (Commented Out) ==========
    /*
   public float radius;




  private void Start() {
      StartCoroutine(CheckIfInRange());
  }


  public IEnumerator CheckIfInRange() {
      while (gameObject.activeInHierarchy) {

          Collider2D[] targetsInViewRange = Physics2D.OverlapCircleAll(transform.position, radius, 9);

          if (targetsInViewRange.Length > 0) {
              GameManager.Instance.CompleteLevel();
          }

          yield return new WaitForSeconds(0.3f);
      }


  }

  */
}
