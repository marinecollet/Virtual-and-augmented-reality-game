using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chimney : MonoBehaviour {


    public Transform player;
    public Transform camera;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name + " " +other.tag);
        if (other.gameObject.CompareTag("Wand"))
        {
            Debug.Log(other.name);
            player.localPosition = new Vector3(0f,0f,0f);
            camera.localPosition = new Vector3(0f,0f, 0f);
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
