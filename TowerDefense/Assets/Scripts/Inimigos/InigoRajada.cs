using UnityEngine;

public class InimigoRajada : inimigoBase
{
    [Header("Atributos Específicos de Rajada")]
    [SerializeField] private float duracaoRajada = 1f; // Duração da rajada em segundos
    [SerializeField] private float intervaloRajada = 2f; // Intervalo entre as rajadas

    private float tempoRajadaAtual = 0f;
    private bool emRajada = true;

    // Atributo de vida específico para este inimigo
    protected override void Start()
    {
        base.Start();
        vidaAtual = 70; // Defina a vida inicial específica para este inimigo
    }

    protected override void Update()
    {
        base.Update(); // Chama o método Update da classe base

        // Alterna entre modo de rajada e pausa
        tempoRajadaAtual += Time.deltaTime;
        if (emRajada && tempoRajadaAtual >= duracaoRajada)
        {
            emRajada = false;
            tempoRajadaAtual = 0f;
        }
        else if (!emRajada && tempoRajadaAtual >= intervaloRajada)
        {
            emRajada = true;
            tempoRajadaAtual = 0f;
        }
    }

    public override void Mover()
    {
        // Movimenta apenas se estiver em rajada, senão mantém a velocidade zero
        if (emRajada)
        {
            Vector2 direcao = (alvo.position - transform.position).normalized;
            rb.velocity = direcao * moveSpeed;
        }
        else
        {
            rb.velocity = Vector2.zero; // Pausa o movimento quando não está em rajada
        }
    }

    protected override void AtualizarDestino()
    {
        pathIndex++;

        // Verifica se ainda há mais pontos no caminho
        if (pathIndex >= LevelManager.main.path.Length)
        {
            LevelManager.main.GameOver();
            OnMorte(); // Chama a destruição do inimigo se chegou ao fim do caminho
        }
        else
        {
            alvo = LevelManager.main.path[pathIndex];
        }
    }

    public override void ReceberDano(int dano)
    {
        base.ReceberDano(dano); // Chama o método da classe base
    }
}
