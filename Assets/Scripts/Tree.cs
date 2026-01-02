using TMPro;
using UnityEngine;

public class Tree : MonoBehaviour {

    public float waterPoints = 5f;
    public TextMeshProUGUI scoreText;

    [Header("Branch")]
    public float growthSpeedHeight;
    public float growthSpeedWidth;
    public float branchStartSize;
    public float branchStopSize;
    public float branchChance = 10f;
    public float waterRequired = 50f;
    public float branchAngle = 90f;
    public int maxBranches;

    public AudioClip branchSFX;

    [Header("Leaf")]
    public float leafChance = 10f;
    public float leafLife = 300f;
    public int maxLeaves;

    // Object pools to reduce instantiation overhead and GC pressure
    private ObjectPool<Branch> branchPool;
    private ObjectPool<Leaf> leafPool;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    public static Tree Instance { get; set; }
    public GameObject branchPrefab;
    public GameObject leafPrefab;


    /// <summary>
    /// Initializes object pools for branches and leaves.
    /// Call this before spawning any objects.
    /// </summary>
    public void InitializePools() {
        if (branchPool == null && branchPrefab != null) {
            branchPool = new ObjectPool<Branch>(branchPrefab, transform, 5);
        }
        if (leafPool == null && leafPrefab != null) {
            leafPool = new ObjectPool<Leaf>(leafPrefab, transform, 10);
        }
    }

    public ObjectPool<Branch> GetBranchPool() {
        if (branchPool == null) InitializePools();
        return branchPool;
    }

    public ObjectPool<Leaf> GetLeafPool() {
        if (leafPool == null) InitializePools();
        return leafPool;
    }

    public GameObject SpawnBranch(Branch branch) {
        // Use object pool instead of Instantiate to reduce GC pressure
        Branch b = GetBranchPool().Get();
        GameObject branchObj = b.gameObject;
        
        branchObj.transform.SetParent(branch.transform);
        branchObj.transform.position = branch.transform.position;
        branchObj.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(branchAngle * -1, branchAngle));

        GameObject rootObj = new GameObject("RootBranch");
        rootObj.transform.SetParent(branch.transform);
        rootObj.transform.position = branch.transform.position;

        b.rootTransform = rootObj.transform;
        b.parentBranch = branch;

      //  SoundManager.PlayRandomSfx(branchSFX);

        return branchObj;
    }


}
