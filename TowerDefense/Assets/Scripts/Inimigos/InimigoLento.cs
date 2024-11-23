using UnityEngine;

public class InimigoLento : inimigoBase
{
    [Header("Atributos Espec�ficos do Inimigo Lento")]
    [SerializeField] private float aumentoDeVelocidade = 1.05f; // Fator de aumento de velocidade a cada ponto alcan�ado

    protected override void Start()
    {
        base.Start(); // Chama o m�todo Start da classe base para inicializar os atributos comuns
        vidaAtual = 150; // Define a vida inicial espec�fica para este inimigo
    }

    protected override void Update()
    {
        base.Update(); // Chama o m�todo Update da classe base para manter comportamentos padr�o

        float distancia = Vector2.Distance(transform.position, alvo.position); // Calcula a dist�ncia at� o alvo

        // Verifica se est� pr�ximo o suficiente do alvo antes de atualizar o pr�ximo ponto
        if (distancia <= 0.2f)
        {
            AtualizarDestino(); // Atualiza o destino se o inimigo estiver pr�ximo do alvo
        }
    }

    protected override void AtualizarDestino()
    {
        pathIndex++; // Move para o pr�ximo ponto do caminho

        // Se o inimigo alcan�a o final do caminho
        if (pathIndex >= LevelManager.main.path.Length)
        {
            LevelManager.main.GameOver();
            OnMorte(); // Chama o m�todo para lidar com a morte do inimigo
        }
        else
        {
            alvo = LevelManager.main.path[pathIndex]; // Atualiza o alvo para o pr�ximo ponto do caminho
            moveSpeed *= aumentoDeVelocidade; // Aumenta a velocidade do inimigo a cada ponto alcan�ado
        }
    }

    public override void Mover()
    {
        Vector2 direcao = (alvo.position - transform.position).normalized; // Calcula a dire��o do movimento
        rb.velocity = direcao * moveSpeed; // Atualiza a velocidade do rigidbody do inimigo
    }

    public override void ReceberDano(int dano)
    {
        base.ReceberDano(dano); // Chama o m�todo da classe base para aplicar o dano
        // Aqui voc� pode adicionar l�gica adicional ao receber dano, se necess�rio
    }
}
