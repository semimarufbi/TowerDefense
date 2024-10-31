using UnityEngine;

public class InimigoCaminhante : inimigoBase
{
    [Header("Atributos Específicos do Inimigo Caminhante")]
    [SerializeField] private float resistencia = 0.5f; // Resistência ao dano

    protected override void Start()
    {
        base.Start(); // Chama o método Start da classe base
        vidaAtual = Mathf.RoundToInt(vidaAtual * (1 + resistencia)); // Aumenta a vida baseada na resistência
    }

    public override void ReceberDano(int dano)
    {
        // Aplica resistência ao dano recebido
        int danoFinal = Mathf.RoundToInt(dano * (1 - resistencia));
        base.ReceberDano(danoFinal); // Chama a implementação da classe base com o dano ajustado
    }

    public override void Mover()
    {
        // Movimento específico, por exemplo, movimento lento
        base.Mover(); // Chama o movimento da classe base
    }
}
