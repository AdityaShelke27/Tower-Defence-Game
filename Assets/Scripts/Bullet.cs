using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float explosionRadius;
    public LayerMask enemyLayer;
    public float damage;
    [SerializeField] private GameObject m_ShatterEffect;
    private bool m_IsChasing = false;
    private Transform target;
    void Update()
    {
        if(target)
        {
            Vector3 dir = target.position - transform.position;
            float distancePerFrame = speed * Time.deltaTime;
            
            transform.Translate(dir.normalized * distancePerFrame, Space.World);
            transform.LookAt(target);

            if (dir.magnitude <= distancePerFrame)
            {
                Die();
            }
        }
        else
        {
            if(m_IsChasing)
            {
                Die();
            }
        }
    }

    public void FollowTarget(Transform enemyTarget)
    {
        if(enemyTarget)
        {
            target = enemyTarget;
            m_IsChasing = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Die()
    {
        GameObject particle = Instantiate(m_ShatterEffect, transform.position, Quaternion.identity);
        Explode();
        Destroy(particle, 2f);
        Destroy(gameObject);
    }
    void Explode()
    {
        if(explosionRadius > 0)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius, enemyLayer);
            foreach (Collider col in colliders)
            {
                col.gameObject.GetComponent<Enemy>().DealDamage(damage);
            }
        }
        else
        {
            if(target)
            {
                target.GetComponent<Enemy>().DealDamage(damage);
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
