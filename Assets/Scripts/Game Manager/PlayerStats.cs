using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    [SerializeField] private int money;
    [SerializeField] private int startMoney = 300;
    private int m_Lives;
    [SerializeField] private int m_maxLives = 20;
    [SerializeField] private TMP_Text m_LivesText;
    [SerializeField] private TMP_Text m_MoneyText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        money = startMoney;
        m_MoneyText.text = money.ToString();

        m_Lives = m_maxLives;
        m_LivesText.text = m_Lives + " Lives";
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public int GetMoney()
    {
        return money;
    }
    public void UpdateMoney(int cost)
    {
        Debug.Log(cost);
        money += cost;
        Debug.Log(money);
        m_MoneyText.text = money.ToString();
    }
    public int GetLives()
    {
        return m_Lives;
    }
    public void UpdateLives(int cost)
    {
        m_Lives += cost;
        m_LivesText.text = m_Lives + " Lives";
    }
}
