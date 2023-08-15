using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IaEnemy : MonoBehaviour
{

    [SerializeField] float MinH;
    [SerializeField] float MaxH;
    [SerializeField] float MinV;
    [SerializeField] float MaxV;

   
    [SerializeField] float Speed;
    [SerializeField] float SpeedBullet;
    [SerializeField] float setTimer;
    [SerializeField] float setTimerFireShot;
    [SerializeField] float SpeedRotation;
    [SerializeField] Transform Cannon;
    [SerializeField] float RangeAlert;
    [SerializeField] LayerMask PlayerMask;

    public Transform PlayerObject;
    public Rigidbody rbBullet;
    public Transform SpawnPositionBullet;
    [SerializeField] GameObject PrefabBullet;

    private float Timer;
    private float TimerFireShot;
    private Vector3 TargetPosition;
    [SerializeField] bool AlertStatus;

    void Start()
    {
        //Flag = false;
        float ValueH = Random.Range(MinH, MaxH);
        float ValueV = Random.Range(MinV, MaxV);

        TargetPosition = new Vector3(ValueH, this.transform.position.y, ValueV);
        this.transform.LookAt(TargetPosition);
        //moveEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        Timer += Time.deltaTime;
        TimerFireShot += Time.deltaTime;

        AlertStatus = Physics.CheckSphere(transform.position, RangeAlert, PlayerMask);

        if (AlertStatus)
        {
            Debug.Log("Esta dentro del rango del enemigo");
            Vector3 PlayerPos = new Vector3(PlayerObject.transform.position.x, PlayerObject.transform.position.y, PlayerObject.transform.position.z);
            this.transform.LookAt(PlayerPos);
            this.transform.position = Vector3.MoveTowards(this.transform.position, PlayerPos , Speed * Time.deltaTime);
            Timer = 0;

            if(TimerFireShot >= setTimerFireShot)
            {
                shootEnemy();
                TimerFireShot = 0;
            }
        }

        else if (Timer >= setTimer && !AlertStatus) 
        { 
            moveEnemy(); 
        } 

        else if(!AlertStatus)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, TargetPosition, Speed * Time.deltaTime);
            this.transform.LookAt(TargetPosition);
        }

        
    }


    void moveEnemy()
    {
        float ValueH = Random.Range(MinH,MaxH);
        float ValueV = Random.Range(MinV, MaxV);

        TargetPosition = new Vector3(ValueH, this.transform.position.y, ValueV);
        

        Quaternion toRotation = Quaternion.LookRotation(TargetPosition, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, SpeedRotation * Time.deltaTime);
    

        //Cannon.transform.LookAt(new Vector3(TargetPosition.x, Cannon.transform.position.y, TargetPosition.z));
        this.transform.LookAt(TargetPosition);
        
        Timer = 0;
        Debug.Log(TargetPosition);
        
    }

    void shootEnemy()
    {
        GameObject bullet = Instantiate(PrefabBullet, new Vector3(SpawnPositionBullet.transform.position.x, 1f, SpawnPositionBullet.transform.position.z), SpawnPositionBullet.rotation);
        rbBullet = bullet.GetComponent<Rigidbody>();
        rbBullet.AddForce(SpawnPositionBullet.forward * SpeedBullet, ForceMode.Impulse);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, RangeAlert);
        Gizmos.color = Color.cyan;
    }

}
