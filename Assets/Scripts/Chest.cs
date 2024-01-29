using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet;
using FishNet.Connection;
using FishNet.Broadcast;

public class Chest : NetworkBehaviour, Interactable
{
    [SerializeField] private string _prompt;

    public string InteractionPrompt => _prompt; 
    [SerializeField]private bool interacted = false;

    //Interact to the interactor 
    public bool Interact(Interactor interactor){
        Debug.Log(_prompt);
        return(true);
        //UpdateInteraction(this , true);
    }
/*
    [ServerRPC]
    public void UpdateInteraction(Chest chest, bool used){
        chest.interacted = used;
    }
    */
}
