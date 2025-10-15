using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using TMPro;

// game state: start/end, player health + keycards
public class GameLoop : MonoBehaviour
{
    private Transform playerCanvas;
    public Transform[] players;
    public Transform[] remainingKeycards;
    public List<Transform> keycards = new List<Transform>();

    void Start()
    {
        playerCanvas = players[0].Find("PlayerCamera").Find("PlayerCanvas");
    }
    
    public void StartGame()
    {
        // enable: Keycard icon & count
        playerCanvas.Find("KeycardCount").gameObject.SetActive(true);
        playerCanvas.Find("KeycardIcon").gameObject.SetActive(true);
        // Heart1-5
        for (int i= 1; i < 6; i++) playerCanvas.Find("Heart" + i).gameObject.SetActive(true);
    }

    private int GameOutcome()
    {
        if (keycards.Count != remainingKeycards.Length) return -1;
        return 0; // return players[0].hp == 0 /* && players[1].hp == 0 */ ? -1 : 1;
    }
    
    public void EndGame()
    {
        if (GameOutcome() == 1) Debug.Log("Winner winner chicken dinner(s).");
        else Debug.Log("Game over, loser(s).");
    }

    public void RegisterKeycard(Transform keycard)
    {
        keycards.Add(keycard);
        //todo: remove from remainingKeycards
        playerCanvas.Find("KeycardCount").GetComponent<TextMeshProUGUI>().text = keycards.Count + "";
    }
}
