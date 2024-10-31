using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Torre : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform turretRotationPoint;
    [SerializeField] private LayerMask enemyMask;




    [Header("Attributes")]
    [SerializeField] private float targetinRange = 5f;
    [SerializeField] private float rotationSpeed = 5f;

    private Transform target;

    private void Update()
    {
        if (target == null)
        {
            FindTarget();
            return;
        }
        RotateTowardsTarget();

        if (!CheckTargetISInRAnge())
        {
            target = null;
        };
    }
     private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetinRange, (Vector2) transform.position,0f,enemyMask);

        if (hits.Length > 0) 
        {
            target = hits[0].transform;
        }
    }

    private bool CheckTargetISInRAnge()
    {
        return Vector3.Distance(target.position,transform.position) <= targetinRange;
    }

    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(target.position.y -transform.position.y,target.position.x -transform.position.x)* Mathf.Rad2Deg + -90f;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f,0f,angle));
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation,rotationSpeed * Time.deltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetinRange);
    }

}
