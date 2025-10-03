using UnityEngine;
using System.Collections;

public class WakeUpSequence : MonoBehaviour
{
    [Header("References")]
    public Transform player;                 // Root of your PlayerCharacter
    public Transform standingReference;      // Empty GameObject at standing position/rotation
    public float wakeUpDuration = 3f;    // Seconds to stand up
    public MonoBehaviour playerController;   // Script to disable (PlayerControls)

    private CharacterController cc;

    private void Awake()
    {
        if (player != null)
            cc = player.GetComponent<CharacterController>();
    }

    private void Start()
    {
        // Disable player input + CharacterController so it doesn’t fight your lerp
        if (playerController != null)  playerController.enabled = false;
        if (cc != null)  cc.enabled = false;
        StartCoroutine(WakeUpRoutine());
    }

    IEnumerator WakeUpRoutine()
    {
        Vector3 startPos = player.position;
        Quaternion startRot = player.rotation;

        Vector3 endPos = standingReference.position;
        Quaternion endRot = standingReference.rotation;

        float elapsed = 0f;

        while (elapsed < wakeUpDuration)
        {
            float t = elapsed / wakeUpDuration;
            t = Mathf.SmoothStep(0f, 1f, t); // Ease in/out

            player.position = Vector3.Lerp(startPos, endPos, t);
            player.rotation = Quaternion.Slerp(startRot, endRot, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Snap to final
        player.position = endPos;
        player.rotation = endRot;

        // Re-enable CharacterController + input
        if (cc != null) cc.enabled = true;
        if (playerController != null) playerController.enabled = true;
    }
}
