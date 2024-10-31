using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TIros : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private int dano = 10; // Dano que o proj�til causa ao inimigo

    private Transform target;
    private bool jaAplicouDano = false; // Garante que o dano seja aplicado apenas uma vez

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    private void FixedUpdate()
    {
        if (!target) return;
        Vector2 direction = (target.position - transform.position).normalized;

        rb.velocity = direction * bulletSpeed;
    }

    public void SetDirection(Vector2 direction)
    {
        rb.velocity = direction * bulletSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (jaAplicouDano) return; // Sai da fun��o se o dano j� foi aplicado
        jaAplicouDano = true;      // Marca o dano como j� aplicado

        IReceberDano inimigo = collision.GetComponent<IReceberDano>();

        if (inimigo != null)
        {
            Debug.Log($"Aplicando {dano} de dano ao inimigo.");
            inimigo.ReceberDano(dano); // Aplica dano
        }

        Destroy(gameObject); // Destr�i o proj�til ap�s a colis�o
    }

}
