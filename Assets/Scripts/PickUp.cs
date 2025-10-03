using UnityEngine;
using UnityEngine.InputSystem;

public class PickUp : MonoBehaviour
{
    public float pickupRange = 3f;
    public Transform holdParent;

    private GameObject heldItem;
    public Camera playerCamera; // assign in inspector

    // Called automatically by PlayerInput
    public void OnInteract()
    {
        if (heldItem != null)
        {
            DropItem();
            return;
        }

        // Raycast from the camera
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
        {
            if (hit.collider.CompareTag("PickUp"))
            {
                heldItem = hit.collider.gameObject;

                // Parent it to hold point
                heldItem.transform.SetParent(holdParent);
                heldItem.transform.localPosition = Vector3.zero;
                heldItem.transform.localRotation = Quaternion.identity;

                Rigidbody rb = heldItem.GetComponent<Rigidbody>();
                if (rb != null) rb.isKinematic = true;
            }
        }
    }

    private void DropItem()
    {
        if (heldItem == null) return;

        Rigidbody rb = heldItem.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = false;

        heldItem.transform.SetParent(null);
        heldItem = null;
        Debug.Log("Item Dropped");
    }
}
