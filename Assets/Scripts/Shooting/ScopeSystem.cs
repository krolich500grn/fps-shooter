using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScopeSystem : MonoBehaviour
{
    public GameObject scopeImage;
    public GameObject playerUI;
    public DeathScreen deathScreen;
    public float zoomSpeed = 2f;
    public Camera mainCamera;
    public Camera scopeCamera;
    private bool isScoped = false;
    private float originalFOV;

    void Start()
    {
        if (scopeCamera != null)
        {
            scopeCamera.enabled = false;
        }

        if (mainCamera != null)
        {
            originalFOV = mainCamera.fieldOfView;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isScoped = !isScoped;
            scopeImage.SetActive(isScoped);

            if (isScoped)
            {
              if (deathScreen.showDeathScreen) {
                isScoped = !isScoped;
                return;
              }

              playerUI.SetActive(false);
              mainCamera.gameObject.SetActive(false);
              scopeCamera.gameObject.SetActive(true);

              if (scopeCamera != null)
              {
                  scopeCamera.enabled = true;
                  scopeCamera.fieldOfView = originalFOV / zoomSpeed;
              }
            }
            else
            {
                playerUI.SetActive(true);
                mainCamera.gameObject.SetActive(true);
                scopeCamera.gameObject.SetActive(false);

                if (scopeCamera != null)
                {
                    scopeCamera.enabled = false;
                    scopeCamera.fieldOfView = originalFOV;
                }
            }
        }

        if (isScoped && scopeCamera != null)
        {
            float zoom = Input.GetAxis("Mouse ScrollWheel");
            scopeCamera.fieldOfView -= zoom * zoomSpeed * 10f;
            scopeCamera.fieldOfView = Mathf.Clamp(scopeCamera.fieldOfView, 10f, 80f);
        }
    }
}
