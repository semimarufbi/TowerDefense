using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Classe base para os inimigos que implementa a interface IReceberDano
public class inimigoBase : MonoBehaviour, IReceberDano
{
    [Header("Atributos")]
    [SerializeField] public float moveSpeed = 100f; // Velocidade de movimento do inimigo
    [SerializeField] private int currentWorth = 50; // Valor que o inimigo d� ao ser derrotado

    [Header("Refer�ncias")]
    [SerializeField] protected Rigidbody2D rb; // Refer�ncia ao componente Rigidbody2D para controle de f�sica

    protected Transform alvo; // Posi��o do alvo atual no caminho
    protected int pathIndex = 0; // �ndice do ponto atual no caminho

    [SerializeField] public int vidaAtual = 100; // Vida inicial do inimigo

    // M�todo da interface que reduz a vida do inimigo e verifica se ele morreu
    public virtual void ReceberDano(int dano)
    {
        vidaAtual -= dano; // Reduz a vida do inimigo pelo dano recebido

        if (vidaAtual <= 0) // Verifica se o inimigo morreu
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
        // Verifica a dist�ncia at� o alvo e atualiza o destino se pr�ximo o suficiente
        if (Vector2.Distance(alvo.position, transform.position) <= 0.1f)
        {
            AtualizarDestino();
        }
    }

    private void FixedUpdate()
    {
        Mover(); // Chama o m�todo de movimento a cada atualiza��o de f�sica
    }

    // M�todo respons�vel pelo movimento do inimigo
    public virtual void Mover()
    {
        // Calcula a dire��o para o alvo e aplica a velocidade
        Vector2 direcao = (alvo.position - transform.position).normalized;
        rb.velocity = direcao * moveSpeed; // Aplica a velocidade ao Rigidbody2D
    }

    // Atualiza o destino do inimigo para o pr�ximo ponto no caminho
    protected virtual void AtualizarDestino()
    {
        pathIndex++; // Incrementa o �ndice do caminho
        if (pathIndex >= LevelManager.main.path.Length) // Verifica se chegou ao final do caminho
        {
            OnMorte(); // Chama o m�todo de morte se alcan�ou o final
        }
        else
        {
            alvo = LevelManager.main.path[pathIndex]; // Atualiza o alvo para o pr�ximo ponto
        }
    }

    // M�todo chamado quando o inimigo morre
    public virtual void OnMorte()
    {
        EnemySpawner.onEnemyDestroy.Invoke(); // Invoca evento de destrui��o do inimigo
        Destroy(gameObject); // Destroi o objeto inimigo
        LevelManager.main.IncreaseCurerency(currentWorth); // Aumenta a moeda do jogador
    }
}
