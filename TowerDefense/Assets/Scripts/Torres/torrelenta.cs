using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class torrelenta : Torre
{
    [Header("Atributos da Torre Lenta")]
    [SerializeField] private GameObject bulletSlowPrefab; // Prefab do projétil que aplica lentidão

    protected override void Shoot()
    {
        // Instancia o projétil de lentidão na posição do ponto de disparo
        GameObject bulletObj = Instantiate(bulletSlowPrefab, firingPoint.position, Quaternion.identity);
        TIrosLento bulletScript = bulletObj.GetComponent<TIrosLento>();

        // Define o alvo do projétil
        bulletScript.SetTarget(target);
    }
}
