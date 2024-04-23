using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveSpawner : MonoBehaviour
{
    public static int s_EnemiesAlive = 0;

    [SerializeField] private Wave[] m_Waves;
    [SerializeField] private GameObject[] m_EnemyTypes;
    public TMP_Text counterUI;
    public float timeBetweenWaves = 5f;
    public Transform spawnPoint;
    public static int waveNumber = 0;
    private float countdown = 2f;
    int enemyCount = 0;

    private void Awake()
    {
        if(SceneManager.GetActiveScene().name == "RandomLevel")
        {
            m_Waves = new Wave[Random.Range(2, 10)];
            for(int i = 0; i < m_Waves.Length; i++)
            {
                m_Waves[i] = new Wave();
                m_Waves[i].enemy = m_EnemyTypes[Random.Range(0, m_EnemyTypes.Length)];
                m_Waves[i].count = Random.Range(0, 10);
                m_Waves[i].spawnRate = Random.Range(0f, 3f);

                enemyCount += m_Waves[i].count;
            }
            
        }
    }
    private void Start()
    {
        waveNumber = 0;
        if(SceneManager.GetActiveScene().name == "RandomLevel")
        {
            PlayerStats.Instance.UpdateMoney(enemyCount * 15);
            PlayerStats.Instance.UpdateLives(enemyCount / 2);
        }
    }
    void Update()
    {
        if(s_EnemiesAlive > 0)
        {
            return;
        }
        if (waveNumber >= m_Waves.Length && s_EnemiesAlive == 0)
        {
            GameManager.s_LevelComplete?.Invoke(waveNumber);
            this.enabled = false;
        }
        if (countdown <= 0)
        {
            SpawnWave();
            countdown = timeBetweenWaves;
            return;
        }

        countdown -= Time.deltaTime;
        countdown = Mathf.Clamp(countdown, 0, Mathf.Infinity);
        counterUI.text = string.Format("{0:00.00}",countdown);
    }

    void SpawnWave()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        Wave wave = m_Waves[waveNumber];
        s_EnemiesAlive = wave.count;
        for(int i = 0; i < wave.count; i++)
        {
            Instantiate(wave.enemy, spawnPoint.position, Quaternion.identity);
            yield return new WaitForSeconds(1 / wave.spawnRate);
        }
        waveNumber++;
        
    }
}
