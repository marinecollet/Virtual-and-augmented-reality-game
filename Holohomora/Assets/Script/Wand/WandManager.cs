using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WandManager : MonoBehaviour {

    // Use this for initialization
    public float range = 100f;
    public List<SpellDefinition> spellList;
    public Transform cameraTransform;
    public float wandSpeed;

    public Teleport teleport;
    public GameObject mesh_teleport;
    public float shotSpeed;
    public Transform spellShotSpawn;
    public ParticleSystem holohomoraParticulePrefab;

    private SpellTree spellTree;
    private ParticleSystem holohomoraParticule;
    private ParticleSystem failSpellParticule;

    [HideInInspector]
    public bool isReading;

    Ray shootRay;
    RaycastHit shootHit;
    LineRenderer gunLine;
    int movableMask, doorMask;

    private Collider spellCollider;

    GameObject spellShot;

    private void Awake()
    {
        isReading = false;

        spellCollider = this.GetComponent<Collider>();
        gunLine = GetComponent<LineRenderer>();
        movableMask = LayerMask.GetMask("Movable");
        doorMask = LayerMask.GetMask("Door");
        spellShot = Resources.Load("Sphere") as GameObject;

        spellTree = new SpellTree();
        cameraTransform = Camera.main.GetComponent<Transform>();
        failSpellParticule = GetComponent<ParticleSystem>();
    }

    void Start () {


 //       spellParticles = GetComponent<ParticleSystem>();


        foreach (SpellDefinition spell in spellList)
        {
            spellTree.addSpell(new List<SpellColliderType>(spell.colliderOrder) , spell.spellName);
        }

        spellTree.DebugTree();


        holohomoraParticule = Instantiate(holohomoraParticulePrefab) as ParticleSystem;


    }

    public void AddSortCollider(SpellColliderType col)
    {
        if (this.spellTree.advance(col))
        {
            string spell = this.spellTree.isSpell();

            if (spell != null)
            {
                launchSpell(spell);
                spellTree.resetActualNode();
            }
        }
        else
        {
            failSpellParticule.Play();
            spellTree.resetActualNode();
        }
    }

    void launchSpell(string spell)
    {
//        spellParticles.Stop();
//        spellParticles.Play();

        switch (spell)
        {

            case "shot":
                GameObject wand = GameObject.FindGameObjectWithTag("Wand");
                GameObject projectile = Instantiate(spellShot) as GameObject;
                projectile.transform.position = spellShotSpawn.position;
                Rigidbody rb = projectile.GetComponent<Rigidbody>();

                shootRay.origin = cameraTransform.position;
                shootRay.direction = cameraTransform.forward;
                
                if (Physics.Raycast(shootRay, out shootHit, range))
                {

                    rb.velocity = (shootHit.point - spellShotSpawn.position).normalized * shotSpeed; 

                    //Set the second position of the line renderer to the point the raycast hit.
                    //gunLine.SetPosition(1, shootHit.point);
                }
                else
                {
                    rb.velocity = wand.transform.forward * shotSpeed;
                }

                break;
            case "tp":
                if (!teleport.getRight() && mesh_teleport.activeSelf)
                {
                    mesh_teleport.SetActive(false);
                }
                else if (teleport.getRight() && mesh_teleport.activeSelf)
                {
                    shootRay.origin = cameraTransform.position;
                    shootRay.direction = cameraTransform.forward;

                    if (Physics.Raycast(shootRay, out shootHit, range))
                    {
                        MazeCell mc = shootHit.transform.GetComponentInParent<MazeCell>();

                        if (mc != null)
                        {
                            Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
                            player.SetTargetCell(mc);
                        }
                    }
                    teleport.validTp();
                }
                else
                {
                    mesh_teleport.SetActive(true);
                }
                break;
            case "holohomora":
                shootRay.origin = cameraTransform.position;
                shootRay.direction = cameraTransform.forward;

                if (Physics.Raycast(shootRay, out shootHit, range,doorMask))
                {
                        DoorManager doorManager = shootHit.collider.GetComponent<DoorManager>();

                        if (doorManager != null)
                        {
                            holohomoraParticule.transform.position = shootHit.point;
                            holohomoraParticule.transform.rotation = Quaternion.LookRotation(-shootRay.direction);
                            holohomoraParticule.startColor = Color.white;
                            holohomoraParticule.Play();
                            doorManager.openTheDoor();
                        }
                }
                else
                {
                    if (Physics.Raycast(shootRay, out shootHit, range))
                    {
                        holohomoraParticule.transform.position = shootHit.point;
                        holohomoraParticule.transform.rotation = Quaternion.LookRotation(-shootRay.direction, Vector3.up);
                        holohomoraParticule.startColor = Color.red;
                        holohomoraParticule.Play();
                    }
                }
                break;
            default:
                break;
        }
        Debug.Log(spell);

        // Stop the particles from playing if they were, then start the particles.


        // Enable the line renderer and set it's first position to be the end of the gun.
    }

    [System.Serializable]
    public struct SpellDefinition
    {
        public string spellName;
        public SpellColliderType[] colliderOrder;
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.I))
        {
            Vector3 temp = this.transform.localPosition + Vector3.up * wandSpeed;
            if(temp.y > 6)
            {
                temp.y = 6;
            }
            this.transform.localPosition = temp;
        }
        if (Input.GetKey(KeyCode.K))
        {
            Vector3 temp = this.transform.localPosition + Vector3.up * -wandSpeed;
            if (temp.y < -6)
            {
                temp.y = -6;
            }
            this.transform.localPosition = temp;
        }
        if (Input.GetKey(KeyCode.L))
        {
            Vector3 temp = this.transform.localPosition + Vector3.right * wandSpeed;
            if (temp.x > 6)
            {
                temp.x = 6;
            }
            this.transform.localPosition = temp;
        }
        if (Input.GetKey(KeyCode.J))
        {
            Vector3 temp = this.transform.localPosition + Vector3.right * -wandSpeed;
            if (temp.x < -6)
            {
                temp.x = -6;
            }
            this.transform.localPosition = temp;
        }
    }
}

