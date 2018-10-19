using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrimoireController : MonoBehaviour {

    private Animator animator;
	// Use this for initialization
	void Start () {
        animator = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if(OVRInput.GetDown(OVRInput.Touch.PrimaryThumbRest, OVRInput.Controller.Touch))
        {
            if (animator.GetBool("IsOpen"))
            {
                animator.SetBool("IsOpen", false);
            }
        }
        if (OVRInput.GetUp(OVRInput.Touch.PrimaryThumbRest, OVRInput.Controller.Touch))
        {
            if (!animator.GetBool("IsOpen"))
            {
                animator.SetBool("IsOpen", true);
            }
        }
    }
}
