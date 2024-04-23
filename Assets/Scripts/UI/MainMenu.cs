using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenu : MonoBehaviour
{
    [SerializeField] private Animator m_CameraAnim;
    [SerializeField] private SceneFader m_SceneFader;
    [SerializeField] private Button[] levelButtons;

    private void Start()
    {
        //PlayerPrefs.SetInt("levelReached", 1);
        int levelReached = PlayerPrefs.GetInt(TagManager.LEVEL_REACHED, 1);
        for(int i = 0; i < levelButtons.Length; i++)
        {
            if(i + 1 > levelReached)
            {
                levelButtons[i].interactable = false;
            }
        }
    }
    public void Play()
    {
        m_CameraAnim.Play("Camera_To_Level_Select");
    }

    public void PlayRandom()
    {
        m_SceneFader.FadeTo("RandomLevel");
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void BackToMainMenu()
    {
        m_CameraAnim.Play("Camera_To_Main_Menu");
    }
    public void LevelSelect(int levelName)
    {
        m_SceneFader.FadeTo("Level " + levelName);
    }
}
