using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{

    public TankTower parent;
    [SerializeField] GameObject FatherObject;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject explosion;

    [SerializeField] float setTimer;
    private float Timer;

    [SerializeField] float setTimerDestroy;
    private float TimerDestroy;
    


    // Update is called once per frame
    void Update()
    {
        Timer += Time.deltaTime;
        if (Timer >= setTimer)
        {
            bullet.SetActive(false);
            explosion.SetActive(true);
        }

        TimerDestroy += Time.deltaTime;
        if(Timer >= setTimerDestroy)
        {
            if (!IsOwner) return;
            parent.DestroyBulletServerRpc();
        }


    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            bullet.SetActive(false);
            explosion.SetActive(true);
        }
    }

}
