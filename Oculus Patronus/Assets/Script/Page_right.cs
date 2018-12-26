using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Page_right : MonoBehaviour {

    public Renderer page_left;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wand"))
        {
            Renderer renderer = GetComponent<SkinnedMeshRenderer>();
            string name_renderer = renderer.material.name;
            Debug.Log(name_renderer);

            changeMaterial(name_renderer);
        }
    }

    void changeMaterial(string name)
    {
        Renderer page_right = GetComponent<SkinnedMeshRenderer>();
        switch (name)
        {
            case "page_right (Instance)":
                page_left.material = Resources.Load("material_page_1", typeof(Material)) as Material;
                page_right.material = Resources.Load("material_page_1", typeof(Material)) as Material;
                break;

            case "material_page_1 (Instance)":
                page_left.material = Resources.Load("material_page_2", typeof(Material)) as Material;
                page_right.material = Resources.Load("material_page_2", typeof(Material)) as Material;
                break;

            case "material_page_2 (Instance)":
                page_left.material = Resources.Load("material_page_3", typeof(Material)) as Material;
                page_right.material = Resources.Load("material_page_3", typeof(Material)) as Material;
                break;

            case "material_page_3 (Instance)":
                page_left.material = Resources.Load("material_page_4", typeof(Material)) as Material;
                page_right.material = Resources.Load("material_page_4", typeof(Material)) as Material;
                break;

            case "material_page_4 (Instance)":
                break;

            default:
                break;
        }
    }
}
