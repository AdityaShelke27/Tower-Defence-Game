using System.Collections.Generic;
using UnityEngine;

public class RandomPathGenerator3 : MonoBehaviour
{
    public bool[,] mat;
    public int width;
    public int height;
    public float size;
    public GameObject path;
    public GameObject surround;
    public Transform GroundParent;
    private int count = 0;
    public int pathLength;
    int x;
    int z;
    void Start()
    {
        mat = new bool[width, height];
        x = 0;
        z = height / 2;

        MakePath(x, z, count, mat);
        CreateMap();
    }
    void MakePath(int x, int z, int count, bool[,] mat)
    {
        int dir;
        List<int> checkLoop = new List<int>();
        while (true)
        {
            dir = Random.Range(0, 4);
            if (checkLoop.Count == 4)
            {
                break;
            }
            else
            {
                checkLoop.Add(dir);
            }
            if (dir == 0 && x + 2 < width && !mat[x + 1, z] && !mat[x + 2, z])
            {
                x++;
                mat[x, z] = true;
                Instantiate(path, new Vector3(x * size, 0, z * size), Quaternion.identity, GroundParent);
                x++;
                mat[x, z] = true;
                Instantiate(path, new Vector3(x * size, 0, z * size), Quaternion.identity, GroundParent);
                count += 2;
                MakePath(x, z, count, mat);
                //yield return new WaitForSeconds(generationTime);
                //StartCoroutine(MakePath(x, z, count, mat));
                //break;
            }
            else if (dir == 1 && z + 2 < height && !mat[x, z + 1] && !mat[x, z + 2])
            {
                z++;
                mat[x, z] = true;
                Instantiate(path, new Vector3(x * size, 0, z * size), Quaternion.identity, GroundParent);
                z++;
                mat[x, z] = true;
                Instantiate(path, new Vector3(x * size, 0, z * size), Quaternion.identity, GroundParent);
                count += 2;
                MakePath(x, z, count, mat);
                //yield return new WaitForSeconds(generationTime);
                //StartCoroutine(MakePath(x, z, count, mat));
                //break;
            }
            else if (dir == 2 && z - 2 > 0 && !mat[x, z - 1] && !mat[x, z - 2])
            {
                z--;
                mat[x, z] = true;
                Instantiate(path, new Vector3(x * size, 0, z * size), Quaternion.identity, GroundParent);
                z--;
                mat[x, z] = true;
                Instantiate(path, new Vector3(x * size, 0, z * size), Quaternion.identity, GroundParent);
                count += 2;
                MakePath(x, z, count, mat);
                //yield return new WaitForSeconds(generationTime);
                //StartCoroutine(MakePath(x, z, count, mat));
                //break;
            }
            else if (dir == 3 && x - 2 > 0 && !mat[x - 1, z] && !mat[x - 2, z])
            {
                x--;
                mat[x, z] = true;
                Instantiate(path, new Vector3(x * size, 0, z * size), Quaternion.identity, GroundParent);
                x--;
                mat[x, z] = true;
                Instantiate(path, new Vector3(x * size, 0, z * size), Quaternion.identity, GroundParent);
                count += 2;
                MakePath(x, z, count, mat);
                //yield return new WaitForSeconds(generationTime);
                //StartCoroutine(MakePath(x, z, count, mat));
                //break;
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
                /*{
                    Instantiate(path, new Vector3(x, 0, z), Quaternion.identity, GroundParent);
                }
                else*/
                {
                    Instantiate(surround, new Vector3(x * size, 0, z * size), Quaternion.identity, GroundParent);
                }
            }
        }
    }
}
