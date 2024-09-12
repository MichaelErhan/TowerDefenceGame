using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed = 10f;
    public int damage = 10;

    private Transform target;

    void Update() 
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
        
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
        
        // Проверка расстояния для попадания
        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            Enemy enemy = target.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                Debug.Log($"Arrow hit enemy for {damage} damage.");
            }
            Destroy(gameObject);
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void SetDamage(int newDamage)
    {
        damage = newDamage;
    }
}