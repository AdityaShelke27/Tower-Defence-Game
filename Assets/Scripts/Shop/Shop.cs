using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public TurretBlueprint standardTurret;
    public TurretBlueprint missileLauncher;
    public TurretBlueprint laserBeamer;

    [SerializeField] private TMP_Text standardTurretCost;
    [SerializeField] private TMP_Text missileLauncherCost;
    [SerializeField] private TMP_Text laserBeamerCost;

    private void Start()
    {
        standardTurretCost.text = standardTurret.cost.ToString();
        missileLauncherCost.text = missileLauncher.cost.ToString();
        laserBeamerCost.text = laserBeamer.cost.ToString();
    }
    public void SetStandardTurret()
    {
        BuildManager.instance.SetTurretToBuild(standardTurret);
    }
    public void SetMissileLauncher()
    {
        BuildManager.instance.SetTurretToBuild(missileLauncher);
    }
    public void SetLaserBeam()
    {
        BuildManager.instance.SetTurretToBuild(laserBeamer);
    }
}
