using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{ 
    public static Action<int> s_GameOver;
    public static Action<int> s_LevelComplete;
    public static int levelReached;
    private bool isGameOver = false;
    [SerializeField] private SceneFader m_SceneFader;
    [SerializeField] private TMP_Text m_WaveNumber;
    [SerializeField] private TMP_Text m_LevelCompleteWaveNumber;
    [SerializeField] private GameObject m_GameOverPanel;
    [SerializeField] private GameObject m_LevelCompletePanel;
    [SerializeField] private GameObject m_PausePanel;
    private void OnEnable()
    {
        s_GameOver += SetGameOver;
        s_LevelComplete += SetLevelComplete;
    }
    private void OnDisable()
    {
        s_GameOver -= SetGameOver;
        s_LevelComplete -= SetLevelComplete;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TooglePause();
        }
        CheckGameOver();
    }

    void CheckGameOver()
    {
        if (PlayerStats.Instance.GetLives() <= 0 && !isGameOver)
        {
            s_GameOver?.Invoke(WaveSpawner.waveNumber);
        }
    }

    void GameOver(int waveNumber)
    {
        m_GameOverPanel.SetActive(true);
        StartCoroutine(CountWaveNumber(m_WaveNumber, waveNumber));
    }

    void SetGameOver(int waveNumber)
    {
        isGameOver = true;
        GameOver(waveNumber);
    }
    void SetLevelComplete(int waveNumber)
    {
        m_LevelCompletePanel.SetActive(true);
        StartCoroutine(CountWaveNumber(m_LevelCompleteWaveNumber, waveNumber));
    }
    public void Retry()
    {
        Time.timeScale = 1;
        m_SceneFader.FadeTo(SceneManager.GetActiveScene().name);
    }
    public void NextLevel()
    {
        Time.timeScale = 1;
        levelReached = PlayerPrefs.GetInt(TagManager.LEVEL_REACHED) + 1;
        PlayerPrefs.SetInt(TagManager.LEVEL_REACHED, levelReached);
        Debug.Log(levelReached);
        m_SceneFader.FadeTo("Level " + levelReached);
    }
    public void Menu()
    {
        Time.timeScale = 1;
        m_SceneFader.FadeTo("MainMenu");
    }
    public void TooglePause()
    {
        if (m_PausePanel.activeSelf)
        {
            m_PausePanel.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            m_PausePanel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    IEnumerator CountWaveNumber(TMP_Text gameText, int waveNumber)
    {
        int wave = 0;
        gameText.text = wave.ToString();

        yield return new WaitForSeconds(0.7f);

        for(; wave < waveNumber; wave++)
        {
            yield return new WaitForSeconds(0.05f);
            gameText.text = wave.ToString();
        }
    }
}
