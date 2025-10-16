using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Side
{
    public Transform t;
    public Transform canvas;
    public Camera cam;
    public CharacterController cc;
    public PlayerHealth health;
    
    public Side(Transform _t)
    {
        t = _t;
        canvas = t.Find("PlayerCamera").Find("PlayerCanvas");
        cam = t.GetComponent<Camera>();
        cc = t.GetComponent<CharacterController>();
        health = t.GetComponent<PlayerHealth>();
    }
}
