using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;


public class PlayerSansCasque : Player
{
    public GameObject[] rends;

    public float speed;

    private Animator grimAnimator;
    private Transform child;
    public GameObject fakeHands;
    public GrimoireController grimoire;

    public float speedH = 2.0f;
    public float speedV = 2.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    //public override IEnumerator moveToFirstRoomAfter()
    //{
    //    yield return new WaitForSeconds(3);
    //    FirstRoom.gameObject.SetActive(true);
    //    this.move(new Vector3(28,1.6f,0));
    //    life = lifeAtStart;
    //    isDead = false;
    //    fakeHands.SetActive(true);
    //}

    public override void moveToFirstRoomAfter()
    {
        FirstRoom.gameObject.SetActive(true);
        this.move(new Vector3(28, 1.6f, 0));
        life = lifeAtStart;
        isDead = false;
        fakeHands.SetActive(true);
    }

    public override void  Awake()
    {
        isDead = false;
        life = lifeAtStart;
        damageOverlay.canvasRenderer.SetAlpha(0);
        grimAnimator = grimoire.GetComponent<Animator>();
        child = this.transform.GetChild(0);
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyShot"))
        {
            Destroy(other.gameObject);
            this.life -= lifeLosePerShot;

            isHurt = true;

            damageOverlay.canvasRenderer.SetAlpha((lifeAtStart - life)/ lifeAtStart);
            fadeOutTime = lifeAtStart - life;
            if (life <= 0)
            {
                isDead = true;
                Destroy(wand.gameObject);
                Destroy(spellDetector.gameObject);
                GameObject maze = GameObject.Find("Maze(Clone)");
                Destroy(maze);
                moveToFirstRoomAfter();
            }
        }
    }

    private void Update()
    {
        if(isHurt && !isDead)
        {
            fadeOutTime -= Time.deltaTime;
            if (fadeOutTime < 0)
            {
                fadeOutTime = 0;
                isHurt = false;
            }
            damageOverlay.canvasRenderer.SetAlpha(fadeOutTime / lifeAtStart);
        }
    }

    private void FixedUpdate()
    {
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (!grimAnimator.GetBool("IsOpen"))
            {
                spellDetector.gameObject.SetActive(false);
                foreach (GameObject rend in rends)
                {
                    rend.SetActive(true);
                }
                grimAnimator.SetBool("IsOpen", true);
            }
            else
            {
                grimAnimator.SetBool("IsOpen", false);
                StartCoroutine(waita(1));
            }
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            float moveWandHorizontal = Input.GetAxis("Mouse X");
            float moveWandVertical = Input.GetAxis("Mouse Y");

            //Vector3 wandForward = new Vector3(wand.up.x, wand.up.y, 0).normalized * moveWandVertical;
            //Vector3 wandRight = new Vector3(wand.right.x,  wand.right.y, 0).normalized * moveWandHorizontal;


            Vector3 wandUp = Vector3.up * moveWandVertical;
            Vector3 wandRight = Vector3.right * moveWandHorizontal;

            //Vector3 wandUp = transform.worldToLocalMatrix.MultiplyVector(wand.up) * moveWandVertical;
            //Vector3 wandRight = transform.worldToLocalMatrix.MultiplyVector(wand.right) * moveWandHorizontal;

            Vector3 move = (wandUp + wandRight) * 0.1f;

            wand.localPosition += move;

            if (wand.localPosition.y > 0.5f)
                wand.localPosition = new Vector3(wand.localPosition.x, 0.5f, wand.localPosition.z);
            else if (wand.localPosition.y < -0.5f)
                wand.localPosition =new Vector3(wand.localPosition.x, -0.5f, wand.localPosition.z);
            if (wand.localPosition.x > 0.3f)
                wand.localPosition = new Vector3(0.3f, wand.localPosition.y, wand.localPosition.z);
            else if (wand.localPosition.x < -0.6f)
                wand.localPosition = new Vector3(-0.6f, wand.localPosition.y, wand.localPosition.z);

        }
        else 
        {
            yaw += speedH * Input.GetAxis("Mouse X");
            pitch -= speedV * Input.GetAxis("Mouse Y");

            child.eulerAngles = new Vector3(pitch, yaw, 0.0f);
        }
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 forward = new Vector3(child.forward.x, 0, child.forward.z).normalized * moveVertical;
        Vector3 right = new Vector3(child.right.x, 0, child.right.z).normalized * moveHorizontal;

        Vector3 movement = (forward + right).normalized * speed;
        move(this.transform.position + movement);
    }

    public override void reset()
    {
        life = lifeAtStart;
    }

    public override void move(Vector3 newPosition){
        this.transform.localPosition = newPosition;
    }

    IEnumerator waita(float a)
    {
        yield return new WaitForSeconds(a);
        foreach (GameObject rend in rends)
        {
            rend.SetActive(false);
        }
        spellDetector.gameObject.SetActive(true);
    }
}