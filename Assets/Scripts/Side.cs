using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Side
{
    public Transform t;
    public Transform canvas;
    public PlayerCamera cam; // PlayerCamera.cs (script)
    public CharacterController cc;
    public PlayerControls controls; // PlayerControls.cs (script)
    public PlayerHealth health;
    
    public Side(Transform _t)
    {
        t = _t;
        canvas = t.Find("PlayerCamera").Find("PlayerCanvas");
        cam = t.GetComponent<PlayerCamera>();
        cc = t.GetComponent<CharacterController>();
        controls = t.GetComponent<PlayerControls>();
        health = t.GetComponent<PlayerHealth>();
    }
}
