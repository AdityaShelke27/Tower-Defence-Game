using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RandomPathGenerator : MonoBehaviour
{
    private bool[,] mat;
    public int width;
    public int height;
    [SerializeField] private GameObject m_Path;
    [SerializeField] private GameObject m_Node;
    [SerializeField] private Transform NodeParent;
    [SerializeField] private Transform m_GroundParent;
    [SerializeField] private Transform m_WaypointParent;
    [SerializeField] private GameObject m_StartPoint;
    [SerializeField] private GameObject m_EndPoint;
    private float m_Size;
    private int count = 0;
    public int pathLength;
    int x;
    int z;
    void Awake()
    {
        m_Size = m_Path.transform.localScale.x;
        mat = new bool[width, height];
        x = 0;
        z = height / 2;
        Instantiate(m_StartPoint, new Vector3(x * (m_Size + 1), 2.5f, -z * (m_Size + 1)), Quaternion.identity);
        m_WaypointParent.position = new Vector3(x * (m_Size + 1), 2.5f, -z * (m_Size + 1));
        MakePath(x, z);
        while (count < pathLength)
        {
            mat = new bool[width, height];
            count = 0;
            MakePath(x, z);
        }
        CreateMap();

        SetWaypoints();
    }
    void MakePath(int x, int z)
    {
        while (x < width - 2)
        {
            bool isLooped = false;
            List<int> checkLoop = new List<int>();
            while(true)
            {
                int move = Random.Range(0, 4);
                if (checkLoop.Count == 4)
                {
                    isLooped = true;
                    break;
                }
                else if (checkLoop.Contains(move))
                {
                    continue;
                }
                else
                {
                    checkLoop.Add(move);
                }
                if (move == 0 && !mat[x + 1, z] && !mat[x + 2, z])
                {
                    mat[x, z] = true;
                    x++;
                    mat[x, z] = true;
                    x++;
                    count += 2;
                    break;
                }
                else if (move == 1 && z + 2 < height && !mat[x, z + 1] && !mat[x, z + 2])
                {
                    mat[x, z] = true;
                    z++;
                    mat[x, z] = true;
                    z++;
                    count += 2;
                    break;
                }
                else if (move == 2 && z - 2 > 0 && !mat[x, z - 1] && !mat[x, z - 2])
                {
                    mat[x, z] = true;
                    z--;
                    mat[x, z] = true;
                    z--;
                    count += 2;
                    break;
                }
                else if(move == 3 && x > 0 && !mat[x - 1, z] && !mat[x - 2, z])
                {
                    mat[x, z] = true;
                    x--;
                    mat[x, z] = true;
                    x--;
                    count += 2;
                    break;
                }
            }
            
            if(isLooped)
            {
                break;
            }
        }
    }
    void CreateMap()
    {
        for(int x = 0; x < width; x++)
        {
            for(int z = 0; z < height; z++)
            {
                if (!mat[x, z])
                {
                    Instantiate(m_Node, new Vector3(x * (m_Size + 1), 0, -z * (m_Size + 1)), Quaternion.identity, NodeParent);
                }
                else
                {
                    Instantiate(m_Path, new Vector3(x * (m_Size + 1), 0, -z * (m_Size + 1)), Quaternion.identity, m_GroundParent);
                    //Instantiate(new GameObject($"Waypoint{m_WaypointParent.childCount}"), new Vector3(x * (m_Size + 1), 0, -z * (m_Size + 1)), Quaternion.identity, m_WaypointParent);
                }
            }
        }
    }

    void SetWaypoints()
    {
        int[] currentPos = new int[2];

        currentPos[0] = x;
        currentPos[1] = z;
        int dir = 0;
        bool isX = true;
        bool isPathOver = false;

        if (currentPos[0] + 1 < mat.GetLength(0) && mat[currentPos[0] + 1, currentPos[1]])
        {
            dir = 0;
            isX = true;
        }
        else if (currentPos[0] - 1 >= 0 && mat[currentPos[0] - 1, currentPos[1]])
        {
            dir = 1;
            isX = true;
        }
        else if (currentPos[1] + 1 < mat.GetLength(1) && mat[currentPos[0], currentPos[1] + 1])
        {
            dir = 2;
            isX = false;
        }
        else if (currentPos[1] - 1 > 0 && mat[currentPos[0], currentPos[1] - 1])
        {
            dir = 3;
            isX = false;
        }
        GameObject _wayPoint = new GameObject($"Waypoint{m_WaypointParent.childCount}");
        _wayPoint.transform.position = new Vector3(currentPos[0] * (m_Size + 1), 0, -currentPos[1] * (m_Size + 1));
        _wayPoint.transform.SetParent(m_WaypointParent);
        do
        {
            if (dir != 1 && currentPos[0] + 1 < mat.GetLength(0) && mat[currentPos[0] + 1, currentPos[1]])
            {
                if (!isX)
                {
                    GameObject wayPoint = new GameObject($"Waypoint{m_WaypointParent.childCount}");
                    wayPoint.transform.position = new Vector3(currentPos[0] * (m_Size + 1), 0, -currentPos[1] * (m_Size + 1));
                    wayPoint.transform.SetParent(m_WaypointParent);
                    isX = true;
                    dir = 0;
                }

                currentPos[0]++;
            }
            else if (dir != 0 && currentPos[0] - 1 >= 0 && mat[currentPos[0] - 1, currentPos[1]])
            {
                if (!isX)
                {
                    GameObject wayPoint = new GameObject($"Waypoint{m_WaypointParent.childCount}");
                    wayPoint.transform.position = new Vector3(currentPos[0] * (m_Size + 1), 0, -currentPos[1] * (m_Size + 1));
                    wayPoint.transform.SetParent(m_WaypointParent);
                    isX = true;
                    dir = 1;
                }

                currentPos[0]--;
            }
            else if (dir != 3 && currentPos[1] + 1 < mat.GetLength(1) && mat[currentPos[0], currentPos[1] + 1])
            {
                if (isX)
                {
                    GameObject wayPoint = new GameObject($"Waypoint{m_WaypointParent.childCount}");
                    wayPoint.transform.position = new Vector3(currentPos[0] * (m_Size + 1), 0, -currentPos[1] * (m_Size + 1));
                    wayPoint.transform.SetParent(m_WaypointParent);
                    isX = false;
                    dir = 2;
                }

                currentPos[1]++;
            }
            else if (dir != 2 && currentPos[1] - 1 >= 0 && mat[currentPos[0], currentPos[1] - 1])
            {
                if (isX)
                {
                    GameObject wayPoint = new GameObject($"Waypoint{m_WaypointParent.childCount}");
                    wayPoint.transform.position = new Vector3(currentPos[0] * (m_Size + 1), 0, -currentPos[1] * (m_Size + 1));
                    wayPoint.transform.SetParent(m_WaypointParent);
                    isX = false;
                    dir = 3;
                }

                currentPos[1]--;
            }
            else
            {
                Instantiate(m_EndPoint, new Vector3(currentPos[0] * (m_Size + 1), 2.5f, -currentPos[1] * (m_Size + 1)), Quaternion.identity);

                GameObject wayPoint = new GameObject($"Waypoint{m_WaypointParent.childCount}");
                wayPoint.transform.position = new Vector3(currentPos[0] * (m_Size + 1), 0, -currentPos[1] * (m_Size + 1));
                wayPoint.transform.SetParent(m_WaypointParent);
                isPathOver = true;
            }
        } while (!isPathOver);
    }
}
