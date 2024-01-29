using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Interactable
{
    public string InteractionPrompt
    {
        //get from the interactor
        get;
    }
    
    public bool Interact(Interactor interactor);
}
