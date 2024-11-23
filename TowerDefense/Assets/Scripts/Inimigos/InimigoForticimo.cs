using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InimigoForticimo : inimigoBase
{
    protected override void Start()
    {
        base.Start(); // Chama o método Start da classe base para inicializações padrão
        vidaAtual = 1; // Define a vida atual do inimigo como 1000, aumentando sua resistência
    }

    // Método que lida com o dano recebido pelo inimigo
    public override void ReceberDano(int dano)
    {
        base.ReceberDano(dano); // Chama o método da classe base para aplicar o dano
        // Aqui você pode adicionar lógica adicional, como efeitos visuais ou sons ao receber dano
    }

    // Método que controla o movimento do inimigo
    public override void Mover()
    {
        // Implementa um movimento específico para este inimigo, como um movimento lento
        base.Mover(); // Chama o movimento da classe base
        // Você pode adicionar lógica adicional para o movimento, como mudar a velocidade ou animações
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
}
