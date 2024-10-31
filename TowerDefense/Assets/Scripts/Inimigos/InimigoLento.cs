using UnityEngine;

public class InimigoLento : inimigoBase
{
    [Header("Atributos Espec�ficos do Inimigo Lento")]
    [SerializeField] private float aumentoDeVelocidade = 1.05f;

    protected override void Start()
    {
        base.Start();
        vidaAtual = 150; // Defina a vida inicial espec�fica para este inimigo
    }

    protected override void Update()
    {
        base.Update();

        float distancia = Vector2.Distance(transform.position, alvo.position);

        // Verifica se est� pr�ximo o suficiente do alvo antes de atualizar o pr�ximo ponto
        if (distancia <= 0.2f)
        {
            AtualizarDestino();
        }
    }

    protected override void AtualizarDestino()
    {
        pathIndex++;

        // Se o inimigo alcan�a o final do caminho
        if (pathIndex >= LevelManager.main.path.Length)
        {
            OnMorte();
        }
        else
        {
            alvo = LevelManager.main.path[pathIndex];
            moveSpeed *= aumentoDeVelocidade;  // Aumenta a velocidade
        }
    }

    public override void Mover()
    {
        Vector2 direcao = (alvo.position - transform.position).normalized;
        rb.velocity = direcao * moveSpeed;
    }

    public override void ReceberDano(int dano)
    {
        base.ReceberDano(dano); // Chama o m�todo da classe base
    }
}
