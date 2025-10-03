using UnityEngine;

public class ZombieProximitySound : MonoBehaviour
{
    // Track player distance from zombie
    public Transform player;

    // Distance at which the sound will play
    public float activationDistance = 5f;

    private AudioSource audioSource;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogError("Cant find player");
        }
    }



    private void Update()
    {
        if (player == null || audioSource == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // player is close to the zombie, play sound
        if (distance <= activationDistance)
        {
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
        // player has moved out of zombie activation distance, stop sound
        else
        {
            if (audioSource.isPlaying)
                audioSource.Stop();
        }

    }
}
