using System.Collections;
using UnityEngine;

/// <summary>
/// Controls the laser beam visualization and lifetime.
/// Handles the LineRenderer component to draw a laser from start to end point.
/// </summary>
public class LaserBeam : MonoBehaviour
{
    [Header("Laser Settings")]
    [Tooltip("Duration the laser remains visible before being destroyed")]
    public float lifetime = 0.5f;

    private LineRenderer lineRenderer;

    private void Awake()
    {
        // Get or add LineRenderer component
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }
    }

    private void Start()
    {
        // Configure LineRenderer settings
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.positionCount = 2;
        
        // Destroy the laser after the specified lifetime
        Destroy(gameObject, lifetime);
    }

    /// <summary>
    /// Sets the laser's start and end positions
    /// </summary>
    /// <param name="startPosition">The position where the laser originates</param>
    /// <param name="endPosition">The position where the laser ends</param>
    public void SetPositions(Vector3 startPosition, Vector3 endPosition)
    {
        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(0, startPosition);
            lineRenderer.SetPosition(1, endPosition);
        }
    }
}
