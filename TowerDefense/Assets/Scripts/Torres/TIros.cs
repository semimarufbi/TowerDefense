using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TIros : MonoBehaviour
{
    private Transform target;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float bulletspeed = 5f;

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    private void FixedUpdate()
    {
        Vector2 direction = (transform.position - target.position).normalized;
        rb.velocity = direction * bulletspeed;
    }
}
