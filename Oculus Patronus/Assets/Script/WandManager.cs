using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class WandManager : MonoBehaviour {

    // Use this for initialization
    public Teleport teleport;

    public float range = 100f;
    public List<SpellDefinition> spellList;
    public float moveObjectSpeed;
    public GameObject mesh_teleport;
    public GameObject SortDetection;
    [SerializeField]
    public Transform player;
    [SerializeField]
    public Transform camera;
    public Transform target;
    private Quaternion q = Quaternion.identity ;
    public GameObject shield;
    public ParticleSystem holohomoraParticulePrefab;
    public GameObject spellShot;
    //private Dictionary<string,List<SpellColliderType>> colliderDictio;

    private SpellTree spellTree;
    private Color colorBegin;
    private Color colorEnd;
    private float Delay = 0.75f;
    private float colorDt;

    [HideInInspector]
    public bool isReading;

    Ray shootRay;
    RaycastHit shootHit;
    private ParticleSystem failSpellParticule;
    private ParticleSystem holohomoraParticule;
    LineRenderer gunLine;

    int movableMask;
    int doorMask;

    Vector3 position = new Vector3();

    private Collider spellCollider;

    ChangeParent changedParentGameObject = null;
    

    [System.Serializable]
    public struct SpellDefinition
    {
        public string spellName;
        public SpellColliderType[] colliderOrder;
    }

    void Awake () {
        spellCollider = this.GetComponent<Collider>();
        gunLine = GetComponent<LineRenderer>();
        colorBegin = gunLine.material.color;
        colorEnd = new Color(colorBegin.r, colorBegin.g, colorBegin.b, 0f);
        isReading = false;
        movableMask = LayerMask.GetMask("Movable");
        doorMask = LayerMask.GetMask("Door");


        spellTree = new SpellTree();

        foreach (SpellDefinition spell in spellList)
        {
            spellTree.addSpell(new List<SpellColliderType>(spell.colliderOrder) , spell.spellName);
        }

        failSpellParticule = GetComponent<ParticleSystem>();
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

        switch (spell)
        {

            case "shot":
                GameObject wand = GameObject.FindGameObjectWithTag("Wand");
                GameObject projectile = Instantiate(spellShot) as GameObject;
                projectile.transform.position = transform.position + this.transform.forward;
                Rigidbody rb = projectile.GetComponent<Rigidbody>();
                rb.velocity = this.transform.forward * 20;
                break;
            case "lave":
                //Set the shootRay so that it starts at the end of the wand and points forward.
                //Debug.Log("rayCast");
                //Debug.Log(transform.position);

                gunLine.enabled = true;
                gunLine.SetPosition(0, transform.position);
                colorDt = 0f;
                shootRay.origin = transform.position;
                shootRay.direction = transform.forward;

                if (Physics.Raycast(shootRay, out shootHit, range, movableMask))
                {
                    //Debug.Log("rayCast success");
                    // Try and find an ChangeParent script on the gameobject hit.
                    changedParentGameObject = shootHit.collider.GetComponent<ChangeParent>();
                    //If the EnemyHealth component exist...
                    if (changedParentGameObject != null)
                    {
                        //Debug.Log("change success");
                        //... the enemy should take damage.
                        changedParentGameObject.changeParent(this.gameObject);
                    }

                    //Set the second position of the line renderer to the point the raycast hit.
                    gunLine.SetPosition(1, shootHit.point);
                }
                else
                {
                    //Debug.Log("rayCast fail");
                    //... set the second position of the line renderer to the fullest extent of the gun's range.
                    gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
                }
                break;
            case "holohomora":
                //Set the shootRay so that it starts at the end of the wand and points forward.

                //gunLine.enabled = true;
                //gunLine.SetPosition(0, transform.position);

                shootRay.origin = transform.position;
                shootRay.direction = transform.forward;

                if (Physics.Raycast(shootRay, out shootHit, range, doorMask))
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

                    //Set the second position of the line renderer to the point the raycast hit.
                    //gunLine.SetPosition(1, shootHit.point);
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
                    //Debug.Log("holohomora rayCast fail");
                    //... set the second position of the line renderer to the fullest extent of the gun's range.
                    //gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
                }
                break;
            case "protego":
                shield.SetActive(true);
                SortDetection.SetActive(false);
                break;

            default:
                break;
        }
        Debug.Log(spell);

        // Stop the particles from playing if they were, then start the particles.


        // Enable the line renderer and set it's first position to be the end of the gun.
    }


    public void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("quit"))
        {
            Debug.Log("quit");
            Application.Quit();
        }

        if (other.gameObject.CompareTag("restart"))
        {
            Debug.Log("restart");
            SceneManager.LoadScene("SceneCasque 1");
        }
    }

    public void Update()
    {

        if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger) && changedParentGameObject != null)
        {
            changedParentGameObject.reset();
            changedParentGameObject = null;
        }

        if ((OVRInput.Get(OVRInput.RawButton.RThumbstickUp) || OVRInput.Get(OVRInput.RawButton.RThumbstickDown)) && changedParentGameObject != null)
        {
            Vector2 vect = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);

            Vector3 dir = (changedParentGameObject.transform.position - this.transform.position).normalized;

            //gunLine.SetPosition(0, changedParentGameObject.transform.position);
            //gunLine.SetPosition(1, this.transform.position);


            Vector3 newPosition = changedParentGameObject.transform.position + dir * vect.y * moveObjectSpeed;

            if((newPosition - this.transform.position).sqrMagnitude > 1)
            {
                changedParentGameObject.transform.position = newPosition;
            }
        }

        /**
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
        }**/
        if (OVRInput.GetDown(OVRInput.RawButton.RThumbstick))
        {

            if (!teleport.getRight() && mesh_teleport.activeSelf == true)
            {
                mesh_teleport.SetActive(false);
                SortDetection.SetActive(true);
            }
            else if (teleport.getRight() && mesh_teleport.activeSelf == true)
            {
                position = teleport.getPosition();
                player.position = new Vector3(position.x, player.position.y, position.z);
                player.transform.localRotation = player.transform.localRotation * teleport.getLocalTargetRot();
                camera.position = new Vector3(position.x, camera.position.y, position.z);
                camera.transform.localRotation = camera.transform.localRotation * teleport.getLocalTargetRot();
                mesh_teleport.SetActive(false);
                SortDetection.SetActive(true);
                teleport.haveValid();
            }
            else
            {
                SortDetection.SetActive(false);
                mesh_teleport.SetActive(true);
                if(changedParentGameObject != null)
                {
                    changedParentGameObject.reset();
                    changedParentGameObject = null;
                }
                teleport.setRotInitialize();
            }


        }

        if(mesh_teleport.activeSelf == true)
        {
            Vector2 thumb_dir = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);
            if (Mathf.Abs(thumb_dir.x)>0.1f)
            {
                Quaternion q_rotate = Quaternion.Euler(0,10*thumb_dir.x,0);
                teleport.setLocalTargetRot(q_rotate);
            }
            

        }


        if (mesh_teleport.activeSelf && (OVRInput.Get(OVRInput.RawButton.RThumbstickUp) || OVRInput.Get(OVRInput.RawButton.RThumbstickDown)))
        {
            Vector2 vector_joystick = new Vector2();
            vector_joystick = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick);
            teleport.SetVelocity(teleport.velocity + vector_joystick.y *0.1f);
            
        }

        if (gunLine.enabled)
        {
            //colorDt += Time.deltaTime;
            //Debug.Log("a " + colorDt);
            //if (colorDt > Delay)
            //{
            //    gunLine.enabled = false;
            //}

            colorDt += Time.deltaTime;
            Color c = Color.Lerp(colorBegin, colorEnd, (colorDt / Delay));
            gunLine.material.color = c;
            if (c.a <= 0)
            {
                gunLine.enabled = false;
            }
        }
    }
}

