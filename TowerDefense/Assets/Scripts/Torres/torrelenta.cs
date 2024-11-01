using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class torrelenta : Torre
{
    [Header("Atributos da Torre Lenta")]
    [SerializeField] private GameObject bulletSlowPrefab; // Prefab do projétil que aplica lentidão

    protected override void Shoot()
    {
        GameObject bulletObj = Instantiate(bulletSlowPrefab, firingPoint.position, Quaternion.identity);
        TIrosLento bulletScript = bulletObj.GetComponent<TIrosLento>();
        bulletScript.SetTarget(target);
    }
}
