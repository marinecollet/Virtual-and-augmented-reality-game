using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Choose_wand : MonoBehaviour {

    public GameObject pivot;
    private bool canGrab =  false;
    private Collider wand;
    private bool grabbed = false;
    public GameObject collider_sort;
    Rigidbody rig = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wand"))
        {
            Debug.Log(other.name);
            wand = other;
            canGrab = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Wand") && !grabbed)
        {
            wand = null;
            Debug.Log("OK");
            canGrab = false;
        }
    }

    void Update () {
        if (canGrab && OVRInput.GetDown(OVRInput.RawButton.RHandTrigger) && wand!=null)
        {
            wand.gameObject.transform.SetParent(pivot.transform);
            grabbed = true;
            canGrab = false;
            collider_sort.SetActive(true);
            Vector3 position = new Vector3(0f,0.07f,0.07f);
            Quaternion rotation = Quaternion.Euler(-45, 0, 0);
            wand.gameObject.transform.localPosition = position;
            wand.gameObject.transform.localRotation = rotation;
            rig = wand.gameObject.GetComponent<Rigidbody>();
            rig.useGravity = false;
            Debug.Log("gravity grab" + (rig.useGravity == false));

        }
        else if (grabbed && OVRInput.GetDown(OVRInput.RawButton.RHandTrigger))
        {
            wand.gameObject.transform.SetParent(null);
            grabbed = false;
            collider_sort.SetActive(false);
            rig.useGravity = true;
            Debug.Log("gravity degrab" + (rig.useGravity == true));
        }

    }
}


