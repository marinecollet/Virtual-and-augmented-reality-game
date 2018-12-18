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

    private bool isShooting;
    private bool isTargeting;
    private float timeSinceLastShot;
    private Transform player;

    // Use this for initialization
    void Awake () {
        timeSinceLastShot = -1;
        isShooting = false;
        isTargeting = false;
        player = null;
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
            if (dir.sqrMagnitude < maxDistTargeting)
            {
                isTargeting = true;
            }
            else
            {
                isTargeting = false;
            }
        }
    }

    private void Update()
    {
        if(Game_Manager.isSetup && player == null )
        {
            player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        }
        if (isTargeting && player != null)
        {
            Vector3 dir = player.position - this.transform.position;
            this.transform.localRotation = Quaternion.LookRotation(dir.normalized, Vector3.up);
            if (timeSinceLastShot > shootingSpeed)
            {
                GameObject projectile = Instantiate(spellShot) as GameObject;
                projectile.transform.position = spellShotSpawn.position;
                Rigidbody rb = projectile.GetComponent<Rigidbody>();

                rb.velocity = (new Vector3(player.position.x, player.position.y + 1.2f, player.position.z)- spellShotSpawn.position).normalized * shotSpeed;
                timeSinceLastShot = 0;
            }
            else if(timeSinceLastShot < shootingSpeed)
            {
                timeSinceLastShot += Time.deltaTime;
            }
        }
    }
}
