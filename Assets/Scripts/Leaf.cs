using DG.Tweening;
using System.Collections;
using UnityEngine;

public enum State { Healthy, Decaying, Dying }

public class Leaf : MonoBehaviour {

    public float life;

    public SpriteRenderer sprite;

    public Branch branch;

    public State state = State.Healthy;

    public bool active = true;

    public Color healthyColor;
    public Color decayingColor;
    public Color dyingColor;

    public Rigidbody2D body;

    // Time step optimization - avoid frame-by-frame updates
    private readonly float timeStep = 0.1f;
    private float time = 0;

    private void OnEnable() {
        // Reset state when object is retrieved from pool
        active = true;
        state = State.Healthy;
        time = 0f;
        
        // Initialize life and scale animation
        life = Tree.Instance.leafLife;
        transform.localScale = Vector3.zero;
        
        // Reset physics state
        if (body != null) {
            body.isKinematic = true;
            body.velocity = Vector2.zero;
        }
        
        // Combine DOTween animations into a sequence for better performance
        Sequence scaleSequence = DOTween.Sequence();
        scaleSequence.Append(transform.DOBlendableScaleBy(new Vector3(1f, 1f), 0.2f));
        scaleSequence.Append(transform.DOPunchScale(new Vector3(0.3f, 0.3f), 0.5f));
    }

    private void Start() {
        if (branch == null) {
            branch = transform.parent.GetComponent<Branch>();
        }
    }

    private void Update() {
        if (!active) return;
        
        time += Time.deltaTime;
        if (time > timeStep) {
            time = 0f;
            
            // Process leaf lifecycle
            if (branch.water > 0f) {
                branch.Grow();
                life++;
                branch.water--;
                
                // Only update color if state changed
                if (state != State.Healthy) {
                    sprite.DOColor(healthyColor, 0.5f);
                    state = State.Healthy;
                }
            } else if (life <= 0f) {
                branch.DeathOfABeutifulLeaf(gameObject);
                StartCoroutine(BreakLeaf());
                branch.water += Tree.Instance.waterPoints;
            } else {
                // Decay logic with state transitions
                if (life < 50f && state == State.Decaying) {
                    sprite.DOColor(dyingColor, 0.5f);
                    state = State.Dying;
                } else if (life < 100f && state == State.Healthy) {
                    sprite.DOColor(decayingColor, 0.5f);
                    state = State.Decaying;
                } else if (life > 100f && state != State.Healthy) {
                    sprite.DOColor(healthyColor, 0.5f);
                } else {
                    life--;
                }
            }
        }
    }

    public IEnumerator BreakLeaf() {
        active = false;
        
        if(body != null) {
            body.isKinematic = false;
            body.AddForce(Vector2.up * 2f, ForceMode2D.Impulse);
            yield return new WaitForSeconds(1f);
        }
        
        // Use DOTween callback to avoid extra coroutine wait
        // Object is returned to pool in DeathOfABeutifulLeaf instead of destroyed
        transform.DOScale(0f, 0.3f);
        yield return new WaitForSeconds(0.3f);
    }
}
