using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TIrosLento : TIros
{
    [Header("Atributos do Proj�til Lento")]
    [SerializeField] private float duracaoLentidao = 3f; // Dura��o do efeito de lentid�o
    [SerializeField] private float fatorLentidao = 0.5f; // Fator de redu��o de velocidade (ex.: 0.5 � 50% da velocidade)

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision); // Chama o m�todo base para aplicar dano

        inimigoBase inimigo = collision.GetComponent<inimigoBase>();
        if (inimigo != null)
        {
            StartCoroutine(AplicarLentidao(inimigo)); // Aplica o efeito de lentid�o
        }
    }

    private IEnumerator AplicarLentidao(inimigoBase inimigo)
    {
        // Assumindo que o inimigo tem uma propriedade moveSpeed
        float velocidadeOriginal = inimigo.moveSpeed; // Supondo que o inimigo tem a propriedade moveSpeed
        inimigo.moveSpeed *= fatorLentidao; // Reduz a velocidade do inimigo

        yield return new WaitForSeconds(duracaoLentidao);

        inimigo.moveSpeed = velocidadeOriginal; // Restaura a velocidade original ap�s a dura��o do efeito
    }
}
