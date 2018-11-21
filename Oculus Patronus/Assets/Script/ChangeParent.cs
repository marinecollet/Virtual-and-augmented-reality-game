using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeParent : MonoBehaviour {

    Rigidbody rb;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void changeParent(GameObject gameObject)
    {
        transform.parent= gameObject.transform.parent;
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    public void reset()
    {
        transform.parent = null;
        rb.isKinematic = false;
        rb.useGravity = true;
    }
}
