using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class TankTower : NetworkBehaviour
{

    [SerializeField] Transform Tower;
    [SerializeField] Transform Cannon;
    [SerializeField] float TowerSpeed;
    [SerializeField] float CannonSpeed;

    [SerializeField] GameObject PrefabBullet;
    [SerializeField] GameObject PrefabExplosion;
    [SerializeField] float Speed;

    private Vector3 MousePos;
    public GameObject SpawnPosition;
    public Transform SpawnPositionBullet;
    public Rigidbody rbBullet;

    private Camera cam;
    ulong clientID;

    [SerializeField] private List<GameObject> SpawnedBullets = new List<GameObject>();
    



    private void Start()
    {
        cam = GameObject.Find("PlayerCamera").GetComponent<Camera>();
        SpawnPosition = GameObject.FindGameObjectWithTag("SpawnPoint");
    }


    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
        RotateTower();
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            shootServerRpc();
        }
    }


    void RotateTower()
    {
        MousePos = cam.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, 
                        Input.mousePosition.y, 
                        cam.transform.position.z - transform.position.z));

        SpawnPosition.transform.position = MousePos;
        Tower.LookAt((new Vector3 (SpawnPosition.transform.position.x, Tower.transform.position.y, SpawnPosition.transform.position.z)));
    }

    [ServerRpc]
    public void shootServerRpc()
    {

        GameObject bullet = Instantiate(PrefabBullet, new Vector3(SpawnPositionBullet.transform.position.x, SpawnPositionBullet.transform.position.y , SpawnPositionBullet.transform.position.z), SpawnPositionBullet.rotation);
        SpawnedBullets.Add(bullet);
        bullet.GetComponent<Bullet>().parent = this;
        bullet.GetComponent<NetworkObject>().Spawn();
        rbBullet = bullet.GetComponent<Rigidbody>();
        rbBullet.AddForce(SpawnPositionBullet.forward * Speed, ForceMode.Impulse);
    }


    [ServerRpc(RequireOwnership = false)]
    public void DestroyBulletServerRpc()
    {
        GameObject BulletDestroy = SpawnedBullets[0];
        BulletDestroy.GetComponent<NetworkObject>().Despawn();
        SpawnedBullets.Remove(BulletDestroy);
        Destroy(BulletDestroy);
    }
}
