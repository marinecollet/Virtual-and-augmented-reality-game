using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    public float rotSpeed;
    public float shootingSpeed;
    public GameObject spellShot;
    public Transform spellShotSpawn;
    public float shotSpeed;

    private Transform target;
    private bool isShooting;
    private float timeSinceLastShot;

	// Use this for initialization
	void Awake () {
        target = null;
        timeSinceLastShot = -1;
        isShooting = false;
    }
	
	// Update is called once per frame
	void OnTriggerEnter(Collider collider) {
		if(collider.tag == "Shot")
        {
            Destroy(collider.gameObject);
            Destroy(this.gameObject);
        }
        if (collider.tag == "Player")
        {
            target = collider.transform;
        }
    }
    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
        {
            target = null;
        }
    }


    private void Update()
    {
        if(target != null)
        {
            Vector3 dir = target.position - this.transform.position;
            this.transform.localRotation = Quaternion.LookRotation(dir.normalized, Vector3.up);
            if (timeSinceLastShot > shootingSpeed)
            {
                GameObject projectile = Instantiate(spellShot) as GameObject;
                projectile.transform.position = spellShotSpawn.position;
                Rigidbody rb = projectile.GetComponent<Rigidbody>();

                rb.velocity = (new Vector3(target.position.x, target.position.y + 0.08f, target.position.z)- spellShotSpawn.position).normalized * shotSpeed;
                timeSinceLastShot = 0;
            }
            else if(timeSinceLastShot < shootingSpeed)
            {
                timeSinceLastShot += Time.deltaTime;
            }
        }
    }
}
