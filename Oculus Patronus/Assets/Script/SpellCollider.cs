using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpellCollider : MonoBehaviour {

    public SpellColliderType colliderList;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enter" + other.gameObject.tag);

        if (other.gameObject.CompareTag("Wand"))
        {
            other.GetComponent<WandManager>().AddSortCollider(colliderList);
        }
    }
}
