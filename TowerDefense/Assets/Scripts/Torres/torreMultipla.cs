using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorreMultipla : Torre
{
    [Header("Atributos da Torre Múltipla")]
    [SerializeField] private int numeroDeBalas = 3;   // Número de projéteis por disparo
    [SerializeField] private float anguloEntreBalas = 15f; // Ângulo entre cada bala

    protected override void Shoot()
    {
        if (target == null) return;

        // Ângulo inicial baseado na quantidade de balas e espaçamento entre elas
        float anguloInicial = -anguloEntreBalas * (numeroDeBalas - 1) / 2;

        // Disparar cada bala com um ângulo espaçado
        for (int i = 0; i < numeroDeBalas; i++)
        {
            float anguloAtual = anguloInicial + i * anguloEntreBalas;
            Vector3 direcaoDaBala = Quaternion.Euler(0, 0, anguloAtual) * (target.position - firingPoint.position).normalized;

            // Instancia e ajusta a direção da bala
            GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
            TIros bulletScript = bulletObj.GetComponent<TIros>();
            bulletScript.SetDirection(direcaoDaBala);
        }
    }
}
