﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeParent : MonoBehaviour {

    Rigidbody rb;
    Vector3 lastPos;
    Transform parent;
    public void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void changeParent(GameObject gameObject)
    {
        parent = this.transform.parent;
        transform.parent = gameObject.transform.parent;
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    public void reset()
    {
        transform.parent = null;
        rb.isKinematic = false;
        rb.useGravity = true;
        this.transform.parent = parent;
        rb.AddForce((this.transform.position - lastPos) * 1000);
    }

    public void FixedUpdate()
    {
        lastPos = this.transform.position;
    }
}
