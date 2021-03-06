﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class EnemyManager : MonoBehaviour
{
    public float rotSpeed;
    public float shootingSpeed;
    public GameObject spellShot;
    public Transform spellShotSpawn;
    public float shotSpeed;
    public float maxDistTargeting;
    public float minDistRotating;
    public AudioSource audioSource;
    public int life;
    public ParticleSystem dieParticule;

    private ParticleSystem particule;
    private bool isShooting;
    private bool isTargeting;
    private float timeSinceLastShot;
    private Player player;
    private Ray shootRay;
    private RaycastHit shootHit;
    private LineRenderer gunLine;
    private Animator anim;
    // Use this for initialization

    void Awake()
    {
        timeSinceLastShot = 0;
        isShooting = false;
        isTargeting = false;
        player = null;
        gunLine = GetComponent<LineRenderer>();
        anim = GetComponent<Animator>();
        particule = GetComponent<ParticleSystem>();
    }

    //// Update is called once per frame
    void OnTriggerEnter(Collider collider)
    {
        //Debug.Log("collider");
        if (collider.tag == "Shot")
        {
            Destroy(collider.gameObject);
            particule.Play();
            life--;
            if(life == 0)
            {
                ParticleSystem part = Instantiate(dieParticule) as ParticleSystem;
                part.transform.position = this.transform.position;
                part.Play();
                Destroy(this.gameObject, 0.35f);
            }
        }
    }

    private void FixedUpdate()
    {
        if (player != null && !player.isDead)
        {
            Vector3 dir = player.transform.position - this.transform.position;
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
        else if (player != null && player.isDead)
        {
            isTargeting = false;
        }
    }

    private void Update()
    {
        if (Game_Manager.isSetup && player == null)
        {
            player = GameObject.FindWithTag("Player").GetComponent<Player>();
            //player = GameObject.Find("OVRCameraRig").GetComponent<Player>();
        }

        if (isTargeting && player != null)
        {
            Vector3 dir = (player.transform.position + Vector3.up) - spellShotSpawn.position;
            shootRay.origin = spellShotSpawn.position;
            shootRay.direction = dir.normalized;
            //gunLine.enabled = true;
            //gunLine.SetPosition(0, shootRay.origin);

            if (Physics.Raycast(shootRay, out shootHit, maxDistTargeting))
            {
                if (shootHit.collider.gameObject.CompareTag("Player") || shootHit.collider.gameObject.CompareTag("SortDetection"))
                {
                    if (timeSinceLastShot > shootingSpeed && !isShooting)
                    {
                        //GameObject projectile = Instantiate(spellShot) as GameObject;
                        //projectile.transform.position = spellShotSpawn.position;
                        //Rigidbody rb = projectile.GetComponent<Rigidbody>();

                        //rb.velocity = (new Vector3(player.position.x, player.position.y + Random.Range(0.06f,0.1f), player.position.z) - spellShotSpawn.position).normalized * shotSpeed;
                        //timeSinceLastShot = 0;
                        anim.SetBool("isShooting", true);
                        audioSource.Play();
                        isShooting = true;
                        //gunLine.SetPosition(1, shootHit.point);

                    }
                    else if (timeSinceLastShot < shootingSpeed)
                    {
                        timeSinceLastShot += Time.deltaTime;
                        //gunLine.SetPosition(1, shootRay.origin + shootRay.direction * maxDistTargeting);

                    }
                }
                else
                {
                    //gunLine.SetPosition(1, shootRay.origin + shootRay.direction * maxDistTargeting);

                    timeSinceLastShot += Time.deltaTime;
                }
            }
            else
            {
                //gunLine.SetPosition(1, shootRay.origin + shootRay.direction * maxDistTargeting);

                timeSinceLastShot += Time.deltaTime;
            }
        }
        else if( player != null)
        {
                timeSinceLastShot += Time.deltaTime;
        }
    }

    public void AnimationShotEnded()
    {
        //Debug.Log("shot");
        anim.SetBool("isShooting", false);
        GameObject projectile = Instantiate(spellShot) as GameObject;
        projectile.transform.position = spellShotSpawn.position;
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        rb.velocity = (new Vector3(player.transform.position.x, player.transform.position.y + Random.Range(1.2f, 1.7f), player.transform.position.z) - spellShotSpawn.position).normalized * shotSpeed;
        timeSinceLastShot = 0;
        isShooting = false;
    }

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(transform.position, maxDistTargeting);
    //}
}
