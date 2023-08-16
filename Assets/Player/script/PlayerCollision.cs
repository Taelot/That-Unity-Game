using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public ParticleSystem cherryInteractParticle;
    public ParticleSystem itemInteractParticle;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Cherry")
        {
            this.cherryInteractParticle.Play();
            Destroy(col.gameObject);
            //Debug.Log("Coll!!!!!!!!!!!!!!");
        }
        if (col.gameObject.tag == "Boot" || col.gameObject.tag == "Potion")
        {
            this.itemInteractParticle.Play();

        }
    }
}
