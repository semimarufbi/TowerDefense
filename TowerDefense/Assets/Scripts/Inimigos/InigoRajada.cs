using UnityEngine;

public class InimigoRajada : inimigoBase
{
    [Header("Atributos Espec�ficos de Rajada")]
    [SerializeField] private float duracaoRajada = 1f; // Dura��o da rajada em segundos
    [SerializeField] private float intervaloRajada = 2f; // Intervalo entre as rajadas

    private float tempoRajadaAtual = 0f;
    private bool emRajada = true;

    // Atributo de vida espec�fico para este inimigo
    private int vidaAtual = 20;

    protected override void Update()
    {
        base.Update(); // Chama o m�todo Update da classe base

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
        // Movimenta apenas se estiver em rajada, sen�o mant�m a velocidade zero
        if (emRajada)
        {
            Vector2 direcao = (alvo.position - transform.position).normalized;
            rb.velocity = direcao * moveSpeed;
        }
        else
        {
            rb.velocity = Vector2.zero; // Pausa o movimento quando n�o est� em rajada
        }
    }

    protected override void AtualizarDestino()
    {
        pathIndex++;

        // Verifica se ainda h� mais pontos no caminho
        if (pathIndex >= LevelManager.main.path.Length)
        {
            OnMorte(); // Chama a destrui��o do inimigo se chegou ao fim do caminho
        }
        else
        {
            alvo = LevelManager.main.path[pathIndex];
        }
    }

    public override void ReceberDano(int dano)
    {
        vidaAtual -= dano;
        if (vidaAtual <= 0)
        {
            OnMorte();
        }
    }
}
