using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InimigosMovimentacao : MonoBehaviour
{
    [Header("Atributos")]
    [SerializeField] protected float moveSpeed = 100f; // Velocidade de movimento do inimigo
    [SerializeField] protected int vida = 100; // Vida inicial do inimigo

    [Header("Referências")]
    [SerializeField] protected Rigidbody2D rb; // Referência ao componente Rigidbody2D para controle de física

    protected Transform alvo; // Posição do alvo atual no caminho
    protected int pathIndex = 0; // Índice do ponto atual no caminho que o inimigo deve seguir

    // Reduz a vida do inimigo quando ele recebe dano e verifica se ele morreu
    public void ReceberDano(int dano)
    {
        vida -= dano; // Reduz a vida do inimigo pelo dano recebido
        Debug.Log($"Inimigo recebeu {dano} de dano! Vida restante: {vida}");

        // Se a vida for menor ou igual a zero, chama o método OnMorte
        if (vida <= 0)
        {
            OnMorte();
        }
    }

    // Configura o alvo inicial do inimigo ao iniciar o jogo
    protected virtual void Start()
    {
        alvo = LevelManager.main.path[pathIndex]; // Ponto inicial do caminho
    }

    // Verifica constantemente se o inimigo chegou ao alvo atual
    protected virtual void Update()
    {
        // Se o inimigo estiver próximo o suficiente do alvo, chama AtualizarDestino para definir o próximo alvo
        if (Vector2.Distance(alvo.position, transform.position) <= 0.1f)
        {
            AtualizarDestino();
        }
    }

    // Chamado em intervalos fixos para movimentar o inimigo em direção ao alvo
    private void FixedUpdate()
    {
        Mover(); // Realiza o movimento do inimigo
    }

    // Realiza o movimento do inimigo em direção ao alvo definido
    public virtual void Mover()
    {
        Vector2 direcao = (alvo.position - transform.position).normalized; // Calcula a direção para o alvo
        rb.velocity = direcao * moveSpeed; // Aplica a velocidade do movimento na direção do alvo
    }

    // Atualiza o próximo destino do inimigo no caminho
    protected virtual void AtualizarDestino()
    {
        pathIndex++; // Passa para o próximo ponto no caminho

        // Se o índice do caminho ultrapassa o tamanho do array de pontos, o inimigo alcançou o final e é destruído
        if (pathIndex >= LevelManager.main.path.Length)
        {
            OnMorte(); // Remove o inimigo do jogo
        }
        else
        {
            alvo = LevelManager.main.path[pathIndex]; // Define o novo alvo do inimigo
        }
    }

    // Método chamado quando o inimigo morre
    public virtual void OnMorte()
    {
        EnemySpawner.onEnemyDestroy.Invoke(); // Dispara o evento de destruição para atualizar o contador de inimigos no EnemySpawner
        Destroy(gameObject); // Destrói o objeto inimigo
    }
}
