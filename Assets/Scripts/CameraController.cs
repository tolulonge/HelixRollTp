using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This script handles following the player with the camera
public class CameraController : MonoBehaviour
{
    public BallController target;
    public float offset;
    private BallController ballController;

    private void Awake()
    {
        offset = transform.position.y - target.transform.position.y;
        ballController = GameObject.FindObjectOfType(typeof(BallController)) as BallController;
    }

    // Update is called once per frame
    void Update()
    {

        if (BallController.isFollowPlayer)
        {
            Vector3 currPos = transform.position;
            currPos.y = target.transform.position.y + offset + 1.2f;
            transform.position = currPos;
        }

      
    }
}
