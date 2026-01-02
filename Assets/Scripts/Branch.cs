using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Branch : MonoBehaviour {
    public LineRenderer lineRenderer;
    public List<GameObject> flowerPrefabs;
    public Transform rootTransform;

    public float radius;
    public float water;

    public AudioClip branchDeathSFX;
    public AudioClip[] flowerSpawnSFX;

    public Branch parentBranch;

    public List<GameObject> leafs;
    public List<GameObject> branches;

    public Rigidbody2D body;

    private void OnEnable() {
        // Reset state when object is retrieved from pool
        water = 0f;
        radius = 0f;
        
        // Initialize lists if null
        if (leafs == null) leafs = new List<GameObject>();
        if (branches == null) branches = new List<GameObject>();
        
        leafs.Clear();
        branches.Clear();
        
        // Reset physics state
        if (body != null) {
            body.isKinematic = true;
            body.velocity = Vector2.zero;
        }
        
        // Reset scale
        transform.localScale = Vector3.one;
    }

    public void Grow() {
        float distance = Vector3.Distance(lineRenderer.GetPosition(0), lineRenderer.GetPosition(1));

        GrowSize(distance);
        if (water > Tree.Instance.waterRequired) {
            SpawnBranch(distance);
            SpawnLeaf();
        }

    }

    public void GrowSize(float distance) {
        if (distance < Tree.Instance.branchStopSize / 2) {
            transform.position += Quaternion.Euler(0f, 0f, Random.Range(Tree.Instance.branchAngle * -1, Tree.Instance.branchAngle)) * transform.up * Tree.Instance.growthSpeedHeight * Time.deltaTime;
            lineRenderer.startWidth += Tree.Instance.growthSpeedWidth * Time.deltaTime;
        } else if (distance < Tree.Instance.branchStopSize) {
            transform.position += Quaternion.Euler(0f, 0f, Random.Range(Tree.Instance.branchAngle * -1, Tree.Instance.branchAngle)) * transform.up * (Tree.Instance.growthSpeedHeight / 5) * Time.deltaTime;
            lineRenderer.startWidth += (Tree.Instance.growthSpeedWidth / 5) * Time.deltaTime;
        } else {
            lineRenderer.startWidth += (Tree.Instance.growthSpeedWidth / 10) * Time.deltaTime;
        }
    }

    public void SpawnBranch(float distance) {
        if (distance > Tree.Instance.branchStartSize && branches.Count < Tree.Instance.maxBranches && Random.Range(0, 100f) < Tree.Instance.branchChance) {
            branches.Add(Tree.Instance.SpawnBranch(this));
        }
    }

    public void SpawnLeaf() {
        if (Random.Range(0, 100f) < Tree.Instance.leafChance && leafs.Count < Tree.Instance.maxLeaves) {
            // Use object pool instead of Instantiate to reduce GC pressure
            Leaf leaf = Tree.Instance.GetLeafPool().Get();
            GameObject leafObj = leaf.gameObject;
            
            leafObj.transform.SetParent(transform);
            leafObj.transform.position = transform.position;
            leafObj.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(Tree.Instance.branchAngle * -1, Tree.Instance.branchAngle));
            
            leaf.branch = this;
            leafs.Add(leafObj);
        } else if (leafs.Count == Tree.Instance.maxLeaves && leafs.Count < Tree.Instance.maxLeaves + 2) {
            GameObject flowerObject = Instantiate(flowerPrefabs[Random.Range(0, flowerPrefabs.Count)], transform.position + new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-1f, 1f)), Quaternion.Euler(0f, 0f, Random.Range(Tree.Instance.branchAngle * -1, Tree.Instance.branchAngle)), transform);
            leafs.Add(flowerObject);
            SoundManager.PlayRandomSfx(flowerSpawnSFX);
            GameManager.Instance.AddScore();
        }
    }

    public void DeathOfABeutifulLeaf(GameObject leaf) {
        leafs.Remove(leaf);
        
        // Note: Leaf will return itself to pool after animation completes in BreakLeaf
        
        if (leafs.Count <= 0) {
            if (parentBranch != null) {
                StartCoroutine(BreakBranch());
            }
        }
    }

    public IEnumerator BreakBranch() {

        rootTransform.SetParent(transform);
        SoundManager.PlayRandomSfx(branchDeathSFX);

        if (body != null) {
            body.isKinematic = false;
            body.AddForce(Vector2.up * 2f, ForceMode2D.Impulse);   
            yield return new WaitForSeconds(1f);
        }

        // Scale down animation
        transform.DOScale(0f, 0.3f);
        yield return new WaitForSeconds(0.3f);
        
        // Cleanup and return to pool
        parentBranch.branches.Remove(gameObject);
        
        // Cleanup root object before returning to pool
        if (rootTransform != null) {
            Destroy(rootTransform.gameObject);
            rootTransform = null;
        }
        
        // Return branch to pool
        Tree.Instance.GetBranchPool().Return(this);

    }

    private void Update() {
        // Update LineRenderer with current positions
        // Note: LineRenderer needs continuous updates as branch grows
        lineRenderer.SetPosition(0, rootTransform.position);
        lineRenderer.SetPosition(1, transform.position);
    }

    private void OnDisable() {
        // Kill any active tweens when object is returned to pool
        transform.DOKill();
    }


}
