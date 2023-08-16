using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemTextScript : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject playerCamera;
    public float DestroyTime = 3f;

    void Start()
    {
        playerCamera = GameObject.FindGameObjectWithTag("playerCamera");
        Destroy(gameObject, DestroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(2*transform.position-playerCamera.transform.position);
    }
}
