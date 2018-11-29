using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WandManager : MonoBehaviour {

    // Use this for initialization

    public float range = 100f;
    public List<SpellDefinition> spellList;
    public float moveObjectSpeed;
    public Renderer mesh_teleport;

    //private Dictionary<string,List<SpellColliderType>> colliderDictio;

    private SpellTree spellTree;

    [HideInInspector]
    public bool isReading;

    Ray shootRay;
    RaycastHit shootHit;
    ParticleSystem spellParticles;
    LineRenderer gunLine;

    int movableMask;
    int doorMask;

    private Collider spellCollider;

    ChangeParent changedParentGameObject = null;

    GameObject spellShot;

    [System.Serializable]
    public struct SpellDefinition
    {
        public string spellName;
        public SpellColliderType[] colliderOrder;
    }

    void Start () {
        spellCollider = this.GetComponent<Collider>();
        gunLine = GetComponent<LineRenderer>();

        isReading = false;
        movableMask = LayerMask.GetMask("Movable");
        doorMask = LayerMask.GetMask("Door");

        spellShot = Resources.Load("Sphere") as GameObject;

        //colliderDictio = new Dictionary<string, List<SpellColliderType>>();

        //List<SpellColliderType> colliderList = new List<SpellColliderType>();
        //colliderList.Add(SpellColliderType.RIGHT);
        //colliderList.Add(SpellColliderType.CENTER);
        //colliderDictio.Add("shoot", colliderList);

        //List<SpellColliderType> colliderList2 = new List<SpellColliderType>();
        //colliderList2.Add(SpellColliderType.RIGHT);
        //colliderList2.Add(SpellColliderType.TOP);
        //colliderList2.Add(SpellColliderType.CENTER);
        //colliderDictio.Add("test", colliderList2);

        spellParticles = GetComponent<ParticleSystem>();

        spellTree = new SpellTree();

        //foreach(string key in colliderDictio.Keys)
        //{
        //    spellTree.addSpell(colliderDictio[key], key);
        //}

        foreach (SpellDefinition spell in spellList)
        {
            spellTree.addSpell(new List<SpellColliderType>(spell.colliderOrder) , spell.spellName);
        }

        spellTree.DebugTree();
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
            spellTree.resetActualNode();
        }
    }

    void launchSpell(string spell)
    {
        spellParticles.Stop();
        spellParticles.Play();

        switch (spell)
        {

            case "shot":
                GameObject wand = GameObject.FindGameObjectWithTag("Wand");
                GameObject projectile = Instantiate(spellShot) as GameObject;
                projectile.transform.position = transform.position + wand.transform.forward;
                Rigidbody rb = projectile.GetComponent<Rigidbody>();
                rb.velocity = wand.transform.forward * 20;
                break;
            case "lave":
                //Set the shootRay so that it starts at the end of the wand and points forward.
                Debug.Log("rayCast");
                Debug.Log(transform.position);

                gunLine.enabled = true;
                gunLine.SetPosition(0, transform.position);

                shootRay.origin = transform.position;
                shootRay.direction = transform.forward;

                if (Physics.Raycast(shootRay, out shootHit, range, movableMask))
                {
                    Debug.Log("rayCast success");
                    // Try and find an ChangeParent script on the gameobject hit.
                    changedParentGameObject = shootHit.collider.GetComponent<ChangeParent>();
                    //If the EnemyHealth component exist...
                    if (changedParentGameObject != null)
                    {
                        Debug.Log("change success");
                        //... the enemy should take damage.
                        changedParentGameObject.changeParent(this.gameObject);
                    }

                    //Set the second position of the line renderer to the point the raycast hit.
                    gunLine.SetPosition(1, shootHit.point);
                }
                else
                {
                    Debug.Log("rayCast fail");
                    //... set the second position of the line renderer to the fullest extent of the gun's range.
                    gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
                }
                break;
            case "holohomora":
                //Set the shootRay so that it starts at the end of the wand and points forward.
                Debug.Log("holohomora racast");
                Debug.Log(transform.position);

                gunLine.enabled = true;
                gunLine.SetPosition(0, transform.position);

                shootRay.origin = transform.position;
                shootRay.direction = transform.forward;

                if (Physics.Raycast(shootRay, out shootHit, range, doorMask))
                {
                    Debug.Log("rayCast success");
                    // Try and find an ChangeParent script on the gameobject hit.
                    //Animator anim = shootHit.collider.GetComponent<Animator>();
                    ////If the EnemyHealth component exist...
                    //if (anim != null && anim.GetBool("isOpen") != true)
                    //{
                    //    Debug.Log("open success success");
                    //    //... the enemy should take damage.
                    //    anim.SetBool("isOpen",true);
                    //}

                    DoorManager doorManager = shootHit.collider.GetComponent<DoorManager>();
                    if(doorManager!= null)
                    {
                        doorManager.openTheDoor();
                    }

                    //Set the second position of the line renderer to the point the raycast hit.
                    gunLine.SetPosition(1, shootHit.point);
                }
                else
                {
                    Debug.Log("holohomora rayCast fail");
                    //... set the second position of the line renderer to the fullest extent of the gun's range.
                    gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
                }
                break;
            default:
                break;
        }
        Debug.Log(spell);

        // Stop the particles from playing if they were, then start the particles.


        // Enable the line renderer and set it's first position to be the end of the gun.
    }

    public void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger) && changedParentGameObject != null)
        {
            changedParentGameObject.reset();
            changedParentGameObject = null;
        }

        if (OVRInput.GetDown(OVRInput.Button.SecondaryThumbstick, OVRInput.Controller.Touch) && changedParentGameObject != null)
        {
            Vector2 vect = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
            Vector3 dir = (changedParentGameObject.transform.position - this.transform.position).normalized;
            changedParentGameObject.transform.localPosition = changedParentGameObject.transform.localPosition + dir * vect.x * moveObjectSpeed;
        }
        if (OVRInput.GetDown(OVRInput.Button.PrimaryHandTrigger))
        {
            if(mesh_teleport.enabled == true)
            {
                mesh_teleport.enabled = false;
            }
            else
            {
                mesh_teleport.enabled = true;
            }
        }
    }
}

