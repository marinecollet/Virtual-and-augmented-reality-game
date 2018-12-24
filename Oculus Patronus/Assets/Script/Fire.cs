using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{

    public Transform player;
    public Transform camera;
    public GameObject FirstRoom;
    ParticleSystem particle;

    bool playerIsIn;

    IEnumerator MovePlayerAfter()
    {

        yield return new WaitForSeconds(3);
        player.localPosition = new Vector3(0f, 0f, 0f);
        camera.localPosition = new Vector3(0f, 0f, 0f);
        Destroy(FirstRoom);
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
            Destroy(other.gameObject);
            particle.Play();
            StartCoroutine(MovePlayerAfter());
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
