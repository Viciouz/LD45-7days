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
        
        // Add water once per collision, not per collision event
        if (leaf && numCollisionEvents > 0) {
            leaf.branch.water += Tree.Instance.waterPoints;
            
            // Only shake if not already shaking to avoid tween accumulation
            if (!DOTween.IsTweening(leaf.transform)) {
                leaf.transform.DOShakeRotation(0.3f);
            }
        }
    }
}
