using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_dropped : MonoBehaviour {


    public AudioSource audioSource;


    void OnCollisionEnter(Collision collision)
    {
        if ((collision.relativeVelocity.magnitude > 2) && (gameObject.transform.parent == null))
        {
            audioSource.Play();
        }
        else if((collision.relativeVelocity.magnitude > 2) && (gameObject.transform.parent.name != "pivot"))
        {
            audioSource.Play();
        }
    }


}
