using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

public class SceneFader : MonoBehaviour
{
    [SerializeField] private Image m_FadeImage;
    [SerializeField] private MinMaxCurve m_Curve;
    // Start is called before the first frame update
    void Start()
    {
        m_FadeImage.color = new Color(0, 0, 0, 1);
        //StartCoroutine(FadeIn());
        LeanTween.alpha(m_FadeImage.rectTransform,0f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void FadeTo(string scene)
    {
        StartCoroutine(FadeOut(scene));
    }

    IEnumerator FadeIn()
    {
        float t = 1f;
        while(t > 0)
        {
            t -= Time.deltaTime;
            m_FadeImage.color = new Color(0, 0, 0, m_Curve.Evaluate(t));

            yield return 0;
        }

        m_FadeImage.color = new Color(0, 0, 0, 0);
    }
    IEnumerator FadeOut(string scene)
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            m_FadeImage.color = new Color(0, 0, 0, m_Curve.Evaluate(t));

            yield return 0;
        }

        m_FadeImage.color = new Color(0, 0, 0, 1);
        SceneManager.LoadScene(scene);
    }
}
