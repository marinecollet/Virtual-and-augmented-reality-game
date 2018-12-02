using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WandManager : MonoBehaviour {

    // Use this for initialization

    public float range = 100f;
    public List<SpellDefinition> spellList;
    
    //private Dictionary<string,List<SpellColliderType>> colliderDictio;

    private SpellTree spellTree;

    [HideInInspector]
    public bool isReading;

    Ray shootRay;
    RaycastHit shootHit;
 //   ParticleSystem spellParticles;
    LineRenderer gunLine;
    int movableMask;

    private Collider spellCollider;

    ChangeParent changedParentGameObject;

    GameObject spellShot;

    void Start () {
        spellCollider = this.GetComponent<Collider>();
        gunLine = GetComponent<LineRenderer>();

        isReading = false;
        movableMask = LayerMask.GetMask("Movable");


        spellShot = Resources.Load("Sphere") as GameObject;

 //       spellParticles = GetComponent<ParticleSystem>();

        spellTree = new SpellTree();


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
//        spellParticles.Stop();
//        spellParticles.Play();

        switch (spell)
        {

            case "shot":
                GameObject wand = GameObject.FindGameObjectWithTag("Wand");
                GameObject projectile = Instantiate(spellShot) as GameObject;
                projectile.transform.position = transform.position + wand.transform.forward;
                Rigidbody rb = projectile.GetComponent<Rigidbody>();
                rb.velocity = wand.transform.forward * 20;
                break;
            //case "lave":
            //    //Set the shootRay so that it starts at the end of the gun and points forward from the barrel.
            //    Debug.Log("rayCast");
            //    Debug.Log(transform.position);

            //    gunLine.enabled = true;
            //    gunLine.SetPosition(0, transform.position);

            //    shootRay.origin = transform.position;
            //    shootRay.direction = transform.forward;

            //    if (Physics.Raycast(shootRay, out shootHit, range, movableMask))
            //    {
            //        Debug.Log("rayCast success");
            //        // Try and find an EnemyHealth script on the gameobject hit.
            //        changedParentGameObject = shootHit.collider.GetComponent<ChangeParent>();
            //        shootHit.transform.parent = this.transform.parent;
            //        //If the EnemyHealth component exist...
            //        if (changedParentGameObject != null)
            //        {
            //            Debug.Log("change success");
            //            //... the enemy should take damage.
            //            changedParentGameObject.Change(this.gameObject);
            //        }

            //        //Set the second position of the line renderer to the point the raycast hit.
            //        gunLine.SetPosition(1, shootHit.point);
            //    }
            //    else
            //    {
            //        Debug.Log("rayCast fail");
            //        //... set the second position of the line renderer to the fullest extent of the gun's range.
            //        gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
            //    }
            //    break;
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

    //public void Update()
    //{
    //    if ()
    //    {
    //        changedParentGameObject.reset();
    //        changedParentGameObject = null;

    //    }
    //}
}

