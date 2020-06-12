using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float cameraMovementDamping = .5f;
    Transform player;
    Transform cam;

    void Start()
    {
        player = GameObject.FindObjectOfType<PlayerMovement>().transform;
        cam = transform;
    }

    // Update the camera position by lerping
    void Update()
    {
        Vector2 newCameraPosition = Vector2.Lerp(cam.position, player.position, cameraMovementDamping);
        cam.position = new Vector3(newCameraPosition.x, newCameraPosition.y, cam.position.z);
    }
}
