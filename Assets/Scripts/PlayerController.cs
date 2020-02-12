using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    public float speed = 25.0f;
    public float rotationSpeed = 90;
    public float force = 0.5f;
    public Score scoreManager;
 
    Rigidbody rb;
    Transform t;
 
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        t = GetComponent<Transform>();
        scoreManager = (Score)GameObject.FindWithTag("ScoreManager").GetComponent<Score>();
    }
 
    // Update is called once per frame
    void FixedUpdate()
    {
        if(!isLocalPlayer){
            return;
        }

        if (Input.GetKey(KeyCode.W) && (rb.velocity.magnitude < 10))
            rb.velocity += this.transform.forward * speed * Time.deltaTime;
        else if (Input.GetKey(KeyCode.S) &&  (rb.velocity.magnitude > -10))
            rb.velocity -= this.transform.forward * speed * Time.deltaTime;
 
        if (Input.GetKey(KeyCode.D))
            rb.velocity += this.transform.right * speed * Time.deltaTime;
        else if (Input.GetKey(KeyCode.A))
            rb.velocity -= this.transform.right * speed * Time.deltaTime;
       
        if (Input.GetKeyDown(KeyCode.Space) && (transform.position.y <= 18))
            rb.AddForce(t.up * force);
    }

    //make sure u replace "Plane" with your gameobject name.on which player is standing
    void OnCollisionEnter(Collision theCollision)
    {
        if(theCollision.gameObject.tag == "Egg")
        {
            if(isLocalPlayer){
                scoreManager.AddPoint();
            }
            NetworkServer.Destroy(theCollision.gameObject);
        }

        if(theCollision.gameObject.tag == "GoldenEgg")
        {
            if(isLocalPlayer){
                scoreManager.AddDoublePoints();
            }
            NetworkServer.Destroy(theCollision.gameObject);
        }
    }

    public override void OnStartLocalPlayer()
     {
        Camera.main.transform.parent = gameObject.transform;
        Camera.main.transform.position = gameObject.transform.position + new Vector3(0.6f,2.5f,-5.6f);
     }
}
