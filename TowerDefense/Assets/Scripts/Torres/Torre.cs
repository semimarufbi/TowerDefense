using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torre : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected Transform turretRotationPoint; // Ponto de rota��o da torre
    [SerializeField] protected LayerMask enemyMask; // M�scara para identificar quais objetos s�o inimigos
    [SerializeField] protected GameObject bulletPrefab; // Prefab da bala que a torre ir� disparar
    [SerializeField] protected Transform firingPoint; // Ponto de onde a bala ser� disparada

    [Header("Attributes")]
    [SerializeField] protected float targetinRange = 5f; // Raio de alcance da torre para identificar inimigos
    [SerializeField] protected float rotationSpeed = 5f; // Velocidade de rota��o da torre
    [SerializeField] protected float bps = 1f; // Balas por segundo (quantidade de disparos que a torre faz em um segundo)

    protected Transform target; // Refer�ncia para o inimigo que a torre est� mirando
    private float timeUntilFire; // Temporizador para controlar quando a pr�xima bala deve ser disparada

    // M�todo chamado a cada quadro
    protected virtual void Update()
    {
        // Se n�o h� um alvo, tenta encontrar um
        if (target == null)
        {
            FindTarget();
            return; // Sai do m�todo se n�o houver alvo
        }

        RotateTowardsTarget(); // Rotaciona a torre em dire��o ao alvo

        // Verifica se o alvo ainda est� dentro do alcance
        if (!CheckTargetIsInRange())
        {
            target = null; // Se o alvo saiu do alcance, redefine para nulo
        }
        else
        {
            // Aumenta o tempo at� o pr�ximo disparo
            timeUntilFire += Time.deltaTime;
            if (timeUntilFire >= 1f / bps) // Verifica se � hora de disparar
            {
                Shoot(); // Dispara a bala
                timeUntilFire = 0f; // Zera o temporizador
            }
        }
    }

    // M�todo respons�vel por disparar a bala
    protected virtual void Shoot()
    {
        // Instancia a bala no ponto de disparo
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        TIros bulletScript = bulletObj.GetComponent<TIros>(); // Obt�m o script da bala
        bulletScript.SetTarget(target); // Define o alvo da bala
    }

    // M�todo para encontrar um alvo dentro do alcance
    protected virtual void FindTarget()
    {
        // Realiza um CircleCast para detectar inimigos dentro do alcance
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetinRange, Vector2.zero, 0f, enemyMask);

        // Se houver inimigos detectados, define o primeiro como alvo
        if (hits.Length > 0)
        {
            target = hits[0].transform;
        }
    }

    // Verifica se o alvo ainda est� dentro do alcance da torre
    protected bool CheckTargetIsInRange()
    {
        return Vector3.Distance(target.position, transform.position) <= targetinRange; // Retorna true se o alvo estiver dentro do alcance
    }

    // Rotaciona a torre em dire��o ao alvo
    protected virtual void RotateTowardsTarget()
    {
        // Calcula o �ngulo entre a torre e o alvo
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg - 90f;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle)); // Cria a rota��o desejada
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, rotationSpeed * Time.deltaTime); // Rotaciona suavemente a torre
    }

    // M�todo para desenhar um gizmo no editor para mostrar o alcance da torre
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan; // Define a cor do gizmo
        Gizmos.DrawWireSphere(transform.position, targetinRange); // Desenha uma esfera no alcance da torre
    }
}
