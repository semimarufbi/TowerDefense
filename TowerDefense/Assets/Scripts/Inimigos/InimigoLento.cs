using UnityEngine;

public class InimigoLento : inimigoBase
{
    [Header("Atributos Específicos do Inimigo Lento")]
    [SerializeField] private float aumentoDeVelocidade = 1.05f; // Fator de aumento de velocidade a cada ponto alcançado

    protected override void Start()
    {
        base.Start(); // Chama o método Start da classe base para inicializar os atributos comuns
        vidaAtual = 150; // Define a vida inicial específica para este inimigo
    }

    protected override void Update()
    {
        base.Update(); // Chama o método Update da classe base para manter comportamentos padrão

        float distancia = Vector2.Distance(transform.position, alvo.position); // Calcula a distância até o alvo

        // Verifica se está próximo o suficiente do alvo antes de atualizar o próximo ponto
        if (distancia <= 0.2f)
        {
            AtualizarDestino(); // Atualiza o destino se o inimigo estiver próximo do alvo
        }
    }

    protected override void AtualizarDestino()
    {
        pathIndex++; // Move para o próximo ponto do caminho

        // Se o inimigo alcança o final do caminho
        if (pathIndex >= LevelManager.main.path.Length)
        {
            LevelManager.main.GameOver();
            OnMorte(); // Chama o método para lidar com a morte do inimigo
        }
        else
        {
            alvo = LevelManager.main.path[pathIndex]; // Atualiza o alvo para o próximo ponto do caminho
            moveSpeed *= aumentoDeVelocidade; // Aumenta a velocidade do inimigo a cada ponto alcançado
        }
    }

    public override void Mover()
    {
        Vector2 direcao = (alvo.position - transform.position).normalized; // Calcula a direção do movimento
        rb.velocity = direcao * moveSpeed; // Atualiza a velocidade do rigidbody do inimigo
    }

    public override void ReceberDano(int dano)
    {
        base.ReceberDano(dano); // Chama o método da classe base para aplicar o dano
        // Aqui você pode adicionar lógica adicional ao receber dano, se necessário
    }
}
