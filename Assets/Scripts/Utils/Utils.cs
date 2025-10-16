using UnityEngine;

public static class Utils
{

    public static GameLoop GetGameLoop()
    {
        return GameObject.Find("Settings").transform.Find("GameLoop").GetComponent<GameLoop>();
    }
    
    public static void ToggleControls(Transform playerTransform)
    {
        Side player = playerTransform.GetComponent<WakeUpSequence>().gameLoop.GetSide(playerTransform);
        
        if (!player.controls) Debug.Log("no controls script"); 
        else player.controls.enabled = !player.controls.enabled;
        
        if (!player.cc)  Debug.Log("no cc");
        else player.cc.enabled = !player.cc.enabled;
        
        if (!player.cam)  Debug.Log("no cam script");
        else player.cam.SetLookEnabled(!player.cam.IsLookEnabled());
    }
    
}