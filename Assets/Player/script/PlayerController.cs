// COMP30019 - Graphics and Interaction
// (c) University of Melbourne, 2022

using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public float speed = 1.0f;
    public float rotationSpeed = 1.0f;
    public float maxSpeed;
    public bool bootBuff;
    public bool speedBuffCheck;
    public bool speedDebuffCheck;
    float bootBuffDuration = 1000;
    float speedBuffDuration = 1000;
    float speedDebuffDuration = 200;
    private Animator anim;
    public GameObject avator;
    private bool inWater = false;
    // how many seconds to play the drowning animation
    private float drownCount = 0;
    private PlayerStat ps;
    void Start()
    {
        anim = avator.GetComponent<Animator>();
        ps = GameObject.FindGameObjectWithTag("PlayerStat").GetComponent<PlayerStat>();
    }

    // Update is called once per frame
    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        transform.position += verticalInput * transform.forward * Time.deltaTime * speed;
        transform.eulerAngles += horizontalInput * Vector3.up * rotationSpeed;

        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        movementDirection.Normalize();

        if (movementDirection != Vector3.zero)
        {
            if (this.speed < maxSpeed && speed != -0.00001f)
            {
                this.speed += 0.5f;
            }
        }
        else
        {
            if (this.speed != 0.0f && speed != -0.00001f)
            {
                this.speed -= 0.5f;
            }
        }
        anim.SetFloat("Speed", this.speed);

        if (speedBuffCheck)
        {
            speedBuffDuration -= Time.deltaTime * 60;

            if (speedBuffDuration <= 0)
            {
                speedBuffCheck = false;
            }
        }

        if (speedDebuffCheck)
        {
            speedDebuffDuration -= Time.deltaTime * 60;

            if (speedDebuffDuration <= 0)
            {
                speedDebuffCheck = false;
            }
        }



        drown();
        bootBuffControl();

    }
    private void bootBuffControl()
    {
        //controls bootBuff
        if (bootBuff)
        {
            bootBuffDuration -= Time.deltaTime * 60;

            if (bootBuffDuration <= 0)
            {
                bootBuff = false;
                bootBuffDuration = 1000;
            }
        }
    }
    private void drown()
    {
        if (!inWater)
        {
            if (inWaterCheck())
            {
                inWater = true;
                // time to play animation
                drownCount = 2f;
                GetComponent<Rigidbody>().useGravity = false;
                GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                speed = -0.00001f;
                //please call on other functions to play drowning animation from here @Yung
                //this if statement will only enter once
                if (!GameObject.FindGameObjectWithTag("drownSound").GetComponent<AudioSource>().isPlaying && this.speed != 0f)
                {
                    GameObject.FindGameObjectWithTag("drownSound").GetComponent<AudioSource>().PlayOneShot(GameObject.FindGameObjectWithTag("drownSound").GetComponent<AudioSource>().clip);
                }
            }
        }
        else
        {
            drownCount -= Time.deltaTime;
            if (drownCount <= 0)
            {   
                ps.gameStop = true;
                SceneManager.LoadScene("DrownEnd");
            }
        }
    }
    public void onMud()
    {
        if (!bootBuff && !speedBuffCheck)
            this.speed = 2.0f;
        else if (!bootBuff && speedBuffCheck)
            this.speed = 5.0f;
        //GameObject.FindGameObjectWithTag("mudSound").GetComponent<AudioSource>().PlayOneShot();
        if (!GameObject.FindGameObjectWithTag("mudSound").GetComponent<AudioSource>().isPlaying&&this.speed!=0f)
        {
            GameObject.FindGameObjectWithTag("mudSound").GetComponent<AudioSource>().PlayOneShot(GameObject.FindGameObjectWithTag("mudSound").GetComponent<AudioSource>().clip);
        }
    }
    public void onGrass()
    {
        if (speedBuffCheck)
        {
            this.maxSpeed = 15.0f;
        }
        else if (speedDebuffCheck)
        {
            this.speed = 3.0f;
        }
        else
            this.maxSpeed = 10.0f;
        //GameObject.FindGameObjectWithTag("grassSound").GetComponent<AudioSource>().PlayOneShot();


    }
    public void onSand()
    {
        if (!bootBuff)
            this.speed = 8.0f;
        //GameObject.FindGameObjectWithTag("grassSound").GetComponent<AudioSource>().PlayOneShot();

    }
    private bool inWaterCheck()
    {
        return transform.position.y < -1.5f;
    }
    public void speedBuff()
    {
        this.speedBuffCheck = true;
        this.speedDebuffCheck = false;
        this.speedBuffDuration = 1000;
    }
    public void speedDebuff()
    {
        this.speedDebuffCheck = true;
        this.speedBuffCheck = false;
        this.speedDebuffDuration = 200;
    }
}
