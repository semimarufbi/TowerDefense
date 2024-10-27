using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class InimigoVeloz : InimigosMovimentacao
{
    [Header("Atributos Específicos do Inimigo Veloz")]
    [SerializeField] private float aumentoDeVelocidade = 1.05f;

    protected override void Update()
    {
        base.Update();

        float distancia = Vector2.Distance(transform.position, alvo.position);
        

        // Verifica se está próximo o suficiente do alvo antes de atualizar o próximo ponto
        if (distancia <= 0.2f)
        {
           
            AtualizarDestino();
        }
    }

    protected override void AtualizarDestino()
    {
        pathIndex++;
        ;

        // Se o inimigo alcança o final do caminho
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
}
