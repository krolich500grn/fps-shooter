using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform playerTransform;
    public float sensitivity = 2f;
    public float minXAngle = -30f;
    public float maxXAngle = 30f;
    public float smoothSpeed = 10f;

    private float rotationX = 0f;
    private float rotationY = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

  void Update()
  {
    LoadPreferences();
    HandleRotationInput();

    rotationX = Mathf.Clamp(rotationX, minXAngle, maxXAngle);

    Quaternion targetRotation = Quaternion.Euler(rotationX, rotationY, 0);

    playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, targetRotation, smoothSpeed * Time.deltaTime);
    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothSpeed * Time.deltaTime);

  }

  void HandleRotationInput()
  {
    float mouseX = Input.GetAxis("Mouse X") * sensitivity;
    float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

    rotationX -= mouseY;
    rotationY += mouseX;
  }

  void LoadPreferences()
  {
    if(PlayerPrefs.HasKey("SmoothSpeed"))
    {
      smoothSpeed = PlayerPrefs.GetFloat("SmoothSpeed");
      }

    if(PlayerPrefs.HasKey("Sensitivity"))
    {
      sensitivity = PlayerPrefs.GetFloat("Sensitivity");
    }
  }
}
