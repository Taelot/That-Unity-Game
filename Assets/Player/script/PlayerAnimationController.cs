// COMP30019 - Graphics and Interaction
// (c) University of Melbourne, 2022

using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private float speed;
    private Animator anim;
    private GameObject player;

    //useing gameobject because
    //  https://stackoverflow.com/questions/67143682/unity-particle-system-issue
    public GameObject waterSplashParticle;
    public GameObject grassParticle;
    public GameObject mudParticle;
    public GameObject sandParticle;

    private bool isMud = false;
    private bool isGrass = false;
    private bool isSand = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        player = transform.parent.gameObject;
        this.speed = 1.0f;
    }

    // Update is called once per frame
    private void Update()
    {


        if (anim.GetFloat("Speed") > 0.0f)
        {

            if (this.isMud)
            {
                this.grassParticle.SetActive(false);
                this.mudParticle.SetActive(true);
                this.sandParticle.SetActive(false);
            }
            else if (this.isGrass)
            {
                this.mudParticle.SetActive(false);
                this.grassParticle.SetActive(true);
                this.sandParticle.SetActive(false);
            }
            else if (this.isSand)
            {
                this.sandParticle.SetActive(true);
                this.mudParticle.SetActive(false);
                this.grassParticle.SetActive(false);
            }

        }
        else
        {
            this.mudParticle.SetActive(false);
            this.grassParticle.SetActive(false);
            this.sandParticle.SetActive(false);

        }
        if (anim.GetBool("Jump"))
        {
            this.mudParticle.SetActive(false);
            this.grassParticle.SetActive(false);
            this.sandParticle.SetActive(false);
        }
        inWaterCheck();


    }

    private void inWaterCheck()
    {

        if (this.player.transform.position.y < -1.5f)
        {
            this.waterSplashParticle.SetActive(true);
            this.anim.SetBool("Drown", true);
        }
        else
        {
            this.waterSplashParticle.SetActive(false);
        }
    }

    public void onMud()
    {
        this.isMud = true;
        this.isSand = false;
        this.isGrass = false;

    }
    public void onGrass()
    {
        this.isMud = false;
        this.isSand = false;
        this.isGrass = true;

    }
    public void onSand()
    {

        this.isMud = false;
        this.isSand = true;
        this.isGrass = false;
    }

}
