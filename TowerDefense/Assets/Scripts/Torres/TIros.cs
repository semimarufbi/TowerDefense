using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TIros : MonoBehaviour
{
    [Header("Refer�ncias")]
    [SerializeField] protected Rigidbody2D rb; // Refer�ncia ao componente Rigidbody2D do proj�til, protegida para acesso em subclasses
    [SerializeField] private float bulletSpeed = 5f; // Velocidade do proj�til
    [SerializeField] protected int dano = 10; // Dano que o proj�til causar� ao atingir um inimigo

    protected Transform target; // Refer�ncia ao alvo que o proj�til est� perseguindo
    private bool jaAplicouDano = false; // Garante que o dano seja aplicado apenas uma vez ao atingir o inimigo

    // M�todo para definir o alvo que o proj�til deve seguir
    public void SetTarget(Transform _target)
    {
        target = _target; // Atribui o alvo ao campo 'target'
    }

    // Atualiza a f�sica do proj�til
    protected virtual void FixedUpdate()
    {
        if (!target) return; // Se n�o houver alvo, sai do m�todo

        // Calcula a dire��o para o alvo e normaliza o vetor
        Vector2 direction = (target.position - transform.position).normalized;

        // Define a velocidade do Rigidbody do proj�til na dire��o do alvo
        rb.velocity = direction * bulletSpeed;
    }

    // M�todo chamado quando o proj�til colide com outro objeto
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (jaAplicouDano) return; // Se j� aplicou dano, sai do m�todo
        jaAplicouDano = true; // Marca que o dano foi aplicado

        // Tenta obter o componente IReceberDano do objeto colidido
        IReceberDano inimigo = collision.GetComponent<IReceberDano>();
        if (inimigo != null)
        {
            // Se o objeto tem um componente IReceberDano, aplica dano
            inimigo.ReceberDano(dano); // Aplica dano ao inimigo
        }

        // Destr�i o proj�til ap�s a colis�o
        Destroy(gameObject);
    }

    // Define a dire��o do proj�til manualmente
    public void SetDirection(Vector2 direction)
    {
        rb.velocity = direction * bulletSpeed; // Define a velocidade do Rigidbody na dire��o especificada
    }
}
