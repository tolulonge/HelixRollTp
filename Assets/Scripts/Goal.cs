using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script handles moving to next level if classic or restart if time attack
public class Goal : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(GameManager.singleton.gameMode == "classic")
        {
            if (GameManager.singleton.currentStage <= 8)
            {
                GameManager.singleton.NextLevel();
            }
            else
            {
                GameManager.singleton.GameCompleted();
            }
        }
        else
        {
            GameManager.singleton.NextLevel();
        }
        
       
    }
}
