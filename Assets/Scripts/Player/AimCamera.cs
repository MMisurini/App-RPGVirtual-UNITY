using UnityEngine;
using System.Collections;

public class AimCamera : MonoBehaviour {
    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX = 2F;
    public float sensitivityY = 2F;
    Vector3 velocity = Vector3.zero;

    public float smoothTime = .5f;
    // For camera movement
    private Vector3 targetPos;

    private CameraAtributtes camAttributes;
    private Camera cam;

    private void OnEnable() {
        cam = GetComponent<Camera>();
    }

    void Update() {
        MouseInput();
    }

    void MouseInput() {
        targetPos = transform.position;

        if (camAttributes == null) 
            camAttributes = GameObject.Find("Maps").GetComponent<Resize>().cameraAtributtes;
        

        if (Input.GetMouseButton(0)) {

        } else if (Input.GetMouseButton(1)) {
        } else if (Input.GetMouseButton(2)) {
            MouseMiddleButtonClicked();
        } else if (Input.GetMouseButtonUp(1)) {
            ShowAndUnlockCursor();
        } else if (Input.GetMouseButtonUp(2)) {
            ShowAndUnlockCursor();
        } else {
            MouseWheeling();
        }

        if (camAttributes.size != 0) {
            targetPos.x = Mathf.Clamp(transform.position.x, X().x, X().y);
            targetPos.y = Mathf.Clamp(transform.position.y, Y().x, Y().y);
            targetPos.z = -6.2f;
        }

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
    }

    Vector2 X() {
        if (camAttributes.size == cam.orthographicSize) {
            return Vector2.zero;
        }
        float SizeX = camAttributes.pixelImageSizeX / 100;
        float CamSizeX = (SizeX/2 * cam.orthographicSize) / camAttributes.size;
        float value = (SizeX / 2) - CamSizeX;

        return new Vector2(-value, value);
    }
    Vector2 Y() {
        if (camAttributes.size == cam.orthographicSize) {
            return Vector2.zero;
        }
        float SizeY = camAttributes.pixelImageSizeY / 100;
        float CamSizeY = (SizeY / 2 * cam.orthographicSize) / camAttributes.size;
        float value = (SizeY / 2) - CamSizeY;

        return new Vector2(-value, value);
    }

    void ShowAndUnlockCursor() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void HideAndLockCursor() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void MouseMiddleButtonClicked() {
        HideAndLockCursor();
        Vector3 NewPosition = new Vector3(Input.GetAxis("Mouse X"), 0, Input.GetAxis("Mouse Y"));
        float speed = 5f;

        if (NewPosition.x > 0.0f) {
            targetPos -= transform.right * Time.deltaTime * speed;
        } else if (NewPosition.x < 0.0f) {
            targetPos += transform.right * Time.deltaTime * speed;
        }
        if (NewPosition.z > 0.0f) {
            targetPos -= transform.up * Time.deltaTime * speed;
        }
        if (NewPosition.z < 0.0f) {
            targetPos += transform.up * Time.deltaTime * speed;
        }

        transform.position = targetPos;
    }

    void MouseWheeling() {
        float pos = Camera.main.orthographicSize;

        if (Input.GetAxis("Mouse ScrollWheel") < 0) {
            pos += 0.1f;

            Camera.main.orthographicSize = Mathf.Clamp(pos, .5f, camAttributes.size);
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0) {
            pos -= 0.1f;

            Camera.main.orthographicSize = Mathf.Clamp(pos, .5f, 5);
        }
    }
}
