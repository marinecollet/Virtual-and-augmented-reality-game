using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    public float rotSpeed;
    public float shootingSpeed;
    public GameObject spellShot;
    public Transform spellShotSpawn;
    public float shotSpeed;
    public float maxDistTargeting;
    public float minDistRotating;


    private bool isShooting;
    public bool isTargeting;
    private float timeSinceLastShot;
    public Transform player;
    private Ray shootRay;
    private RaycastHit shootHit;
    LineRenderer gunLine;
    // Use this for initialization

    void Awake()
    {
        timeSinceLastShot = 0;
        isShooting = false;
        isTargeting = false;
        player = null;
        gunLine = GetComponent<LineRenderer>();
    }

    //// Update is called once per frame
    void OnTriggerEnter(Collider collider)
    {
        //Debug.Log("collider");
        if (collider.tag == "Shot")
        {
            Destroy(collider.gameObject);
            Destroy(this.gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (player != null)
        {
            Vector3 dir = player.position - this.transform.position;
            if (dir.magnitude < maxDistTargeting)
            {
                isTargeting = true;
            }
            else
            {
                isTargeting = false;
            }
            if (dir.magnitude > minDistRotating && isTargeting)
                this.transform.localRotation = Quaternion.LookRotation(new Vector3(dir.x, 0f, dir.z), Vector3.up);
        }
    }

    private void Update()
    {
        if (Game_Manager.isSetup && player == null)
        {
            player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        }

        if (isTargeting && player != null)
        {
            Vector3 dir = player.position - spellShotSpawn.position;
            shootRay.origin = spellShotSpawn.position;
            shootRay.direction = dir.normalized;
            //gunLine.enabled = true;
            //gunLine.SetPosition(0, shootRay.origin);

            if (Physics.Raycast(shootRay, out shootHit, maxDistTargeting))
            {
                if (shootHit.collider.gameObject.CompareTag("Player"))
                {
                    if (timeSinceLastShot > shootingSpeed)
                    {
                        GameObject projectile = Instantiate(spellShot) as GameObject;
                        projectile.transform.position = spellShotSpawn.position;
                        Rigidbody rb = projectile.GetComponent<Rigidbody>();

                        rb.velocity = (new Vector3(player.position.x, player.position.y + Random.Range(1.2f, 1.7f), player.position.z) - spellShotSpawn.position).normalized * shotSpeed;
                        timeSinceLastShot = 0;
                        //gunLine.SetPosition(0, shootHit.point);

                    }
                    else if (timeSinceLastShot < shootingSpeed)
                    {
                        timeSinceLastShot += Time.deltaTime;
                        //gunLine.SetPosition(1, shootRay.origin + shootRay.direction * maxDistTargeting);

                    }
                }
            }
            else
            {
                //gunLine.SetPosition(1, shootRay.origin + shootRay.direction * maxDistTargeting);

                timeSinceLastShot += Time.deltaTime;
            }
        }
    }
}
