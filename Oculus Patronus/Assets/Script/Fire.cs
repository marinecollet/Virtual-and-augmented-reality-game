using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{

    ParticleSystem particle;
    // Use this for initialization
    public Transform player;
    public Transform camera;
    bool playerIsIn;

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Wand"))
    //    {
    //        Debug.Log(other.name);
    //        StartCoroutine(Example());
    //    }
    //}
    IEnumerator Example()
    {

        yield return new WaitForSeconds(3);
        player.localPosition = new Vector3(0f, 0f, 0f);
        camera.localPosition = new Vector3(0f, 0f, 0f);
    }
    void Awake()
    {
        particle = GetComponent<ParticleSystem>();
        playerIsIn = false;

    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsIn = true;
        }
        if (other.CompareTag("cheminette") && playerIsIn)
        {
            particle.Play();
            StartCoroutine(Example());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsIn = false;
        }
    }
}
