using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Handles coloring the death parts
public class DeathPart : MonoBehaviour
{
    
    private void OnEnable()
    {
        GetComponent<Renderer>().material.color = Color.red;
    }

    public void HitDeathPart()
    {
        if (GameManager.singleton.gameMode == "timed")
        {
            GameManager.singleton.RestartLevel();
            
           
        }
        else GameManager.singleton.GameOver();
       


    }
}
