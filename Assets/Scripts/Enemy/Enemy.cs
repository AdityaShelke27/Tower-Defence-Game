using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private static Transform m_MainCamera;
    private float m_Speed = 10;
    [SerializeField]private Transform target;
    [SerializeField]private int waypointIndex = 0;
    public float startSpeed = 10;
    [SerializeField] private Transform m_HealthCanvas;
    [SerializeField] private Slider m_Healthbar;
    [SerializeField] private int m_Worth = 10;
    [SerializeField] private float m_MaxHealth = 10;
    private float health;
    [SerializeField] private GameObject m_DieEffect;
    // Start is called before the first frame update
    private void Awake()
    {
        m_MainCamera = Camera.main.transform;
    }
    void Start()
    {
        m_Speed = startSpeed;
        target = Waypoints.points[waypointIndex++];
        health = m_MaxHealth;
        m_Healthbar.maxValue = m_MaxHealth;
        m_Healthbar.value = health;
    }
    void Update()
    {
        Movement();
    }
    private void LateUpdate()
    {
        UILookAtCamera();
    }
    void Movement()
    {
        Vector3 dir = target.position - transform.position;
        Vector3 moveFrame = dir.normalized * m_Speed * Time.deltaTime;
        transform.position += moveFrame;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir.normalized), 0.3f);

        if (dir.magnitude < moveFrame.magnitude)
        {
            if(waypointIndex < Waypoints.points.Length)
            {
                target = Waypoints.points[waypointIndex++];
            }
            else
            {
                PlayerStats.Instance.UpdateLives(-1);
                WaveSpawner.s_EnemiesAlive--;
                Destroy(gameObject);
            }
        }

        m_Speed = startSpeed;
    }
    public void ApplySlow(float slowPct)
    {
        m_Speed = startSpeed * (1 - slowPct);
    }
    public void DealDamage(float damage)
    {
        health -= damage;
        m_Healthbar.value = health;
        if (health <= 0)
        {
            PlayerStats.Instance.UpdateMoney(m_Worth);
            GameObject dieEffect = Instantiate(m_DieEffect, transform.position, Quaternion.identity);
            
            Destroy(dieEffect, 5);
            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {
        WaveSpawner.s_EnemiesAlive--;
    }
    void UILookAtCamera()
    {
        m_HealthCanvas.LookAt(m_HealthCanvas.position + m_MainCamera.rotation * Vector3.forward, m_MainCamera.rotation * Vector3.up);
    }
}
