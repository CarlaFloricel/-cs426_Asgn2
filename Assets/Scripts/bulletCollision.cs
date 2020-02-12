using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class bulletCollision : NetworkBehaviour
{
    void OnCollisionEnter(Collision collision){
        NetworkServer.Destroy(gameObject);
    }
}
