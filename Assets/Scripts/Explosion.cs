using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    [SerializeField] float setTimer;
    [SerializeField] GameObject explosion;
    private float Timer;


    // Update is called once per frame
    void Update()
    {
        Timer += Time.deltaTime;
        if(Timer >= setTimer)
        {
            Destroy(explosion);
        }
    }
}
