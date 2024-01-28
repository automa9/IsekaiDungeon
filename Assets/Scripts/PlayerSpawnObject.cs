using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class PlayerSpawnObject : NetworkBehaviour
{
    public GameObject objectSpawn;
    [HideInInspector] public GameObject spawnedObject;

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (!base.IsOwner)
        {
            GetComponent<PlayerSpawnObject>().enabled = false;
        }
    }

    private void Update()
    {
        if (spawnedObject == null && Input.GetKeyDown(KeyCode.Alpha1))
        {
            //run the spawn on the server
            SpawnObject(objectSpawn, transform, this);
        }
        if (spawnedObject != null && Input.GetKeyDown(KeyCode.Alpha2))
        {
            //despawn the spawned
            DespawnObject(spawnedObject);
        }
    }

    [ServerRpc]
    public void SpawnObject(GameObject obj, Transform player, PlayerSpawnObject script)
    {
        //Instantiate the object 
        GameObject spawned = Instantiate(obj, player.position + player.forward, Quaternion.identity);
        //put the object to the server
        ServerManager.Spawn(spawned);
        //begin to spawn to observers so they can also see the spawned object
        SetSpawnedObject(spawned,script);
    }

    [ObserversRpc]
    public void SetSpawnedObject(GameObject spawned, PlayerSpawnObject script)
    {
        //reference the spawned from the script
        script.spawnedObject = spawned;
    }

    [ServerRpc(RequireOwnership = false)]
    public void DespawnObject(GameObject obj)
    {
        ServerManager.Despawn(obj);
    }
}
