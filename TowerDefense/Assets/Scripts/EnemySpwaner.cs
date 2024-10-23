using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpwaner : MonoBehaviour
{
[Header("atributos")]
[SerializeField]private int inimigoBase = 8;
[SerializeField] private float inimigosPorSeg = 0.5f;
    [SerializeField] private float tempoEntreOrdas;


    [Header("Referencias")]
    [SerializeField] private GameObject[] prefabInimigo;

    private int ordaAtual = 1;
    private float tempodepoisdospawn;
    private int inimigoVivo;
    private int inimigosSaiuDoSpawn;


    // Start is called before the first frame update
    void Start()
    {
        inimigosSaiuDoSpawn = inimigoBase;
    }

    // Update is called once per frame
   private void InimigoPorOrda()
    {

    }
}
