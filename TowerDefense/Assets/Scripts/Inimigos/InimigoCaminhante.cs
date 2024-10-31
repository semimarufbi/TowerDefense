using UnityEngine;

public class InimigoCaminhante : inimigoBase
{
    [Header("Atributos Espec�ficos do Inimigo Caminhante")]
    [SerializeField] private float resistencia = 0.5f; // Resist�ncia ao dano

    protected override void Start()
    {
        base.Start(); // Chama o m�todo Start da classe base
        vidaAtual = Mathf.RoundToInt(vidaAtual * (1 + resistencia)); // Aumenta a vida baseada na resist�ncia
    }

    public override void ReceberDano(int dano)
    {
        // Aplica resist�ncia ao dano recebido
        int danoFinal = Mathf.RoundToInt(dano * (1 - resistencia));
        base.ReceberDano(danoFinal); // Chama a implementa��o da classe base com o dano ajustado
    }

    public override void Mover()
    {
        // Movimento espec�fico, por exemplo, movimento lento
        base.Mover(); // Chama o movimento da classe base
    }
}
