using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Water : MonoBehaviour
{

    public ParticleSystem part;
    // Pre-allocated list to avoid GC allocations on each collision
    public List<ParticleCollisionEvent> collisionEvents;

    void Start() {
        part = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    void OnParticleCollision(GameObject other) {
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);

        Leaf leaf = other.GetComponent<Leaf>();
        
        // Optimized loop - only process if leaf exists
        if (leaf) {
            // Process all collision events efficiently
            for (int i = 0; i < numCollisionEvents; i++) {
                leaf.branch.water += Tree.Instance.waterPoints;
            }
            // Consolidate shake animation instead of per-collision
            leaf.transform.DOShakeRotation(0.3f);
        }
    }
}
