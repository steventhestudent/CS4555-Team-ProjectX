using System;
using System.Collections;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

// game state: start/end, player health + keycards
public class GameLoop : MonoBehaviour
{
    private Transform playerCanvas;
    public Transform[] players;
    public Transform[] remainingKeycards;
    public Transform[] keycards;

    void Start()
    {
        playerCanvas = players[0].Find("PlayerCamera").Find("PlayerCanvas");
    }
    
    // enable: keycard icon+count, heart1-5
    public void StartGame()
    {
        Debug.Log("start game???", playerCanvas.Find("KeycardCount").gameObject);
        playerCanvas.Find("KeycardCount").gameObject.SetActive(true);
        playerCanvas.Find("KeycardIcon").gameObject.SetActive(true);
        for (int i= 1; i < 6; i++) playerCanvas.Find("Heart" + i).gameObject.SetActive(true);
    }

    private int GameOutcome()
    {
        if (remainingKeycards.Length > 0) return -1;
        return 0; // return players[0].hp == 0 /* && players[1].hp == 0 */ ? -1 : 1;
    }
    
    public void EndGame()
    {
        if (GameOutcome() == 1) Debug.Log("Winner winner chicken dinner(s).");
        else Debug.Log("Game over, loser(s).");
    }
}
