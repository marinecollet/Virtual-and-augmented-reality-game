using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WandManager : MonoBehaviour {

    // Use this for initialization

    public float range = 100f;

    private Dictionary<string,List<SpellColliderType>> colliderDictio;

    private SpellTree spellTree;

    [HideInInspector]
    public bool isReading;

    Ray shootRay;
    RaycastHit shootHit;
    ParticleSystem spellParticles;

    

    private Collider spellCollider;
	void Start () {
        spellCollider = this.GetComponent<Collider>();
        isReading = false;

        colliderDictio = new Dictionary<string, List<SpellColliderType>>();

        List<SpellColliderType> colliderList = new List<SpellColliderType>();
        colliderList.Add(SpellColliderType.RIGHT);
        colliderList.Add(SpellColliderType.CENTER);
        colliderDictio.Add("shoot", colliderList);

        List<SpellColliderType> colliderList2 = new List<SpellColliderType>();
        colliderList2.Add(SpellColliderType.RIGHT);
        colliderList2.Add(SpellColliderType.TOP);
        colliderList2.Add(SpellColliderType.CENTER);
        colliderDictio.Add("test", colliderList2);


        spellParticles = GetComponent<ParticleSystem>();

        spellTree = new SpellTree();

        foreach(string key in colliderDictio.Keys)
        {
            spellTree.addSpell(colliderDictio[key], key);
        }

        spellTree.DebugTree();
    }

    public void AddSortCollider(SpellColliderType col)
    {
        if (this.spellTree.advance(col))
        {
            string spell = this.spellTree.isSpell();

            if (spell != null)
            {
                launchSpell(spell);
                spellTree.resetActualNode();
            }
        }
        else
        {
            spellTree.resetActualNode();
        }
    }

    void launchSpell(string spell)
    {
        Debug.Log(spell);

        // Stop the particles from playing if they were, then start the particles.
        spellParticles.Stop();
        spellParticles.Play();

        // Enable the line renderer and set it's first position to be the end of the gun.
    }

    public struct SpellDefinition
    {
        public string spellName;
        public List<SpellColliderType> colliderOrder;
    }
}
