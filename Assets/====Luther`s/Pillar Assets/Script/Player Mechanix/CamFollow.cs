using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    public Camera mainCamera;
    public float startSize = 15f;
    public float targetSize = 5f;
    public float zoomSpeed = 2f;
    public float smoothSpeed = 0.125f;
    
    // Start is called before the first frame update
    void LateUpdate()
    {
        Vector3 desiredPos = player.position + offset;
        Vector3 smoothPos = Vector3.Lerp(transform.position, desiredPos , smoothSpeed); 
        transform.position = new Vector3(smoothPos.x, smoothPos.y ,smoothPos.z = -1);    // Z = -1
    }
    void Start()
    {
        mainCamera.orthographicSize = startSize;

    }

    private void Update()
    {
        // Gradually reduce the camera size to the target size
        if (mainCamera.orthographicSize > targetSize)
        {
            mainCamera.orthographicSize -= zoomSpeed * Time.deltaTime;
            mainCamera.orthographicSize = Mathf.Max(mainCamera.orthographicSize, targetSize); // Clamp to target size
        }
    }
}
