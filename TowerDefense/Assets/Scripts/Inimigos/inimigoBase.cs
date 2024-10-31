using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inimigoBase : MonoBehaviour, IReceberDano
{
    [Header("Atributos")]
    [SerializeField] protected float moveSpeed = 100f; // Velocidade de movimento do inimigo
    [SerializeField] private int currentWorth = 50; // Quantidade de moeda que este inimigo vale

    [Header("Referências")]
    [SerializeField] protected Rigidbody2D rb; // Referência ao componente Rigidbody2D para controle de física

    protected Transform alvo; // Posição do alvo atual no caminho
    protected int pathIndex = 0; // Índice do ponto atual no caminho

    [SerializeField] public int vidaAtual = 100; // Vida inicial do inimigo

    // Método de interface que reduz a vida e verifica se o inimigo morreu
    public virtual void ReceberDano(int dano)
    {
        vidaAtual -= dano; // Reduz a vida do inimigo pelo dano recebido

        // Verifica se a vida do inimigo chegou a zero ou menos
        if (vidaAtual <= 0)
        {
            OnMorte(); // Chama o método de morte se a vida for igual ou menor que zero
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
            OnMorte(); // Chama o método de morte se o inimigo atingir o final do caminho
        }
        else
        {
            alvo = LevelManager.main.path[pathIndex]; // Atualiza o alvo para o próximo ponto
        }
    }

    public virtual void OnMorte()
    {
        LevelManager.main.IncreaseCurerency(currentWorth); // Aumenta a moeda ao morrer
        EnemySpawner.onEnemyDestroy.Invoke(); // Invoca o evento de inimigo destruído
        Destroy(gameObject); // Destrói o objeto inimigo
    }
}
