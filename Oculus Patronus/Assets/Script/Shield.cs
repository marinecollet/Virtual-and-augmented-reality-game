<<<<<<< HEAD
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {

    public float tempAction = 1.5f; // Action temporisée de 0.5s
    float temps = 0;
    bool action = false;
    public GameObject sortDetection;


    void Update()
    {
        if (temps == 0 && gameObject.activeSelf)
        {
            action = true;
        }

        if (action)
        {
            temps += Time.deltaTime; // temps écoulé
            if (temps >= tempAction)
            {
                action = false;
                temps = 0;
                sortDetection.SetActive(true);
                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyShot"))
        {
            Destroy(other.gameObject);
        }
    }
}

=======
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {

    public float tempAction = 2f; // Action temporisée de 0.5s
    float temps = 0;
    bool action = false;
    public GameObject sortDetection;


    void Update()
    {
        if (temps == 0 && gameObject.activeSelf)
        {
            action = true;
        }

        if (action)
        {
            temps += Time.deltaTime; // temps écoulé
            if (temps >= tempAction)
            {
                action = false;
                temps = 0;
                sortDetection.SetActive(true);
                gameObject.SetActive(false);
            }
        }
    }
}

>>>>>>> master
