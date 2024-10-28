using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Torre : MonoBehaviour
{
    [SerializeField] private float alvoNoRaio = 5f;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] private Transform pontoDeTiro;
    [SerializeField] private float bps = 1f;//balas por segundo
    [SerializeField] private LayerMask inimigoMask;
    private Transform alvo;
    private float tempoEntreBalas;


    private void Update()
    {
        if(alvo == null)
        {
            FindTarget();
            return;
        }
        ChecarAlvoEstaNoAlcance();


    }   

    private void FindTarget()
    {
    
         RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, alvoNoRaio, (Vector2)transform.position, 0f, inimigoMask);

        if(hits.Length > 0 )
        {
            alvo = hits[0].transform;
        }
    }

    private bool ChecarAlvoEstaNoAlcance()
    {
        return Vector2.Distance(alvo.position, transform.position) <= alvoNoRaio;
    }

}
