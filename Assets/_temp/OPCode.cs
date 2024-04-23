using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OPCode : MonoBehaviour
{
    public bool[,] mat;
    public int width;
    public int height;
    public GameObject path;
    public GameObject surround;
    public Transform GroundParent;
    private int count = 0;
    public int pathLength;
    public float generationTime = 0.3f;
    int x;
    int z;
    void Start()
    {
        mat = new bool[width, height];
        x = 0;
        z = height / 2;

        StartCoroutine(MakePath(x, z, count, mat));
        //CreateMap();
    }
    IEnumerator MakePath(int x, int z, int count, bool[,] mat)
    {
        bool isLooped = false;
        int dir;
        List<int> checkLoop = new List<int>();
        while (true)
        {
            dir = UnityEngine.Random.Range(0, 4);
            if (checkLoop.Count == 4)
            {
                isLooped = true;
                break;
            }
            else if (checkLoop.Contains(dir))
            {
                continue;
            }
            else
            {
                checkLoop.Add(dir);
            }
            if (dir == 0 && x + 2 < width && !mat[x + 1, z] && !mat[x + 2, z])
            {
                x++;
                mat[x, z] = true;
                Instantiate(path, new Vector3(x, 0, z), Quaternion.identity, GroundParent);
                x++;
                mat[x, z] = true;
                Instantiate(path, new Vector3(x, 0, z), Quaternion.identity, GroundParent);
                count += 2;

                yield return new WaitForSeconds(generationTime);
                StartCoroutine(MakePath(x, z, count, mat));
                //break;
            }
            else if (dir == 1 && z + 2 < height && !mat[x, z + 1] && !mat[x, z + 2])
            {
                z++;
                mat[x, z] = true;
                Instantiate(path, new Vector3(x, 0, z), Quaternion.identity, GroundParent);
                z++;
                mat[x, z] = true;
                Instantiate(path, new Vector3(x, 0, z), Quaternion.identity, GroundParent);
                count += 2;

                yield return new WaitForSeconds(generationTime);
                StartCoroutine(MakePath(x, z, count, mat));
                //break;
            }
            else if (dir == 2 && z - 2 > 0 && !mat[x, z - 1] && !mat[x, z - 2])
            {
                z--;
                mat[x, z] = true;
                Instantiate(path, new Vector3(x, 0, z), Quaternion.identity, GroundParent);
                z--;
                mat[x, z] = true;
                Instantiate(path, new Vector3(x, 0, z), Quaternion.identity, GroundParent);
                count += 2;

                yield return new WaitForSeconds(generationTime);
                StartCoroutine(MakePath(x, z, count, mat));
                //break;
            }
            else if (dir == 3 && x - 2 > 0 && !mat[x - 1, z] && !mat[x - 2, z])
            {
                x--;
                mat[x, z] = true;
                Instantiate(path, new Vector3(x, 0, z), Quaternion.identity, GroundParent);
                x--;
                mat[x, z] = true;
                Instantiate(path, new Vector3(x, 0, z), Quaternion.identity, GroundParent);
                count += 2;

                yield return new WaitForSeconds(generationTime);
                StartCoroutine(MakePath(x, z, count, mat));
                //break;
            }
        }
    }/*
    void CreateMap()
    {
        for(int x = 0; x < width; x++)
        {
            for(int z = 0; z < height; z++)
            {
                if (mat[x, z])
                {
                    Instantiate(path, new Vector3(x, 0, z), Quaternion.identity, GroundParent);
                }
                else
                {
                    Instantiate(surround, new Vector3(x, 0, z), Quaternion.identity, GroundParent);
                }
            }
        }
    }*/
}
