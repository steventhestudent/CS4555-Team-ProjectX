using System.Collections.Generic;
using UnityEngine;
using TMPro;

// game state: start/end, player health + keycards
public class GameLoop : MonoBehaviour
{
    public Transform[] playerObjects;
    public Transform[] scatteredKeycards;
    private List<Transform> keycards = new List<Transform>();
    private List<Side> players = new List<Side>();

    void Start()
    {
        for (int i = 0 ; i < playerObjects.Length; i++) players.Add(new Side(playerObjects[i]));
        //todo: n=2 players... seee if can duplicate Player n times (and have n cameras)
    }
    
    public void StartGame()
    {
        // enable: Keycard icon & count
        players[0].canvas.Find("KeycardCount").gameObject.SetActive(true);
        players[0].canvas.Find("KeycardIcon").gameObject.SetActive(true);
        // Heart1-5
        for (int i= 1; i < 6; i++) players[0].canvas.Find("Heart" + i).gameObject.SetActive(true);
    }

    private int GameOutcome()
    {
        if (keycards.Count != scatteredKeycards.Length) return -1;
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
        players[0].canvas.Find("KeycardCount").GetComponent<TextMeshProUGUI>().text = keycards.Count + "";
    }
}
