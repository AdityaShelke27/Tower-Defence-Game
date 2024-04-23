using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NodeUI : MonoBehaviour
{
    [SerializeField] private GameObject m_UI;
    [SerializeField] private TMP_Text UpgradeCost;
    [SerializeField] private TMP_Text SellCost;
    [SerializeField] private Button upgradeButton;
    private Node m_Target;

    public void SetTarget(Node target)
    {
        m_Target = target;
        transform.position = target.GetBuildPosition();
        if(!m_Target.isUpgraded)
        {
            UpgradeCost.text = m_Target.turretBluePrint.upgradeCost.ToString();
            
            upgradeButton.interactable = true;
        }
        else
        {
            UpgradeCost.text = "Done";
            upgradeButton.interactable = false;
        }
        SellCost.text = m_Target.turretBluePrint.sellValue.ToString();
        m_UI.SetActive(true);
    }
    public void Hide()
    {
        m_UI.SetActive(false);
    }

    public void Upgrade()
    {
        m_Target.UpgradeTurret();
        BuildManager.instance.DeselectNode();
    }
    public void Sell()
    {
        m_Target.SellTurret();
        BuildManager.instance.DeselectNode();
    }
}
