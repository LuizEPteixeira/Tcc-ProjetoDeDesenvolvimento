using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FixedButton : MonoBehaviour, IPointerClickHandler
{

    [HideInInspector]
    public bool Pressed = false;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if (Pressed){

            Pressed = false;

        }
            
            
		
	}

    
    public void OnPointerClick(PointerEventData eventData)
    {
        Pressed = true;
    }
}
