using UnityEngine;
using UnityEngine.Events;

public class OnObjectDestroy : MonoBehaviour
{
    public UnityEvent OnDestroyed;

    private void OnDestroy() {
        if (OnDestroyed != null) {
            OnDestroyed.Invoke();
        }
    }


}
