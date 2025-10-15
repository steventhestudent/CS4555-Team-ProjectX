using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
public class PlayerCamera : MonoBehaviour
{
    public Camera playerCamera;

    private float xRotation = 0;

    public float xSens = 30f;
    public float ySens = 30f;
    public float kbXSens = 45f;
    public float kbYSens = 45f;
    private Vector2? keyboardLook;

    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    // This function is automatically called by Player Input component
    public void OnLook(InputValue value) { OnLookHandler(value.Get<Vector2>()); }

    // InputValue decays (when passed to fn (i.e.: keyboardLookRepeat)), hence the handler:
    public void OnLookHandler(Vector2 lookInput)
    {
        Vector2 lookInputAbs = new Vector2(Mathf.Abs(lookInput.x), Mathf.Abs(lookInput.y));
        // handle keyboard
        bool keyboardLookWasInactive = keyboardLook == null;
        keyboardLook = !keyboardLookWasInactive ? lookInput : (((lookInputAbs.x == 1 && lookInput.y == 0) ||
                                                               (lookInput.x == 0 && lookInputAbs.y == 1) || (lookInputAbs.x == 1 && lookInputAbs.y == 1)) ? lookInput : null);

        if (!keyboardLookWasInactive && lookInput.x == 0 && lookInput.y == 0)
        {
            keyboardLook = null;
            // Debug.Log("keyboardLook finished");
        }
        // else if (keyboardLook != null) print("iskeyboard");
        // else print("notkeyboard");
        
        if (keyboardLookWasInactive && keyboardLook != null) StartCoroutine(keyboardLookRepeat());
        if (keyboardLook != null) lookInput = new Vector2(lookInput.x * kbXSens, lookInput.y * kbYSens);
        
        // Apply sensitivity
        float mouseX = lookInput.x * xSens * Time.deltaTime;
        float mouseY = lookInput.y * ySens * Time.deltaTime;

        // Rotate the player body left and right (Y-axis)
        transform.Rotate(Vector3.up * mouseX);

        // Handle vertical camera rotation (X-axis) with clamping
        xRotation -= mouseY; // Subtract to invert Y-axis
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Apply the clamped rotation to the camera
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
    
    private IEnumerator keyboardLookRepeat()
    {
        while (keyboardLook != null) // Continue until the condition is met
        {
            OnLookHandler(keyboardLook ?? Vector2.zero);
            yield return new WaitForSeconds(0.025f);
        }
    }
}
