using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    public GameObject turret;
    public Color hoverColor;
    public Color invalidColor;
    public float offset = 0.5f;
    public TurretBlueprint turretBluePrint;
    private Color startColor;
    private Renderer rend;
    public bool isUpgraded = false;
    void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
    }
    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (turret)
        {
            BuildManager.instance.SelectNode(this);
            Debug.Log("Cannot build there");
            return;
        }
        if (!BuildManager.instance.canBuild)
        {
            return;
        }
        BuildTurret(BuildManager.instance.GetTurretToBuild());
    }
    private void OnMouseEnter()
    {
        if(EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if(!BuildManager.instance.canBuild)
        {
            return;
        }
        if(turret || !BuildManager.instance.hasMoney)
        {
            rend.material.color = invalidColor;
        }
        else
        {
            rend.material.color = hoverColor;
        }
    }
    private void OnMouseExit()
    {
        rend.material.color = startColor;
    }

    void BuildTurret(TurretBlueprint blueprint)
    {
        if (blueprint.cost > PlayerStats.Instance.GetMoney())
        {
            Debug.Log("Not Enough Money");
            turret = null;
            return;
        }
        turretBluePrint = blueprint;
        PlayerStats.Instance.UpdateMoney(-blueprint.cost);
        GameObject effect = Instantiate(BuildManager.instance.buildEffect.gameObject, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 4f);
        turret = Instantiate(blueprint.turretPrefab, GetBuildPosition(), transform.rotation);
    }
    public void UpgradeTurret()
    {
        if (turretBluePrint.upgradeCost > PlayerStats.Instance.GetMoney())
        {
            Debug.Log("Not Enough Money");
            //turret = null;
            return;
        }
        PlayerStats.Instance.UpdateMoney(-turretBluePrint.upgradeCost);
        GameObject effect = Instantiate(BuildManager.instance.buildEffect.gameObject, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 4f);
        Destroy(turret);
        turret = Instantiate(turretBluePrint.upgradedTurretPrefab, GetBuildPosition(), transform.rotation);
        isUpgraded = true;
    }
    public void SellTurret()
    {
        PlayerStats.Instance.UpdateMoney(turretBluePrint.sellValue);
        isUpgraded = false;
        Destroy(turret);
        turretBluePrint = null;
        GameObject effect = Instantiate(BuildManager.instance.sellEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 4f);
    }
    public Vector3 GetBuildPosition()
    {
        return transform.position + transform.up * offset;
    }
}
