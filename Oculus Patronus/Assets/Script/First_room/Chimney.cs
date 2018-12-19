using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chimney : MonoBehaviour {


    public Transform player;
    public Transform camera;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wand"))
        {
            Debug.Log(other.name);
            player.position = new Vector3(0f,player.position.y + 5f,0f);
            camera.position = new Vector3(0f,camera.position.y + 5f, 0f);
        }
    }


    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
