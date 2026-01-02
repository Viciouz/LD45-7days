using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generic object pool implementation to reduce instantiation overhead and garbage collection.
/// Pools reusable GameObjects to improve performance by avoiding frequent Instantiate/Destroy calls.
/// </summary>
/// <typeparam name="T">Component type that must inherit from MonoBehaviour</typeparam>
public class ObjectPool<T> where T : MonoBehaviour {
    private readonly GameObject prefab;
    private readonly Transform parent;
    private readonly Queue<T> pool = new Queue<T>();
    private readonly int initialSize;

    public ObjectPool(GameObject prefab, Transform parent = null, int initialSize = 10) {
        this.prefab = prefab;
        this.parent = parent;
        this.initialSize = initialSize;
        
        // Pre-populate pool to avoid allocations during gameplay
        for (int i = 0; i < initialSize; i++) {
            CreateNewObject();
        }
    }

    private T CreateNewObject() {
        GameObject obj = Object.Instantiate(prefab, parent);
        T component = obj.GetComponent<T>();
        obj.SetActive(false);
        pool.Enqueue(component);
        return component;
    }

    /// <summary>
    /// Gets an object from the pool or creates a new one if pool is empty
    /// </summary>
    public T Get() {
        T obj = pool.Count > 0 ? pool.Dequeue() : CreateNewObject();
        obj.gameObject.SetActive(true);
        return obj;
    }

    /// <summary>
    /// Returns an object to the pool for later reuse
    /// </summary>
    public void Return(T obj) {
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }

    /// <summary>
    /// Clears and destroys all pooled objects
    /// </summary>
    public void Clear() {
        while (pool.Count > 0) {
            T obj = pool.Dequeue();
            if (obj != null) {
                Object.Destroy(obj.gameObject);
            }
        }
    }
}
