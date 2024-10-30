using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Torre : MonoBehaviour
{
    [SerializeField] private float targetInRange = 5f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform pontoDeTiro;
    [SerializeField] private float bps = 1f; // balas por segundo
    [SerializeField] private LayerMask inimigoMask;
    [SerializeField] private Transform turrentRotationPoint;
    private Transform alvo;
    private float tempoEntreBalas; // Controla o tempo para o próximo disparo
    [SerializeField] private float rotateSpeed = 5f;
     

    private void Update()
    {
        if (alvo == null)
        {
            FindTarget();
            return;
        }
        Rotacaodiretoalvo();
        if (!ChecarAlvoEstaNoAlcance())
        {
            alvo = null;
        }
        else
        {
            tempoEntreBalas += Time.deltaTime;
            if(tempoEntreBalas >= 1f / bps)
            {
                Shoot();
                tempoEntreBalas = 0f;
            }
        }
    }
    private void Shoot()
    {
        GameObject bulletobj = Instantiate(bulletPrefab,pontoDeTiro.position,Quaternion.identity) ;
        bullet bulletScript = bulletobj.GetComponent<Bullet>
    }

    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetInRange, Vector2.zero, 0f, inimigoMask);

        if (hits.Length > 0)
        {
            alvo = hits[0].transform;
        }
    }

    private void Rotacaodiretoalvo()
    {
        if (alvo == null) return;

        // Calcula o ângulo para o alvo e rotaciona a torre continuamente para segui-lo
        float angle = Mathf.Atan2(alvo.position.y - transform.position.y, alvo.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;
        Quaternion rotacaoalvo = Quaternion.Euler(0f, 0f, angle);
        turrentRotationPoint.rotation = Quaternion.RotateTowards(turrentRotationPoint.rotation, rotacaoalvo, rotateSpeed * Time.deltaTime);
    }

    private bool ChecarAlvoEstaNoAlcance()
    {
        return alvo != null && Vector2.Distance(alvo.position, transform.position) <= targetInRange;
    }

  

    private void OnDrawGizmos()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, Vector3.forward, targetInRange);
    }
}
