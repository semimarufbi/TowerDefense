using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TIros : MonoBehaviour
{
    [Header("Refer�ncias")]
    [SerializeField] protected Rigidbody2D rb; // Torne a refer�ncia protegida para acesso em subclasses
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] protected int dano = 10; // Dano do proj�til

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

        Destroy(gameObject); // Destr�i o proj�til ap�s a colis�o
    }

    public void SetDirection(Vector2 direction)
    {
        rb.velocity = direction * bulletSpeed;
    }

}
