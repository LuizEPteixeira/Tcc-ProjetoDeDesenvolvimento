using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TerminarFase : MonoBehaviour {

    private Rigidbody2D rb;

    [Header("Verificadores")]
    [SerializeField] private LayerMask finalFase;
    [SerializeField] private string levelLoad;


    // Use this for initialization
    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        
    }



    // Update is called once per frame
    void Update () {

        if (rb.IsTouchingLayers(finalFase))
        {
            
            SceneManager.LoadScene(levelLoad);
        }


		
	}
}
