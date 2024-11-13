using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InimigoForticimo : inimigoBase
{
    protected override void Start()
    {
        base.Start(); // Chama o m�todo Start da classe base para inicializa��es padr�o
        vidaAtual = 1; // Define a vida atual do inimigo como 1000, aumentando sua resist�ncia
    }

    // M�todo que lida com o dano recebido pelo inimigo
    public override void ReceberDano(int dano)
    {
        base.ReceberDano(dano); // Chama o m�todo da classe base para aplicar o dano
        // Aqui voc� pode adicionar l�gica adicional, como efeitos visuais ou sons ao receber dano
    }

    // M�todo que controla o movimento do inimigo
    public override void Mover()
    {
        // Implementa um movimento espec�fico para este inimigo, como um movimento lento
        base.Mover(); // Chama o movimento da classe base
        // Voc� pode adicionar l�gica adicional para o movimento, como mudar a velocidade ou anima��es
    }
}
