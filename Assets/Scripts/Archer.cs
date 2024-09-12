using System.Collections;
using UnityEngine;

public class Archer : MonoBehaviour
{
    public GameObject arrowPrefab; // Префаб стрелы
    public float shootInterval = 2f; // Интервал стрельбы в секундах
    public float detectionRange = 10f; // Радиус поиска врага
    public float moveSpeed = 3f; // Скорость перемещения лучника
    public float shootingRange = 5f; // Дистанция, на которой лучник останавливается и начинает стрелять
    public int damage = 10; // Урон, наносимый стрелой

    private Transform target;
    private bool canShoot = true;

    void Start()
    {
        FindClosestEnemy();
    }

    void Update()
    {
        if (target == null)
        {
            FindClosestEnemy();
        }

        if (target != null)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, target.position);

            if (distanceToEnemy > shootingRange)
            {
                MoveTowardsEnemy();
            }
            else
            {
                if (canShoot)
                {
                    StartCoroutine(ShootArrow());
                }
            }
        }
    }
    void FindClosestEnemy()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        float closestDistance = Mathf.Infinity;
        target = null;

        foreach (Enemy enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance && distance <= detectionRange)
            {
                closestDistance = distance;
                target = enemy.transform;
            }
        }
    }
    void MoveTowardsEnemy()
    {
        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;

            // Поворачиваем лучника в сторону врага
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            
            Debug.Log("Moving towards enemy: " + target.position); 
        }
    }
    IEnumerator ShootArrow()
    {
        canShoot = false;

        GameObject arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
        Arrow arrowScript = arrow.GetComponent<Arrow>();

        if (arrowScript != null)
        {
            arrowScript.SetTarget(target);
            arrowScript.SetDamage(damage); 
        }

        yield return new WaitForSeconds(shootInterval);
        canShoot = true;
    }
}
