using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Adds score on each passed check
public class PassCheck : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameManager.singleton.AddScore(2);
        FindObjectOfType<BallController>().perfectPass++;
        BallController.isFollowPlayer = true;
    }
}
