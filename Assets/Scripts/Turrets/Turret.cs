using System.Collections;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private GameObject target;
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public Transform partToRotate;
    public float range = 15f;
    public float rotSpeed = 1f;
    public float fireRate = 2f;
    [SerializeField] private bool isLaser = false;
    private LineRenderer lineRenderer;
    [SerializeField] private float fireCountdown = 0f;
    [SerializeField] private float randomRotationSpeed;
    private Quaternion randomRotation = Quaternion.identity;
    [SerializeField] private GameObject laserImpactEffect;
    [SerializeField] private float m_DamageOvertime;
    [SerializeField] private float slowPct;
    private Enemy m_TargetEnemy;

    private void Awake()
    {
        if(isLaser)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        TargetLockOn();
    }
    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(TagManager.ENEMY_TAG);
        float minDistance = Mathf.Infinity;
        GameObject targetEnemy = null;

        foreach(GameObject enemy in enemies)
        {
            float enemyDistance = Vector3.Distance(transform.position, enemy.transform.position);
            if(enemyDistance < minDistance)
            {
                minDistance = enemyDistance;
                targetEnemy = enemy;
            }
        }

        if(targetEnemy != null && minDistance <= range)
        {
            target = targetEnemy;
            m_TargetEnemy = target.GetComponent<Enemy>();
        }
        else
        {
            target = null;
            m_TargetEnemy = null;
        }
    }

    void TargetLockOn()
    {
        fireCountdown -= Time.deltaTime;
        if(target == null)
        {
            if(isLaser)
            {
                laserImpactEffect.SetActive(false);
                lineRenderer.enabled = false;
            }
            StartCoroutine(IdleTurretRotation());
            return;
        }
        StopCoroutine(IdleTurretRotation());
        Vector3 dir = target.transform.position - partToRotate.position;
        Quaternion rot = Quaternion.LookRotation(dir);
        partToRotate.rotation = Quaternion.Lerp(partToRotate.rotation, rot, rotSpeed * Time.deltaTime);

        if(isLaser)
        {
            ShootLaser();
        }
        else
        {
            Shoot();
        }
        
    }

    void Shoot()
    {
        if (fireCountdown <= 0)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
            bullet.GetComponent<Bullet>().FollowTarget(target.transform);
            fireCountdown = 1 / fireRate;
        }
    }

    void ShootLaser()
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, bulletSpawnPoint.position);
        lineRenderer.SetPosition(1, target.transform.position);
        laserImpactEffect.SetActive(true);
        laserImpactEffect.transform.position = target.transform.position;
        laserImpactEffect.transform.LookAt(transform.position);

        m_TargetEnemy.DealDamage(m_DamageOvertime * Time.deltaTime);
        m_TargetEnemy.ApplySlow(slowPct);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    IEnumerator IdleTurretRotation()
    {
        if (Quaternion.Angle(partToRotate.rotation, randomRotation) < 0.1f)
        {
            randomRotation = Quaternion.Euler(0f, Random.Range(-45, 45), 0f);
            yield return new WaitForSeconds(Random.Range(1f, 3));
        }
        else
        {
            partToRotate.rotation = Quaternion.Lerp(partToRotate.rotation, randomRotation, randomRotationSpeed * Time.deltaTime);
        }
    }
}
