using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inimigofraco : inimigoBase
{



    protected override void Start()
    {
        base.Start(); // Chama o método Start da classe base
        vidaAtual = 10; // Aumenta a vida baseada na resistência
    }

    public override void ReceberDano(int dano)
    {
        base.ReceberDano(dano); // Chama o método da classe base
    }

    public override void Mover()
    {
        // Movimento específico, por exemplo, movimento lento
        base.Mover(); // Chama o movimento da classe base
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
