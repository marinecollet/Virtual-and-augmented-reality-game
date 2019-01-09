using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter))]
public class Teleport : MonoBehaviour
{

    Ray shootRay;
    RaycastHit shootHit;
    LineRenderer gunLine;
    int groundLayer;
    Mesh mesh;
    public Transform targetPrefab;
    public Material[] lineMaterials;
    private Transform target;
    public Transform cameraTransform;
    public Transform spellShotSpawn;
    private Renderer targetRenderer;
    Vector3 position = new Vector3();
    bool right;
    float distance = 5;

    void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        gunLine = GetComponent<LineRenderer>();
        groundLayer = 10;
        GameObject go = GameObject.Find("target(Clone)");
        if(go != null)
        {
            target = go.GetComponent<Transform>();
        }
        else
        {
            target = Instantiate(targetPrefab) as Transform;
        }
        
        targetRenderer = target.GetComponent<Renderer>();
        targetRenderer.enabled = false;
        cameraTransform = Camera.main.GetComponent<Transform>();
    }

    public void Update()
    {
        shootRay.origin = cameraTransform.position + cameraTransform.forward*0.1f;
        shootRay.direction = cameraTransform.forward;

        
        //MakeArcMesh(CalculateArcArray());

        if (Physics.Raycast(shootRay, out shootHit, distance))
        {
            gunLine.enabled = true;
            gunLine.SetPosition(0, spellShotSpawn.position);
            if (shootHit.collider.gameObject.layer == groundLayer) { 
                //renderer.material = Resources.Load("Correct_zone", typeof(Material)) as Material;
                right = true;
                position = shootHit.point;

                if (!targetRenderer.enabled)
                {
                    targetRenderer.enabled = true;
                }
                Vector3 pos = new Vector3();
                pos.x = position.x;
                pos.y = position.y + 0.001f;
                pos.z = position.z;
                target.transform.localPosition = pos;
                Debug.Log("oui "+pos);
                gunLine.material = lineMaterials[0];
                gunLine.SetPosition(1, shootHit.point);

            }
            else
            {
                //Debug.Log("nop " + shootHit.collider.gameObject.name + " " + groundLayer);
                //renderer.material = Resources.Load("Bad_zone", typeof(Material)) as Material;
                right = false;
                if (targetRenderer.enabled)
                {
                    targetRenderer.enabled = false;
                }
                gunLine.material = lineMaterials[1];
                gunLine.SetPosition(1, shootHit.point);
            }
        }
        else
        {
            //renderer.material = Resources.Load("Bad_zone", typeof(Material)) as Material;
            //Debug.Log("non ");
            if (targetRenderer.enabled)
            {
                targetRenderer.enabled = false;
            }
            gunLine.enabled = false;
        }

    }

    public Vector3 getPosition()
    {
        return position;
    }

    public bool getRight()
    {
        if (right)
            return true;
        else
            return false;
    }

    public void validTp()
    {
        //targetRenderer.enabled = false;
        this.gameObject.SetActive(false);
    }

    public void unValidTp()
    {
        targetRenderer.enabled = false;
        this.gameObject.SetActive(false);
    }
}
