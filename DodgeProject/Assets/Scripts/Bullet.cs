using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    private float speed = 8f;
    private Rigidbody bulletRigidbody;

    public IObjectPool<Bullet> pool;

    void Start()
    {
        bulletRigidbody = GetComponent<Rigidbody>();
        bulletRigidbody.velocity = transform.forward * speed;

        Invoke("PoolRelease", 3);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            PlayerController playerController = other.GetComponent<PlayerController>();

            if (playerController)
            {
                playerController.Die();
            }
        }
    }

    public void PoolRelease()
    {
        pool.Release(this);
    }
}
