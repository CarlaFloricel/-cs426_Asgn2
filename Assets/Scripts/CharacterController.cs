using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class CharacterController : NetworkBehaviour
{
    public float speed = 30.0f;
    public float rotationSpeed = 150;
    public float force;
    public float forceSpaceHeld = 2f;
    public int    jumps = 0;
    public int maxJumps = 4;
    public float waitTimeTillNextJump = 1.8f;
    public GameObject cannon;
    public GameObject bullet;
    private bool isFrozen = false;
    private float timeFrozen = 0;
    Rigidbody rb;
    Transform t;
    public Score scoreManager;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        t = GetComponent<Transform>();
        //StartCoroutine(waiter());
        rb.drag = 3f;
        scoreManager = (Score)GameObject.FindWithTag("ScoreManager").GetComponent<Score>();
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, 0.1f);
    }

    // Everyone Second decrease jump
    IEnumerator waiter()
    {
        if(jumps > 0)
            jumps -= 1;

        yield return new WaitForSeconds(waitTimeTillNextJump);

        StartCoroutine(waiter());
    }

    // Update is called once per frame
    void Update()
    {

        if(!isLocalPlayer){
            return;
        }

        //If the player is frozen, return
        //This prevents them from doing anything
        if(isFrozen){
            timeFrozen += Time.deltaTime;
            if(timeFrozen > 10.0f){
                isFrozen = false;
            }
            else{
                return;
            }
        }
        
        if (Input.GetKey(KeyCode.W))
            rb.velocity += this.transform.forward * speed * Time.deltaTime;
        else if (Input.GetKey(KeyCode.S))
            rb.velocity -= this.transform.forward * speed * Time.deltaTime;

        if (Input.GetKey(KeyCode.D))
            rb.velocity += transform.right * speed * Time.deltaTime;
        else if (Input.GetKey(KeyCode.A))
            rb.velocity += -transform.right * speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.M))
            SceneManager.LoadScene(0);


        if (IsGrounded())
        {
            jumps = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space) && jumps < maxJumps)
        {
            if(jumps == 0)
                force = 500f;
            else
                force = 400f;
            jumps += 1;
            rb.AddForce(t.up * force);
        }

        if (Input.GetKey(KeyCode.Space))
            rb.AddForce(t.up * forceSpaceHeld);
        else
            rb.AddForce(-t.up * 3f * forceSpaceHeld);

        if (Input.GetButtonDown("Fire1")){
            CmdShoot();
        }
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

        if(theCollision.gameObject.tag == "Bullet"){
            isFrozen = true;
            timeFrozen = 0f;
        }
        
    }

    [Command]
    void CmdShoot(){
        GameObject newBullet = GameObject.Instantiate(bullet, cannon.transform.position, cannon.transform.rotation) as GameObject;
        newBullet.GetComponent<Rigidbody>().velocity += Vector3.up * 3;
        newBullet.GetComponent<Rigidbody>().AddForce(newBullet.transform.forward * 1500);
        NetworkServer.Spawn(newBullet);
    }

    public override void OnStartLocalPlayer()
    {
        Camera.main.transform.parent = gameObject.transform;
        Camera.main.transform.position = gameObject.transform.position + Camera.main.transform.position; //+ new Vector3(0.6f,2.5f,-5.6f);
    }

}