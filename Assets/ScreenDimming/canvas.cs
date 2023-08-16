using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class canvas : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        DontDestroyOnLoad(this);
    }
    // Update is called once per frame
    void Update()
    {
        if (!SceneManager.GetSceneByName("Game").isLoaded) {
            Destroy(gameObject);
        }
    }
}
