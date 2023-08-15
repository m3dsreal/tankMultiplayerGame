using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CamFollow : NetworkBehaviour
{

    public Transform target;
    public float smoothspeed = 0.12f;
    public Vector3 offset;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {
        if (!IsOwner) return;

        Vector3 desirePost = target.position + offset;
        Vector3 smoothPost = Vector3.Lerp(transform.position, desirePost, smoothspeed);
        transform.position = smoothPost;
    }
}
