using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InimigosMovimentacao : MonoBehaviour
{
    [Header("Atributos")]
    [SerializeField] protected float moveSpeed = 100f;

    [Header("Referências")]
    [SerializeField] protected Rigidbody2D rb;

    protected Transform alvo;
    protected int pathIndex = 0;
    [Header("Atributos")]
   
    [SerializeField] protected int vida = 100; // Adicionando vida ao inimigo

    // ... (restante do código)

    public void ReceberDano(int dano)
    {
        vida -= dano; // Reduz a vida do inimigo pelo dano recebido
        Debug.Log($"Inimigo recebeu {dano} de dano! Vida restante: {vida}");

        if (vida <= 0)
        {
            OnMorte();
        }
    }

    protected virtual void Start()
    {
        alvo = LevelManager.main.path[pathIndex];
    }

    protected virtual void Update()
    {
        if (Vector2.Distance(alvo.position, transform.position) <= 0.1f)
        {
            AtualizarDestino();
        }
    }

    private void FixedUpdate()
    {
        Mover();
    }

    public virtual void Mover()
    {
        Vector2 direcao = (alvo.position - transform.position).normalized;
        rb.velocity = direcao * moveSpeed;
    }

    protected  virtual void AtualizarDestino()
    {
        pathIndex++;
        if (pathIndex >= LevelManager.main.path.Length)
        {
            OnMorte();
        }
        else
        {
            alvo = LevelManager.main.path[pathIndex];
        }
    }

    public virtual void OnMorte()
    {
        EnemySpawner.onEnemyDestroy.Invoke();
        Destroy(gameObject);
    }
}


