using UnityEngine;
using Cinemachine;

public class CameraSystem : MonoBehaviour
{
    [SerializeField] private bool isEdgdeScrolling = true;
    [SerializeField] private bool isCameraZooming = true;
    [SerializeField] private bool isCameraRotating = true;
    [SerializeField] private float scrollSize = 25;
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] private float targetFieldOfView = 50f;
    [SerializeField] private float maxZoom = 50f;
    [SerializeField] private float minZoom = 10f;
    [SerializeField] private float cameraRotationSpeed = 40f;
    [SerializeField] private float cameraZoomSpeed = 5f;
    [SerializeField] private float cameraMovementSpeed = 50f;

    private void Update() {
        HandleCameraMovement();

        if(isEdgdeScrolling) {
            HandleEdgeScrolling();
        }

        if (isCameraZooming) {
            HandleCameraZoom();
        }

        if (isCameraRotating) {
            HandleCameraRotation();
        }
    }

    private void HandleEdgeScrolling() {
        Vector3 inputDirection = new Vector3(0, 0, 0);

        if (Input.mousePosition.x < scrollSize) {
            inputDirection.x = -1f;
        }

        if (Input.mousePosition.x > Screen.width - scrollSize) {
            inputDirection.x = +1f;
        }

        if (Input.mousePosition.y < scrollSize) {
            inputDirection.z = -1f;
        }

        if (Input.mousePosition.y > Screen.height - scrollSize) {
            inputDirection.z = +1f;
        }

        UpdateCameraPosition(inputDirection);
    }

    private void UpdateCameraPosition(Vector3 inputDirection) {
        Vector3 moveDirection = transform.forward * inputDirection.z + transform.right * inputDirection.x;

        transform.position += moveDirection * cameraMovementSpeed * Time.deltaTime;
    }

    private void HandleCameraMovement() {
        Vector3 inputDirection = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W)) {
            inputDirection.z = +1f;
        }

        if (Input.GetKey(KeyCode.S)) {
            inputDirection.z = -1f;
        }

        if (Input.GetKey(KeyCode.A)) {
            inputDirection.x = -1f;
        }

        if (Input.GetKey(KeyCode.D)) {
            inputDirection.x = +1f;
        }

        UpdateCameraPosition(inputDirection);
    }

    private void HandleCameraZoom() {
        if (Input.mouseScrollDelta.y > 0) {
           targetFieldOfView -= 5f;
        }
        if (Input.mouseScrollDelta.y < 0) {
            targetFieldOfView += 5f;
        }

        targetFieldOfView = Mathf.Clamp(targetFieldOfView, minZoom, maxZoom);
        cinemachineVirtualCamera.m_Lens.FieldOfView = Mathf.Lerp(cinemachineVirtualCamera.m_Lens.FieldOfView, targetFieldOfView, Time.deltaTime * cameraZoomSpeed);
    }

    private void HandleCameraRotation() {
        float rotateDirection = 0f;

        if (Input.GetKey(KeyCode.Q)) {
            rotateDirection = +1f;
        }
        if (Input.GetKey(KeyCode.E)) {
            rotateDirection = -1f;
        }

        transform.eulerAngles += new Vector3(0, rotateDirection * cameraRotationSpeed * Time.deltaTime, 0);
    }
 }
