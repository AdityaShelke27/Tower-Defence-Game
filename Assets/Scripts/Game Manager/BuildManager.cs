using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;

    [SerializeField] private NodeUI nodeUI;
    private TurretBlueprint turretToBuild;
    [SerializeField] private Node selectedNode;
    public ParticleSystem buildEffect;
    public GameObject sellEffect;
    private void Awake()
    {
        if (instance == null) 
        {
            instance = this;
        }
    }
    public bool canBuild { get {return turretToBuild != null;} }
    public bool hasMoney { get { return PlayerStats.Instance.GetMoney() >= turretToBuild.cost; } }
    public void SelectNode(Node node)
    {
        if(node == selectedNode)
        {
            DeselectNode();
            return;
        }
        selectedNode = node;
        turretToBuild = null;

        nodeUI.SetTarget(node);
    }
    public void SetTurretToBuild(TurretBlueprint turret)
    {
        turretToBuild = turret;
        DeselectNode();
    }
    public TurretBlueprint GetTurretToBuild()
    {
        return turretToBuild;
    }
    public void DeselectNode()
    {
        selectedNode = null;
        nodeUI.Hide();
    }
}
