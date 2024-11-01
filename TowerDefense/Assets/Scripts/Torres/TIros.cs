using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TIros : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField] protected Rigidbody2D rb; // Torne a referência protegida para acesso em subclasses
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] protected int dano = 10; // Dano do projétil

    protected Transform target;
    private bool jaAplicouDano = false; // Garante que o dano seja aplicado apenas uma vez

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    protected virtual void FixedUpdate()
    {
        if (!target) return;
        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * bulletSpeed;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (jaAplicouDano) return;
        jaAplicouDano = true;

        IReceberDano inimigo = collision.GetComponent<IReceberDano>();
        if (inimigo != null)
        {
            inimigo.ReceberDano(dano); // Aplica dano ao inimigo
        }

        Destroy(gameObject); // Destrói o projétil após a colisão
    }

    public void SetDirection(Vector2 direction)
    {
        rb.velocity = direction * bulletSpeed;
    }

}
