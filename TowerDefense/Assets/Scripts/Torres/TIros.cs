using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TIros : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField] protected Rigidbody2D rb; // Referência ao componente Rigidbody2D do projétil, protegida para acesso em subclasses
    [SerializeField] private float bulletSpeed = 5f; // Velocidade do projétil
    [SerializeField] protected int dano = 10; // Dano que o projétil causará ao atingir um inimigo

    protected Transform target; // Referência ao alvo que o projétil está perseguindo
    private bool jaAplicouDano = false; // Garante que o dano seja aplicado apenas uma vez ao atingir o inimigo

    // Método para definir o alvo que o projétil deve seguir
    public void SetTarget(Transform _target)
    {
        target = _target; // Atribui o alvo ao campo 'target'
    }

    // Atualiza a física do projétil
    protected virtual void FixedUpdate()
    {
        if (!target) return; // Se não houver alvo, sai do método

        // Calcula a direção para o alvo e normaliza o vetor
        Vector2 direction = (target.position - transform.position).normalized;

        // Define a velocidade do Rigidbody do projétil na direção do alvo
        rb.velocity = direction * bulletSpeed;
    }

    // Método chamado quando o projétil colide com outro objeto
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (jaAplicouDano) return; // Se já aplicou dano, sai do método
        jaAplicouDano = true; // Marca que o dano foi aplicado

        // Tenta obter o componente IReceberDano do objeto colidido
        IReceberDano inimigo = collision.GetComponent<IReceberDano>();
        if (inimigo != null)
        {
            // Se o objeto tem um componente IReceberDano, aplica dano
            inimigo.ReceberDano(dano); // Aplica dano ao inimigo
        }

        // Destrói o projétil após a colisão
        Destroy(gameObject);
    }

    // Define a direção do projétil manualmente
    public void SetDirection(Vector2 direction)
    {
        rb.velocity = direction * bulletSpeed; // Define a velocidade do Rigidbody na direção especificada
    }
}
