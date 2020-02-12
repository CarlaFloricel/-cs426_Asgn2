using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class OldMouseCamera : NetworkBehaviour
{

    public float speedH = 2.0f;
    public float speedV = 2.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;
    Transform t;

    void Start()
    {
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
        t.eulerAngles = new Vector3(0, yaw, 0.0f);


        if (Input.GetKey(KeyCode.M))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
