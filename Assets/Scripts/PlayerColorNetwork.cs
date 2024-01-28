using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Connection;
using FishNet.Object;


public class PlayerColorNetwork : NetworkBehaviour
{

    public GameObject body;
    public Color endColor;
    public override void OnStartClient()
    {
        base.OnStartClient();
        if (base.IsOwner)
        {
            GetComponent<PlayerColorNetwork>().enabled = true;
        }
        else
        {
            GetComponent<PlayerColorNetwork>().enabled = false;
        }
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.F))
        {
            ChangeColour(gameObject, endColor);
        }
    }

    [ServerRpc]
    public void ChangeColourServer(GameObject player, Color color)
    {
        ChangeColour(player,color);
    }

    [ObserversRpc]
    public void ChangeColour(GameObject player, Color color )
    {
        player.GetComponent<PlayerColorNetwork>().body.GetComponent<Renderer>().material.color = color;
    }
}
