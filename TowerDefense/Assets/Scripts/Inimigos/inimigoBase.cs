using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class inimigoBase : MonoBehaviour, IReceberDano
{
    [SerializeField] public Button botao;
    [Header("Atributos")]
    [SerializeField] public float moveSpeed = 100f; // Velocidade de movimento do inimigo
    [SerializeField] private int currentWorth = 50; // Valor que o inimigo dá ao ser derrotado
    public GameObject painel;

    [Header("Referências")]
    [SerializeField] protected Rigidbody2D rb; // Referência ao componente Rigidbody2D para controle de física

    protected Transform alvo; // Posição do alvo atual no caminho
    protected int pathIndex = 0; // Índice do ponto atual no caminho

    [SerializeField] public int vidaAtual = 100; // Vida inicial do inimigo

    // Método da interface que reduz a vida do inimigo e verifica se ele morreu
    public virtual void ReceberDano(int dano)
    {
        vidaAtual -= dano; // Reduz a vida do inimigo pelo dano recebido

        if (vidaAtual <= 0) // Verifica se o inimigo morreu
        {
            OnMorte();
            painel.SetActive(true); // Ativa o painel
        }
    }

    protected virtual void Start()
    {
        alvo = LevelManager.main.path[pathIndex]; // Ponto inicial do caminho
    }

    protected virtual void Update()
    {
        // Verifica a distância até o alvo e atualiza o destino se próximo o suficiente
        if (Vector2.Distance(alvo.position, transform.position) <= 0.1f)
        {
            AtualizarDestino();
        }
    }

    private void FixedUpdate()
    {
        Mover(); // Chama o método de movimento a cada atualização de física
    }

    // Método responsável pelo movimento do inimigo
    public virtual void Mover()
    {
        // Calcula a direção para o alvo e aplica a velocidade
        Vector2 direcao = (alvo.position - transform.position).normalized;
        rb.velocity = direcao * moveSpeed; // Aplica a velocidade ao Rigidbody2D
    }

    // Atualiza o destino do inimigo para o próximo ponto no caminho
    protected virtual void AtualizarDestino()
    {
        pathIndex++; // Incrementa o índice do caminho
        if (pathIndex >= LevelManager.main.path.Length) // Verifica se chegou ao final do caminho
        {
            OnMorte(); // Chama o método de morte se alcançou o final
            GameOver(); // Chama o método Game Over
        }
        else
        {
            alvo = LevelManager.main.path[pathIndex]; // Atualiza o alvo para o próximo ponto
        }
    }

    // Método chamado quando o inimigo morre
    public virtual void OnMorte()
    {
        EnemySpawner.onEnemyDestroy.Invoke(); // Invoca evento de destruição do inimigo
        Destroy(gameObject); // Destroi o objeto inimigo
        LevelManager.main.IncreaseCurrency(currentWorth); // Aumenta a moeda do jogador
    }

    // Método para lidar com o Game Over
    public virtual void GameOver()
    {
        // Pausa o jogo (Time.timeScale = 0)
        Time.timeScale = 0;

        // Exibe o painel de Game Over
        if (painel != null)
        {
            painel.SetActive(true); // Mostra o painel de Game Over
        }

        // Aqui, você pode adicionar mais ações, como tocar uma música ou animação de Game Over

        // Opcional: Se você deseja reiniciar o jogo ou voltar para a tela inicial, pode adicionar um botão no painel
        // que chama um método para reiniciar o nível ou voltar para o menu principal.
        // Por exemplo, se o botão "Restart" no painel for pressionado:
        // RestartGame();
    }

    // Método para reiniciar o jogo (apenas exemplo)
    public void RestartGame()
    {
        Time.timeScale = 1; // Retorna o tempo para a velocidade normal
        // Aqui você pode adicionar lógica para reiniciar o nível, como recarregar a cena atual
        // Exemplo:
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
