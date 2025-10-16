using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpGun : MonoBehaviour
{
    public GameObject GunOnPlayer;   // The gun model on the player
    private bool isEquipped = false; // Tracks if gun is equipped

    void Start()
    {
        GunOnPlayer.SetActive(false); // Start without the gun equipped
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Press E to pick up if not equipped
            if (Input.GetKeyDown(KeyCode.E) && !isEquipped)
            {
                PickUp(other.transform);
            }
        }
    }

    void Update()
    {
        // Press Q to drop if equipped
        if (isEquipped && Input.GetKeyDown(KeyCode.Q))
        {
            Drop();
        }
    }

    private void PickUp(Transform player)
    {
        this.gameObject.SetActive(false);  // Hide world gun
        GunOnPlayer.SetActive(true);       // Show gun on player
        isEquipped = true;
    }

    private void Drop()
    {
        GunOnPlayer.SetActive(false);  // Hide equipped gun
        this.gameObject.SetActive(true);   // Show world gun again

        // Place it slightly in front of the player
        Transform player = GunOnPlayer.transform.parent;
        this.transform.position = player.position + player.forward * 1f + Vector3.up * 0.5f;

        isEquipped = false;
    }
}
