using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public float jumpforce = 250f;
    Rigidbody rb;
    float offset;
    private bool isGrounded = false;
    private Animator anim;
    public GameObject avator;
    private bool potionCheck = false;
    private float potionDuration = 1000;

    void Start()
    {
        anim = avator.GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

    }
    void OnCollisionEnter(Collision hit)
    {

        //Debug.Log(hit.gameObject);
        if (hit.gameObject.CompareTag("Ground"))
        {
            //Debug.Log("hit ground");
            anim.SetBool("Jump", false);
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision hit)
    {
        if (hit.gameObject.CompareTag("Ground"))
        {
            //Debug.Log("not on ground ");
            isGrounded = false;
        }
    }
    void Update()
    {


        if (isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                anim.SetBool("Jump", true);
                rb.AddForce(Vector3.up * jumpforce);
            }
        }

        //control jump
        if (potionCheck)
        {
            potionDuration -= Time.deltaTime * 60;
            this.jumpforce = 310f;
            if (potionDuration <= 0)
            {
                potionCheck = false;
                this.jumpforce = 250f;
                potionDuration = 1000;
            }
        }
    }

    public void eatPotion()
    {
        this.potionCheck = true;
    }

}
