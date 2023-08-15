using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private float movementSpeed = 7f;
    [SerializeField] private float rotationSpeed = 500f;

    [SerializeField] Rigidbody rb;
    [SerializeField] GameObject FlameAnimation;
    [SerializeField] GameObject TankTowerHit;
    
    public Transform SpawnPosition;

 
    private Vector3 movementDirection;

    ulong clientID;

    private void Awake()
    {
        clientID = NetworkManager.Singleton.LocalClientId;
    }

    private void Start()
    {
        if (!IsOwner) return;
        InstatiateTankTowerServerRpc(clientID);
    }


    void Update()
    {
        if (!IsOwner) return;
        movementDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    }

    private void FixedUpdate()
    {
        if (!IsOwner) return;      
        Movement();
    }



    void Movement()
    {
        float horizontalInput = Input.GetAxis("Horizontal"); ;
        float verticalInput = Input.GetAxis("Vertical");
        movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        movementDirection.Normalize();
        transform.Translate(movementDirection * movementSpeed * Time.deltaTime, Space.World);

        if(movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            FlameAnimation.SetActive(true);
        }
        else
        {
            FlameAnimation.SetActive(false);
        }
    }

    [ServerRpc]
    private void InstatiateTankTowerServerRpc(ulong clientId)
    {
        GameObject go = Instantiate(TankTowerHit, Vector3.zero, Quaternion.identity);
        go.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
        go.transform.parent = this.transform;
    }



 
}
