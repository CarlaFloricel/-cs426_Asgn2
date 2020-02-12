using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class MouseCamera : NetworkBehaviour
{
    public GameObject cam;

    public float speedH = 2.0f;
    public float speedV = 2.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;
    Transform t;

    void Start()
    {
        cam = GameObject.Find("Main Camera");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        t = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isLocalPlayer){
            return;
        }

        yaw += speedH * Input.GetAxis("Mouse X");
        pitch -= speedV * Input.GetAxis("Mouse Y");
        
        t.eulerAngles = new Vector3(0, yaw, 0.0f);
        cam.transform.eulerAngles = new Vector3(Mathf.Clamp(pitch,-50,28), cam.transform.eulerAngles.y, cam.transform.eulerAngles.z);

        if (Input.GetKey(KeyCode.M))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
