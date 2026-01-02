using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    // Cache to avoid redundant calculations
    private Camera mainCamera;
    
    private void Awake() {
        mainCamera = Camera.main;
    }
    
    void LateUpdate(){
        // Cache ScreenToWorldPoint result to avoid calling it twice
        Vector3 worldPoint = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(worldPoint.x, worldPoint.y, transform.position.z);
    }

}
