using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inimigoBase : MonoBehaviour, IReceberDano
{
    [Header("Atributos")]
    [SerializeField] public float moveSpeed = 100f; // Velocidade de movimento do inimigo
    [SerializeField] private int currentWorth = 50;

    [Header("Referências")]
    [SerializeField] protected Rigidbody2D rb; // Referência ao componente Rigidbody2D para controle de física

    protected Transform alvo; // Posição do alvo atual no caminho
    protected int pathIndex = 0; // Índice do ponto atual no caminho

    [SerializeField] public int vidaAtual = 100; // Vida inicial do inimigo, usada somente no método `ReceberDano`

    // Método de interface que reduz a vida e verifica se o inimigo morreu
    public virtual void ReceberDano(int dano)
    {
        vidaAtual -= dano;
       
        if (vidaAtual <= 0)
        {
            OnMorte();
        }
    }

    protected virtual void Start()
    {
        alvo = LevelManager.main.path[pathIndex]; // Ponto inicial do caminho
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

    protected virtual void AtualizarDestino()
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
        LevelManager.main.IncreaseCurerency(currentWorth);
    }
}
