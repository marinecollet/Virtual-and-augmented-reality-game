using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpellCollider : MonoBehaviour {

    public SpellColliderType colliderList;

    private Renderer render;

    public void Start()
    {
        render = this.GetComponent<Renderer>();
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enter" + other.gameObject.tag);

        if (other.gameObject.CompareTag("Wand"))
        {
            other.GetComponent<WandManager>().AddSortCollider(colliderList);
        }

        render.material.color = Color.green;
    }

    private void OnTriggerExit(Collider other)
    {
        render.material.color = Color.gray;
    }
}
