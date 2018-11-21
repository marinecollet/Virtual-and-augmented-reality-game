using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaguetteManager : MonoBehaviour {

    // Use this for initialization

    private Collider collider;
	void Start () {
        collider = this.GetComponent<Collider>();
	}

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
		Debug.Log("Enter" + other.gameObject.tag);

        if (other.gameObject.CompareTag("Sort"))
        {
			Debug.Log("Succes");
            MeshRenderer meshRender = other.gameObject.GetComponent<MeshRenderer>();
            meshRender.sharedMaterial.color = Color.green; 
        }
    }
}
