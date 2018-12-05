using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshFilter))]
public class Teleport : MonoBehaviour
{


    Mesh mesh;
    [SerializeField]
    public float meshWidth;
    [SerializeField]
    public float velocity;
    [SerializeField]
    public float angle;
    [SerializeField]
    public int resolution = 10;
    float g; //force of gravity on the y axis
    float radianAngle;
    private Renderer renderer;
    Vector3 position = new Vector3();
    bool right;

    private void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        g = Mathf.Abs(Physics2D.gravity.y);
        renderer = GetComponent<Renderer>();
    }

    //check that mesh is not null and that the game is playing
    private void OnValidate()
    {
        if (mesh != null && Application.isPlaying)
        {
            MakeArcMesh(CalculateArcArray());
        }
    }

    void Start()
    {
        MakeArcMesh(CalculateArcArray());
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    void MakeArcMesh(Vector3[] arcVerts)
    {
        mesh.Clear();
        Vector3[] vertices = new Vector3[(resolution + 1) * 2];
        int[] triangles = new int[resolution * 6 * 2];

        for (int i = 0; i<= resolution; i++)
        {
            //set vertices
            vertices[i * 2] = new Vector3(meshWidth * 0.5f, arcVerts[i].y ,arcVerts[i].x);
            vertices[i * 2 + 1] = new Vector3(meshWidth * -0.5f, arcVerts[i].y, arcVerts[i].x);

            //set triangles
            if(i != resolution)
            {
                triangles[i * 12] = i * 2;
                triangles[i * 12 + 1] = triangles[i * 12 + 4] = i * 2 + 1;
                triangles[i * 12 + 2] = triangles[i * 12 + 3] = (i + 1) * 2;
                triangles[i * 12 + 5] = (i + 1) * 2 + 1;

                triangles[i * 12 + 6] = i * 2;
                triangles[i * 12 + 7] = triangles[i * 12 + 10] = (i + 1) * 2;
                triangles[i * 12 + 8] = triangles[i * 12 + 9] = i * 2 + 1;
                triangles[i * 12 + 11] = (i + 1) * 2 + 1;
            }

            mesh.vertices = vertices;
            mesh.triangles = triangles;
        }
    }

    //create an array of Vector3 position for parabola
    Vector3[] CalculateArcArray()
    {
        Vector3[] arcArray = new Vector3[resolution + 1];
        radianAngle = Mathf.Deg2Rad * angle;
        float maxDistance = (velocity * velocity * Mathf.Sin(2 * radianAngle)) / g;

        for (int i = 0; i <= resolution; i++)
        {
            float t = (float)i / (float)resolution;
            arcArray[i] = CalculateArcPoint(t, maxDistance);
        }

        return arcArray;
    }

    //calculate height and distance of each vertex
    Vector3 CalculateArcPoint(float t, float maxDistance)
    {
        float x = t * maxDistance;
        float y = x * Mathf.Tan(radianAngle) - ((g * x * x) / (2 * velocity * velocity * Mathf.Cos(radianAngle) * Mathf.Cos(radianAngle)));
        return new Vector3(x, y);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ground") && renderer.enabled == true)
        {
            renderer.material = Resources.Load("Correct_zone", typeof(Material)) as Material;
            right = true;
            position = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
            renderer.enabled = false;
        }
        else
        {
            renderer.material = Resources.Load("Bad_zone", typeof(Material)) as Material;
            right = false;
        }
    }

    public void SetVelocity(float vel)
    {
        this.velocity = vel;
        MakeArcMesh(CalculateArcArray());
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

}
