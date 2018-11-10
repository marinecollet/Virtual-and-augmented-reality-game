using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Page_left : MonoBehaviour {

    private Renderer renderer;


    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Wand"))
        {
            renderer = GetComponent<SkinnedMeshRenderer>();
            renderer.material = Resources.Load("material1", typeof(Material)) as Material;
        }
    }
    


}
