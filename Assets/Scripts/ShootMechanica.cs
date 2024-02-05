using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class ShootMechanica : NetworkBehaviour
{
    [SerializeField] private GameObject bulletprefab; 
    [SerializeField] private float bulletSpeed;
    private List<PerformantBullet> _spawnedBullet = new List<PerformantBullet>();

    // Update is called once per frame
    void Update()
    {
        foreach (var bullet in _spawnedBullet){
            bullet.bulletTrasform.position += bullet.Direction*Time.deltaTime * bulletSpeed;
        }

        if(!IsOwner){
            return;
        }
        //everyone runs
        if(Input.GetKeyDown(KeyCode.Mouse0)){
            Shoot();
        }
    }

    private void Shoot(){
        Vector3 startpos = transform.position + transform.forward *0.5f+transform.up;
        Vector3 direction = transform.forward;
        SpawnBullet(startpos, direction);
        SpawnBulletServer(startpos, direction,TimeManager.Tick);
    }

    private void SpawnBullet(Vector3 startpos,Vector3 direction){
        GameObject bullet = Instantiate(bulletprefab, startpos,Quaternion.identity);
        _spawnedBullet.Add(new PerformantBullet(){bulletTrasform = bullet.transform,Direction = direction});
    }

    [ServerRpc]
    private void SpawnBulletServer(Vector3 startpos, Vector2 direction, uint startTick)
    {
        SpawnBulletObserver(startpos, direction, startTick);
    }

    [ObserversRpc (ExcludeOwner = true)]
    private void SpawnBulletObserver(Vector3 startpos, Vector2 direction, uint startTick){
        float timeDifference= (float)(TimeManager.Tick-startTick)/TimeManager.TickRate;
        Vector3 spawnPosition = startpos * direction * bulletSpeed * timeDifference;
        GameObject bullet  = Instantiate(bulletprefab, spawnPosition, Quaternion.identity);
        _spawnedBullet.Add(new PerformantBullet(){bulletTrasform = bullet.transform,Direction = direction});
    }

    private class PerformantBullet{
        public Transform bulletTrasform;
        public Vector3 Direction;
    }
}
