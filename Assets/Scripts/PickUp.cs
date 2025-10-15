using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IWeapon
{
    void Fire();
}

public class PickUp : MonoBehaviour
{
    private readonly HashSet<string> pickUpTags = new HashSet<string> { "PickUp" /* , "Weapon" */};
    public float pickupRange = 3f;
    public Transform holdParent;
    public Camera playerCamera; // assign in inspector

    private GameObject heldItem;
    private IWeapon heldWeapon; // interface to allow WeaponGun, WeaponSword, etc.
    public GameLoop gameLoop;

    // Called automatically by PlayerInput
    public void OnInteract()
    {
        if (heldItem != null)
        {
            DropItem();
            return;
        }

        // Raycast from the cameraf
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
        {
            if (pickUpTags.Contains(hit.collider.tag))
            {
                heldItem = hit.collider.gameObject;

                // Parent it to hold point
                heldItem.transform.SetParent(holdParent);
                heldItem.transform.localPosition = Vector3.zero;
                heldItem.transform.localRotation = Quaternion.identity;

                Rigidbody rb = heldItem.GetComponent<Rigidbody>();
                if (rb != null) rb.isKinematic = true;

                // See if this item has a weapon script
                heldWeapon = heldItem.GetComponent<IWeapon>();

                OnItemHeld(hit);
            }
        }
    }

    private void OnItemHeld(RaycastHit hit)
    {
        if (hit.collider.gameObject.name.StartsWith("Keycard"))
        {
            gameLoop.RegisterKeycard(hit.collider.transform);
            DropItem(); //drop immediately after
            // Disappear
            StartCoroutine(Disappear());
            IEnumerator Disappear()
            {
                yield return new WaitForSeconds(1.2f);
                hit.collider.gameObject.SetActive(false);
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
        heldWeapon = null;
        Debug.Log("Item Dropped");
    }

    void Update()
    {
        // Shoot if holding a weapon
        if (heldWeapon != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            heldWeapon.Fire();
        }
    }
}