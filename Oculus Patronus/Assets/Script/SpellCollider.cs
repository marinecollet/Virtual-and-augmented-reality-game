using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpellCollider : MonoBehaviour {

    public SpellColliderType colliderList;

    //bool triggerIn = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enter" + other.gameObject.tag);

        if (other.gameObject.CompareTag("Wand"))// && !triggerIn)
        {
            Debug.Log("Succes");
            //triggerIn = true;
            other.GetComponent<WandManager>().AddSortCollider(colliderList);
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    Debug.Log("Exit" + other.gameObject.tag);

    //    if (other.gameObject.CompareTag("Baguette"))
    //    {
    //        triggerIn = false;
    //    }
    //}
}
