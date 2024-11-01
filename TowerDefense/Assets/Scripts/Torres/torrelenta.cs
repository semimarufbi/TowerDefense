using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class torrelenta : Torre
{
    [Header("Atributos da Torre Lenta")]
    [SerializeField] private GameObject bulletSlowPrefab; // Prefab do proj�til que aplica lentid�o

    protected override void Shoot()
    {
        // Instancia o proj�til de lentid�o na posi��o do ponto de disparo
        GameObject bulletObj = Instantiate(bulletSlowPrefab, firingPoint.position, Quaternion.identity);
        TIrosLento bulletScript = bulletObj.GetComponent<TIrosLento>();

        // Define o alvo do proj�til
        bulletScript.SetTarget(target);
    }
}
