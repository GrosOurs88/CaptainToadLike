using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Speed = 1.0f;
    public float LifeTime = 1.0f;
    private float Timer = 0.0f;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        rb.velocity = transform.forward * Speed * Time.deltaTime;

        Timer += Time.deltaTime;
        if (Timer >= LifeTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().TakeDamage();
            Destroy(gameObject);
        }
        if (other.CompareTag("Floor"))
        {
            Destroy(gameObject);
        }
    }
}
