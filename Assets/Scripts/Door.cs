using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, Interactable
{
    [SerializeField] private string _prompt;

    public string InteractionPrompt => _prompt; 
    
    //Interact to the interactor 
    public bool Interact(Interactor interactor){
        Debug.Log("Door");
        return(true);
    }
}
