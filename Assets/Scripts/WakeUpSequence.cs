using UnityEngine;
using System.Collections;

public class WakeUpSequence : MonoBehaviour
{
    [Header("References")]
    public Transform standingReference;      // GameObject @ standing position/rotation
    public float wakeUpDuration = 3f;    // seconds to stand up
    public MonoBehaviour playerController;   // script to disable: PlayerControls.cs

    private CharacterController cc;
    public GameLoop gameLoop;

    private void Awake()
    {
        cc = transform.GetComponent<CharacterController>();
    }

    private void BeforeWakeUpStarted()
    {
        // disable player input + CharacterController so it doesn’t fight lerp
        if (playerController != null)  playerController.enabled = false;
        if (cc != null)  cc.enabled = false;
    }

    private void Start()
    {
        BeforeWakeUpStarted();
        StartCoroutine(WakeUpRoutine());
    }


    IEnumerator WakeUpRoutine()
    {
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        Vector3 endPos = standingReference.position;
        Quaternion endRot = standingReference.rotation;

        float elapsed = 0f;

        while (elapsed < wakeUpDuration)
        {
            float t = elapsed / wakeUpDuration;
            t = Mathf.SmoothStep(0f, 1f, t); // Ease in/out

            transform.position = Vector3.Lerp(startPos, endPos, t);
            transform.rotation = Quaternion.Slerp(startRot, endRot, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Snap to final
        transform.position = endPos;
        transform.rotation = endRot;

        WakeUpRoutineFinished();
    }

    private void WakeUpRoutineFinished()
    {
        // re-enable cc + playerControls.cs
        if (cc != null) cc.enabled = true;
        if (playerController != null) playerController.enabled = true;

        gameLoop.StartGame();
    }
}
