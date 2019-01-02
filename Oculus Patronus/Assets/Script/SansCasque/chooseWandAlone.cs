using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class chooseWandAlone : MonoBehaviour {

    public GameObject pivot;
    private bool canGrab = false;
    private Collider wand;
    private bool grabbed = false;
    public GameObject collider_sort;
    Rigidbody rig = null;
    public GameObject objectDisappear;

    private Renderer renderer;

    public void Awake()
    {
        renderer = this.GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);

        if (other.gameObject.CompareTag("Wand"))
        {
            Debug.Log(other.name);
            wand = other;
            canGrab = true;
            this.renderer.material.color = Color.green;
        }

        if (other.gameObject.CompareTag("quit"))
        {
            Debug.Log("quit");
            Application.Quit();
        }

        if (other.gameObject.CompareTag("restart"))
        {
            Debug.Log("restart");
            SceneManager.LoadScene("SceneSansCasque autonome");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Wand") && !grabbed)
        {
            wand = null;
            Debug.Log("OK");
            canGrab = false;
            this.renderer.material.color = Color.white;
        }
    }

    void Update()
    {
        if (canGrab && Input.GetMouseButtonDown(0) && wand != null)
        {
            wand.gameObject.transform.SetParent(pivot.transform);
            grabbed = true;
            canGrab = false;
            collider_sort.SetActive(true);
            Vector3 position = new Vector3(0f, 0.07f, 0.07f);
            //Quaternion rotation = Quaternion.Euler(-45, 0, 0);
            wand.gameObject.transform.localPosition = position;
            wand.gameObject.transform.localRotation = Quaternion.identity;
            rig = wand.gameObject.GetComponent<Rigidbody>();
            rig.useGravity = false;
            objectDisappear.SetActive(false);
            Debug.Log("disppear");
            wand.gameObject.GetComponent<WandManagerAlone>().enabled = true;
            this.GetComponent<BoxCollider>().enabled = false;
            this.GetComponent<Renderer>().enabled = false;
        }

    }
}
