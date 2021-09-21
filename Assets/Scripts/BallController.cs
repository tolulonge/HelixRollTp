using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    private bool ignoreNextCollision;
    public Rigidbody rb;
    public float impulseForce = 5f;
    private Vector3 startPos;
    public int perfectPass = 0;
    public bool isSuperSpeedActive;
    private float gravityModifier = 2f;
  


    public AudioClip bounceSound;
    public AudioClip collideSound;


    private AudioSource playerAudio;

    public static bool isFollowPlayer = true;



    // Start is called before the first frame update
    void Awake()
    {
      
        startPos = transform.position;
        Physics.gravity *= gravityModifier;
        playerAudio = GetComponent<AudioSource>();
        isFollowPlayer = true;
    }

    // Update is called once per frame
    void Update()
    {

        if(perfectPass >= 2 && !isSuperSpeedActive)
        {
            isSuperSpeedActive = true;
            
            rb.AddForce(Vector3.down * 10, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
      
        if (ignoreNextCollision)
            return;

        if (isSuperSpeedActive)
        {
            if (!collision.transform.GetComponent<Goal>())
            {
                Destroy(collision.transform.parent.gameObject, 0.3f);
                playerAudio.PlayOneShot(collideSound);
                isFollowPlayer = true;
            }
        }
        else
        {
            // Adding resetLevel functionality via Deathpart - initialized when deathPart hit
            DeathPart deathPart = collision.transform.GetComponent<DeathPart>();
            if (deathPart)
                deathPart.HitDeathPart();
        }


        playerAudio.PlayOneShot(bounceSound);
        rb.velocity = Vector3.zero;
        rb.AddForce(Vector3.up * impulseForce, ForceMode.Impulse);

        ignoreNextCollision = true;
        Invoke("AllowCollision", .2f);

        perfectPass = 0;
        
        if(!isSuperSpeedActive)
        isFollowPlayer = false;
        isSuperSpeedActive = false;

    }

    private void AllowCollision()
    {
        ignoreNextCollision = false;
    }

    public void ResetBall()
    {
        isFollowPlayer = true;
        transform.position = startPos;
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x,  FindObjectOfType<CameraController>().offset + 1.2f,Camera.main.transform.position.z);
    }
}
