using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

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
    private Transform player;
    private Ray shootRay;
    private RaycastHit shootHit;
    private LineRenderer gunline;
    private Animator anim;

    // Use this for initialization
    void Awake () {
        timeSinceLastShot = -1;
        isShooting = false;
        isTargeting = false;
        player = null;
        gunline = GetComponent<LineRenderer>();
        anim = GetComponent<Animator>();
    }

    //// Update is called once per frame
    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Shot")
        {
            Destroy(collider.gameObject);
            Destroy(this.gameObject);
        }
    }

    private void FixedUpdate()
    {
        if(player != null)
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
        if(Game_Manager.isSetup && player == null )
        {
            player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        }

        //if(Input.GetKeyDown(KeyCode.B))
        //{
        //    anim.SetBool("isShooting",true);
        //}

        //if (this.anim.GetCurrentAnimatorStateInfo(0).IsName("YourAnimationName"))
        //{
            
        //}

        if (isTargeting && player != null)
        {
            Vector3 dir = player.position - spellShotSpawn.position;
            shootRay.origin = spellShotSpawn.position;
            shootRay.direction = dir.normalized;

            gunline.SetPosition(0, shootRay.origin);
            

            if (Physics.Raycast(shootRay, out shootHit, maxDistTargeting))
            {
                gunline.SetPosition(1, shootHit.point);
                if (shootHit.collider.gameObject.CompareTag("Player"))
                {
                    gunline.SetPosition(0, shootRay.origin);
                    if (timeSinceLastShot > shootingSpeed &&  !isShooting)
                    {
                        //GameObject projectile = Instantiate(spellShot) as GameObject;
                        //projectile.transform.position = spellShotSpawn.position;
                        //Rigidbody rb = projectile.GetComponent<Rigidbody>();

                        //rb.velocity = (new Vector3(player.position.x, player.position.y + Random.Range(0.06f,0.1f), player.position.z) - spellShotSpawn.position).normalized * shotSpeed;
                        //timeSinceLastShot = 0;
                        anim.SetBool("isShooting", true);
                        isShooting = true;
                    }
                    else if (timeSinceLastShot < shootingSpeed)
                    {
                        timeSinceLastShot += Time.deltaTime;
                        gunline.SetPosition(1, shootRay.origin + shootRay.direction * maxDistTargeting);
                    }
                }
            }
            else
            {
                timeSinceLastShot += Time.deltaTime;
                gunline.SetPosition(1, shootRay.origin + shootRay.direction * maxDistTargeting);
            }
        }   
    }
    public void AnimationShotEnded()
        {
             //Debug.Log("shot");
            anim.SetBool("isShooting", false);
            GameObject projectile = Instantiate(spellShot) as GameObject;
            projectile.transform.position = spellShotSpawn.position;
            Rigidbody rb = projectile.GetComponent<Rigidbody>();

            rb.velocity = (new Vector3(player.position.x, player.position.y + Random.Range(0.06f, 0.1f), player.position.z) - spellShotSpawn.position).normalized * shotSpeed;
            timeSinceLastShot = 0;
            isShooting = false;
    }
}
