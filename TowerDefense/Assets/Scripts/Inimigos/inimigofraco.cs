using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inimigofraco : inimigoBase 
{ 
  
    

    protected override void Start()
    {
        base.Start(); // Chama o m�todo Start da classe base
        vidaAtual = 10; // Aumenta a vida baseada na resist�ncia
    }

    public override void ReceberDano(int dano)
    {
        base.ReceberDano(dano); // Chama o m�todo da classe base
    }

    public override void Mover()
    {
        // Movimento espec�fico, por exemplo, movimento lento
        base.Mover(); // Chama o movimento da classe base
    }
}
