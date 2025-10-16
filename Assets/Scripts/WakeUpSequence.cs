using UnityEngine;
using System.Collections;

public class WakeUpSequence : MonoBehaviour
{
    public float wakeUpDuration = 3f / 4;    // seconds to stand up
    public Transform standingReference;      // GameObject @ standing position/rotation
    public GameLoop gameLoop;
    private Side player;

    private void Awake()
    {
        gameLoop = Utils.GetGameLoop();
        player = gameLoop.GetSide(transform);
    }

    private void BeforeWakeUpStarted()
    {
        Utils.ToggleControls(player.t);
    }

    private void Start()
    {
        BeforeWakeUpStarted(); // disable player controls/input
        
        // stare at the ceiling
        StartCoroutine(ImmersionDelay());
        IEnumerator ImmersionDelay()
        {
            yield return new WaitForSeconds(0.333f);
            StartCoroutine(WakeUpRoutine());
        }
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
        Utils.ToggleControls(player.t);

        gameLoop.StartGame();
    }
}
