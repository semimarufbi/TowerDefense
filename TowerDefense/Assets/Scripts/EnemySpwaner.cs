using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpwaner : MonoBehaviour
{
[Header("atributos")]
[SerializeField]private int inimigoBase = 8;
[SerializeField] private float inimigosPorSeg = 0.5f;
    [SerializeField] private float tempoEntreOrdas = 5f;
    [SerializeField] private float dificuldade = 0.75f;


    [Header("Referencias")]
    [SerializeField] private GameObject[] prefabInimigo;

    private int ordaAtual = 1;
    private float tempodepoisdospawn;
    private int inimigoVivo;
    private int inimigosSaiuDoSpawn;
    private bool estaSpawnando = false;


    private void Start()
    {
        StartWave();
    }
    private void Update()
    { if (!estaSpawnando) return;
        tempodepoisdospawn += Time.deltaTime;
        if (tempodepoisdospawn >= (1f / inimigosPorSeg)&& inimigosSaiuDoSpawn>0 )
        {
            spawnarinimigo();
            inimigosSaiuDoSpawn--;
            inimigoVivo++;
            tempodepoisdospawn = 0f;
            
        }
    }
    // Start is called before the first frame update
    void StartWave()
    {
      estaSpawnando = true;
        inimigosSaiuDoSpawn = InimigoPorOrda();
    }

    // Update is called once per frame
   private int InimigoPorOrda()
    {
        return Mathf.RoundToInt(inimigoBase * Mathf.Pow(ordaAtual,dificuldade));
    }

    private void spawnarinimigo()
    {
       GameObject prefabparaspawnar = prefabInimigo[0];
        Instantiate(prefabparaspawnar, LevelManager.main.startPoint.position, Quaternion.identity);

    }
}
