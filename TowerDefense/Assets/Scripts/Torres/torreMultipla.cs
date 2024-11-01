using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorreMultipla : Torre
{
    [Header("Atributos da Torre M�ltipla")]
    [SerializeField] private int numeroDeBalas = 3; // N�mero de proj�teis por disparo
    [SerializeField] private float anguloEntreBalas = 15f; // �ngulo entre cada bala

    // M�todo sobrescrito para disparar m�ltiplas balas
    protected override void Shoot()
    {
        if (target == null) return; // Se n�o h� alvo, n�o faz nada

        // �ngulo inicial baseado na quantidade de balas e espa�amento entre elas
        float anguloInicial = -anguloEntreBalas * (numeroDeBalas - 1) / 2;

        // Disparar cada bala com um �ngulo espa�ado
        for (int i = 0; i < numeroDeBalas; i++)
        {
            // Calcula o �ngulo atual para a bala
            float anguloAtual = anguloInicial + i * anguloEntreBalas;

            // Calcula a dire��o da bala com base no �ngulo atual
            Vector3 direcaoDaBala = Quaternion.Euler(0, 0, anguloAtual) * (target.position - firingPoint.position).normalized;

            // Instancia e ajusta a dire��o da bala
            GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
            TIros bulletScript = bulletObj.GetComponent<TIros>();
            bulletScript.SetDirection(direcaoDaBala); // Define a dire��o da bala
        }
    }
}
