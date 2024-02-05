using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Connection;
using FishNet.Object;
using FishNet;
using FishNet.Demo.AdditiveScenes;

public class PickupGuide : NetworkBehaviour
{
    [SerializeField] private Transform _pickupPoint;
    [SerializeField] LayerMask pickupLayer;
    [SerializeField] Transform pickupPosition;
    [SerializeField] KeyCode pickupButton = KeyCode.E;
    [SerializeField] KeyCode dropButton = KeyCode.Q;
    [SerializeField] private int pickupfound;
 
    //Camera cam;
    bool hasObjectInHand;
    GameObject objInHand;
    Transform worldObjectHolder;
    private readonly Collider[] _colliders = new Collider[3];

    public override void OnStartClient()
    {
        base.OnStartClient();
 
        if (!base.IsOwner)
            enabled = false;
 
       // cam = Camera.main;
       worldObjectHolder = GameObject.FindGameObjectWithTag("WorldObjects").transform;
        Debug.Log(worldObjectHolder.name);
    }
 
    private void Update()
    {
        if (Input.GetKeyDown(pickupButton))
            Pickup();
 
        if (Input.GetKeyDown(dropButton))
            Drop();
    }
 
    void Pickup()
    {
        pickupfound = Physics.OverlapSphereNonAlloc(_pickupPoint.position, 0.5f, _colliders, pickupLayer);
        
        if (pickupfound>0){
            //refrencint the interactable from this collider
            var pickupable = _colliders[0].transform.gameObject;
            var entity = pickupable.GetComponent<IEntity>();
            Debug.Log(pickupable);
           
            if( entity != null && !hasObjectInHand)
            {
                SetObjectInHandServer(pickupable, pickupPosition.position, pickupPosition.rotation, gameObject);
                hasObjectInHand = true;
                objInHand = pickupable;
                pickupable.transform.parent = this.transform;
            }
            else if(entity == null && hasObjectInHand)
            {
                Drop();
                //SetObjectInHandServer(pickupable, pickupPosition.position, pickupPosition.rotation, gameObject);
                objInHand = pickupable;
                hasObjectInHand = false;
                pickupable.transform.parent = worldObjectHolder.transform;
            }
            else if (entity == null && !hasObjectInHand )
            {
                //pickupable.transform.parent = worldObjectHolder.transform;
                Drop();
                //SetObjectInHandServer(pickupable, pickupPosition.position, pickupPosition.rotation, gameObject);
                objInHand = pickupable;
                hasObjectInHand = false;
                pickupable.transform.parent = worldObjectHolder.transform;
            }
        }
    }
 
    [ServerRpc(RequireOwnership = false)]
    void SetObjectInHandServer(GameObject obj, Vector3 position, Quaternion rotation, GameObject player)
    {
        SetObjectInHandObserver(obj, position, rotation, player);
    }
 
    [ObserversRpc]
    void SetObjectInHandObserver(GameObject obj, Vector3 position, Quaternion rotation, GameObject player)
    {
            obj.transform.parent = player.transform;
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            

            if (obj.GetComponent<Rigidbody>() != null)
                obj.GetComponent<Rigidbody>().isKinematic = true;
    }
 
    void Drop()
    {
        if(!hasObjectInHand)
            return;
 
        DropObjectServer(objInHand, worldObjectHolder);
        hasObjectInHand = false;
        objInHand = null;
    }
 
    [ServerRpc(RequireOwnership = false)]
    void DropObjectServer(GameObject obj, Transform worldHolder)
    {
        DropObjectObserver(obj, worldHolder);
    }
 
    [ObserversRpc]
    void DropObjectObserver(GameObject obj, Transform worldHolder)
    {
        obj.transform.parent = worldObjectHolder;
        Debug.Log(worldHolder);

        if(obj.GetComponent<Rigidbody>() != null)
            obj.GetComponent<Rigidbody>().isKinematic = false;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_pickupPoint.position,0.5f);
    }
}