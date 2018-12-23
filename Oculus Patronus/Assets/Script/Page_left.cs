using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Page_left : MonoBehaviour {
       
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wand"))
        {
            Renderer renderer = GetComponent<SkinnedMeshRenderer>();
            renderer.material = Resources.Load("material_page_1", typeof(Material)) as Material;
        }
    }
}
