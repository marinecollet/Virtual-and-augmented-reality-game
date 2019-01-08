using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;


public class Player : MonoBehaviour
{
    public int lifeAtStart;
    public int lifeLosePerShot;
    public bool isDead { get; protected set; }
    public Image damageOverlay;
    public Transform wand;
    public Transform spellDetector;
    protected int life;
    public Transform localAvatar;
    public Transform FirstRoom;

    protected bool isHurt = false;
    protected float fadeOutTime;

    //public virtual IEnumerator moveToFirstRoomAfter(int a)
    //{
    //    yield return new WaitForSeconds(a);
    //    FirstRoom.gameObject.SetActive(true);
    //    this.move(new Vector3(28,0,0));
    //    life = lifeAtStart;
    //    isDead = false;
    //}

    public virtual void moveToFirstRoomAfter()
    {
        FirstRoom.gameObject.SetActive(true);
        this.move(new Vector3(27.5f, 0, 0));
        life = lifeAtStart;
        isDead = false;
    }

    public virtual void Awake()
    {
        isDead = false;
        life = lifeAtStart;
        damageOverlay.canvasRenderer.SetAlpha(0);
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyShot"))
        {
            Destroy(other.gameObject);
            this.life -= lifeLosePerShot;

            isHurt = true;

            damageOverlay.canvasRenderer.SetAlpha((lifeAtStart - life)/ lifeAtStart);
            Debug.Log("a "+ (float)((lifeAtStart - life) / lifeAtStart));
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

    public virtual void reset()
    {
        life = lifeAtStart;
    }

    public virtual void move(Vector3 newPosition){
        this.transform.localPosition = newPosition;
        localAvatar.localPosition = newPosition;
    }
}