using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Page_left : MonoBehaviour
{
    public Renderer page_right;

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
        Renderer page_left = GetComponent<SkinnedMeshRenderer>();
        switch (name)
        {
            case "page_left (Instance)":
                break;

            case "material_page_1 (Instance)":
                page_left.material = Resources.Load("page_left", typeof(Material)) as Material;
                page_right.material = Resources.Load("page_right", typeof(Material)) as Material;
                break;

            case "material_page_2 (Instance)":
                page_left.material = Resources.Load("material_page_1", typeof(Material)) as Material;
                page_right.material = Resources.Load("material_page_1", typeof(Material)) as Material;
                break;

            case "material_page_3 (Instance)":
                page_left.material = Resources.Load("material_page_2", typeof(Material)) as Material;
                page_right.material = Resources.Load("material_page_2", typeof(Material)) as Material;
                break;

            case "material_page_4 (Instance)":
                page_left.material = Resources.Load("material_page_3", typeof(Material)) as Material;
                page_right.material = Resources.Load("material_page_3", typeof(Material)) as Material;
                break;

            default:
                page_left.material = Resources.Load("page_left", typeof(Material)) as Material;
                page_right.material = Resources.Load("page_right", typeof(Material)) as Material;
                break;
        }
    }
}
