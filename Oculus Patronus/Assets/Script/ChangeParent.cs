using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeParent : MonoBehaviour {

    Transform oldParrent;
    Rigidbody rb;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void changeParent(GameObject gameObject)
    {
        oldParrent = transform;
        transform.parent= gameObject.transform.parent;
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    public void reset()
    {
        transform.parent = oldParrent;
        rb.isKinematic = false;
        rb.useGravity = true;
    }
}
