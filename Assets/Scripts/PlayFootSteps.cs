using UnityEngine;

public class PlayFootSteps : MonoBehaviour
{

    private CharacterController controller;  // Controls player movement and collision
    private AudioSource audioSource;

    void Start()
    {
        // get character controller componement that is attached to the player game object
        controller = GetComponent<CharacterController>();
    }
    void Update()
    {
      
    }
}
