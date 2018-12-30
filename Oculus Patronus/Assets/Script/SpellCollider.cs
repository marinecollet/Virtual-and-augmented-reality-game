using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpellCollider : MonoBehaviour {

    public SpellColliderType colliderList;
    public Material defaultMat;
    public Material EnterMat;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enter" + other.gameObject.tag);

        if (other.gameObject.CompareTag("Wand"))
        {
            this.GetComponent<Renderer>().material= EnterMat;
            if(other.GetComponent<WandManagerAlone>() != null)
                other.GetComponent<WandManagerAlone>().AddSortCollider(colliderList);
            else
                other.GetComponent<WandManager>().AddSortCollider(colliderList);

        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Enter" + other.gameObject.tag);

        if (other.gameObject.CompareTag("Wand"))
        {
            this.GetComponent<Renderer>().material = defaultMat;
        }
    }
}
