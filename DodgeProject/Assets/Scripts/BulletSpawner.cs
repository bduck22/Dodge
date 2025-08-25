using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float spawnRateMin = 0.5f;
    public float spawnRateMax = 3f;

    private Transform target;
    private float spawnRate;
    private float timeAfterSpawn;

    IObjectPool<Bullet> pool;

    public Transform[] Spawners;
    private void Awake()
    {
        Spawners = new Transform[transform.childCount];
        for(int i = 0; i < Spawners.Length; i++)
        {
            Spawners[i] = transform.GetChild(i);
        }
        pool = new ObjectPool<Bullet>(OnCreateBullet, OnGetBullet, OnReleaseBullet, OnDestroyBullet, maxSize:25);
    }


    void Start()
    {
        timeAfterSpawn = 0f;
        spawnRate = Random.Range(spawnRateMin, spawnRateMax);
        target = FindObjectOfType<PlayerController>().transform;
    }

    
    void Update()
    {
        timeAfterSpawn += Time.deltaTime;

        if(timeAfterSpawn >= spawnRate)
        {
            timeAfterSpawn = 0f;
            var bullet = pool.Get();
            spawnRate = Random.Range(spawnRateMin, spawnRateMax);
        }
    }

    public Bullet OnCreateBullet()
    {
        Bullet bullet = Instantiate(bulletPrefab, Spawners[Random.Range(0, 4)].position, transform.rotation).GetComponent<Bullet>();
        bullet.transform.LookAt(target);
        bullet.pool = pool;
        return bullet;
    }

    public void OnGetBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(true);
        bullet.transform.position = Spawners[Random.Range(0, 4)].position;
        bullet.transform.LookAt(target);
    }

    public void OnReleaseBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    public void OnDestroyBullet(Bullet bullet)
    {
        Destroy(bullet.gameObject);
    }
}
